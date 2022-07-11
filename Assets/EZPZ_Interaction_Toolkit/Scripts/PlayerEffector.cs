//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 11 Jul 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerEffector : MonoBehaviour
{
    public FirstPersonController myController;

    private void Start()
    {
        if(myController == null)
            myController = GetComponent<FirstPersonController>();
    }

    public void ChangeMoveSpeed(float delta)
    {
        myController.MoveSpeed += delta;
    }

    public void SetMoveSpeed(float newSpeed)
    {
        myController.MoveSpeed = newSpeed;
    }
}
