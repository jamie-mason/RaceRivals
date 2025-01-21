using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class AudioSliderInputfieldPair
{
    Slider slider;
    TMP_InputField inputField;

    public AudioSliderInputfieldPair(Slider s, TMP_InputField inp)
    {
        slider = s;
        inputField = inp;
    }

    Slider GetSlider()
    {
        return slider;
    }

    TMP_InputField GetInputField()
    {
        return inputField;
    }
}
