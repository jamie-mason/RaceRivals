using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderPrintIncrementValueModificationForPreviews : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI ValuePrintPreview;
    [SerializeField] private TextMeshProUGUI ValuePrintMenu;
    [SerializeField] private string textAfter;
    [SerializeField] private string textBefore;
    [SerializeField] private float IncrementMultiplier = 1f;
    [SerializeField] private float IncrementDivider = 1f;
    [SerializeField] private bool printAsPercentage;

    bool OnStart;
    PauseGame pauseGame;

    public float value{get; set;}
    
    void Awake(){
        pauseGame = FindObjectOfType<PauseGame>();
    }
    void Start()
    {
        OnStart = true;
        slider.onValueChanged.AddListener(UpdateTextValues);
    }

    public void SetSliderValues(){
        slider.value = value * IncrementDivider / IncrementMultiplier;
    }
    public void UpdateTextValues(){
        if(slider!=null){
            if(ValuePrintPreview!=null){
                value = slider.value * IncrementMultiplier / IncrementDivider;
                float percentage = Mathf.Round(value * 100f);

                if(printAsPercentage){
                    ValuePrintPreview.text = textBefore + percentage.ToString() + "%" + textAfter;
                }
                else{
                    ValuePrintPreview.text = textBefore + value.ToString() + textAfter;
                }
                if(ValuePrintMenu != null){
                    ValuePrintMenu.text = ValuePrintPreview.text;
                }
            }
            
        }
    }
    public void UpdateTextValues(float value){
        if(slider!=null){
            if(ValuePrintPreview!=null){
                value = slider.value;
                float percentage = Mathf.Round(value * 100f);

                if(printAsPercentage){
                    percentage = percentage * IncrementMultiplier / IncrementDivider;
                    ValuePrintPreview.text = textBefore + percentage.ToString() + "%" + textAfter;
                }
                else{
                    value = value * IncrementMultiplier / IncrementDivider;
                    ValuePrintPreview.text = textBefore + value.ToString() + textAfter;
                }
                if(ValuePrintMenu != null){
                    ValuePrintMenu.text = ValuePrintPreview.text;
                }
            }
            
        }
    }

    public float getIncrementMultiplier()=>IncrementMultiplier;
    public float getIncrementDivider()=>IncrementDivider;
    
    void Update()
    {
        if(OnStart){
            SetSliderValues();
            OnStart = false;
        }
        else{
            value = slider.value * IncrementMultiplier / IncrementDivider;
        }
    }
}
