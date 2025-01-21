using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrottleBody
{
    private ThrottleValve valve;
    private float acceleratorInput;
    
    public ThrottleBody(ThrottleValve valveRef){
        acceleratorInput = 0f;
        valve = valveRef;
    }
    public ThrottleValve getThrottleValve(){
        return valve;
    }
    public void setAcceleratorInput(float input){
        acceleratorInput = Mathf.Clamp(input,0f,100f);

    }
    public float getAcceleratorInput(){
        return acceleratorInput;
    }

    public void controlThrottle(){
        float desiredPosition = acceleratorInput * 100f;
        valve.setValvePosition(desiredPosition);
    }

    public void adjustThrottle(float targetPosition, float rate){
        float currentPosition = valve.getValvePosition();
        if(currentPosition < targetPosition){
            valve.open(rate);
        }
        else if(currentPosition > targetPosition){
            valve.close(rate);
        }
    }

    ~ThrottleBody(){

    }
}
