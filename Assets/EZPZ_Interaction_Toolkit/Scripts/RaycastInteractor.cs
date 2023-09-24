//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 8 Apr 2022

using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastInteractor : MonoBehaviour
{
    [Header("Primary Settings")]
    public Transform rayPointer;
    public LayerMask layerMask;
    public LayerMask environmentLayer;

    [Header("User Feedback")]
    public GameObject clickableIndicator;
    public GameObject aimingCrosshair;
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
    public FirstPersonController myFPSController;
    public float originalFPSMoveSpeed;
    Rigidbody subjectRbody;
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

        if (myFPSController == null)
            myFPSController = GetComponent<FirstPersonController>();

        originalFPSMoveSpeed = myFPSController.MoveSpeed;
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

    public void OnFire()
    {
        //Debug.Log("OnFire");
        ForceInteract();
    }


    public void ForceInteract()
    {
        interactState = true;
    }

    public void HandleEnvironmentRaycast()
    {
        if(environmentHit != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(rayPointer.position, rayPointer.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, environmentLayer))
            {
                if (environmentHit != null)
                    environmentHit.position = hit.point;

            }
        }
    }

    public void HandleRaycastInteractions()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayPointer.position, rayPointer.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(rayPointer.position, rayPointer.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit " + hit.collider.name);

            HandleInteractables(ref hit);

        }
        else
        {
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

                if(prevHitSubject != null)
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
                moveSubject.moving = true;

                if (moveSubject.groundPlace)
                {
                    moveSubject.transform.position = environmentHit.position;
                    moveSubject.transform.parent = environmentHit;
                }
                else
                {                    
                    //moveSubject.transform.position = rayPointer.forward * 0.5f;
                    moveSubject.transform.parent = rayPointer;

                    subjectRbody = moveSubject.GetComponent<Rigidbody>();
                    if(subjectRbody != null)
                    {
                        subjectRbody.useGravity = false;
                        subjectRbody.isKinematic = true;
                    }
                }
            }
            else
            {
                moveSubject.moving = false;

                if (subjectRbody != null)
                {
                    subjectRbody.useGravity = true;
                    subjectRbody.isKinematic = false;

                    if(moveSubject.throwForce > 0)
                    {
                        Vector3 direction = moveSubject.transform.position - rayPointer.position;
                        subjectRbody.AddForce(moveSubject.throwForce * direction * 100);
                    }
                    
                }

                moveSubject.transform.parent = null;
            }
        }
    }

    public void HandleTypables(InteractableGeneral hitSubject)
    {
        typeSubject = hitSubject.GetComponent<Typable>();

        if(typeSubject != null)
        {
            if(typeSubject.typeCapture)
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
        //revert to moving
        typeSubject.typeCapture = false;
        typeSubject.raycastInteractor = null;
        myFPSController.MoveSpeed = originalFPSMoveSpeed;
    }

    public void FreezeForTyping()
    {
        //move to typing
        typeSubject.typeCapture = true;
        typeSubject.raycastInteractor = this;
        typeSubject.SyncText();
        myFPSController.MoveSpeed = 0;
    }

    public void OnNoClickable()
    {
        clickableIndicator.SetActive(false);
        aimingCrosshair.SetActive(true);

        if(environmentHit != null)
        {
            if (hitIndicatorRenderer != null)
            {
                hitIndicatorRenderer.enabled = true;
            }
        }

        if(prevHitSubject != null)
            prevHitSubject.onHoverExit.Invoke();

        if (hitSubject != null)
            hitSubject.onHoverExit.Invoke();
    }

    public void OnClickableHover()
    {
        clickableIndicator.SetActive(true);
        aimingCrosshair.SetActive(false);

        if (environmentHit != null)
        {
            if (hitIndicatorRenderer != null)
            {
                hitIndicatorRenderer.enabled = false;
            }
        }

    }
}
