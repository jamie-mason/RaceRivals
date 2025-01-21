using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[System.Serializable]
public class GameData
{
    public float BackgroundMusicVolume;

    SerializableInputBinding SerializableInputBinding;
    public List<string> xboxOverridePaths;
    public List<string> keyOverridePaths;

    public float baseFOV;
    public float maxFOV;
    public float xCameraSensitivity;
    public float yCameraSensitivity;
    public float distance;
    public float height;
    public float horrizontalOffset;

    public bool freelookOnly;
    public bool chaseOnly;

    public bool InvertXFreelook;

    public bool InvertYFreelook;

   
    //the values in the constructor will be the intitial starting values
    //the game starts with these values when there are no values to load
    public GameData()
    {

        xboxOverridePaths = new List<string>();
        baseFOV = 90f;
        maxFOV = 120f;
        xCameraSensitivity = 1f;
        yCameraSensitivity = 1f;
        horrizontalOffset = 0f;
        height = 4;
        distance = 20;
        chaseOnly = false;
        freelookOnly = false;
        keyOverridePaths = new List<string>();
    }
    
}
