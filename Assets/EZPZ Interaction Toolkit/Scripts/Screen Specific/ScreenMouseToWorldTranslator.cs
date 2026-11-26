//script for EZPZ Interaction Toolkit
//by Matt Cabanag
//created 25 Nov 2026

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class ScreenMouseToWorldTranslator : MonoBehaviour
{

    public enum ClickMode{LEFT_CLICK, RIGHT_CLICK, ONE_TOUCH, TWO_TOUCH};

    [Header("Primary Settings")]
   
    public Transform subject;
    public ClickMode clickMode;
    public UnityEvent onClick;
    

    [Header("Raycast Settings")]
    public Camera referenceCamera;
    public Vector2 pointerScreenPos;
    public float rayLength = 100;
    public LayerMask includeLayers;
    public LayerMask excludeLayers;

    [Header("System Stuff - Usually Don't Touch")]
    public Mouse currentMouse;
    public bool didHitInclude = false;
    public bool didHitExclude = false;
    RaycastHit hitInclude;
    RaycastHit hitExclude;
    Ray screenRay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Mouse.current != null)
            currentMouse = Mouse.current;         

        if(referenceCamera == null)
            referenceCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        TrackMouse();

        if(clickMode == ClickMode.LEFT_CLICK)
            LeftClickPoll();

        if(clickMode == ClickMode.RIGHT_CLICK)
            RightClickPoll();

        if(clickMode == ClickMode.ONE_TOUCH)
            OneTouchPoll();

        if(clickMode != ClickMode.TWO_TOUCH)
            TwoTouchPoll();
    }

    public void TrackMouse()
    {
        if (currentMouse != null)
        {
            pointerScreenPos = currentMouse.position.ReadValue();
        }
    }

    public void LeftClickPoll()
    {
        if (currentMouse != null)
        {
            if(currentMouse.leftButton.wasPressedThisFrame)
            {
                Debug.Log("-----PRESS----");
                HandleRaycast();
                onClick.Invoke();
            }
        }
    }

    public void RightClickPoll()
    {
        if (currentMouse != null)
        {
            if (currentMouse.rightButton.wasPressedThisFrame)
            {
                HandleRaycast();
                onClick.Invoke();
            }
        }
    }

    public void OneTouchPoll()
    {
        if (Touch.activeTouches.Count == 1)
        {
            Debug.Log("-----scmt ONE TOUCH----");

            pointerScreenPos = Touch.activeTouches[0].screenPosition;

            HandleRaycast();
            onClick.Invoke();
        }
    }

    public void TwoTouchPoll()
    {
        if (Touch.activeTouches.Count == 2)
        {
            Debug.Log("-----scmt TWO TOUCH----");

            pointerScreenPos = Touch.activeTouches[1].screenPosition;

            HandleRaycast();
            onClick.Invoke();
        }
    }

    public void HandleRaycast()
    {

        screenRay = referenceCamera.ScreenPointToRay(pointerScreenPos);

        didHitInclude = Physics.Raycast(screenRay.origin, screenRay.direction, out hitInclude, rayLength, includeLayers);
        didHitExclude = Physics.Raycast(screenRay.origin, screenRay.direction, out hitExclude, rayLength, excludeLayers);

        if(!didHitExclude)
        {
            if (didHitInclude)
            {
                Debug.Log("---HIT!");

                Debug.Log("----POINT COORDINATES: ----- " + hitInclude.point.ToString());


                if (subject != null)
                {
                    subject.position = hitInclude.point;
                    subject.rotation = Quaternion.FromToRotation(transform.up, hitInclude.normal);
                }
            }
        }
    }
}
