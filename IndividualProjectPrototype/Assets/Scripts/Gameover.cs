using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Gameover : MonoBehaviour
{
    [SerializeField] private GameObject gameoverMenu;
    public TrackPlayerPosition trackPlayerPosition;
    [SerializeField] private TextMeshProUGUI numLapsDispay;
    [SerializeField] private CarController carController;
    [SerializeField] private GameObject restartButton;
    public GameloseSound loseSound { get; private set; }
    public GameLoseZeroLaps zeroLapsLose { get; private set; }

    void Start()
    {
        gameoverMenu.SetActive(false);
        loseSound = new GameloseSound();
        zeroLapsLose = new GameLoseZeroLaps();
        

    }
    public void EndEngineInstances()
    {
        carController.carEngineSound.GetFmodEngineObject().EndSoundInstance();
        trackPlayerPosition.carEngineSound.GetFmodEngineObject().EndSoundInstance();
    }
    // Update is called once per frame
    void Update()
    {
        if (loseSound == null)
        {
            loseSound = new GameloseSound();
        }
        if(zeroLapsLose == null)
        {
            zeroLapsLose = new GameLoseZeroLaps();
        }
        if (trackPlayerPosition.gameover == true)
        {
            Time.timeScale = 0.0f;
            EndEngineInstances();

            if (gameoverMenu.activeSelf == false)
            {
                gameoverMenu.SetActive (true);

                if (trackPlayerPosition.lapCount == 0)
                {
                    numLapsDispay.text = "HAHA LOSER YOU ARE SO BAD AT THIS GAME.";
                    zeroLapsLose.GetZeroLapsLoseSound().StartEventSound();
                }
                else if (trackPlayerPosition.lapCount > 0)
                {
                    numLapsDispay.text = "You Completed: " + trackPlayerPosition.lapCount + " laps";
                    loseSound.FmodGameLoseSound().StartEventSound();

                }
                else
                {
                    zeroLapsLose.GetZeroLapsLoseSound().StartEventSound();
                    numLapsDispay.text = $"YOUR LAP COUNT {trackPlayerPosition.lapCount.ToString()} IS NOT EVEN POSSIBLE";
                }
                EventSystem.current.SetSelectedGameObject(restartButton);
            }
            
        }
    }
}
