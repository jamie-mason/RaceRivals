using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsDisplay;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float currentFPS = 1.0f / Time.deltaTime;

        fpsDisplay.text = "FPS: " + Mathf.Ceil(currentFPS).ToString();

    }
}
