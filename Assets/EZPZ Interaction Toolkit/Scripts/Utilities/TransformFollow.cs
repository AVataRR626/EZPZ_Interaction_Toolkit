using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollow : MonoBehaviour
{
    public Transform subject;
    public float positionSyncRate = 3;

    public Quaternion startRotation;
    public float yRotation;
    public bool autoSyncYRotation = false;
    public float rotationSyncRate = 0.1f;

    private void Start()
    {
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        SyncPosGradual();

        if (autoSyncYRotation)
            SyncYRotationGradual();
    }

    public void SyncPosGradual()
    {
        if(subject != null)
            transform.position = Vector3.Lerp(transform.position, subject.position, positionSyncRate * Time.deltaTime);
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

    public void ToggleAutoSyncYRotation()
    {
        autoSyncYRotation = !autoSyncYRotation;
    }
    
    public void SetAutoSyncYRotation(bool n)
    {
        autoSyncYRotation = n;
    }
}
