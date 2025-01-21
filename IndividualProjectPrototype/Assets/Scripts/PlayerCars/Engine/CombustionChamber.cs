using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombustionChamber
{
    public float volume { get; private set; }
    public float compressionRatio { get; private set; }
    public float maxTemperature { get; private set; }

    public float currentTemperature { get; private set; }
    public float fuelAirMixture { get; private set; }
    public bool isIgnited { get; private set; }


    public float pressure { get; private set; }
    public float fuelConsumption { get; private set; }
    public float exhaustEmissions { get; private set; }


    public CombustionChamber(float volume, float compressionRatio, float maxTemp)
    {
        this.volume = volume;
        this.compressionRatio = compressionRatio;
        maxTemperature = maxTemp;
    }

    public void ignite()
    {
        isIgnited = true;
        pressure = compressionRatio * fuelAirMixture;  // Simulated pressure based on fuel mix and compression
        currentTemperature += 200.0f;  // Increase in temperature due to ignition
        fuelConsumption += fuelAirMixture * 0.01f;  // Example consumption rate per cycle
    }
    public void extinguish()
    {
        isIgnited = false;
        pressure = 0.0f;
    }
    public void intakeFuelAirMix(float fuelAmount, float airAmount)
    {
        fuelAirMixture = fuelAmount / (fuelAmount + airAmount);

    }
    public void performCombustionCycle(float deltaTime)
    {
        if (isIgnited)
        {
            // Simulate temperature rise
            currentTemperature += 50.0f * deltaTime * fuelAirMixture;  // Increase based on fuel richness

            // Calculate pressure and power output
            pressure = compressionRatio * fuelAirMixture * currentTemperature / 300.0f;  // Simplified calculation
            fuelConsumption += fuelAirMixture * deltaTime * 0.05f;  // Fuel used over time

            // Simulate some exhaust emissions
            updateEmissions();
        }
        else
        {
            coolChamber(deltaTime * 5.0f);  // Faster cooling if not ignited
        }
    }
    public float calculatePowerOutput()
    {
        return pressure * volume * 0.1f;  // Simplified formula for power based on pressure and chamber volume

    }

    public void coolChamber(float coolingRate)
    {
        currentTemperature -= coolingRate;
        if (currentTemperature < 25.0f)
        {
            currentTemperature = 25.0f;
        }
    }

    public float calculateEfficiency()
    {
        return (compressionRatio / 10.0f) * fuelAirMixture;  // Simplified efficiency calculation

    }


    public void updateEmissions()
    {
        exhaustEmissions = fuelAirMixture * currentTemperature * 0.001f;  // Simulate emissions based on temp and fuel

    }
}
