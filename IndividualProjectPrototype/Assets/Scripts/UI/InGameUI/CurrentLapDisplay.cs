using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentLapDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentLapTimeText;
    [SerializeField] private TextMeshProUGUI bestLapTimeText;
    [SerializeField] private TextMeshProUGUI lapsCompletedText;
    string currentLabel, bestLabel;
    [SerializeField] private TrackPlayerPosition trackPlayerPosition;
    float currentTime;
    float lastBestLap;
    
    void Start()
    {
        lastBestLap = trackPlayerPosition.bestLapTime;
        bestLabel = bestLapTimeText.text;
        bestLapTimeText.gameObject.SetActive(false);
        currentTime = trackPlayerPosition.currentTime;
        currentLabel = currentLapTimeText.text; 
    }

    void Update()
    {
        
        currentTime = trackPlayerPosition.currentTime;

        

        // Optional: Format as a string

        currentLapTimeText.text = currentLabel + getTimeFormat(currentTime);
        if(trackPlayerPosition.bestLapTime != 0f && !bestLapTimeText.gameObject.activeSelf)
        {
            bestLapTimeText.gameObject.SetActive(true);
            bestLapTimeText.text = bestLabel + getTimeFormat(trackPlayerPosition.bestLapTime);
        }
        else if (bestLapTimeText.gameObject.activeSelf)
        {
            bestLapTimeText.text = bestLabel + getTimeFormat(trackPlayerPosition.bestLapTime);
        }

        lapsCompletedText.text = "Laps Completed: " + trackPlayerPosition.lapCount.ToString();

        lastBestLap = trackPlayerPosition.bestLapTime;
    }

    string getTimeFormat(float floatTime)
    {
        int minutes = Mathf.FloorToInt(floatTime / 60F); // Get the total minutes
        int seconds = Mathf.FloorToInt(floatTime % 60F); // Get the remaining seconds

        string timeFormatted = string.Format("{0:00}:{1:00}", minutes, seconds);

        return timeFormatted;

    }
}
