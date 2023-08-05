using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : InteractableGeneral
{
    public bool groundPlace = true;
    public bool moving = false;
    public float throwForce = 0;

    public void RotateY(float angle)
    {
        transform.Rotate(0, angle, 0);
    }

}