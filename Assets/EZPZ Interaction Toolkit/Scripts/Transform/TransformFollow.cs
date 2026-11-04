//EZPZ Interaction Toolkit
//by Matt Cabanag
//Created - don't remember when, some time in 2022-2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollow : MonoBehaviour
{
    [Header("Position Settings")]
    public Transform subject;
    public string defaultSubjectTag = "Player";
    public float positionSyncRate = 3;
    public Vector3 offset;
    public bool autoSyncPosition = true;

    [Header("General Rotation Settings (prioritised)")]
    [Tooltip("Full rotation settings are prioritised over Y rotation settings")]
    public bool autoSyncRotation = false;
    public float rotationSyncRate = 0.1f;
    public float rotationDeltaThreshold = 15f;

    [Header("Y Rotation Settings")]    
    [Tooltip("If you want this active, make sure full rotation syncing is disabled")]
    public bool autoSyncYRotation = false;
    public float yRotation;





    private void Start()
    {
        if (subject == null)
        {
            GameObject candidate = GameObject.FindGameObjectWithTag(defaultSubjectTag);
            if (candidate != null)
                subject = candidate.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(autoSyncPosition)
            SyncPosGradual();

        if(autoSyncRotation)
        {
            SyncRotationGradual();
        }
        else if (autoSyncYRotation)
        {
            if (Vector3.Angle(subject.forward, transform.forward) >= rotationDeltaThreshold)
            {
                SyncYRotationGradual();
            }
        }
    }

    public void SyncPosGradual()
    {
        if(subject != null)
            transform.position = Vector3.Lerp(transform.position, subject.position + offset, positionSyncRate * Time.deltaTime);
    }

    public void SyncRotation()
    {
        transform.rotation = subject.rotation;
    }

    public void SyncYRotation()
    {
        yRotation = subject.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void SyncYRotationGradual()
    {
        Quaternion subjectRotation = subject.rotation;
        Quaternion currentRotation = transform.rotation;
        Quaternion goalRotation = Quaternion.Lerp(currentRotation, subjectRotation, rotationSyncRate * Time.deltaTime);
        yRotation = goalRotation.eulerAngles.y;

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void SyncRotationGradual()
    {
        Quaternion subjectRotation = subject.rotation;
        Quaternion currentRotation = transform.rotation;
        Quaternion goalRotation = Quaternion.Lerp(currentRotation, subjectRotation, rotationSyncRate * Time.deltaTime);

        transform.rotation = goalRotation;
    }

    public void ToggleAutoSyncPosition()
    {
        autoSyncPosition = !autoSyncPosition;
    }
    public void SetAutoSyncPosition(bool n)
    {
        autoSyncPosition = n;
    }

    public void ToggleAutoSyncYRotation()
    {
        autoSyncYRotation = !autoSyncYRotation;
    }
    
    public void SetAutoSyncYRotation(bool n)
    {
        autoSyncYRotation = n;
    }

    public void SoftStop()
    {
        SyncDestination();
    }

    public void SyncDestination()
    {
        if(subject != null)
        {
            subject.transform.position = transform.position;
        }
    }
}
