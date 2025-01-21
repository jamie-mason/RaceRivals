using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSound
{
    public string eventPath = "event:/PauseSound";
    FmodCarSoundManager PauseEvent;
    // Start is called before the first frame update

    public PauseSound()
    {
        PauseEvent = new FmodCarSoundManager(eventPath);
    }

    public FmodCarSoundManager GetPauseSound() => PauseEvent;
}
