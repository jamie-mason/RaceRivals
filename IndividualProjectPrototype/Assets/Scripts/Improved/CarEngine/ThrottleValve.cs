using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ThrottleValve
{
    private float valvePosition;

    public ThrottleValve(){
        valvePosition = 0.0f;
    }

    public float getValvePosition(){
        return valvePosition;  
    }
    
    public void setValvePosition(float newPosition){
        valvePosition = Mathf.Clamp(newPosition,0f,100f);
    }
    public void open(float increment){
        setValvePosition(valvePosition + increment);
    }
    public void close(float decrement){
        setValvePosition(valvePosition - decrement);

    }


    ~ThrottleValve(){

    }
    
}

