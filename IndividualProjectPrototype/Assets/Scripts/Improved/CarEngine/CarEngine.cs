using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarEngine
{
    private float pedalPosition;

    private bool carIsOn;


    public CarEngine(){
        pedalPosition = 0;
        carIsOn = false;
    }

    public bool getCarOn(){
        return carIsOn;
    }

    public void setCarIsOn(bool On){
        carIsOn = On;
    }
    public void flipCarIsOn(){
        carIsOn = !carIsOn;
    }
    public float getPedalPosition(){
        return pedalPosition;
    }
    public void setPedalPosition(float position){
        pedalPosition = position;
    }
    
    ~CarEngine(){

    }

}
