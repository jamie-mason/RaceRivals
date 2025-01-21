using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundWind
{
    public FmodCarSoundManagerSetParameters setEngineParameters;
    private readonly string bankPath = "bank:/Master";
    public readonly string eventPath = "event:/WindBuildingUp";
    public BackgroundWind(){
    }

    public FmodCarSoundManagerSetParameters GetFmodEngineObject() => setEngineParameters;
    
}
