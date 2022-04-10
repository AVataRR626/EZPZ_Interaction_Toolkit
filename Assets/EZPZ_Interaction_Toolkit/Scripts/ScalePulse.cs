/*BounceBlaster Project
QSI Util
ScalePulse.cs

Can be used for beating hearts, juciness effects for touch, etc.

-Matt Cabanag; 2 Oct 2016
*/

using UnityEngine;
using System.Collections;

public class ScalePulse : MonoBehaviour
{
    public float pulseTime = 0.25f;
    public Vector3 scaleFactor = new Vector3(0.15f, 0.15f, 1);
    public Vector3 scaleAdd;
    public bool pulseOnStart = false;
    public float startDelay = 2;
    public bool pulseRepeat = false;
    public float pulseInterval = 3;

    private Vector3 originalScale;
    private float pulseClock = 0;   
    private bool pulseSwitch = false;
    private float timeLimit;
    private bool resetSwitch = false;

	// Use this for initialization
	void Start ()
    {
        originalScale = transform.localScale;

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
        if(pulseSwitch)
        { 
            if (pulseClock < timeLimit)
            {
                pulseClock += Time.deltaTime;

                float pulseRatio = pulseClock / pulseTime;

                scaleAdd = scaleFactor;
                scaleAdd *= Mathf.Sin(Mathf.Deg2Rad * (pulseRatio * 180));

                transform.localScale = originalScale + scaleAdd;
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
                transform.localScale = originalScale;
                resetSwitch = true;
            }
        }
    }

    public void PulseUp()
    {
        //Debug.Log("PulseUp");
        pulseClock = 0;
        timeLimit = pulseTime/2;
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
        pulseClock = 0;
        timeLimit = pulseTime;
        pulseSwitch = true;
        resetSwitch = false;
    }
}
