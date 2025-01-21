using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmission
{
    public int gearCount { get; private set; }
    public int currentGear { get; private set; }
    public float[] gearRatios;  // Array to store gear ratios for each gear

    // Constructor with flexible parameters
    public Transmission(int gearCount, float[] gearRatios = null, int initialGear = 1)
    {
        if (gearRatios == null)
        {
            // Default gear ratios (fallback if no array is provided)
            this.gearCount = gearCount;
            this.gearRatios = new float[gearCount];

            // Example: Default gear ratios (you can tweak or extend this formula as needed)
            for (int i = 0; i < gearCount; i++)
            {
                this.gearRatios[i] = 3.0f + i * 0.5f;  // Example: Increasing gear ratio with each gear
            }
        }
        else
        {
            // If gear ratios are provided, use them
            this.gearCount = gearRatios.Length;
            this.gearRatios = new float[gearCount];
            gearRatios.CopyTo(this.gearRatios, 0);
        }

        // Set the initial gear (ensure it's within bounds)
        currentGear = Mathf.Clamp(initialGear, 1, gearCount);
    }

    public Transmission(int gearCount, float gearRatio, float[] gearRatios = null, int initialGear = 1)
    {
        if (gearRatios == null)
        {
            // Default gear ratios (fallback if no array is provided)
            this.gearCount = gearCount;
            this.gearRatios = new float[gearCount];

            // Example: Default gear ratios (you can tweak or extend this formula as needed)
            for (int i = 0; i < gearCount; i++)
            {
                this.gearRatios[i] = gearRatio + i * 0.5f;  // Example: Increasing gear ratio with each gear
            }
        }
        else
        {
            // If gear ratios are provided, use them
            this.gearCount = gearRatios.Length;
            this.gearRatios = new float[gearCount];
            gearRatios.CopyTo(this.gearRatios, 0);
        }

        // Set the initial gear (ensure it's within bounds)
        currentGear = Mathf.Clamp(initialGear, 1, gearCount);
    }

    // Method to shift up a gear
    public void ShiftUp()
    {
        if (currentGear < gearCount)
        {
            currentGear++;
        }
    }

    // Method to shift down a gear
    public void ShiftDown()
    {
        if (currentGear > 1)
        {
            currentGear--;
        }
    }

    // Method to calculate the output RPM based on the input RPM and current gear
    public float GetOutputRPM(float inputRPM)
    {
        // Adjust the output RPM based on the gear ratio for the current gear
        return inputRPM;  // Use current gear's ratio to adjust RPM
    }
}
