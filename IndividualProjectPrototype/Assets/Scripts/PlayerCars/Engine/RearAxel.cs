using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearAxle
{
    // Common properties for both Spool and Differential
    public float leftWheelTorque { get; protected set; }
    public float rightWheelTorque { get; protected set; }

    // Method to be overridden by children (Differential and Spool)
    public virtual void SetInputTorque(float inputTorque)
    {
        // Default implementation, can be left empty or have some default behavior
    }

    // Method to get wheel torques
    public (float leftWheelTorque, float rightWheelTorque) GetWheelTorques()
    {
        return (leftWheelTorque, rightWheelTorque);
    }
}
