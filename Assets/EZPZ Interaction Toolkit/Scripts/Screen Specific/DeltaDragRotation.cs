//script for EZPZ Interaction Toolkit
//by Matt Cabanag
//created 26 Nov 2026


using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class DeltaDragRotation : MonoBehaviour
{
    public enum ClickMode {LEFT_CLICK, RIGHT_CLICK, ONE_TOUCH, TWO_TOUCH};

    [Header("Core Settings")]
    public Transform yawReference;
    public Transform pitchReference;
    public float rotationScale = 20;
    public ClickMode clickMode;
    public UnityEvent onDrag;


    [Header("System Stuff - Usually Don't Touch")]
    public Vector2 delta;

    public Mouse currentMouse;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Mouse.current != null)
            currentMouse = Mouse.current;


        EnhancedTouchSupport.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMouse != null)
        {
            delta = currentMouse.delta.ReadValue();

            if(clickMode == ClickMode.LEFT_CLICK) 
                LeftPressPoll();   

            if(clickMode == ClickMode.RIGHT_CLICK)
                RightPressPoll();

            if (clickMode == ClickMode.ONE_TOUCH)
                OneTouchDragPoll();

            if (clickMode == ClickMode.TWO_TOUCH)
                TwoTouchDragPoll();
        }

        
    }

    public void LeftPressPoll()
    {
        if(currentMouse.leftButton.isPressed)
        {
            HandleDragYaw();
            HandleDragPitch();

            onDrag.Invoke();
        }
    }

    public void RightPressPoll()
    {
        if (currentMouse.rightButton.isPressed)
        {
            HandleDragYaw();
            HandleDragPitch();

            onDrag.Invoke();
        }
    }

    public void OneTouchDragPoll()
    {
        if (Touch.activeTouches.Count == 1)
        {
            Debug.Log("---- ddr: OneTouch");

            delta = Touch.activeTouches[0].delta;

            HandleDragPitch();
            HandleDragYaw();

            onDrag.Invoke();
        }
    }

    public void TwoTouchDragPoll()
    {
        if(Touch.activeTouches.Count == 2)
        {
            Debug.Log("---- ddr: TwoTouch");

            delta = Touch.activeTouches[1].delta;

            HandleDragPitch();
            HandleDragYaw();

            onDrag.Invoke();
        }
    }

    public void HandleDragYaw()
    {
        if(yawReference != null)
            yawReference.Rotate(0, -delta.x * rotationScale * Time.deltaTime, 0);

    }

    public void HandleDragPitch()
    {
        if(pitchReference != null)
            pitchReference.Rotate(delta.y * rotationScale * Time.deltaTime, 0, 0);
    }
}
