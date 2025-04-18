using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraUtil : MonoBehaviour
{
    public Camera subject;
    public float outputRatio = 1;


    // Start is called before the first frame update
    void Start()
    {
        if (subject == null)
            subject = GetComponent<Camera>();
        subject.aspect = outputRatio;

        
    }

    // Update is called once per frame
    void Update()
    {
        subject.aspect = outputRatio;



    }
}
