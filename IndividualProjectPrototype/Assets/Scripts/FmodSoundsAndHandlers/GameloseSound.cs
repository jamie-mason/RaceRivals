using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameloseSound
{
    public string eventPath = "event:/GameLoseSound";
    FmodCarSoundManager GameLostEvent;
    // Start is called before the first frame update

    public GameloseSound()
    {
        GameLostEvent = new FmodCarSoundManager(eventPath);
    }

    public FmodCarSoundManager FmodGameLoseSound() => GameLostEvent;
}
