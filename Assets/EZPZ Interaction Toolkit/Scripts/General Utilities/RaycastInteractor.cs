//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 8 Apr 2022

using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public bool cameraCleanupOnStart = true;
    // Start is called before the first frame update
    void Start()
    {

        if (transform.localScale.x != 1 || transform.localScale.y != 1 || transform.localScale.z != 1)
        {
            Debug.LogError("!!!!! ALERT !!!!!" + name + " SCALE IS NOT (1,1,1). This will cause object pickup & drop problems. Reset scale to (1,1,1)");
        }

        if (rayPointer == null)
            rayPointer = transform;

        if (environmentHit != null)
        {
            hitIndicatorRenderer = environmentHit.GetComponent<Renderer>();

            if (hitIndicatorRenderer != null)
            {
                hitIndicatorRenderer.enabled = true;
            }

            /*
            if (environmentHit.transform.localScale.x != 1 || environmentHit.transform.localScale.y != 1 || environmentHit.transform.localScale.z != 1)
            {
                Debug.LogError("!!!!! ALERT !!!!!" + environmentHit.name + " SCALE IS NOT (1,1,1). This will cause object pickup & drop problems. Reset scale to (1,1,1)");
            }
            */
        }

        if (myPlayerInput == null)
            myPlayerInput = GetComponent<PlayerInput>();

        originalRayLength = rayLength;

        if(cameraCleanupOnStart)
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
        if (myEventSystem != null)
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

        bool didHit = Physics.Raycast(rayPointer.position, rayPointer.TransformDirection(Vector3.forward), out hit, rayLength, layerMask);

        if (didHit)
        {
            Debug.DrawRay(rayPointer.position, rayPointer.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            HandleInteractables(hit);
        }
        else
        {
            hitSubject = null;
            OnNoClickable();
        }
    }

    void HandleInteractables(RaycastHit hit)
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
                subject.onPrimaryInteract.Invoke();

                //legacy-----
                subject.onFirstInteract.Invoke();
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

    void HandleMovables(InteractableGeneral hitSubject)
    {
        moveSubject = hitSubject.GetComponent<Movable>();

        if (moveSubject != null)
        {
            if (!moveSubject.moving)
            {
                //Pick up objects
                moveSubject.Grab();

                previousMoveParent = moveSubject.transform.parent;
                moveSubject.moving = true;

                if (moveSubject.noCollideOnHold)
                {
                    SetColliderIsTrigger(moveSubject, true);
                }

                if (moveSubject.groundPlace)
                {

                    moveSubject.transform.position = environmentHit.position + moveSubject.groundPlaceOffset;
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
                //Release objects
                moveSubject.moving = false;
                moveSubject.Drop();

                if (moveSubject.noCollideOnHold)
                {
                    SetColliderIsTrigger(moveSubject, false);
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

    public static void SetColliderIsTrigger(Movable m, bool setting)
    {
        Collider c = m.GetComponent<Collider>();
        if (c != null)
        {
            c.isTrigger = setting;

            if (m.subCollliders.Length > 0)
            {
                foreach (Collider subC in m.subCollliders)
                {
                    if (subC != null)
                    {
                        CharacterController cc = subC.GetComponent<CharacterController>();
                        if (cc == null)
                            subC.isTrigger = setting;
                    }
                }
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
        if (clickableIndicator != null)
            clickableIndicator.SetActive(false);

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
        if (clickableIndicator != null)
            clickableIndicator.SetActive(true);

        if (aimingCrosshair != null)
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


    public void OnRestart()
    {
        SceneManager.LoadScene(0);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
