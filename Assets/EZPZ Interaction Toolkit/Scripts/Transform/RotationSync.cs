//script for EZPZ Interaction Toolkit
//by Matt Cabanag
//created 26 Nov 2026

using UnityEngine;

public class RotationSync : MonoBehaviour
{
    public Transform goalReference;
    public Transform subject;
    public float alignmentSpeed = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(subject  == null)
            subject = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(goalReference != null)
        {
            subject.rotation = Quaternion.Lerp(transform.rotation, goalReference.rotation, alignmentSpeed * Time.deltaTime);
        }
    }
}
