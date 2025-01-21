using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeSound
{
    public string eventPath = "event:/ResumeSound";
    FmodCarSoundManager ResumeEvent;
    // Start is called before the first frame update

    public ResumeSound()
    {
        ResumeEvent = new FmodCarSoundManager(eventPath);
    }

    public FmodCarSoundManager GetResumeSound() => ResumeEvent;
}
