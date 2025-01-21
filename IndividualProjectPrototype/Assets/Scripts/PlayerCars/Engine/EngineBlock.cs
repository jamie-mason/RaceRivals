using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineBlock
{
    // Engine Specifications
    public float airPressure { get; private set; }
    public float displacement { get; private set; }
    public int cylinderCount { get; private set; }
    public float maxRPM { get; private set; }
    public float idleRPM { get; private set; }

    public float totalPower { get; private set; }
    // Performance Parameters.
    public float currentRPM { get; private set; }
    public float fuelConsumptionRate { get; private set; }
    public float temperature { get; private set; }
    public bool isRunning { get; private set; }

    // Reverse state
    private bool isReversing;

    CombustionChamber[] chambers;

    public EngineBlock(float displacement, int cylinders, float maxRPM, float idleRPM, CombustionChamber combustionChamber)
    {
        this.maxRPM = maxRPM;
        this.idleRPM = idleRPM;
        this.displacement = displacement;
        cylinderCount = cylinders;
        this.currentRPM = 0f;
        chambers = new CombustionChamber[cylinders];
        for (int i = 0; i < chambers.Length; i++)
        {
            chambers[i] = new CombustionChamber(combustionChamber.volume, combustionChamber.compressionRatio, combustionChamber.maxTemperature);
        }
    }

    public void startEngine()
    {
        isRunning = true;
        currentRPM = idleRPM;
        for (int i = 0; i < chambers.Length; i++)
        {
            chambers[i].ignite();
        }
    }

    public void stopEngine()
    {
        isRunning = false;
        currentRPM = 0f;
        for (int i = 0; i < chambers.Length; i++)
        {
            chambers[i].extinguish();
        }
    }

    // Throttle method now handles reverse as well
    public void throttle(float throttlePosition, bool reverse = false)
    {
        if (isRunning)
        {
            throttlePosition = Mathf.Clamp01(throttlePosition);

            if (reverse)
            {
                isReversing = true;
                currentRPM = idleRPM + throttlePosition * (maxRPM - idleRPM);
            }
            else
            {
                isReversing = false;
                if (currentRPM > maxRPM)
                {
                    currentRPM = maxRPM;
                }
                else
                {
                    currentRPM = idleRPM + throttlePosition * (maxRPM - idleRPM);  // Smooth transition for forward
                }
            }

            // Simulate intake fuel and air mix based on throttle
            for (int i = 0; i < chambers.Length; i++)
            {
                chambers[i].intakeFuelAirMix(throttlePosition * 10.0f, 10.0f);  // Intake more fuel with higher throttle
            }
        }
    }

    public void updateEngineState(float deltaTime)
    {
        float totalPower = 0.0f;
        for (int i = 0; i < chambers.Length; i++)
        {
            chambers[i].performCombustionCycle(deltaTime);  // Update each chamber’s state
            totalPower += chambers[i].calculatePowerOutput();  // Sum up power from each chamber
        }
        this.totalPower = totalPower;
        fuelConsumptionRate = totalPower * 0.01f;
        temperature += totalPower * deltaTime * 0.05f;

        if (temperature > 250.0f)
        {
            temperature = 250.0f; // Simulate overheating limit
        }
    }

    public float calculateFuelConsumption()
    {
        float totalFuel = 0.0f;
        for (int i = 0; i < chambers.Length; i++)
        {
            totalFuel += chambers[i].fuelConsumption;
        }

        return totalFuel;
    }

    public void coolEngine(float coolingRate)
    {
        temperature -= coolingRate;
        for (int i = 0; i < chambers.Length; i++)
        {
            chambers[i].coolChamber(coolingRate / chambers.Length);
        }

        if (temperature < 30.0f)
        {
            temperature = 30.0f;  // Prevent cooling below ambient temperature
        }
    }

    // Reverse helper method
    public bool IsReversing()
    {
        return isReversing;
    }
}
