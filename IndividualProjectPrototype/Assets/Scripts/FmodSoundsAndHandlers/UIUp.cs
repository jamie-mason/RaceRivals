using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUp
{
    public string eventPath = "event:/UIUp";
    FmodCarSoundManager uiUpEvent;

    public UIUp()
    {
        uiUpEvent = new FmodCarSoundManager(eventPath);
    }
    public FmodCarSoundManager UIUPSound() => uiUpEvent;

}
