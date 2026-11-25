using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenMouseToWorldTranslator : MonoBehaviour
{

    [Header("Primary Settings")]
    public Mouse currentMouse;    
    public Transform subject;

    [Header("Raycast Related")]
    public Camera referenceCamera;
    public Vector2 pointerScreenPos;
    public float rayLength;
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

        LeftClickPoll();
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

    public void HandleRaycast()
    {

        screenRay = referenceCamera.ScreenPointToRay(pointerScreenPos);

        didHit = Physics.Raycast(screenRay.origin, screenRay.direction, out hit, rayLength, layerMask);

        if (didHit)
        {
            Debug.Log("HIT!");

            if(subject != null)
                subject.position = hit.point;
        }
    }
}
