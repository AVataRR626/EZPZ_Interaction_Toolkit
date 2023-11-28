using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FreezeRotation : MonoBehaviour
{
    public Quaternion freezeAngle;

    // Start is called before the first frame update
    void Start()
    {
        freezeAngle = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = freezeAngle;
    }
}
