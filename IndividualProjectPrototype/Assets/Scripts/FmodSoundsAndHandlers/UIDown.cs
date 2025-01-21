using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDown
{
    public string eventPath = "event:/UIDown";
    FmodCarSoundManager uiDownEvent;
    // Start is called before the first frame update

    public UIDown()
    {
        uiDownEvent = new FmodCarSoundManager(this.eventPath);
    }

    public FmodCarSoundManager UIDownSound() => uiDownEvent;
}
