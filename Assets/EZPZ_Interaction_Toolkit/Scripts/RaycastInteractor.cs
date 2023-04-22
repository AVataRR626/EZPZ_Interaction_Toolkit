//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 8 Apr 2022

using System.Collections;
using System.Collections.Generic;
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
    public bool interactState = false;
    public bool prevInteractState = false;
    public Renderer hitIndicatorRenderer;
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
                moveSubject.transform.position = environmentHit.position;
                moveSubject.transform.parent = environmentHit;
            }
            else
            {
                moveSubject.moving = false;
                moveSubject.transform.position = environmentHit.position;
                moveSubject.transform.parent = null;
            }
        }
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
