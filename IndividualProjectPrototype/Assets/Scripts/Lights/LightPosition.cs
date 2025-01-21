using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightPosition : MonoBehaviour
{
    Light[] lights;
    Light[] directionalLights;
    Vector4[] lightPositions;
    Color[] lightColours;
    [SerializeField] Material mat;
    void Awake()
    {
        int numOfDirectionalLights = 0;
        lights = new Light[FindObjectsOfType<Light>().Count()];
        lights = FindObjectsOfType<Light>();
        foreach (Light obj in lights){
            if(obj.type == LightType.Directional){
                numOfDirectionalLights++;
            }
        }
        directionalLights = new Light[numOfDirectionalLights];
        lightPositions = new Vector4[numOfDirectionalLights];
        lightColours = new Color[numOfDirectionalLights];
        for (int i = 0; i<lights.Length; i++){
            if(lights[i].type == LightType.Directional){
                directionalLights = lights;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (lights != null){
            for(int i = 0; i<directionalLights.Length;i++){
                lightPositions[i] = directionalLights[i].transform.position;
                lightColours[i] = directionalLights[i].color;
            }
            if(mat != null){
                    if(mat.HasProperty("_LightPositions")){
                        mat.SetVectorArray("_LightPositions", lightPositions);
                    }
                    if(mat.HasProperty("_LightColours")){
                        mat.SetColorArray("_LightColours", lightColours);
                    }
                    if(mat.HasProperty("_LightCount")){
                        mat.SetInt("_LightCount", lightPositions.Length);
                    }
                }
            
        }
    }
}
