using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoseZeroLaps
{
    string eventPath = "event:/Terrible";
    FmodCarSoundManager zeroLapsLose;
    public GameLoseZeroLaps()
    {
        zeroLapsLose = new FmodCarSoundManager(eventPath);
    }
    public FmodCarSoundManager GetZeroLapsLoseSound() => zeroLapsLose;

}
