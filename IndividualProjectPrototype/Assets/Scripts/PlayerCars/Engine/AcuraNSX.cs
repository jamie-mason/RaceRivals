using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcuraNSX
{
    // Components of the Acura NSX drivetrain
    public EngineBlock engineBlock { get; private set; }
    public TorqueConverter torqueConverter { get; private set; }
    public Transmission transmission { get; private set; }
    public RearAxle rearAxle { get; private set; }
    public PropellerShaft propellerShaft { get; private set; }
    public Drivetrain drivetrain { get; private set; }

    // Car-specific properties (e.g., performance metrics)
    public float throttlePosition { get; private set; }
    public float currentSpeed { get; private set; }
    public float currentRPM { get; private set; }

    // Constructor to initialize the Acura NSX with drivetrain components
    public AcuraNSX()
    {
        // Define the components
        engineBlock = new EngineBlock(3.5f, 6, 8000f, 800f, new CombustionChamber(2.5f, 10f, 900f));
        torqueConverter = new TorqueConverter(0.9f, 0.2f);
        transmission = new Transmission(6, 3.5f); // 6-speed transmission with a 3.5 gear ratio
        rearAxle = new Spool(); // Example differential values
        propellerShaft = new PropellerShaft();

        // Initialize drivetrain with all components
        drivetrain = new Drivetrain(engineBlock, torqueConverter, transmission, rearAxle, propellerShaft);
    }

    // Method to simulate throttle input (accelerating the car)
    public void SetThrottle(float throttle)
    {
        throttlePosition = Mathf.Clamp01(Mathf.Abs(throttle)); // Ensure throttle is between 0 and 1
        if (throttle < 0)
        {
            drivetrain.isReversing = true;
        }
        else
        {
            drivetrain.isReversing = false;
        }
        engineBlock.throttle(throttlePosition);
    }

    // Method to update the Acura NSX state each frame
    public void UpdateCar(float deltaTime)
    {
        // Update the drivetrain with the current throttle input and deltaTime
        drivetrain.UpdateDrivetrain(throttlePosition, torqueConverter.GetSlipRatio(), deltaTime);

        // Optionally, update car speed, engine RPM, or other attributes
        currentSpeed = CalculateSpeedFromTorque();
        currentRPM = engineBlock.currentRPM;

        // You can add more detailed behavior here, like handling, braking, etc.
    }

    // Helper method to calculate car speed from wheel torque (simplified)
    private float CalculateSpeedFromTorque()
    {
        // Simplified: Calculate speed based on wheel torque and a constant factor
        var wheelTorques = rearAxle.GetWheelTorques();
        float averageTorque = (wheelTorques.leftWheelTorque + wheelTorques.rightWheelTorque) / 2.0f;
        return averageTorque * 0.05f; // Arbitrary constant for simulation
    }

    // Optional: Car-specific behaviors (e.g., braking, turning, etc.)
}
