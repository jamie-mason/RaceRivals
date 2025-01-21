using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartSoundEffect 
{
    public string eventPath = "event:/RestartSound";
    FmodCarSoundManager RestartEvent;
    public RestartSoundEffect()
    {
        RestartEvent = new FmodCarSoundManager(eventPath);

    }

    public FmodCarSoundManager RestartSound() => RestartEvent;
}
