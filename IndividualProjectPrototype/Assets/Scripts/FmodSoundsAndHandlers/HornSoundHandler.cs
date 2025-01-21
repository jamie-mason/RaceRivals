using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornSoundHandler
{
    public string eventPath = "event:/CarHorn2";
    FmodCarSoundManager horn;

    public HornSoundHandler()
    {
        horn = new FmodCarSoundManager(this.eventPath);
    }

    public HornSoundHandler(string eventPath)
    {
        this.eventPath = eventPath;
        horn = new FmodCarSoundManager(this.eventPath);
    }
    public FmodCarSoundManager GetFmodHornSound() => horn;

   

}
