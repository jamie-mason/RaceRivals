using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //This script will manage the properties of the main camera to follow and look at an object and manage moving the camera to a different position

   
    [SerializeField] private Vector3 defaultCamPosition;

    bool cameraLookEnabled;
    private Vector3 lastCamPosition;

    [SerializeField] private Vector3 camPositionOffset;

    [SerializeField] private float minSpeedThreshold;

    private Camera cam;

    [SerializeField] private GameObject followObject;
    [SerializeField] private GameObject LookAtPoint;

    [SerializeField] private float smoothTimeX = 0.3f;   // Damping time for X axis
    [SerializeField] private float smoothTimeY = 0.3f;   // Damping time for Y axis
    [SerializeField] private float smoothTimeZ = 0.3f;   // Damping time for Z axis
    [SerializeField] private float smoothTime = 0.3f;   // Damping time for all axes
    [SerializeField] private float angleSmoothTime = 0.3f;   // Damping time for all axes



    private float velocityX = 0f;    // X-axis velocity for SmoothDamp
    private float velocityY = 0f;    // Y-axis velocity for SmoothDamp
    private float velocityZ = 0f;    // Z-axis velocity for SmoothDamp

    private float angleDampVelocity = 0f;
    private Vector3 velocity = Vector3.zero;

    private Vector3 lastPosition;


    private Vector3 followPointVelocity;


    


    private bool hasRigidBody;

    [SerializeField] private Rigidbody rb;
    private void Start()
    {   
        
        if (cam == null)
        {
            cam = Camera.main;
        }

        rb = followObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            hasRigidBody = true;
        }
        else
        {
            hasRigidBody = false;
        }
        lastPosition = followObject.transform.position;
        camPositionOffset = defaultCamPosition;
        lastCamPosition = defaultCamPosition;

       
    }


    public Camera getCamera()
    {
        return cam;
    }





    private void Update()
    {
        if (lastCamPosition != defaultCamPosition)
        {
            camPositionOffset = defaultCamPosition;
        }

        

        lastCamPosition = defaultCamPosition;

    }
    private void FixedUpdate()
    {
        // Get velocity based on Rigidbody or manual calculation
        followPointVelocity = hasRigidBody
            ? rb.velocity  // Use Rigidbody velocity 
            : (followObject.transform.position - lastPosition) / Time.fixedDeltaTime; // Calculate velocity without Rigidbody

        // Convert to local velocity (relative to follow object)
        Vector3 relativeVelocity = followObject.transform.InverseTransformDirection(followPointVelocity);

        // Get current local camera position and calculate swivel target
        Vector3 camLocalPosition = followObject.transform.InverseTransformPoint(cam.transform.position);
        Vector3 swivelTarget = GetFollowOffsetPosition(camLocalPosition, Vector3.zero, relativeVelocity, Mathf.Abs(defaultCamPosition.z), Mathf.Abs(defaultCamPosition.z));

        // Calculate the smoothed target position
        Vector3 targetPosition = followObject.transform.TransformPoint(camPositionOffset);
        Vector3 targetLocalPosition = followObject.transform.InverseTransformPoint(targetPosition);

        // Smooth the camera position in local space
        Vector3 smoothedLocalPosition = new Vector3(
            Mathf.SmoothDamp(camLocalPosition.x, targetLocalPosition.x, ref velocityX, smoothTimeX),
            Mathf.SmoothDamp(camLocalPosition.y, targetLocalPosition.y, ref velocityY, smoothTimeY),
            Mathf.SmoothDamp(camLocalPosition.z, targetLocalPosition.z, ref velocityZ, smoothTimeZ)
        );

        // Convert smoothed local position back to world space
        Vector3 smoothTransform = followObject.transform.TransformPoint(smoothedLocalPosition);

        // Calculate current and target angles for swiveling
        float currentAngle = Vector3.Angle(Vector3.zero, camLocalPosition);
        float targetAngle = GetSwivelAngleDifference(camLocalPosition, targetLocalPosition, Vector3.zero);

        // Smoothly adjust swivel angle
        float swivelAngle = Mathf.SmoothDamp(currentAngle, targetAngle, ref angleDampVelocity, angleSmoothTime);

        // Rotate camera around the follow object
        cam.transform.RotateAround(followObject.transform.position, followObject.transform.up, swivelAngle);

        // Smooth the camera's final position
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref velocity, smoothTime);

        // Update the camera position offset
        camPositionOffset = swivelTarget;

        // Ensure the camera is looking at the follow object
        cam.transform.LookAt(followObject.transform);

        // Store the last position and velocity for the next frame
        lastPosition = followObject.transform.position;
    }



    public Vector3 GetFollowOffsetPosition(Vector3 followerPosition, Vector3 followObjectPosition, Vector3 followObjectVelocity, float semiMajorDistance, float semiMinorDistance)
    {
        // If the followed object is stationary, just return the current position (no offset).
        Vector3 noYVelocity = new Vector3(followObjectVelocity.x, 0f, followObjectVelocity.z);
        if (noYVelocity.magnitude < minSpeedThreshold)
        {
            return defaultCamPosition; // No significant velocity, return current follower position.
        }

        // Get the direction of the followed object's velocity (ignoring y component for horizontal following).
        Vector3 velocityDirection = noYVelocity.normalized;
        Vector3 offsetPosition = new Vector3(Mathf.Sign(velocityDirection.z) * velocityDirection.x, 0f, -velocityDirection.z);
        if (Mathf.Abs(semiMajorDistance) == Mathf.Abs(semiMinorDistance))
        {
            // Calculate the offset position: followObject's position minus the offset along the velocity direction.
             offsetPosition *= Mathf.Abs(semiMajorDistance);
        }
        else
        {
            offsetPosition.x *= Mathf.Abs(semiMinorDistance);
            offsetPosition.z *= Mathf.Abs(semiMajorDistance);
        }


        offsetPosition.y = defaultCamPosition.y;

        // Return the calculated offset position.
        return offsetPosition;
    }

    private float GetSwivelAngleDifference(Vector3 currentSwivelPos, Vector3 targetSwivelPos, Vector3 followObjectPos)
    {
        Vector3 currentSwivelVector = currentSwivelPos - followObjectPos;
        Vector3 targetSwivelVector = targetSwivelPos - followObjectPos;

        currentSwivelVector.y = 0;
        targetSwivelVector.y = 0;

        if (currentSwivelVector.magnitude < Mathf.Epsilon || targetSwivelVector.magnitude < Mathf.Epsilon)
        {
            Debug.LogError("Swivel vectors are too small to normalize.");
            return 0f;
        }

        currentSwivelVector.Normalize();
        targetSwivelVector.Normalize();

        // Standard dot product calculation (no need for z-axis check).
        float dotProduct = Vector3.Dot(currentSwivelVector, targetSwivelVector);

        dotProduct = Mathf.Clamp(dotProduct, -1f, 1f);

        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

        // Cross product to determine the sign of the angle.
        Vector3 crossProduct = Vector3.Cross(currentSwivelVector, targetSwivelVector);
        if (crossProduct.y < 0)
        {
            angle = -angle;
        }

        return angle;
    }
    
    




    }



    


