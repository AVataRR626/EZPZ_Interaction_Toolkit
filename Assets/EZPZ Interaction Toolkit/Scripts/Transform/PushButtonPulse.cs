/*
Position OffSet Pulse
EZPZ Interaction Toolkit

Created 7 Feb 2026

-Matt
*/

using UnityEngine;

public class PushButtonPulse : MonoBehaviour
{
    public float pulseTime = 0.25f;
    public float pushScale = 0.1f;
    public bool pulseOnStart = false;
    public float startDelay = 2;
    public bool pulseRepeat = false;
    public float pulseInterval = 3;


    [Header("System Stuff - Usually Don't Touch")]
    public Vector3 positionAdd;
    public Vector3 originalPosition;
    public float pulseClock = 0;
    public bool pulseSwitch = false;
    public float timeLimit;
    public bool resetSwitch = false;

    // Use this for initialization
    void Start()
    {
        originalPosition = transform.localPosition;

        Reset();
    }

    void OnEnable()
    {
        if (pulseOnStart)
            Invoke("Pulse", startDelay);

        if (pulseRepeat)
            InvokeRepeating("Pulse", startDelay, pulseInterval);
    }

    void Reset()
    {
        pulseClock = 0;
        pulseSwitch = false;
        resetSwitch = false;
        //timeLimit = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (pulseSwitch)
        {
            if (pulseClock < timeLimit)
            {
                pulseClock += Time.deltaTime;

                float pulseRatio = pulseClock / pulseTime;

                if (transform.parent == null)
                {
                    positionAdd = transform.forward * -pushScale;
                }
                else
                {
                    positionAdd = Vector3.forward * -pushScale;
                }

                positionAdd *= Mathf.Sin(Mathf.Deg2Rad * (pulseRatio * 180));

                transform.localPosition = originalPosition + positionAdd;
                
            }
            else
            {
                pulseSwitch = false;                
            }
        }
        else
        {
            if (!resetSwitch)
            {
                transform.localPosition = originalPosition;
                resetSwitch = true;
            }
        }
    }

    public void PulseUp()
    {
        originalPosition = transform.localPosition;
        //Debug.Log("PulseUp");
        pulseClock = 0;
        timeLimit = pulseTime / 2;
        pulseSwitch = true;
        resetSwitch = true;
    }

    public void PulseDown()
    {   
        pulseClock = pulseTime / 2;
        timeLimit = pulseTime;
        pulseSwitch = true;
        resetSwitch = false;
    }


    [ContextMenu("Pulse")]
    public void Pulse()
    {
        originalPosition = transform.localPosition;
        pulseClock = 0;
        timeLimit = pulseTime;
        pulseSwitch = true;
        resetSwitch = false;
    }

    public void ResetPulse()
    {
        transform.localPosition = originalPosition;
        pulseClock = 0;
        timeLimit = 0;
        pulseSwitch = false;
        resetSwitch = true;
    }

    private void OnTransformParentChanged()
    {
        if(transform.parent == null)
        {
            originalPosition = transform.localPosition;
        }
    }
}
