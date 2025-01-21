using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerShaft
{
    public float inputTorque { get; private set; }
    public float outputTorque { get; private set; }
    public float rotationalSpeed { get; private set; }

    // A simple constructor that initializes the propeller shaft with some parameters
    public PropellerShaft()
    {
        inputTorque = 0f;
        outputTorque = 0f;
        rotationalSpeed = 0f;
    }

    // Method to receive torque from the transmission and pass it to the rear axle
    public void TransferTorque(float inputTorque, float inputRPM)
    {
        this.inputTorque = inputTorque;

        // Calculate output torque based on some mechanical efficiency loss or gain (if any)
        outputTorque = inputTorque;  // You can introduce efficiency losses here if needed

        // Calculate the rotational speed (RPM) at the propeller shaft, assuming no losses
        rotationalSpeed = inputRPM;  // In a real scenario, the shaft might affect the RPM slightly

        // Debugging information
        Debug.Log($"Propeller Shaft: Input Torque = {inputTorque} Nm, Output Torque = {outputTorque} Nm, Rotational Speed = {rotationalSpeed} RPM");
    }
}
