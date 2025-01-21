using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngineSoundHandler
{
    private float currentSpeed;
    private readonly float maxSpeed;
    private FmodCarSoundManagerSetParameters setEngineParameters;
    public BackgroundWind backgroundWind;
    private readonly string parameterName = "InstrumentIndex";
    private readonly string bankPath = "bank:/Master";
    private readonly string eventPath = "event:/CarEngineAudio";

    public CarEngineSoundHandler(float maxSpeed)
    {

        this.maxSpeed = maxSpeed;
        currentSpeed = 0f;
        setEngineParameters = new FmodCarSoundManagerSetParameters(eventPath, parameterName, 0f, 7f);
        backgroundWind = new BackgroundWind();
        backgroundWind.setEngineParameters = new FmodCarSoundManagerSetParameters(backgroundWind.eventPath, parameterName, 0f, 7f); 
    }

    public FmodCarSoundManagerSetParameters GetFmodEngineObject() => setEngineParameters;


    public float CurrentSpeed
    {
        get => currentSpeed;
        set => currentSpeed = Mathf.Clamp(value, 0, maxSpeed);
    }

    public float MaxSpeed => maxSpeed;

    private float GetSpeedFraction() => Mathf.Clamp01(currentSpeed / maxSpeed);

    private bool HasSignificantDifference(float a, float b, float tolerance = 0.01f)
    {
        return Mathf.Abs(a - b) > tolerance;
    }

    public void UpdateCarEngineSound()
    {
        float speedFraction = GetSpeedFraction();
        float targetParameterValue = speedFraction * setEngineParameters.GetMaxParameterRange();

        if (HasSignificantDifference(setEngineParameters.GetContinuousParameterValue(), targetParameterValue))
        {
            setEngineParameters.SetContinuousValue(targetParameterValue);
            backgroundWind.setEngineParameters.SetContinuousValue(targetParameterValue);
        }
    }

}
