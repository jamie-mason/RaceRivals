using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class AudioSettingsUIHandler : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_InputField inputValue;
    [SerializeField] private AudioMixerGroup backgroundMusic;
    [SerializeField] private PauseGame pauseGame;

    [SerializeField] private AudioSliderInputfieldPair backgroundMusicSettings;
    float lastSliderValue;
    private void Start()
    {

        if(lastSliderValue == 0f)
        {
            lastSliderValue = slider.value;
        }
        else
        {
            slider.value = lastSliderValue;
        }
        float dB = Mathf.Log10(lastSliderValue) * 20;
        backgroundMusic.audioMixer.SetFloat("BackgroundMusicVolume", dB);
        setInputText();
        inputValue.onEndEdit.AddListener(CopyInputValuesToSlider);
        slider.onValueChanged.AddListener(CopySliderValuesToInput);
    }
    
    public void LoadData(GameData data)
    {
        this.lastSliderValue = data.BackgroundMusicVolume;
       
    }
    public void SaveData(ref GameData data)
    {
        data.BackgroundMusicVolume = this.lastSliderValue;
    }
    void setInputText()
    {
        float roundedSliderValue = Mathf.Round(slider.value * 1000f) /10f;
        inputValue.text = roundedSliderValue.ToString() + "%";
    }
    float FormatInputValue()
    {
        float temp;
        float sliderValue;
        float.TryParse(inputValue.text, out temp);
        temp = Mathf.Clamp(temp, 0f, 100f);
        float dec = temp / 100f;
        dec = Mathf.Clamp01(dec);
        sliderValue = dec;
        dec = Mathf.Round(dec * 1000f)/10f;
        inputValue.text = dec.ToString() + "%";
        return sliderValue;

    }

    private void CopyInputValuesToSlider(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            setInputText();
        }
        else
        {
            slider.value = FormatInputValue();
        }
    }
    private void CopySliderValuesToInput(float volume)
    {
        setInputText();
        lastSliderValue = slider.value;
        
       
      
    }
    private void Update()
    {
        if (slider.gameObject.activeInHierarchy)
        {
            lastSliderValue = slider.value;
            if (backgroundMusic != null)
            {
                float dB = Mathf.Log10(lastSliderValue) * 20; //convert from 0-1 ti -80 to 0

                backgroundMusic.audioMixer.SetFloat("BackgroundMusicVolume", dB);
            }
        }
        

    }
   



}
