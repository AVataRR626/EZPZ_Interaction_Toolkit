using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollow : MonoBehaviour
{
    public Transform subject;
    

    // Update is called once per frame
    void Update()
    {
        if(subject != null)
        {
            transform.position = subject.position;
        }
    }
}
