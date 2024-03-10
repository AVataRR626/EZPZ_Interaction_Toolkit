//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 8 Apr 2022

using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RaycastInteractor : MonoBehaviour
{
    [Header("Primary Settings")]
    public Transform rayPointer;
    public LayerMask layerMask;
    public LayerMask environmentLayer;
    public float rayLength = 4;
    public float holdingDistance = 1.5f;

    [Header("User Feedback")]
    public GameObject clickableIndicator;
    public GameObject aimingCrosshair;
    public GameObject keyboardFreezeIcon;
    public Transform environmentHit;

    [Header("System Stuff (do not touch, usually)")]
    public InteractableGeneral subject;
    public InteractableGeneral prevHitSubject;
    public InteractableGeneral hitSubject;
    public Movable moveSubject;
    public Typable typeSubject;
    public bool interactState = false;
    public bool prevInteractState = false;
    public Renderer hitIndicatorRenderer;
    public PlayerInput myPlayerInput;
    Rigidbody subjectRbody;
    public float originalRayLength;
    public EventSystem myEventSystem;
    public Transform previousMoveParent;
    // Start is called before the first frame update
    void Start()
    {
        if (rayPointer == null)
            rayPointer = transform;

        if (environmentHit != null)
        {
            hitIndicatorRenderer = environmentHit.GetComponent<Renderer>();

            if (hitIndicatorRenderer != null)
            {
                hitIndicatorRenderer.enabled = true;
            }
        }

        if (myPlayerInput == null)
            myPlayerInput = GetComponent<PlayerInput>();

        originalRayLength = rayLength;

        CameraCleanup();
        EventSystemCleanup();
    }

    // Update is called once per frame
    void FixedUpdate()
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
                Camera[] allCams = FindObjectsOfType<Camera>();

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
        if(myEventSystem != null)
        {
            EventSystem[] allEventSystems = FindObjectsOfType<EventSystem>();

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
    }

    public void OnFireLift()
    {
        //Debug.Log("--FireLift");
    }


    public void ForceInteract()
    {
        interactState = true;
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
        RaycastHit hit;
        if (Physics.Raycast(rayPointer.position, rayPointer.TransformDirection(Vector3.forward), out hit, rayLength, layerMask))
        {
            Debug.DrawRay(rayPointer.position, rayPointer.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit " + hit.collider.name);

            HandleInteractables(ref hit);

        }
        else
        {
            hitSubject = null;
            OnNoClickable();
        }
    }

    void HandleInteractables(ref RaycastHit hit)
    {
        hitSubject = hit.collider.gameObject.GetComponent<InteractableGeneral>();

        if (hitSubject != null)
        {
            subject = hitSubject;
            OnClickableHover();

            if (subject != prevHitSubject)
            {
                subject.onHoverEnter.Invoke();

                if (prevHitSubject != null)
                    prevHitSubject.onHoverExit.Invoke();
            }

            if (interactState && !prevInteractState)
            {
                subject.onFirstInteract.Invoke();
                HandleMovables(subject);
                HandleTypables(subject);
            }

            /*
            if(interactState && interactState == prevInteractState)
            {
                //subject.onHoldInteract.Invoke();
            }
            */
        }
        else
        {
            OnNoClickable();
        }
    }

    void HandleMovables(InteractableGeneral hitSubject)
    {
        moveSubject = hitSubject.GetComponent<Movable>();

        if (moveSubject != null)
        {
            if (!moveSubject.moving)
            {
                previousMoveParent = moveSubject.transform.parent;
                moveSubject.moving = true;

                if (moveSubject.noCollideOnHold)
                {
                    Collider c = moveSubject.GetComponent<Collider>();
                    c.isTrigger = true;
                }

                if (moveSubject.groundPlace)
                {   

                    moveSubject.transform.position = environmentHit.position;
                    moveSubject.transform.parent = environmentHit;
                }
                else
                {
                    moveSubject.transform.position = rayPointer.position + rayPointer.forward * holdingDistance;
                    moveSubject.transform.parent = rayPointer;

                    subjectRbody = moveSubject.GetComponent<Rigidbody>();
                    if (subjectRbody != null)
                    {
                        subjectRbody.useGravity = false;
                        subjectRbody.isKinematic = true;
                    }
                }
            }
            else
            {
                moveSubject.moving = false;
                

                if (moveSubject.noCollideOnHold)
                {
                    Collider c = moveSubject.GetComponent<Collider>();
                    c.isTrigger = false;
                }

                if (subjectRbody != null)
                {
                    subjectRbody.useGravity = true;
                    subjectRbody.isKinematic = false;

                    if (moveSubject.throwForce > 0)
                    {
                        Vector3 direction = moveSubject.transform.position - rayPointer.position;
                        subjectRbody.AddForce(moveSubject.throwForce * direction * 100);
                    }
                }


                //moveSubject.transform.parent = previousMoveParent;
                moveSubject.transform.parent = null;
                previousMoveParent = null;
            }
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
        myPlayerInput.enabled = false;

        rayLength = 0;
        //clickableIndicator.SetActive(false);
        //aimingCrosshair.SetActive(false);
        //keyboardFreezeIcon.SetActive(true);
    }

    public void OnNoClickable()
    {
        clickableIndicator.SetActive(false);

        if (rayLength > 0)
        {
            aimingCrosshair.SetActive(true);

            if (keyboardFreezeIcon != null)
                keyboardFreezeIcon.SetActive(false);
        }
        else
        {
            aimingCrosshair.SetActive(false);

            if (keyboardFreezeIcon != null)
                keyboardFreezeIcon.SetActive(true);
        }

        if (environmentHit != null)
        {
            if (hitIndicatorRenderer != null)
            {
                hitIndicatorRenderer.enabled = true;
            }
        }

        if (prevHitSubject != null)
            prevHitSubject.onHoverExit.Invoke();

        if (hitSubject != null)
            hitSubject.onHoverExit.Invoke();
    }

    public void OnClickableHover()
    {
        clickableIndicator.SetActive(true);
        aimingCrosshair.SetActive(false);

        if (keyboardFreezeIcon != null)
            keyboardFreezeIcon.SetActive(false);

        if (environmentHit != null)
        {
            if (hitIndicatorRenderer != null)
            {
                hitIndicatorRenderer.enabled = false;
            }
        }
    }
}
