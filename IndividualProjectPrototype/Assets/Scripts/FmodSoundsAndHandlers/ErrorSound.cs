using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorSound
{
    public string eventPath = "event:/ErrorSound";
    FmodCarSoundManager ErrorEvent;

    public ErrorSound()
    {
        ErrorEvent = new FmodCarSoundManager(eventPath);
    }
    public FmodCarSoundManager GetErrorSound() => ErrorEvent;
}
