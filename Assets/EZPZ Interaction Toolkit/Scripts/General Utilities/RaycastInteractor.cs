//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 8 Apr 2022

using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RaycastInteractor : MonoBehaviour
{


    [Header("Primary Settings")]
    public Transform rayPointer;
    public LayerMask layerMask;
    public LayerMask environmentLayer;
    public float rayLength = 15;
    public float touchDistanceDefault = 5;
    public float holdingDistanceDefault = 1.5f;
    public Transform pickupAttachPoint;

    [Header("Hover Text Settings")]
    public GameObject hoverTextRig;
    public TextMeshProUGUI hoverTextDisplay;
    public string useKeyTag = "%USE_KEY%";
    public string useKeyString = "[F]";

    [Header("General User Feedback")]
    public GameObject interactableIndicator;
    public GameObject interactingIndicator;
    public GameObject aimingCrosshair;
    public GameObject keyboardFreezeIcon;
    public GameObject tooFarIcon;
    public Transform environmentHit;
    public Transform generalHit;

    [Header("System Stuff (usually do not touch)")]
    public InteractableGeneral subject;
    public InteractableGeneral prevHitSubject;
    public InteractableGeneral hitSubject;
    public Holdable holdableSubject;
    public Holdable prevMoveSubject;
    public Typable typeSubject;
    public bool interactState = false;
    public bool prevInteractState = false;
    public Renderer environmentHitIndicatorRenderer;
    public Renderer generalHitIndicatorRenderer;
    public PlayerInput myPlayerInput; 
    public float originalRayLength;
    public EventSystem myEventSystem;
    public Transform previousMoveParent;
    public bool cameraCleanupOnStart = true;
    public bool didHit = false;
    public bool primaryLiftFlag = false;
    Rigidbody subjectRbody;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.localScale.x != 1 || transform.localScale.y != 1 || transform.localScale.z != 1)
        {
            Debug.LogError("!!!!! ALERT !!!!!" + name + " SCALE IS NOT (1,1,1). This will cause object pickup & drop problems. Reset scale to (1,1,1)");
        }

        if (rayPointer == null)
            rayPointer = transform;

        if (pickupAttachPoint == null)
            pickupAttachPoint = rayPointer;

        if (environmentHit != null)
        {
            environmentHitIndicatorRenderer = environmentHit.GetComponent<Renderer>();
        }

        if (generalHit != null)
        {
            generalHitIndicatorRenderer = generalHit.GetComponent<Renderer>();
        }

        if (tooFarIcon != null)
        {
            tooFarIcon.SetActive(false);
        }

        if (myPlayerInput == null)
            myPlayerInput = GetComponent<PlayerInput>();

        originalRayLength = rayLength;

        if (cameraCleanupOnStart)
            CameraCleanup();

        EventSystemCleanup();
    }

    // Update is called once per frame
    void Update()
    {
        //interactState = Key

        HandleEnvironmentRaycast();
        HandleRaycastInteractions();

        prevHitSubject = hitSubject;
        prevInteractState = interactState;
        interactState = false;
    }

    public void CameraCleanup()
    {
        Debug.Log("Camera Cleanup");

        if (rayPointer != null)
        {
            Camera mainCam = rayPointer.GetComponent<Camera>();

            if (mainCam != null)
            {

                Camera[] allCams = FindObjectsByType<Camera>(FindObjectsSortMode.None);


                if (allCams.Length > 1)
                {
                    foreach (Camera c in allCams)
                    {
                        if (c != mainCam)
                        {
                            Destroy(c.gameObject);
                        }
                    }
                }
            }
        }
    }

    public void EventSystemCleanup()
    {
        if (myEventSystem != null)
        {
            EventSystem[] allEventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

            if (allEventSystems.Length > 1)
            {
                foreach (EventSystem e in allEventSystems)
                {
                    if (e != myEventSystem)
                    {
                        Destroy(e.gameObject);
                    }
                }
            }
        }
    }

    public void OnFire()
    {
        //Debug.Log("OnFire");
        ForceInteract();

        primaryLiftFlag = false;

    }

    public void OnFireLift()
    {
        //Debug.Log("--FireLift");

        primaryLiftFlag = true;

        if (subject != null)
        {
            subject.onPrimaryInteractLift.Invoke();
        }

        if (holdableSubject != null)
        {
            if (holdableSubject.dropOnKeyLift)
                DropMovable();
        }
    }

    public void OnUse()
    {
        //Debug.Log("--OnUse");
        HandleSecondaryInteract();
    }

    public void OnUseLift()
    {
        //Debug.Log("--OnUseLift");
        HandleSecondaryInteractLift();
    }


    public void ForceInteract()
    {
        interactState = true;
    }

    public void HandleSecondaryInteract()
    {
        //Debug.Log("On Secondary Interact");

        if (subject != null)
        {
            if (!subject.restrictSecondaryToHeldOnly)
            {
                subject.onSecondaryInteract.Invoke();
            }
        }

        if (holdableSubject != null)
        {
            if (holdableSubject.restrictSecondaryToHeldOnly)
            {
                if (holdableSubject.isActiveAndEnabled)
                {
                    holdableSubject.onSecondaryInteract.Invoke();
                }

                //check if last event disabled the subject
                if (!holdableSubject.isActiveAndEnabled)
                {
                    //delink subject if it is disabled
                    //(e.g. for when it is being used for an "eat" interaction)
                    holdableSubject.Drop();
                    holdableSubject = null;
                    subject = null;
                }
            }
        }
    }

    public void HandleSecondaryInteractLift()
    {
        if (subject != null)
        {
            if (!subject.restrictSecondaryToHeldOnly)
            {
                subject.onSecondaryInteractLift.Invoke();
            }
            else
            {
                if (holdableSubject != null)
                {
                    if (holdableSubject.isActiveAndEnabled)
                    {
                        subject.onSecondaryInteractLift.Invoke();
                    }

                    //check if last event disabled the subject
                    if (!holdableSubject.isActiveAndEnabled)
                    {
                        //delink subject if it is disabled
                        //(e.g. for when it is being used for an "eat" interaction)
                        holdableSubject = null;
                        subject = null;
                    }
                }
            }
        }
    }

    public void HandleEnvironmentRaycast()
    {
        if (environmentHit != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(rayPointer.position, rayPointer.TransformDirection(Vector3.forward), out hit, rayLength, environmentLayer))
            {
                if (environmentHit != null)
                {
                    environmentHit.gameObject.SetActive(true);
                    environmentHit.position = hit.point;
                }

            }
            else
            {
                if (environmentHit != null)
                    environmentHit.gameObject.SetActive(false);
            }
        }
    }

    public void HandleRaycastInteractions()
    {
        didHit = Physics.Raycast(rayPointer.position, rayPointer.TransformDirection(Vector3.forward), out hit, rayLength, layerMask);

        if (didHit)
        {
            Debug.DrawRay(rayPointer.position, rayPointer.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            if (generalHit != null)
            {
                generalHit.gameObject.SetActive(true);
                generalHit.position = hit.point;
            }

            HandleInteractables(hit);
            HandleUnityButton(hit);
        }
        else
        {

            if (generalHit != null)
            {
                generalHit.gameObject.SetActive(false);
                generalHit.position = transform.position;
            }

            hitSubject = null;
            ActivateTooFarIcon(false);
            OnNoClickable();
        }
    }

    void HandleInteractables(RaycastHit hit)
    {
        hitSubject = hit.collider.gameObject.GetComponent<InteractableGeneral>();

        if (hitSubject != null)
        {
            if (hitSubject.customTouchDistance <= 0)
            {
                if (hit.distance <= touchDistanceDefault)
                {
                    ActivateTooFarIcon(false);
                }
                else
                {
                    ActivateTooFarIcon(true);
                    hitSubject = null;
                }
            }
            else
            {
                if (hit.distance <= hitSubject.customTouchDistance)
                {
                    ActivateTooFarIcon(false);
                }
                else
                {
                    ActivateTooFarIcon(true);
                    hitSubject = null;
                }
            }
        }
        else
        {
            ActivateTooFarIcon(false);
        }


        if (hitSubject != null)
        {
            subject = hitSubject;

            OnClickableHover();

            if (subject != prevHitSubject)
            {
                subject.onHoverEnter.Invoke();

                if (prevHitSubject != null)
                {
                    prevHitSubject.onHoverExit.Invoke();
                }

            }

            if (interactState && !prevInteractState)
            {
                subject.onPrimaryInteract.Invoke();

                //legacy-----
                //subject.onFirstInteract.Invoke();
                //legacy-----

                HandleMovables(subject);
                HandleTypables(subject);
            }
        }
        else
        {
            OnNoClickable();
        }
    }

    public void SyncHoverText(string newText)
    {
        if (newText.Length > 0)
        {
            if (hoverTextDisplay != null)
            {
                hoverTextDisplay.text = newText.Replace(useKeyTag, useKeyString);
            }
        }
        else
        {
            if (hoverTextRig != null)
                hoverTextRig.SetActive(false);

        }
    }

    public void HandleHoverText(bool mode)
    {
        if (hoverTextRig != null)
        {
            if (mode)
            {
                hoverTextRig.SetActive(true);

                if (holdableSubject == null)
                {
                    SyncHoverText(subject.hoverText);
                }
                else
                {
                    if (holdableSubject.heldText.Length > 0)
                        SyncHoverText(holdableSubject.heldText);
                    else
                        SyncHoverText(holdableSubject.hoverText);
                }
            }
            else
            {
                hoverTextRig.SetActive(false);
            }

        }
    }

    void HandleUnityButton(RaycastHit hit)
    {

        //Debug.Log("HandleUnityButton - before");
        Button b = hit.collider.gameObject.GetComponent<Button>();

        if (b != null)
        {
            if (interactState)
            {
                b.onClick.Invoke();
            }
        }

    }

    void HandleMovables(InteractableGeneral hitSubject)
    {

        if (holdableSubject == null)
        {
            holdableSubject = hitSubject.GetComponent<Holdable>();
        }
        else
        {
            //Debug.Log("Pre-existing object!");
            DropMovable();
        }

        if (holdableSubject != null)
        {
            if (!holdableSubject.moving)
            {
                GrabMovable();
            }
            else
            {
                if (holdableSubject.myRayManipulator == this)
                {
                    DropMovable();
                }
                else
                {
                    Holdable newGrab = holdableSubject;
                    holdableSubject.myRayManipulator.DropMovable();

                    holdableSubject = newGrab;
                    GrabMovable();
                }
            }
        }
    }

    public void GrabMovable()
    {
        //Pick up objects
        holdableSubject.Grab(this);
        previousMoveParent = holdableSubject.transform.parent;
        holdableSubject.moving = true;

        if (holdableSubject.noCollideOnHold)
        {
            Holdable.SetColliderIsTrigger(holdableSubject, true);
        }

        if (holdableSubject.groundPlace)
        {
            holdableSubject.transform.position = environmentHit.position + holdableSubject.groundPlaceOffset;
            holdableSubject.transform.parent = environmentHit;
        }
        else
        {
            Vector3 attachPos = pickupAttachPoint.position;

            if (holdableSubject.customHoldDistance <= 0)
            {
                attachPos += pickupAttachPoint.forward * holdingDistanceDefault;
            }
            else
            {
                attachPos += pickupAttachPoint.forward * holdableSubject.customHoldDistance;
            }


            if (holdableSubject.attachPoint != null)
            {
                //swap parentage first
                holdableSubject.attachPoint.parent = null;
                holdableSubject.transform.parent = holdableSubject.attachPoint;

                //align rotations and positions;
                holdableSubject.attachPoint.transform.position = attachPos;
                holdableSubject.attachPoint.transform.rotation = pickupAttachPoint.rotation;

                //return parentage
                holdableSubject.transform.parent = pickupAttachPoint;
                holdableSubject.attachPoint.parent = holdableSubject.transform;

            }
            else
            {
                holdableSubject.transform.position = attachPos;
                holdableSubject.transform.parent = pickupAttachPoint;
            }

            subjectRbody = holdableSubject.GetComponent<Rigidbody>();
            if (subjectRbody != null)
            {
                subjectRbody.useGravity = false;
                subjectRbody.isKinematic = true;
            }
        }
    }

    public void DropMovable()
    {
        if (holdableSubject != null)
        {
            holdableSubject.moving = false;
            holdableSubject.Drop();

            if (holdableSubject.noCollideOnHold)
            {
                Holdable.SetColliderIsTrigger(holdableSubject, false);
            }

            if (subjectRbody != null)
            {
                subjectRbody.useGravity = true;
                subjectRbody.isKinematic = false;

                if (holdableSubject.throwForce > 0)
                {
                    Vector3 direction = holdableSubject.transform.position - rayPointer.position;
                    subjectRbody.AddForce(holdableSubject.throwForce * direction * 100);
                }
            }

            if (holdableSubject.myMagnetSnapper != null)
            {
                holdableSubject.myMagnetSnapper.ReleaseSubject();
            }

            //moveSubject.transform.parent = previousMoveParent;
            holdableSubject.transform.parent = null;
            previousMoveParent = null;
            holdableSubject = null;
        }
    }


    public void HandleNonclickableMovable()
    {
        if(holdableSubject != null)
        {
            if(holdableSubject.hoverText.Length > 0)
            {
                HandleHoverText(true);
            }
        }


        if (interactState && !prevInteractState)
        {
            DropMovable();
        }
    }

    public void HandleTypables(InteractableGeneral hitSubject)
    {
        typeSubject = hitSubject.GetComponent<Typable>();

        if (typeSubject != null)
        {
            if (typeSubject.typeCapture)
            {
                ReleaseFromTyping();
            }
            else
            {
                FreezeForTyping();
            }
        }
    }

    public void ReleaseFromTyping()
    {
        if (typeSubject != null)
        {
            //revert to moving
            typeSubject.typeCapture = false;
            typeSubject.raycastInteractor = null;

            if (myPlayerInput != null)
                myPlayerInput.enabled = true;

            rayLength = originalRayLength;
            //clickableIndicator.SetActive(true);
            //aimingCrosshair.SetActive(true);
            //keyboardFreezeIcon.SetActive(false);
        }
    }

    public void FreezeForTyping()
    {
        //move to typing
        typeSubject.typeCapture = true;
        typeSubject.raycastInteractor = this;
        typeSubject.SyncText();

        if (myPlayerInput != null)
            myPlayerInput.enabled = false;

        rayLength = 0;
        //clickableIndicator.SetActive(false);
        //aimingCrosshair.SetActive(false);
        //keyboardFreezeIcon.SetActive(true);
    }

    public void OnNoClickable()
    {
        subject = null;//make sure to de-register any previously selected subjects
        HandleHoverText(false);
        

        if (interactableIndicator != null)
            interactableIndicator.SetActive(false);

        if(interactingIndicator != null)
            interactingIndicator.SetActive(false);

        if (rayLength > 0)
        {
            if (aimingCrosshair != null)
                aimingCrosshair.SetActive(true);

            if (keyboardFreezeIcon != null)
                keyboardFreezeIcon.SetActive(false);
        }
        else
        {
            if (aimingCrosshair != null)
                aimingCrosshair.SetActive(false);

            if (keyboardFreezeIcon != null)
                keyboardFreezeIcon.SetActive(true);
        }

        if (prevHitSubject != null)
            prevHitSubject.onHoverExit.Invoke();

        if (hitSubject != null)
            hitSubject.onHoverExit.Invoke();

        HandleNonclickableMovable();
    }

    public void OnClickableHover()
    {
        HandleHoverText(true);

        if (primaryLiftFlag)
        {
            if (interactableIndicator != null)            
                interactableIndicator.SetActive(true);

            if (interactingIndicator != null)
                interactingIndicator.SetActive(false);    
        }
        else
        {
            if (interactableIndicator != null)
                interactableIndicator.SetActive(false);

            if (interactingIndicator != null)
                interactingIndicator.SetActive(true);
        }



        if (aimingCrosshair != null)
            aimingCrosshair.SetActive(false);

        if (keyboardFreezeIcon != null)
            keyboardFreezeIcon.SetActive(false);

    }

    public void ActivateTooFarIcon(bool mode)
    {
        if (tooFarIcon != null)
        {
            tooFarIcon.SetActive(mode);
        }
    }

    public void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            subjectRbody = null;
            subject = null;
        }
    }
}
