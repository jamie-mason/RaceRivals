using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireVibration
{

    public string eventPath = "event:/TireVibrations";

    private FmodCarSoundManagerSetParameters setVibrationParameters;
    
    public TireVibration(){

    }
    public FmodCarSoundManagerSetParameters GetFmodEngineObject() => setVibrationParameters;
}
