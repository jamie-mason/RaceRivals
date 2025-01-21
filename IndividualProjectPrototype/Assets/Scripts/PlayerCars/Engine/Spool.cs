using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spool : RearAxle
{
    // Constructor for Spool (no gear ratio needed as torque is evenly distributed)
    public Spool()
    {
        // No additional initialization needed
    }

    // Override the SetInputTorque method to simulate spool torque distribution
    public override void SetInputTorque(float inputTorque)
    {
        // A spool splits torque evenly between the two wheels
        leftWheelTorque = inputTorque;
        rightWheelTorque = inputTorque;
    }
}
