//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 10 Mar 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUtility : MonoBehaviour
{

    public void RotateAxis(Vector3 axis, float angle)
    {
        //transform.Rotate(axis, angle * Mathf.Deg2Rad);
        transform.Rotate(axis, angle);
    }

    public void RotateAxisX(float angle)
    {
        RotateAxis(Vector3.right, angle);
    }

    public void RotateAxisY(float angle)
    {
        RotateAxis(Vector3.up, angle);
    }

    public void RotateAxisZ(float angle)
    {
        RotateAxis(Vector3.forward, angle);
    }

    public void MoveUp(float distance)
    {
        transform.position += new Vector3(0, distance, 0);
    }
    

    public void MovePosDelta(Vector3 delta)
    {
        transform.position += delta;
    }

    public void ScaleUniformDelta(float delta)
    {
        transform.localScale *= (1 + delta);
    }

    public void MoveRight(float distance)
    {
        transform.position += transform.right * distance;
    }

    public void MoveLeft(float distance)
    {
        transform.position += transform.right * -distance;
    }

}
