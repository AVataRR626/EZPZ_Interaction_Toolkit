using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : InteractableGeneral
{
    public bool moving = false;

    public void RotateY(float angle)
    {
        transform.Rotate(0, angle, 0);
    }

}