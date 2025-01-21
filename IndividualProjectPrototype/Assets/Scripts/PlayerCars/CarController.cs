using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public enum CarState
{
    Idle,
    Driving,
    Braking,
    Reversing,
    Paused,
    Flipped
}

public class CarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheel;
    [SerializeField] private WheelCollider frontRightWheel;
    [SerializeField] private WheelCollider rearLeftWheel;
    [SerializeField] private WheelCollider rearRightWheel;

    [Header("Wheel Meshes")]
    [SerializeField] public Transform frontLeftMesh;
    [SerializeField] private Transform frontRightMesh;
    [SerializeField] private Transform rearLeftMesh;
    [SerializeField] private Transform rearRightMesh;

    [Header("Specifications")]
    [SerializeField] private float brakeForce;
    [SerializeField] private float maxTurnAngle;
    [SerializeField] private float topSpeed;
    [SerializeField] private float handBrakeForce;
    [SerializeField] private float maxRPM = 6000f;

    [Header("Flip Specs")]
    [SerializeField] private float flipThreshold = 0.25f;
    [SerializeField] private float rightingTorque = 1000f;


    [Header("Key Inputs")]
    [SerializeField] private KeyCode brakeKey = KeyCode.Space;
    [SerializeField] private InputAction[] handBrakeInput;
    [SerializeField] private InputAction[] DriveInput;
    [SerializeField] private InputAction[] steerAction;

    [SerializeField] private InputAction[] carHorn;
    public GetInputSystem getInputSystem;

    private Rigidbody rb;
    bool launched;
    public AcuraNSX acura { get; private set; }

    public CarEngineSoundHandler carEngineSound { get; private set; }
    public HornSoundHandler hornSound { get; private set; }
    public string hornEventPath = "event:/CarHorn2";
    public bool hornLoops = false;

    public PauseGame pauseGame {get; private set;}
    public Gameover gameover;

    [SerializeField] private float switchDirectionDelay = 0.5f;
    private float switchDirectionTimer = 0f;

    public float currentTurnAngle{get; private set;}
    private float currentBrakeForce;
    private float currentHandBrakeForce;
   

    public bool hornIsPlaying { get; protected set; }
    private int minGroundedWheels = 2;

    private CarState currentState = CarState.Idle;

    private Vector3 speedHolder;


    bool lastPauseState;


    private void Awake()
    {
        carHorn = new InputAction[2];
        handBrakeInput = new InputAction[2];
        steerAction = new InputAction[2];
        DriveInput = new InputAction[2];
        acura = new AcuraNSX();
        carEngineSound = new CarEngineSoundHandler(acura.engineBlock.maxRPM);



    }
    private void Start()
    {
        InitializeComponents();
        acura.engineBlock.startEngine();
        gameover = FindObjectOfType<Gameover>();

        hornSound = new HornSoundHandler(hornEventPath);
        handBrakeInput[0] = getInputSystem.xbox.FindAction("HandBrake");
        handBrakeInput[1] = getInputSystem.keyboard.FindAction("HandBrake");
        steerAction[0] = getInputSystem.xbox.FindAction("Steer");
        steerAction[1] = getInputSystem.keyboard.FindAction("Steer");
        DriveInput[0] = getInputSystem.xbox.FindAction("Drive");
        DriveInput[1] = getInputSystem.keyboard.FindAction("Drive");
        carHorn[0] = getInputSystem.xbox.FindAction("Horn");
        carHorn[1] = getInputSystem.keyboard.FindAction("Horn");

        launched = false;
        EnableActions(handBrakeInput, true);
        EnableActions(DriveInput, true);
        EnableActions(carHorn, true);
        EnableActions(steerAction, true);
        lastPauseState = pauseGame.paused;



    }
    void OnDestroy()
    {
        DisposeActions(handBrakeInput);
        DisposeActions(DriveInput);
        DisposeActions(carHorn);
        DisposeActions(steerAction);
        carEngineSound.GetFmodEngineObject().EndSoundInstance();
        hornSound.GetFmodHornSound().EndSoundInstance();
    }

    void EnableActions(InputAction[] actions, bool enable)
    {
        foreach (InputAction action in actions)
        {
            if (action == null)
            {
                Debug.LogError("action not found in input system.");
            }
            else
            {
                if (enable)
                {
                    action.Enable();
                }
                else
                {
                    action.Disable();
                }
            }
        }
    }
    private void DisposeActions(InputAction[] actions)
    {
        foreach (InputAction action in actions)
        {
            if (action == null)
            {
                Debug.LogError("action not found in input system.");
            }
            else
            {
                action.Dispose();
            }
        }
    }
    private void Update()
    {
        if (hornSound == null)
        {
            hornSound = new HornSoundHandler(hornEventPath);
        }
        if(acura == null)
        {
            acura = new AcuraNSX();
        }
        if(carEngineSound == null)
        {
            carEngineSound = new CarEngineSoundHandler(acura.engineBlock.maxRPM);
        }
        HandleInputs();

        if (!pauseGame.paused && !gameover.trackPlayerPosition.gameover)
        {
            if(lastPauseState != pauseGame.paused){
                rb.velocity = speedHolder;
            }
            speedHolder = rb.velocity;

            HandleAudio();
            UpdateWheelMeshes();
            HandleCarFlipping();
            UpdateEngineSound();

            
        

            switch (currentState)
            {
                case CarState.Driving:
                    // Additional logic for driving can be added here
                    break;
                case CarState.Braking:
                    // Additional logic for braking can be added here
                    break;
                case CarState.Reversing:
                    // Additional logic for reversing can be added here
                    break;
                case CarState.Flipped:
                    break;
                case CarState.Idle:
                    // Logic for idling can be added here
                    break;
            }
        }
        else
        {
            HandlePause();
            if(lastPauseState != pauseGame.paused){
                speedHolder = rb.velocity;
            }
            rb.velocity = Vector3.zero;

        }

        lastPauseState = pauseGame.paused;




    }

    private void FixedUpdate()
    {
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // Adjust Y value as necessary

        
        if (!pauseGame.paused)
        {

            Vector3 posStartFront = GetAxisRayLine(frontLeftMesh, frontRightMesh);
            Vector3 posStartBack = GetAxisRayLine(rearLeftMesh, rearRightMesh);
            Vector3 posStartLeft = GetAxisRayLine(frontLeftMesh, rearLeftMesh);
            Vector3 posStartRight = GetAxisRayLine(frontRightMesh, rearRightMesh);
            Vector3 tempPosStartLeft = transform.InverseTransformPoint(posStartLeft);
            tempPosStartLeft.x /= 1f;
            tempPosStartLeft.z = 0f;

            Vector3 tempPosStartRight = transform.InverseTransformPoint(posStartRight);
            tempPosStartRight.x /= 1f;
            tempPosStartRight.z = 0f;

            posStartLeft = transform.TransformPoint(tempPosStartLeft);
            posStartRight = transform.TransformPoint(tempPosStartRight);
            

            Vector3 posEndFront = posStartFront;
            Vector3 posEndBack = posStartBack;
            Vector3 posEndLeft = posStartLeft;
            Vector3 posEndRight = posStartRight;


            Mesh mFrontLeft = frontLeftMesh.gameObject.GetComponent<MeshFilter>().mesh;
            Mesh mFrontRight = frontRightMesh.gameObject.GetComponent<MeshFilter>().mesh;
            Mesh mBackLeft = rearLeftMesh.gameObject.GetComponent<MeshFilter>().mesh;
            Mesh mBackRight = rearRightMesh.gameObject.GetComponent<MeshFilter>().mesh;

            posEndFront.y = posStartFront.y + AverageVal(mFrontLeft.bounds.extents.y, mFrontRight.bounds.extents.y) * AverageVal(frontLeftMesh.lossyScale.y, frontRightMesh.lossyScale.y);
            posEndBack.y = posStartBack.y + AverageVal(mBackLeft.bounds.extents.y, mBackRight.bounds.extents.y) * AverageVal(rearLeftMesh.lossyScale.y, rearRightMesh.lossyScale.y);
            posEndLeft.y = posStartLeft.y + AverageVal(mFrontLeft.bounds.extents.y, mBackLeft.bounds.extents.y) * AverageVal(frontLeftMesh.lossyScale.y, rearLeftMesh.lossyScale.y);
            posEndRight.y = posStartRight.y + AverageVal(mFrontRight.bounds.extents.y, mBackRight.bounds.extents.y) * AverageVal(frontRightMesh.lossyScale.y, rearRightMesh.lossyScale.y);

            posEndFront = posEndFront.y * Vector3.up * 3f;
            posEndBack = posEndBack.y * Vector3.up * 3f;
            posEndLeft = posEndLeft.y * Vector3.up * 1.5f;
            posEndRight = posEndRight.y * Vector3.up * 1.5f;

            Ray rayFront = new Ray(posStartFront, posEndFront);
            Ray rayBack = new Ray(posStartBack, posEndBack);
            Ray rayLeft = new Ray(posStartLeft, posEndLeft);
            Ray rayRight = new Ray(posStartRight, posEndRight);

            bool getFrontRayHitTrack = getHit(rayFront);
            bool getBackRayHitTrack = getHit(rayBack);
            bool getLeftRayHitTrack = getHit(rayLeft);
            bool getRightRayHitTrack = getHit(rayRight);

            Debug.DrawRay(posStartFront, posEndFront, Color.green);
            Debug.DrawRay(posStartBack, posEndBack, Color.red);
            Debug.DrawRay(posStartLeft, posEndLeft, Color.white);
            Debug.DrawRay(posStartRight, posEndRight, Color.cyan);

            float adjustedAngleX = ((transform.localEulerAngles.x + 180) % 360) - 180;
            float adjustedAngleZ = ((transform.localEulerAngles.z + 180) % 360) - 180;


            if (getFrontRayHitTrack ^ getBackRayHitTrack)
            {
                launched = false;
                if (Mathf.Abs(adjustedAngleX) > 2f)
                {
                    if (launched == false)
                    {
                        Debug.Log("Launch");
                        rb.AddForce(Mathf.Abs(GetMoveDirection()) * transform.up * 0.01f * rb.mass, ForceMode.Force);
                        launched = true;
                    }
                }
            }
            else if (getLeftRayHitTrack ^ getRightRayHitTrack)
            {
                launched = false;
                //Debug.Log(adjustedAngleZ);
                if (Mathf.Abs(adjustedAngleZ) > 15f)
                {
                    if (launched == false)
                    {
                        rb.AddForce(( Mathf.Abs(GetMoveDirection()) * transform.up) * 0.2f * rb.mass, ForceMode.Force);
                        launched = true;
                    }
                }
                else
                {

                }
            }
            else
            {
                launched = false;
            }
           
        }
        
    }

    bool getHit(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "track")
            {
                return true;
            }
        }
        return false;
    }
    float AverageVal(float a, float b) => (a + b) / 2f;
    Vector3 GetAxisRayLine(Transform wheelOne, Transform wheelTwo) => (wheelOne.position + wheelTwo.position) / 2f;
    float GetAxisRayLine(int axis, Transform wheelOne, Transform wheelTwo)
    {

        if (axis == 0)
        {
            return (wheelOne.position.x + wheelTwo.position.x) / 2f;
        }
        else if (axis == 1)
        {
            return (wheelOne.position.y + wheelTwo.position.y) / 2f;
        }
        else
        {
            return (wheelOne.position.z + wheelTwo.position.z) / 2f;

        }


    }
    private void HandlePause()
    {
        if (carEngineSound != null)
        {
            if (carEngineSound.GetFmodEngineObject() != null)
            {
                carEngineSound.GetFmodEngineObject().PauseEventSound();
                carEngineSound.backgroundWind.GetFmodEngineObject().PauseEventSound();
            }
        }
        else
        {
            carEngineSound = new CarEngineSoundHandler(acura.engineBlock.maxRPM);

        }
    }

    private void InitializeComponents()
    {
        pauseGame = GameObject.Find("PauseMenuManager").GetComponent<PauseGame>();
        rb = GetComponent<Rigidbody>();
    }

    private void HandleAudio()
    {
        if (carEngineSound != null)
        {
            if (carEngineSound.GetFmodEngineObject() != null)
            {
                if (!carEngineSound.GetFmodEngineObject().IsEventPlaying())
                {
                    carEngineSound.GetFmodEngineObject().StartEventSound();
                    carEngineSound.backgroundWind.GetFmodEngineObject().StartEventSound();

                }
                carEngineSound.GetFmodEngineObject().setSoundPlayPosition(transform.position);
                carEngineSound.GetFmodEngineObject().ResumeEventSound();
                carEngineSound.backgroundWind.GetFmodEngineObject().ResumeEventSound();


            }
        }
        else
        {

            carEngineSound = new CarEngineSoundHandler(acura.engineBlock.maxRPM);

        }

        UpdateHornSound();
    }

    private void HandleInputs()
    {
        if (pauseGame.paused)
        {
            currentState = CarState.Paused;
            return;
        }

        // Handle braking when input direction is opposite to movement direction
        float moveDirection = GetMoveDirection();
        acura.SetThrottle(moveDirection);
        acura.UpdateCar(Time.deltaTime);
        float carVelocityDirection = Mathf.Sign(rb.velocity.z);
        foreach (InputAction handBrake in handBrakeInput)
        {
            if (handBrake != null && handBrake.IsPressed())
            {
                currentState = CarState.Braking;
                ApplyHandBrakeTorque(brakeForce);
                return;
            }
        }

        for (int i = 0; i < carHorn.Length; i++)
        {
            if (carHorn[i] != null)
            {

                if (carHorn[i].WasPressedThisFrame())
                {
                    hornIsPlaying = true;

                }
                if (carHorn[i].WasReleasedThisFrame())
                {
                    hornIsPlaying = false;

                }


            }
        }
        // Reset switch timer if car's movement aligns with input or is idle
        currentBrakeForce = 0f;
        ApplyBrakeTorque(currentBrakeForce);
        ApplyMotorTorque();

        float turnDirection = GetTurnDirection();
        ApplySteering(turnDirection);
    }


    public float GetMoveDirection()
    {
        return GetDirection(DriveInput);
    }

    public float GetTurnDirection()
    {
        return GetDirection(steerAction);
    }

    private float GetDirection(InputAction[] inputActions)
    {
        for (int i = 0; i < inputActions.Length; i++)
        {
            if (inputActions[i] != null)
            {
                float value = inputActions[i].ReadValue<float>();
                if (value != 0f)
                {
                    return value;  // Return first non-zero value
                }
            }
        }
        return 0f;  // Return 0 if no actions yield a non-zero value
    }

    private void ApplyBrakeTorque(float brakeForce)
    {
        frontLeftWheel.brakeTorque = brakeForce;
        frontRightWheel.brakeTorque = brakeForce;
        rearLeftWheel.brakeTorque = brakeForce;
        rearRightWheel.brakeTorque = brakeForce;
    }

    private void ApplyHandBrakeTorque(float handBrakeForce)
    {
        frontLeftWheel.brakeTorque = handBrakeForce;
        frontRightWheel.brakeTorque = handBrakeForce;
    }

    private void ApplyMotorTorque()
    {
        rearLeftWheel.motorTorque = acura.rearAxle.leftWheelTorque;
        rearRightWheel.motorTorque = acura.rearAxle.rightWheelTorque;
    }
  
    private void ApplySteering(float turnDirection)
    {
        currentTurnAngle = Mathf.Clamp(turnDirection * maxTurnAngle, -maxTurnAngle, maxTurnAngle);
        frontLeftWheel.steerAngle = currentTurnAngle;
        frontRightWheel.steerAngle = currentTurnAngle;
    }

    private void UpdateWheelMeshes()
    {
        UpdateWheelMesh(frontLeftWheel, frontLeftMesh);
        UpdateWheelMesh(frontRightWheel, frontRightMesh);
        UpdateWheelMesh(rearLeftWheel, rearLeftMesh);
        UpdateWheelMesh(rearRightWheel, rearRightMesh);
    }

    private void UpdateEngineSound()
    {
        float vel = 0;
        carEngineSound.CurrentSpeed = Mathf.SmoothDamp(carEngineSound.CurrentSpeed, Mathf.Abs(acura.engineBlock.currentRPM),ref vel,0.2f);

        carEngineSound.UpdateCarEngineSound();

    }
    void UpdateHornSound()
    {
        if (hornIsPlaying)
        {
            if (!hornSound.GetFmodHornSound().IsEventPlaying())
            {
                hornSound.GetFmodHornSound().StartEventSound();
            }
            hornSound.GetFmodHornSound().setSoundPlayPosition(transform.position);
        }
        else
        {
            if (hornSound.GetFmodHornSound().IsEventPlaying())
            {
                hornSound.GetFmodHornSound().stopSound();
            }
        }
    }

    

    private void HandleCarFlipping()
    {
        if (IsCarFlipped() && !AreEnoughWheelsGrounded())
        {
            currentState = CarState.Flipped;

            ApplyUprightTorque(currentTurnAngle);
        }
        else if (AreEnoughWheelsGrounded())
        {
            currentState = CarState.Idle; // Reset state when grounded
        }
    }

    private bool IsCarFlipped() => Vector3.Dot(transform.up, Vector3.up) < Mathf.Abs(flipThreshold);


    private bool AreEnoughWheelsGrounded()
    {
        int groundedWheelCount = 0;

        if (frontLeftWheel.isGrounded) groundedWheelCount++;
        if (frontRightWheel.isGrounded) groundedWheelCount++;
        if (rearLeftWheel.isGrounded) groundedWheelCount++;
        if (rearRightWheel.isGrounded) groundedWheelCount++;

        return groundedWheelCount >= minGroundedWheels;
    }

    private void UpdateWheelMesh(WheelCollider wheelCollider, Transform wheelTransform)
    {
        wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }

    private void ApplyUprightTorque(float steerInput)
    {
        if (Mathf.Abs(steerInput) > 0)
        {
            Vector3 torque = transform.forward * steerInput * rightingTorque;
            rb.AddTorque(torque);
        }
    }

    private float GetWheelSpeed(WheelCollider col)
    {
        WheelHit wheelHit;
        if (col.GetGroundHit(out wheelHit))
        {

            float anglularSpeed = col.rotationSpeed;
            float wheelRadius = col.radius;
            float pi = Mathf.PI;

            float vel = anglularSpeed * wheelRadius * 2f * pi;
            vel = vel / 360f;
            vel = vel * 3.6f;

            return vel;

        }
        return 0f; // Return 0 if the wheel is not grounded
    }
    public Transform wheelRL() => rearLeftMesh;

    public Transform wheelRR() => rearRightMesh;

    public Transform wheelFR() => frontRightMesh;

    public Transform wheelFL() => frontLeftMesh;
    public float GetMaxTurnAngle() => maxTurnAngle;

}
