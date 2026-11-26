using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class ScreenMouseToWorldTranslator : MonoBehaviour
{

    public enum ClickMode{LEFT_CLICK, RIGHT_CLICK};

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

                Debug.Log("-----PRESS----");
                HandleRaycast();
                onClick.Invoke();
            }
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
