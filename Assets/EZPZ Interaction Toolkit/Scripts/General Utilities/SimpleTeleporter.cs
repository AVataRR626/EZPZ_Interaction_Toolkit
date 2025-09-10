//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 26 Jul 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTeleporter : MonoBehaviour, ISubjectRelay
{
    [Header("Target Settings")]
    public Transform subject;
    public Transform destination;
    public bool syncOrientation = false;

    [Header("Cooldown Management")]
    public float activaitonDelay = 0.25f;
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
            ForceTeleport();
        }
        
    }

    public void ForceTeleport()
    {
        Debug.Log("FORCE TELEPORT");
        subject.position = destination.position;

        if (syncOrientation)
            subject.transform.rotation = destination.rotation;

        Physics.SyncTransforms();
    }

    void ISubjectRelay.SyncSubject(GameObject newSubject)
    {
        subject = newSubject.transform;
    }
}
