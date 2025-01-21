using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueConverter
{
    private float inputRPM; // Engine RPM
    private float inputTorque; // Engine Torque
    private float outputRPM; // RPM delivered to the transmission
    private float outputTorque; // Torque delivered to the transmission
    private float efficiency; // Efficiency of the torque converter (0 to 1)

    // Simulating the slip ratio between engine and transmission
    private float slipRatio; // Value between 0 (no slip) and 1 (maximum slip)

    public TorqueConverter(float efficiency = 0.85f, float slipRatio = 0.2f)
    {
        this.efficiency = efficiency;
        this.slipRatio = slipRatio;
    }

    // Method to set the engine RPM input to the torque converter
    public void SetInputRPM(float rpm)
    {
        inputRPM = rpm;
        // Output RPM is reduced based on the slip ratio; the higher the slip, the lower the RPM to the transmission
        outputRPM = inputRPM * (1 - slipRatio);
    }

    // Apply torque based on the input torque (from engine)
    public float ApplyTorque(float torque)
    {
        inputTorque = CalculateTorque(torque);
        outputTorque = inputTorque * efficiency;


        // Adjust engine RPM based on the torque conversion
        return outputRPM;
    }

    // Method to calculate torque based on engine RPM (simplified version)
    private float CalculateTorque(float power)
    {
        // Simplified torque calculation (e.g., torque is proportional to engine RPM and a fixed factor)
        float enginePower = power; // Just an example calculation for engine power
        return enginePower;
    }

    // For debugging: Show the current slip ratio
    public float GetSlipRatio()
    {
        return slipRatio;
    }

    // Method to simulate the behavior of the torque converter during acceleration or load changes
    public void UpdateSlip(float load)
    {
        // The slip ratio might increase with higher load (as more slippage occurs)
        slipRatio = Mathf.Clamp(slipRatio + load * 0.01f, 0, 1);
        Debug.Log($"Torque Converter: Updated Slip Ratio = {slipRatio}");
    }
}
