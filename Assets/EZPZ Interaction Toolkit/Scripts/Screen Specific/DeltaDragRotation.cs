using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DeltaDragRotation : MonoBehaviour
{
    public enum ClickMode { LEFT_CLICK, RIGHT_CLICK };

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
