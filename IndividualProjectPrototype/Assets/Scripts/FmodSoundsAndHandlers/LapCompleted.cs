using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapCompleted
{
    FmodCarSoundManager Ding;
    public string eventPath = "event:/LapCompleted";

    public LapCompleted()
    {
        Ding = new FmodCarSoundManager(eventPath);
    }

    public FmodCarSoundManager GetFmodLapCompletedSound() => Ding;

}
