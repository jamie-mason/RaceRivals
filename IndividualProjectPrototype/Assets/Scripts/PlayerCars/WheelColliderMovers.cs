using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMeshRotator : MonoBehaviour
{
    [Header("Wheel Meshes")]
    [SerializeField] private Transform frontLeftMesh;
    [SerializeField] private Transform frontRightMesh;
    [SerializeField] private Transform rearLeftMesh;
    [SerializeField] private Transform rearRightMesh;

    [Header("Settings")]
    [SerializeField] private float wheelRadius = 0.33f; // Adjust this to match your wheel's actual radius

    private Rigidbody rb;
    private float currentRotationAngle = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RotateWheels();
    }

    private void RotateWheels()
    {
        // Calculate the distance travelled in this frame (ignores turning for simplicity)
        float distanceTravelled = 10f * Time.deltaTime;

        // Calculate the rotation angle based on the wheel circumference
        float rotationAngle = (distanceTravelled / (2 * Mathf.PI * wheelRadius)) * 360f;

        // Accumulate the rotation angle and apply it to each wheel
        currentRotationAngle += rotationAngle;
        Vector3 rotationVector = new Vector3(currentRotationAngle, 0f, 0f);

        frontLeftMesh.localRotation = Quaternion.Euler(rotationVector);
        frontRightMesh.localRotation = Quaternion.Euler(rotationVector);
        rearLeftMesh.localRotation = Quaternion.Euler(rotationVector);
        rearRightMesh.localRotation = Quaternion.Euler(rotationVector);
    }
}
