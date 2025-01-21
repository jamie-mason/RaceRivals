using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectSound
{
    public string eventPath = "event:/UISelect";
    FmodCarSoundManager uiSelectEvent;

    public UISelectSound()
    {
        uiSelectEvent = new FmodCarSoundManager(eventPath);
    }
    public FmodCarSoundManager FmodUISelectSound() => uiSelectEvent;
}
