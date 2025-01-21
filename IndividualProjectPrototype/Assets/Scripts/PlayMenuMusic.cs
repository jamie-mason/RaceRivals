using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenuMusic : MonoBehaviour
{
    BackgroundMenuMusic menuMusic;
    void Start()
    {
        menuMusic = new BackgroundMenuMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if(menuMusic == null){
            menuMusic = new BackgroundMenuMusic();

        }
        else{
            if(!menuMusic.GetMenuMusicSound().IsEventPlaying()){
                menuMusic.GetMenuMusicSound().StartEventSound();
            }
        }
    }
    void OnDestroy(){
        menuMusic.GetMenuMusicSound().EndSoundInstance();

    }
}
