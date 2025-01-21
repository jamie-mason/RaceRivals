using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CinimacheneCentred : MonoBehaviour
{

    protected bool isFreeLookActive;
    public Transform followObject;              // Object to follow (your player or moving object)
    Rigidbody rb;
    public CinemachineFreeLook freeLookCamera;  // Reference to your Cinemachine FreeLook camera
    public float activationThreshold = 0.1f; // Sensitivity threshold
    [SerializeField] private float centredXValue;
    [SerializeField] private float centredYValue;
    [SerializeField] private float xCenterThreshold = 5f;
    [SerializeField] private float yCenterThreshold = 1f;
    
    [SerializeField] private float radiusMinorScale = 1.5f;
    [SerializeField] private float transitionSpeed = 1.5f;
    private Vector3 acceleration;
    private Vector3 lastVelocity;
    private Vector3 velocity;
    private Vector3 lastPosition;
    private Vector3 followObjectPosition;
    private float lastAcceleration;

    [SerializeField] private CinemachineInputProvider inputProvider;

    public float defaultMiddleRigHeight;
    public float defaultMiddleRigRadius;
    public float defaultBottomRigRadius;
    public float defaultTopRigRadius;
    private float middleRigMag;

    private float lastMiddleRigHeight;

    public float minHeight = 1.0f;      // Minimum height when experiencing high forward Gs
    public float maxHeight = 5.0f;      // Maximum height when experiencing low or negative Gs
    public float maxGs = 2.0f;          // Maximum G-force value for scaling


    bool lastActive;
    private void Awake()
    {
    }
    void Start()
    {
        centredXValue = 0f;
        centredYValue = 0.5f;
        isFreeLookActive = false;
        followObject = freeLookCamera.Follow;
        lastActive = false;
        if (followObject.gameObject.GetComponent<Rigidbody>() != null)
        {
            rb = followObject.gameObject.GetComponent<Rigidbody>();
        }
        velocity = Vector3.zero;
        lastVelocity = velocity;
        followObjectPosition = followObject.transform.position;
        lastPosition = followObjectPosition;
     
        var topRig = freeLookCamera.m_Orbits[0];
        var middleRig = freeLookCamera.m_Orbits[1];
        var bottomRig = freeLookCamera.m_Orbits[2];

        freeLookCamera.m_XAxis.Value = 0f;
        freeLookCamera.m_YAxis.Value = 0f;
        freeLookCamera.m_XAxis.m_InputAxisValue = 0f;
        freeLookCamera.m_YAxis.m_InputAxisValue = 0f;

        float middleRigRadius = middleRig.m_Radius;
        defaultMiddleRigRadius = middleRigRadius; 

        defaultBottomRigRadius = bottomRig.m_Radius;

        defaultTopRigRadius = topRig.m_Radius;

        float middleRigHeight = middleRig.m_Height;
        defaultMiddleRigHeight = middleRigHeight;

        Vector2 midRig = new Vector2(middleRigRadius, middleRigHeight);
        middleRigMag = midRig.magnitude;
        lastMiddleRigHeight = defaultMiddleRigHeight;

    } 

    float ManageRadius(float semiMajorRadius, float semiMinorRadius){
        float toCameraX = freeLookCamera.transform.position.x - followObject.position.x;
        float toCameraZ = freeLookCamera.transform.position.z - followObject.position.z;
        Vector3 toCamera = (freeLookCamera.transform.position - followObject.position);
        toCamera.x = toCameraX;
        toCamera.z = toCameraZ;
        toCamera.y = 0;
        toCamera = toCamera.normalized;
        
        // Calculate the angle relative to the target's forward vector
        float angle = Vector3.SignedAngle(followObject.forward, toCamera, followObject.up);
        //Debug.Log(angle);
        // Determine the interpolated radius
        float absAngle = Mathf.Abs(angle);
        float t = absAngle <= 90f 
                    ? Mathf.InverseLerp(0f, 90f, absAngle)
                    : Mathf.InverseLerp(90f, 180f, absAngle);
        float targetRadius = absAngle <= 90f 
                    ? Mathf.Lerp(semiMajorRadius, semiMinorRadius, t)
                    : Mathf.Lerp(semiMinorRadius, semiMajorRadius, t);
        return targetRadius;
    }
    void FixedUpdate()
    {
        if (freeLookCamera != null)
        {
            Vector3 G = Physics.gravity;
            if(rb != null)
            {
                velocity = rb.velocity;
            }
            else
            {
                velocity = (followObjectPosition - lastPosition) / Time.fixedDeltaTime;
            }
            acceleration = (velocity - lastVelocity) / Time.fixedDeltaTime;
            Vector3 accelerationDirection = acceleration.normalized;

            float forwardBackAcceleration = Mathf.Round(Vector3.Dot(acceleration, followObject.transform.forward) * 1000f) / 1000f;
            float rightLeftAcceleration = Mathf.Round(Vector3.Dot(acceleration, followObject.transform.right) * 1000f) / 1000f;

            float forwardGs = Mathf.Round(forwardBackAcceleration / G.magnitude * 1000f) / 1000f;

            forwardGs = Mathf.Clamp(forwardGs, -maxGs, maxGs);

            float targetHeight = Mathf.Lerp(defaultMiddleRigHeight - maxHeight, defaultMiddleRigHeight + maxHeight, (forwardGs + maxGs) / (2 * maxGs));

            float xValue = freeLookCamera.m_XAxis.Value;
            float yValue = freeLookCamera.m_YAxis.Value;

            float inputXValue = inputProvider.GetAxisValue(0);
            float inputYValue = inputProvider.GetAxisValue(1);


            float upperXThreshold = centredXValue + xCenterThreshold;
            float lowerXThreshold = centredXValue - xCenterThreshold;


            float upperYThreshold = centredYValue + yCenterThreshold;
            float lowerYThreshold = centredYValue - yCenterThreshold;


            var topRig = freeLookCamera.m_Orbits[0];
            var middleRig = freeLookCamera.m_Orbits[1];
            var bottomRig = freeLookCamera.m_Orbits[2];

            Vector2 camRig = new Vector2(middleRig.m_Radius, middleRig.m_Height);

            if (xValue >= lowerXThreshold && xValue <= upperXThreshold &&
            yValue >= lowerYThreshold && yValue <= upperYThreshold && inputXValue == 0f && inputYValue == 0f)
            {
                isFreeLookActive = false;
            }
            else
            {
                isFreeLookActive=true;

            }
            if (!isFreeLookActive)
            {
                middleRig.m_Height = targetHeight;
            }
            else
            {
                middleRig.m_Height = defaultMiddleRigHeight;
            }
            if(lastMiddleRigHeight != middleRig.m_Height)
            {
                //Debug.Log(middleRig.m_Height);
            }
            float radiusMajor = Mathf.Sqrt(Mathf.Pow(middleRigMag, 2f) - Mathf.Pow(middleRig.m_Height, 2f));
            float radiusMinor = radiusMajor * radiusMinorScale;
            float middleRigRadius = ManageRadius(defaultMiddleRigRadius,radiusMinor);
            float topRadius = ManageRadius(defaultTopRigRadius,defaultTopRigRadius * radiusMinorScale);
            float bottomRadius = ManageRadius(defaultBottomRigRadius,defaultBottomRigRadius * radiusMinorScale);

            freeLookCamera.m_Orbits[1].m_Radius = Mathf.Lerp(freeLookCamera.m_Orbits[1].m_Radius, middleRigRadius, Time.deltaTime * transitionSpeed);
            freeLookCamera.m_Orbits[0].m_Radius = Mathf.Lerp(freeLookCamera.m_Orbits[0].m_Radius, topRadius, Time.deltaTime * transitionSpeed);
            freeLookCamera.m_Orbits[2].m_Radius = Mathf.Lerp(freeLookCamera.m_Orbits[2].m_Radius, bottomRadius, Time.deltaTime * transitionSpeed);
            
            

            lastActive = isFreeLookActive;
            velocity = lastVelocity;
            lastPosition = followObjectPosition;
            lastMiddleRigHeight = middleRig.m_Height;
        }
        else
        {
            Debug.LogError($"freeLookCamera variable in CinemachineCentred script attatched to {gameObject.name} has not been instanced.");
        }

        
        //isFreeLookActive = Mathf.Abs(horizontalInput) > activationThreshold || Mathf.Abs(verticalInput) > activationThreshold;


       
    }


   
}
