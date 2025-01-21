using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderPrintIncrementValueModification : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI ValuePrint;
    [SerializeField] private string textAfter;
    [SerializeField] private string textBefore;
    [SerializeField] private float IncrementMultiplier = 1f;
    [SerializeField] private float IncrementDivider = 1f;
    [SerializeField] private bool printAsPercentage;

    public float value{get;set;}

    void Start()
    {
        UpdateTextValues();
        slider.onValueChanged.AddListener(UpdateTextValues);

    }
    public void UpdateTextValues(float value){
        if(slider!=null){
            if(ValuePrint!=null){
                this.value = slider.value * IncrementMultiplier / IncrementDivider;
                float percentage = Mathf.Round(this.value * 100f);

                if(printAsPercentage){
                    ValuePrint.text = textBefore + percentage.ToString() + "%" + textAfter;
                }
                else{
                    ValuePrint.text = textBefore + this.value.ToString() + textAfter;
                }
                
            }
            
        }
    }
    public void UpdateTextValues(){
        if(slider!=null){
            if(ValuePrint!=null){
                this.value = slider.value * IncrementMultiplier / IncrementDivider;
                float percentage = Mathf.Round(this.value * 100f);

                if(printAsPercentage){
                    ValuePrint.text = textBefore + percentage.ToString() + "%" + textAfter;
                }
                else{
                    ValuePrint.text = textBefore + this.value.ToString() + textAfter;
                }
                
            }
            
        }
    }

    public void SetSliderValues(){
        slider.value = value * IncrementDivider / IncrementMultiplier;
    }
    
    void Update()
    {
        
       
        
        
    }
}
