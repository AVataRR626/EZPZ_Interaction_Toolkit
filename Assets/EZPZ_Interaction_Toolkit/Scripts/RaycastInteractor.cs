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

    [Header("User Feedback")]
    public GameObject clickableIndicator;
    public GameObject aimingCrosshair;

    [Header("System Stuff (do not touch, usually)")]
    public InteractableGeneral subject;
    public InteractableGeneral prevHitSubject;
    public InteractableGeneral hitSubject;
    public bool interactState = false;
    public bool prevInteractState = false;
    // Start is called before the first frame update
    void Start()
    {
        if (rayPointer == null)
            rayPointer = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //interactState = Key

        HandleRaycastInteractions();

        prevHitSubject = hitSubject;
        prevInteractState = interactState;
        interactState = false;
    }

    public void OnFire()
    {
        Debug.Log("OnFir");
        ForceInteract();
    }


    public void ForceInteract()
    {
        interactState = true;
    }

    public void HandleRaycastInteractions()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayPointer.position, rayPointer.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(rayPointer.position, rayPointer.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit " + hit.collider.name);

            hitSubject = hit.collider.gameObject.GetComponent<InteractableGeneral>();

            if (hitSubject != null)
            {
                subject = hitSubject;
                OnClickableHover();

                if(subject != prevHitSubject)
                {
                    subject.onHoverEnter.Invoke();
                }

                if (interactState && !prevInteractState)
                {
                    subject.onFirstInteract.Invoke();
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

                if (subject != null)
                {
                    if (prevHitSubject == subject)
                    {
                        subject.onHoverExit.Invoke();
                    }
                }

                OnNoClickable();
            }

        }
        else
        {
            OnNoClickable();
        }
    }

    public void OnNoClickable()
    {
        clickableIndicator.SetActive(false);
        aimingCrosshair.SetActive(true);
    }

    public void OnClickableHover()
    {
        clickableIndicator.SetActive(true);
        aimingCrosshair.SetActive(false);

    }
}
