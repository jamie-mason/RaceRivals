using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;


public class AdjustFOV : MonoBehaviour
{
    Camera cam;
    public CinemachineFreeLook freeLookCamera;  // Reference to your Cinemachine FreeLook camera

    public float targetHorizontalFOV;

    private float originalVerticalFOV;
    private float originalTopRigRadius;
    private float originalMiddleRigRadius;
    private float originalBottomRigRadius;


    GameObject followObject;
    Vector3 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        followObject = freeLookCamera.Follow.gameObject;
        cam = Camera.main;
        lastPos = Vector3.zero;

        originalVerticalFOV = freeLookCamera.m_Lens.FieldOfView;
        originalTopRigRadius = freeLookCamera.m_Orbits[0].m_Radius;
        originalMiddleRigRadius = freeLookCamera.m_Orbits[1].m_Radius;
        originalBottomRigRadius = freeLookCamera.m_Orbits[2].m_Radius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float aspectRatio = cam.aspect;

        float targetVerticalFOV = 2 * Mathf.Atan(Mathf.Tan(targetHorizontalFOV * Mathf.Deg2Rad / 2) / aspectRatio) * Mathf.Rad2Deg;

        // Set the vertical FOV of the Cinemachine camera
        freeLookCamera.m_Lens.FieldOfView = targetVerticalFOV;


        float radiusScale = Mathf.Tan(originalVerticalFOV * Mathf.Deg2Rad / 2) / Mathf.Tan(targetVerticalFOV * Mathf.Deg2Rad / 2);

        // Apply the scaled radii to each rig to maintain the same perspective distance
        freeLookCamera.m_Orbits[0].m_Radius = originalTopRigRadius * radiusScale;
        freeLookCamera.m_Orbits[1].m_Radius = originalMiddleRigRadius * radiusScale;
        freeLookCamera.m_Orbits[2].m_Radius = originalBottomRigRadius * radiusScale;
    }

}
