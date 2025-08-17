//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 18 Aug 2025

using UnityEngine;
using UnityEngine.Events;

public class CooldownGate : MonoBehaviour
{
    public float cooldownTime;
    public float cooldownClock;
    public UnityEvent onSuccessfulExecute;
    public UnityEvent onCooldownBlocked;
    public bool resetOnFail = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldownClock > 0)
        {
            cooldownClock -= Time.deltaTime;
        }
    }

    public void AttemptExecute()
    {
        ExecuteAttempt();
    }

    public void ExecuteAttempt()
    {
        if (cooldownClock <= 0)
        {
            onSuccessfulExecute.Invoke();
            cooldownClock = cooldownTime;
        }
        else
        {
            onCooldownBlocked.Invoke();

            if (resetOnFail)
                cooldownClock = cooldownTime;
        }
    }

}
