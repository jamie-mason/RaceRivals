using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Differential : RearAxle
{
    private float gearRatio;
    private float slipRatio;

    public Differential(float gearRatio, float slipRatio)
    {
        this.gearRatio = gearRatio;
        this.slipRatio = slipRatio;
    }

    // Override the SetInputTorque method for Differential-specific behavior
    public override void SetInputTorque(float inputTorque)
    {
        // Differential logic: torque is split between the left and right wheels based on the gear and slip ratios
        float leftTorque = inputTorque / gearRatio;
        float rightTorque = inputTorque * slipRatio;
        leftWheelTorque = leftTorque;
        rightWheelTorque = rightTorque;
    }
}
