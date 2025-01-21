using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit
{
   public float orbitHeight;
   public float orbitSemiMajorDistance;
   public float orbitSemiMinorDistance;

   public Orbit(){
    orbitHeight = 0;
    orbitSemiMajorDistance = 0;
    orbitSemiMinorDistance = 0;
   }
   public Orbit(float orbitHeight, float orbitSemiMajorDistance, float orbitSemiMinorDistance){
        this.orbitHeight = orbitHeight;
        this.orbitSemiMajorDistance = orbitSemiMajorDistance;
        this.orbitSemiMinorDistance = orbitSemiMajorDistance;
   }
   public Orbit(Orbit orbit){
        this.orbitSemiMinorDistance = orbit.orbitSemiMinorDistance;
        this.orbitHeight = orbit.orbitHeight;
        this.orbitSemiMajorDistance = orbit.orbitSemiMajorDistance;
   }
   
   

}

