//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 26 Jul 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTeleporter : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform subject;
    public Transform destination;
    public float activaitonDelay = 0.25f;

    [Header("Cooldown Management")]
    public float cooldown = 0.25f;
    public float cooldownClock;

    private void FixedUpdate()
    {
        if (cooldownClock > 0)
            cooldownClock -= Time.fixedDeltaTime;
        else
            cooldownClock = 0;
    }

    public void Teleport()
    {
        if (cooldownClock <= 0)
        {
            Debug.Log("Teleporting! " + name);            
            cooldownClock = cooldown;
            Invoke("ForceTeleport", activaitonDelay);
        }
        
    }

    public void ForceTeleport()
    {
        subject.position = destination.position;
    }
}
