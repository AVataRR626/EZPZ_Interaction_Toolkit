using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenMouseToWorldTranslator : MonoBehaviour
{

    public enum ClickMode{LEFT_CLICK, RIGHT_CLICK};

    [Header("Primary Settings")]
    public Mouse currentMouse;    
    public Transform subject;
    public ClickMode clickMode;
    

    [Header("Raycast Related")]
    public Camera referenceCamera;
    public Vector2 pointerScreenPos;
    public float rayLength = 100;
    public LayerMask layerMask;

    public bool didHit = false;
    RaycastHit hit;
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
            }
        }
    }

    public void HandleRaycast()
    {

        screenRay = referenceCamera.ScreenPointToRay(pointerScreenPos);

        didHit = Physics.Raycast(screenRay.origin, screenRay.direction, out hit, rayLength, layerMask);

        if (didHit)
        {
            Debug.Log("---HIT!");

            Debug.Log("----POINT COORDINATES: ----- " + hit.point.ToString());

            if(subject != null)
                subject.position = hit.point;
        }
    }
}
