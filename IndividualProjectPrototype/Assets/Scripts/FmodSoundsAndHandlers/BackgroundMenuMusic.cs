using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMenuMusic
{
    public string eventPath = "event:/MenuMusic";
    FmodCarSoundManager MenuMusic;

    public BackgroundMenuMusic()
    {
        MenuMusic = new FmodCarSoundManager(eventPath);
    }
    public FmodCarSoundManager GetMenuMusicSound() => MenuMusic;

}
