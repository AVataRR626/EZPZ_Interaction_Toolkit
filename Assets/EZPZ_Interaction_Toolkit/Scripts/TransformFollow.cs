using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollow : MonoBehaviour
{
    public Transform subject;

    public Quaternion startRotation;
    public float yRotation;

    private void Start()
    {
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(subject != null)
        {
            transform.position = subject.position;
        }
    }

    public void SyncYRotation()
    {
        yRotation = subject.rotation.eulerAngles.y;

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
