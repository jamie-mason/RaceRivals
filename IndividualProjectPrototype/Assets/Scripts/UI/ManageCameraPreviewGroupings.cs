using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public enum CameraSetting{
    None = 0,
    BaseFOV = 1,
    MaxFOV = 2,
    LookAhead = 3,
    Distance = 4,
    Height = 5,

    sensitivityX = 6,
    sensitivityY = 7,
    invertX = 8,
    invertY = 9,



}

public class ManageCameraPreviewGroupings : MonoBehaviour
{
    [SerializeField] private Button changeSettingButton;
    [SerializeField] private GameObject SliderLayout;
    public Slider previewSlider {get; private set;}

    public Toggle toggle {get; private set;}

    public SliderPrintIncrementValueModificationForPreviews sliderPrintIncrementValueModificationForPreviews ;
    public SliderPrintIncrementValueModification sliderPrintIncrementValueModification ;
    public MaxFieldOfViewSlider maxFieldOfViewSlider;
    ManageToggles manageToggles;
    public float value;
    public float lastValue;
    public bool previewActive;

    public bool ISPreviewSetting;

    public bool valueChanged{get; private set;}

    public PauseGame pauseGame;
    public CameraScriptTester cameraScriptTester;


    [SerializeField] private CameraSetting setting = CameraSetting.None;

    
    
    
    void Start()
    {
    
        if(SliderLayout!=null){
            SliderLayout.SetActive(false);
            for(int i = 0; i<SliderLayout.transform.childCount; i++){
                if(SliderLayout.transform.GetChild(i).GetComponent<Slider>() != null){
                    previewSlider = SliderLayout.transform.GetChild(i).GetComponent<Slider>();
                    break;
                }
            }
            
            
        }
        sliderPrintIncrementValueModificationForPreviews = GetComponent<SliderPrintIncrementValueModificationForPreviews>();
        sliderPrintIncrementValueModification = GetComponent<SliderPrintIncrementValueModification>();
        maxFieldOfViewSlider = GetComponent<MaxFieldOfViewSlider>();
        manageToggles = GetComponent<ManageToggles>();
        if(manageToggles!=null){
            toggle = manageToggles.toggle;
        }
        SetSettings();
        //ManageCarSettingValueReturn();
        if(ISPreviewSetting){
            changeSettingButton.onClick.AddListener(setPreviewActive);
        }
        
        previewActive = false;
        lastValue = value;

    }
    public void SetSettings(){
        if(setting == CameraSetting.BaseFOV){
            if(sliderPrintIncrementValueModificationForPreviews!=null){
                sliderPrintIncrementValueModificationForPreviews.value = cameraScriptTester.baseFOV;
            }
        }
        else if (setting == CameraSetting.LookAhead){
            if(sliderPrintIncrementValueModificationForPreviews!=null){
                sliderPrintIncrementValueModificationForPreviews.value = cameraScriptTester.forwardLookOffset;
            }
            
        }
        else if (setting == CameraSetting.MaxFOV){
            if(maxFieldOfViewSlider!=null){
            }
        }
        else if (setting == CameraSetting.Height){
            sliderPrintIncrementValueModificationForPreviews.value = cameraScriptTester.offset.y;
        }
        else if (setting == CameraSetting.Distance){
            sliderPrintIncrementValueModificationForPreviews.value = -cameraScriptTester.offset.z;
        }
        else if(setting == CameraSetting.sensitivityX){
            sliderPrintIncrementValueModification.value = cameraScriptTester.freelookSensitivityX;
            sliderPrintIncrementValueModification.SetSliderValues();
            sliderPrintIncrementValueModification.UpdateTextValues();


        }
        else if(setting == CameraSetting.sensitivityY){
            sliderPrintIncrementValueModification.value = cameraScriptTester.freelookSensitivityY;
            sliderPrintIncrementValueModification.SetSliderValues();
            sliderPrintIncrementValueModification.UpdateTextValues();

        }
        else if (setting == CameraSetting.invertX){
            manageToggles.toggle.isOn = cameraScriptTester.InvertXFreelook;
        }
        else if (setting == CameraSetting.invertY){
            manageToggles.toggle.isOn = cameraScriptTester.InvertYFreelook;
        }
    }
    public void ManageCarSettingValueReturn(){
        if(setting == CameraSetting.BaseFOV){
            if(sliderPrintIncrementValueModificationForPreviews!=null){
                cameraScriptTester.baseFOV = sliderPrintIncrementValueModificationForPreviews.value;
            }
        }
        else if (setting == CameraSetting.LookAhead){
            if(sliderPrintIncrementValueModificationForPreviews!=null){
                cameraScriptTester.forwardLookOffset = sliderPrintIncrementValueModificationForPreviews.value;
            }
            
        }
        else if (setting == CameraSetting.MaxFOV){
            if(maxFieldOfViewSlider!=null){
            }
        }
        else if (setting == CameraSetting.Height){
            cameraScriptTester.offset.y = sliderPrintIncrementValueModificationForPreviews.value;
        }
        else if (setting == CameraSetting.Distance){
            cameraScriptTester.offset.z = -sliderPrintIncrementValueModificationForPreviews.value;
        }
        else if(setting == CameraSetting.sensitivityX){
            cameraScriptTester.freelookSensitivityX = sliderPrintIncrementValueModification.value;
        }
        else if(setting == CameraSetting.sensitivityY){
            cameraScriptTester.freelookSensitivityY = sliderPrintIncrementValueModification.value;
            
        }
        else if (setting == CameraSetting.invertX){
            cameraScriptTester.InvertXFreelook = manageToggles.toggle.isOn;

        }
        else if (setting == CameraSetting.invertY){
            cameraScriptTester.InvertYFreelook = manageToggles.toggle.isOn;

        }
    }
    
    public void setPreviewActive(){
        previewActive = true;
        SliderLayout.SetActive(true);
        EventSystem.current.SetSelectedGameObject(previewSlider.gameObject);
        pauseGame.GetPauseMenuBackground().SetActive(false);
    }

    public void setPreviewInActive(){
        previewActive = false;
        SliderLayout.SetActive(false);
        EventSystem.current.SetSelectedGameObject(changeSettingButton.gameObject);
        pauseGame.GetPauseMenuBackground().SetActive(true);
    }
    public CameraSetting GetSetting(){
        return setting;
    }
    void Update(){
        if(pauseGame.paused){
        if(value != lastValue){
            valueChanged = true;
        }
        else{
            valueChanged = false;
        }
        lastValue = value;
        }
    }
}
