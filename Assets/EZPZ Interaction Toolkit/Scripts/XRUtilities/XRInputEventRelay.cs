//Input Event Replay - General Tool
//Matt Cabanag, Feb 2024

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
public class XRInputEventRelay : MonoBehaviour
{
    public enum InputMode { Grip, Trigger, Joystic};
    
    public InputDeviceCharacteristics deviceFilter;
    public InputMode inputMode;
    
    public float pressValue;
    public float pressThreshold = 0.1f;
    public UnityEvent OnPress;

    public ValueRelayFloat pressValueRelay;

    private bool lastButtonState = false;
    private List<InputDevice> validDevices;
    public bool pressOnceFlag = false;



    private void Awake()
    {
        if (pressValueRelay == null)
            pressValueRelay = GetComponent<ValueRelayFloat>();

        validDevices = new List<InputDevice>();        
    }

    public InputFeatureUsage<float> UsageModeFloat(InputMode im)
    {
        if(im == InputMode.Grip)
            return CommonUsages.grip;

        if (im == InputMode.Trigger)
            return CommonUsages.trigger;



        return CommonUsages.trigger;
    }

    public InputFeatureUsage<Vector2> UsageModeVector2(InputMode im)
    {
        return CommonUsages.primary2DAxis;
    }

    void OnEnable()
    {
        List<InputDevice> allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);
        foreach (InputDevice device in allDevices)
            InputDevices_deviceConnected(device);

        InputDevices.deviceConnected += InputDevices_deviceConnected;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= InputDevices_deviceConnected;
        InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
        validDevices.Clear();
    }

    private void InputDevices_deviceConnected(InputDevice device)
    {
        bool discardedValue;

        if (device.TryGetFeatureValue(CommonUsages.gripButton, out discardedValue))
        {
            if ((device.characteristics & deviceFilter) == deviceFilter)
            {
                validDevices.Add(device);
            }
        }
    }

    private void InputDevices_deviceDisconnected(InputDevice device)
    {
        if (validDevices.Contains(device))
            validDevices.Remove(device);
    }

    void Update()
    {
        //HandlePrimaryButton();
        //HandleTrigger();
        HandleTriggerFloat();
    }

    public void HandleTriggerFloat()
    {
        foreach(InputDevice d in validDevices)
        {
            if (d.TryGetFeatureValue(UsageModeFloat(inputMode), out pressValue))
            {
                if (pressValueRelay != null)
                    pressValueRelay.value = pressValue;

                if (pressValue > pressThreshold) // Adjust the threshold as needed
                {
                    // Trigger is pressed
                    //Debug.Log("Trigger pressed!");
                    // Add your logic here when the trigger is pulled
                    if (!pressOnceFlag)
                    {
                        pressOnceFlag = true;
                        OnPress.Invoke();
                    }
                }
                else
                {
                    pressOnceFlag = false;
                }
            }
        }
    }

    public void HandleTrigger()
    {
        bool tempState = false;
        foreach (var device in validDevices)
        {
            bool primaryButtonState = false;
            tempState = device.TryGetFeatureValue(CommonUsages.triggerButton, out primaryButtonState) // did get a value
                        && primaryButtonState // the value we got
                        || tempState; // cumulative result from other controllers
        }

        if (tempState != lastButtonState) // Button state changed since last frame
        {
            OnPress.Invoke();
            lastButtonState = tempState;
        }
    }

    public void HandlePrimaryButton()
    {
        bool tempState = false;
        foreach (var device in validDevices)
        {
            bool primaryButtonState = false;
            tempState = device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonState) // did get a value
                        && primaryButtonState // the value we got
                        || tempState; // cumulative result from other controllers
        }

        if (tempState != lastButtonState) // Button state changed since last frame
        {
            OnPress.Invoke();
            lastButtonState = tempState;
        }
    }
}
