using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalThrottle
{
   private ThrottleBody throttleBody;
   private ThrottleValve throttleValve;
   public MechanicalThrottle(){
        throttleValve = new ThrottleValve(); // Initialize throttle valve
        throttleBody = new ThrottleBody(throttleValve); // Initialize throttle body with the valve
   }

    public ThrottleBody GetThrottleBody()
    {
        return throttleBody; // Get reference to throttle body
    }

    public void SetAcceleratorInput(float input)
    {
        throttleBody.setAcceleratorInput(input); // Set input for the accelerator

    }

    public void UpdateThrottle(){
        throttleBody.controlThrottle();

   }


   ~MechanicalThrottle(){

   }
}
