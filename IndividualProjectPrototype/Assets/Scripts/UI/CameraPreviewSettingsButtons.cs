using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class CameraPreviewSettingsButtons : MonoBehaviour
{
    public ManageCameraPreviewGroupings[] CameraGroupings; 
    public bool previewSliderOpen;
    public PauseGame pauseGame;
    public CameraScriptTester cameraScriptTester;

    void Awake(){
        
        CameraGroupings = new ManageCameraPreviewGroupings [FindObjectsOfType<ManageCameraPreviewGroupings>().Length];
        CameraGroupings = FindObjectsOfType<ManageCameraPreviewGroupings>();
        for (int i = 0; i<CameraGroupings.Length;i++){
            CameraGroupings[i].pauseGame = pauseGame;
            CameraGroupings[i].cameraScriptTester = cameraScriptTester;
            
        }
        
        
    } 
    private void Update(){
        if(pauseGame.paused){
        for (int i = 0; i<CameraGroupings.Length;i++){
            if(CameraGroupings[i].ISPreviewSetting){
            if(CameraGroupings[i].previewActive){
                previewSliderOpen = true;
                cameraScriptTester.customizingCameraSettings = true;
                if(CameraGroupings[i].valueChanged == true){
                    CameraGroupings[i].ManageCarSettingValueReturn();
                }
                
                break;
            }
            else{
                previewSliderOpen = false;
                
            }
            }
            else{
                CameraGroupings[i].ManageCarSettingValueReturn();
                
                

            }

        }

        if(previewSliderOpen){
            if(pauseGame.backButtonXbox.WasPressedThisFrame() || pauseGame.pauseButtonKeyboard.WasPressedThisFrame() || pauseGame.pauseButtonXbox.WasPressedThisFrame()){
                for (int i = 0; i<CameraGroupings.Length;i++){
                    CameraGroupings[i].setPreviewInActive();  
                    cameraScriptTester.customizingCameraSettings = false;  
                }
            }
        }
    }
    }
}
