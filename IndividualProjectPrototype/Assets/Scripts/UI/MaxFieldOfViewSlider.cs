using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class MaxFieldOfViewSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI ValuePrintMenu;
    [SerializeField] private TextMeshProUGUI ValuePrintPreview;
    private float baseFOV;
    [SerializeField] private SliderPrintIncrementValueModificationForPreviews baseFOVSliderValue;


    

    public float value{get;private set;}

    void Start()
    {
        UpdateTextValues();
        slider.onValueChanged.AddListener(UpdateTextValues);
    }

    public void UpdateTextValues(float value){
        if(slider!=null){
            
            if(ValuePrintPreview!=null){
                if(baseFOVSliderValue!=null){
                    baseFOV = baseFOVSliderValue.value;
                     slider.minValue = baseFOV;
                    slider.maxValue = 2 * baseFOV;
                }
                slider.wholeNumbers = true;
                value = slider.value;
                ValuePrintPreview.text = value.ToString();
                
            }
            if(ValuePrintMenu != null){
                ValuePrintMenu.text = slider.value.ToString();
            }
        }
        
    }
    public void UpdateTextValues(){
        if(slider!=null){
            
            if(ValuePrintPreview!=null){
                if(baseFOVSliderValue!=null){
                    baseFOV = baseFOVSliderValue.value;
                     slider.minValue = baseFOV;
                    slider.maxValue = 2 * baseFOV;
                }
                slider.wholeNumbers = true;
                value = slider.value;
                ValuePrintPreview.text = value.ToString();
                
            }
            if(ValuePrintMenu != null){
                ValuePrintMenu.text = slider.value.ToString();
            }
        }
        
    }

    void Update(){
        UpdateTextValues();
    }

}
