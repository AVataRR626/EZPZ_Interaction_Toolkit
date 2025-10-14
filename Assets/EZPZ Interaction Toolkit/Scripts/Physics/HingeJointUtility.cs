//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 11 Oct 2025

using UnityEngine;

public class HingeJointUtility : MonoBehaviour
{
    public HingeJoint myJoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        if(myJoint == null)
            myJoint = GetComponent<HingeJoint>();
    }

    public void SetMotorForce(float force)
    {
        var motor = myJoint.motor;
        motor.force = force;

        myJoint.motor = motor;
    }

    public void SetTargetVelocity(float velocity)
    {
        
        var motor = myJoint.motor;
        motor.targetVelocity = velocity;

        myJoint.motor = motor;
    }

    public void ChangeTargetVelocity(float delta)
    {
        var motor = myJoint.motor;
        motor.targetVelocity += delta;

        myJoint.motor = motor;
    }
}
