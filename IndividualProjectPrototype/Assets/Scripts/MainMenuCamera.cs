using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [Header("Camera Shake Settings")]
    [SerializeField] private float shakeFrequency = 10f; // How fast the shake oscillates
    [SerializeField] private float shakeAmplitude = 0.1f; // How much the camera moves

    [Header("Zoom Settings")]
    [SerializeField] private float baseFOV = 60f;
    [SerializeField] private float maxFOV = 90f;
    [SerializeField] private float zoomSpeed = 2f;

    private Camera cam;
    private float timeCounter = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Camera shake
        timeCounter += Time.deltaTime * shakeFrequency;
        float shakeOffset = Mathf.Sin(timeCounter) * shakeAmplitude;
        transform.position += transform.forward * shakeOffset;

        // Dynamic zoom
        float targetFOV = Mathf.Lerp(baseFOV, maxFOV, Mathf.Abs(shakeOffset) * 10f);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }


}
