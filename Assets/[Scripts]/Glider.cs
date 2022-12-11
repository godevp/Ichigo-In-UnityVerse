using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 
 Source file Name - Glider.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 11/20/2022 
 Program description: script for ice platform

 */
public class Glider : MonoBehaviour
{
    private HingeJoint2D hingeJoint2D;
    private bool max = false;
    void Start()
    {
        hingeJoint2D = GetComponent<HingeJoint2D>();
    }


    void Update()
    {

        if ((hingeJoint2D.jointAngle <= hingeJoint2D.limits.max && !max) || (hingeJoint2D.jointAngle >= hingeJoint2D.limits.min && max))
        {
            max = !max;
            var hj = hingeJoint2D.motor.motorSpeed;
            hj *= -1;
            JointMotor2D motor = new JointMotor2D();
            motor.motorSpeed = hj;
            motor.maxMotorTorque = 40;
            hingeJoint2D.motor = motor;
        }
        
    }
}
