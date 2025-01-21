using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSound
{
    public string eventPath = "event:/BackButton";
    FmodCarSoundManager BackEvent;

    public BackSound()
    {
        BackEvent = new FmodCarSoundManager(eventPath);
    }
    public FmodCarSoundManager GetBackSound() => BackEvent;


}
