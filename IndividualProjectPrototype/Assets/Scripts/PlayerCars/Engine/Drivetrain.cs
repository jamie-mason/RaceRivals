using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drivetrain
{
    // Components
    private EngineBlock engineBlock;
    private TorqueConverter torqueConverter;
    private Transmission transmission;
    private RearAxle rearAxle;  // Could be a Differential or Spool
    private PropellerShaft propellerShaft;

    // Reverse flag to track reverse gear
    public bool isReversing = false;

    // Constructor
    public Drivetrain(EngineBlock engineBlock, TorqueConverter torqueConverter, Transmission transmission, RearAxle rearAxle, PropellerShaft propellerShaft)
    {
        this.engineBlock = engineBlock;
        this.torqueConverter = torqueConverter;
        this.transmission = transmission;
        this.rearAxle = rearAxle;
        this.propellerShaft = propellerShaft;
    }

    // Method to enable reverse
    public void EnableReverse(bool reverse)
    {
        isReversing = reverse;
    }

    // Update drivetrain (called every frame or during updates)
    public void UpdateDrivetrain(float throttlePosition, float slipRatio, float deltaTime)
    {
        // Get current RPM from engine (RPM may need to be adjusted based on reverse state)
        float engineRPM = engineBlock.currentRPM;

        // Apply throttle to engine
        engineBlock.throttle(throttlePosition, isReversing);  // Pass the reverse state to throttle
        engineBlock.updateEngineState(Time.deltaTime);

        // If reversing, we need to adjust the RPM and torque directions accordingly
        if (isReversing)
        {
            engineRPM = -engineRPM;  // Reverse RPM direction
        }

        // Process torque from engine to torque converter
        torqueConverter.SetInputRPM(engineRPM);
        var outputRPM = torqueConverter.ApplyTorque(engineBlock.totalPower);

        // Get output RPM from transmission (reverse logic handled in transmission)
        float transmissionRPM = transmission.GetOutputRPM(outputRPM * throttlePosition);  // Pass reverse state to transmission

        // Apply torque to the rear axle (Differential or Spool)
        if (rearAxle is Spool spool)
        {
            // Set torque on the spool (reverse torque direction if in reverse)
            spool.SetInputTorque(isReversing ? -Mathf.Abs(transmissionRPM) : Mathf.Abs(transmissionRPM));
        }
        else if (rearAxle is Differential differential)
        {
            // Apply torque, reverse if needed
            differential.SetInputTorque(isReversing ? -Mathf.Abs(transmissionRPM) : Mathf.Abs(transmissionRPM));
        }

        // Get wheel torques after axle distributes torque (consider reverse torque here as well)
        var wheelTorques = rearAxle.GetWheelTorques();
    }
}
