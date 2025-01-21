using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    
    public void ChangeScene(string sceneName){
        SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
    }
    public void ChangeScene(int sceneID){
        SceneManager.LoadScene(sceneID,LoadSceneMode.Single);
    }
    public void ReloadCurrentScene(){
        string thisScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(thisScene,LoadSceneMode.Single);
    }
    public void QuitButtonChangeScene(string sceneName)
    {
        var gameOver = FindObjectsOfType<Gameover>();
        if (gameOver != null)
        {
            foreach (Gameover gameover in gameOver)
            {
                gameover.loseSound.FmodGameLoseSound().EndSoundInstance();
                gameover.zeroLapsLose.GetZeroLapsLoseSound().EndSoundInstance();
                gameover.EndEngineInstances();

            }
        }
        QuitSoundEffect quitSound = new QuitSoundEffect();
        quitSound.QuitSound().StartEventSound();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    public void QuitButtonChangeScene(int sceneID)
    {
        var gameOver = FindObjectsOfType<Gameover>();
        if (gameOver != null)
        {
            foreach (Gameover gameover in gameOver)
            {
                gameover.loseSound.FmodGameLoseSound().EndSoundInstance();
                gameover.zeroLapsLose.GetZeroLapsLoseSound().EndSoundInstance();
                gameover.EndEngineInstances();
            }
        }
        QuitSoundEffect quitSound = new QuitSoundEffect();
        quitSound.QuitSound().StartEventSound();
        SceneManager.LoadScene(sceneID, LoadSceneMode.Single);
    }
    public void RestartButtonReloadCurrentScene()
    {
        RestartSoundEffect restartSound = new RestartSoundEffect();
        restartSound.RestartSound().StartEventSound();
        string thisScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(thisScene, LoadSceneMode.Single);
    }

    

}
