using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitSoundEffect
{

    public string eventPath = "event:/QuitSound";
    FmodCarSoundManager QuitEvent;

    public QuitSoundEffect()
    {
        QuitEvent = new FmodCarSoundManager(eventPath);

    }



    public FmodCarSoundManager QuitSound() => QuitEvent;
}
