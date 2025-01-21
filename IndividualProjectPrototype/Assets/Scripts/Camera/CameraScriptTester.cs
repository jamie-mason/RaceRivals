using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class CameraScriptTester : MonoBehaviour, IDataPersistence
{
    public UnityEngine.Vector3 offset = new UnityEngine.Vector3(0, 5, -10); // Default offset
    [SerializeField] private float smoothTime = 0.2f; // Smooth transition time for chase mode
    [SerializeField] private float smoothTimeFOV = 0.2f; // Smooth transition time for chase mode
    [SerializeField] private float camRotationSmoothTime = 0.2f; // Smooth transition time for chase mode
    [SerializeField] private float SpeedDampModifier = 0.2f; // Smooth transition time for chase mode
    [SerializeField] private float YawDampModifier = 0.2f; // Smooth transition time for chase mode
    [SerializeField] private float freelookClamp = 30f; // Smooth transition time for chase mode
    [SerializeField] private float freelookYawClamp = 30f; // Smooth transition time for chase mode
    public float forwardLookOffset = 2f;
    [SerializeField] private float maxLateralOffset = 2f;
    public float ZoomModifierMultiplier = 1.5f;

    public float maxZoomFOV = 120f;

    public bool showMaxFOV = false;
    bool onStart;
    
    [SerializeField] private float transitionBlendSpeed = 10f; // Speed of blending between modes
    [SerializeField] private float ReturnToForwardDelay = 5f; // Speed of blending between modes
    private float transitionFactor = 0f; // Transition factor: 0 = Chase, 1 = Freelook
    private float transitionToDirection = 0f; // Transition factor: 0 = away from camera, 1 = toward camera;
    public float baseFOV = 60f;
    public bool freelookOnly;
    public bool chaseOnly;
    public bool customizingCameraSettings;
    private float speed;
    private UnityEngine.Vector3 lastFreeLookOffset;
    private UnityEngine.Vector3 lastLookPosition;
    private UnityEngine.Vector3 lastCamPos;
    private UnityEngine.Vector3 lastPos;
    private UnityEngine.Vector3 CurrentPos;
    UnityEngine.Vector3 nonRigidbodyVelocity;

    [SerializeField] private int FreelookCameraDefault;

    Rigidbody rb;
    public float freelookSensitivityX = 2f; // Sensitivity for freelook input
    public float freelookSensitivityY = 2f; // Sensitivity for freelook input
    [SerializeField] private float returnToChaseDelay = 2f; // Time before returning to chase mode after freelook
    public bool InvertXFreelook;
    public bool InvertYFreelook;
    private Camera mainCamera;
    private Transform target;

    UnityEngine.Vector3 targetVelocity;
    UnityEngine.Vector3 localVelocity;
    UnityEngine.Vector3 angularVelocity;

    private InputAction look;
    private InputAction[] reverseCamera = new InputAction [2];

    private UnityEngine.Vector3 velocity = UnityEngine.Vector3.zero; // Velocity for SmoothDamp
    private float FOVvelocity = 0; // Velocity for SmoothDamp
    private float ForwardtoReverseTransVel = 0; // Velocity for SmoothDamp
    private float ReverseToForwardTransVel = 0; // Velocity for SmoothDamp

    private UnityEngine.Quaternion currentRotation; // Current freelook rotation
    private UnityEngine.Quaternion currentRotation2; // Current freelook rotation
    private UnityEngine.Quaternion lastRotation; // Current freelook rotation
    private float yawRotationForwardToReverse;
    private bool isFreelooking = false; // Whether the player is freelooking
    private float freelookTimer = 0f; // Timer for transitioning back to chase mode
    private float returnToForwardTimer = 0f;
    private float returnToForwardRotationTimer = 0f;

    UnityEngine.Vector3 targetAcceleration;
    UnityEngine.Vector3 lastChaseCamOffset;
    UnityEngine.Vector3 lastVelocity;

    float rotVelx;
    float rotVely;
    float rotVelz;


    float chasePosRevert = 0f;

    [SerializeField] float chaseRotationSmoothDamp = 0.2f;

    PauseGame pauseGame;
    private void Awake(){
        pauseGame = FindObjectOfType<PauseGame>();
    }
    private void Start()
{
    onStart = true;
    // Initialize references
    mainCamera = Camera.main;
    target = transform;
    rb = target.gameObject.GetComponent<Rigidbody>();

    // Setup initial values
    yawRotationForwardToReverse = 0;
    isFreelooking = false;
    targetAcceleration = UnityEngine.Vector3.zero;
    transitionFactor = 0;

    // Set camera position and initial offsets
    mainCamera.transform.position = target.position + offset;
    ResetToChaseMode();
    ObtainRotation();

    // Get input system actions
    GetInputSystem inputSystem = FindObjectOfType<GetInputSystem>();
    look = inputSystem.look.FindAction("look");
    reverseCamera[0] = inputSystem.xbox.FindAction("ReverseCamera");
    reverseCamera[1] = inputSystem.keyboard.FindAction("ReverseCamera");
    
    look.Enable();
    foreach (var camReverse in reverseCamera) camReverse.Enable();

    // Store last position and velocity
    CurrentPos = lastPos = target.position;
    lastVelocity = targetVelocity;

    // Set dynamic height and camera offsets
    float dynamicHeight = GetDynamicHeight(0);
    UnityEngine.Vector3 flatForward = UnityEngine.Vector3.ProjectOnPlane(target.forward, UnityEngine.Vector3.up).normalized;
    UnityEngine.Vector3 flatRight = UnityEngine.Vector3.ProjectOnPlane(target.right, UnityEngine.Vector3.up).normalized;

    lastCamPos = offset.x * flatRight + offset.z * flatForward + dynamicHeight * UnityEngine.Vector3.up;
    lastChaseCamOffset = offset.x * UnityEngine.Vector3.right + dynamicHeight * UnityEngine.Vector3.up + offset.z * UnityEngine.Vector3.forward;
    lastFreeLookOffset = lastChaseCamOffset.normalized;
    lastRotation = target.transform.rotation;
    lastLookPosition = target.transform.position + target.transform.forward;
}


    private void ObtainRotation(){
        UnityEngine.Vector3 CamOffsetWithoutY = mainCamera.transform.position - target.position;
        CamOffsetWithoutY.y = 0;
        UnityEngine.Vector3 defaultWithoutY = new UnityEngine.Vector3(offset.x,0,offset.z);
        float currentYaw = ConvertAngle( UnityEngine.Vector3.SignedAngle(defaultWithoutY.normalized, CamOffsetWithoutY.normalized, UnityEngine.Vector3.up) );
        currentRotation = UnityEngine.Quaternion.Euler(0,currentYaw, 0);
    }
    private void FixedUpdate()
    {
        if(rb != null){
            speed = rb.velocity.magnitude; // Vehicle speed
            targetVelocity = rb.velocity;
            localVelocity = transform.InverseTransformDirection(rb.velocity);
            angularVelocity = rb.angularVelocity;
        }
        else{
            speed = nonRigidbodyVelocity.magnitude;
            targetVelocity = nonRigidbodyVelocity;
            localVelocity = transform.InverseTransformDirection(nonRigidbodyVelocity);
            UnityEngine.Quaternion deltaRotation = target.transform.rotation * UnityEngine.Quaternion.Inverse(lastRotation);
                angularVelocity = new UnityEngine.Vector3(
                deltaRotation.eulerAngles.x / Time.fixedDeltaTime,
                deltaRotation.eulerAngles.y / Time.fixedDeltaTime,
                deltaRotation.eulerAngles.z / Time.fixedDeltaTime
            );
            lastRotation = target.transform.rotation; // Store the current rotation for the next calculation

        }
        targetAcceleration = (targetVelocity - lastVelocity) / Time.fixedDeltaTime;

        if(localVelocity.z >= 0){
            yawRotationForwardToReverse = Mathf.Lerp(yawRotationForwardToReverse, 0, Time.fixedDeltaTime * SpeedDampModifier);
        }
        else{
            yawRotationForwardToReverse = Mathf.Lerp(yawRotationForwardToReverse, 180, Time.fixedDeltaTime * SpeedDampModifier);

        }
        CurrentPos = target.transform.position;
        nonRigidbodyVelocity = (CurrentPos - lastPos) / Time.fixedDeltaTime;
        UnityEngine.Vector2 freelookInput = look.ReadValue<UnityEngine.Vector2>();

        if (freelookInput.magnitude > 0.1f)
        {
            isFreelooking = true;
            freelookTimer = 0f; // Reset chase return timer
            if(transitionFactor != 1){
                transitionFactor = Mathf.Lerp(transitionFactor, 1, transitionBlendSpeed * Time.fixedDeltaTime);
            }
        }
        else
        {
            // Increment timer when no freelook input
            freelookInput = UnityEngine.Vector2.zero;
            freelookTimer += Time.fixedDeltaTime;
            isFreelooking = freelookTimer <= returnToChaseDelay;
        }
        if(!isFreelooking && transitionFactor != 0){
            transitionFactor = Mathf.Lerp(transitionFactor, 0, transitionBlendSpeed * Time.fixedDeltaTime);   
        }
        transitionFactor = Mathf.Clamp01(transitionFactor);
        HandleCameraTransition(freelookInput);

        lastPos = CurrentPos;
        lastVelocity = targetVelocity;
    }
    private void ResetToChaseMode()
    {
        // Position the camera directly behind and above the target
        mainCamera.transform.position = target.position + offset;
        mainCamera.transform.LookAt(target);

        // Initialize current rotation to camera's current orientation
        currentRotation = mainCamera.transform.rotation;
        currentRotation2 = mainCamera.transform.rotation;

    }

    private UnityEngine.Vector3 CalculateEllipticalOffset(UnityEngine.Vector3 offset, float horizontalMultiplier, float verticalMultiplier, float depthMultiplier)
    {
        // Apply scaling to simulate an elliptical path
        return new UnityEngine.Vector3(offset.x * horizontalMultiplier, offset.y * verticalMultiplier, offset.z * depthMultiplier);
    }
    private UnityEngine.Quaternion CalculateFreeLookRotation()
    {
        UnityEngine.Vector3 lookDir = (target.position - mainCamera.transform.position).normalized;
        return UnityEngine.Quaternion.LookRotation(lookDir, UnityEngine.Vector3.up);
    }

    
    private UnityEngine.Vector3 CalculateFreeLookPosition(UnityEngine.Vector2 freelookInput, UnityEngine.Vector3 offset){

        // Adjust the offsets for elliptical movement
        float yaw = freelookInput.x * freelookSensitivityX;
        float pitch2 = -freelookInput.y * freelookSensitivityY;
        float dynamicHeight = GetDynamicHeight(speed);

        UnityEngine.Vector3 DynamicOffset = 
            offset.x * UnityEngine.Vector3.right +
            dynamicHeight * UnityEngine.Vector3.up +        // Adjusted height
            offset.z * UnityEngine.Vector3.forward; 
        UnityEngine.Vector3 a = mainCamera.transform.position - target.position;
        a.y = 0;
        UnityEngine.Vector3 defaultWithoutY = DynamicOffset;
        defaultWithoutY.y = 0;
       
        UnityEngine.Vector3 flatTargetVelocity = UnityEngine.Vector3.ProjectOnPlane(targetVelocity, UnityEngine.Vector3.up); // Forward without vertical tilt

        UnityEngine.Vector3 flatTargetVelocityDirection = flatTargetVelocity.normalized;

        currentRotation *= UnityEngine.Quaternion.Euler(0, yaw, 0f);
        currentRotation2 *= UnityEngine.Quaternion.Euler(pitch2, 0f, 0f);

        float upperBound = ConvertAngle(180 * Mathf.Lerp(1.0f, 0.33f, Mathf.Clamp01(flatTargetVelocity.magnitude/20)));
        float lowerBound = ConvertAngle(-180 * Mathf.Lerp(1.0f, 0.33f, Mathf.Clamp01(flatTargetVelocity.magnitude/20)));


        if(upperBound<lowerBound){
            float temp = upperBound;
            upperBound = lowerBound;
            lowerBound = temp;
        }
        
        if(currentRotation.eulerAngles.y>180){
            yaw = Mathf.Clamp(currentRotation.eulerAngles.y - 360,lowerBound,upperBound);
        }
        else{
            yaw = Mathf.Clamp(currentRotation.eulerAngles.y,lowerBound,upperBound);
        }

        float currentYawDot = UnityEngine.Vector3.Dot(defaultWithoutY.normalized, a.normalized);
        float pitch1 = ConvertAngle(target.rotation.eulerAngles.x);
        float roll1 = ConvertAngle(target.rotation.eulerAngles.z);
       

        float centerPitch = pitch1 * currentYawDot;
        float lowerPitchBound = centerPitch - freelookClamp;
        float upperPitchBound = centerPitch + freelookClamp;


        if(currentRotation2.eulerAngles.x>180){
            pitch2 = Mathf.Clamp(currentRotation2.eulerAngles.x - 360, lowerPitchBound, upperPitchBound);
        }
        else{
            pitch2 = Mathf.Clamp(currentRotation2.eulerAngles.x, lowerPitchBound, upperPitchBound);
        }
        
        currentRotation = UnityEngine.Quaternion.Euler(pitch2, yaw, roll1 * currentYawDot);
        currentRotation2 = UnityEngine.Quaternion.Euler(pitch2, 0, roll1 * currentYawDot);
        

        UnityEngine.Vector3 rotatedOffsetY = currentRotation2 * DynamicOffset;

        UnityEngine.Vector3 rotatedOffsetXZ;
        UnityEngine.Vector3 rota = currentRotation * flatTargetVelocityDirection;
        rota.y = 0;
        float dynamicDominantXZ;
        float nonDominantXZ;
        if(Mathf.Abs(DynamicOffset.z) >= Mathf.Abs(DynamicOffset.x)){
            dynamicDominantXZ = DynamicOffset.z;
            nonDominantXZ = DynamicOffset.x;
        }
        else{
            dynamicDominantXZ = DynamicOffset.x;
            nonDominantXZ = DynamicOffset.z;
        }
        if(flatTargetVelocity.magnitude > 0.1f){
            rotatedOffsetXZ = rota * dynamicDominantXZ;
            rotatedOffsetXZ += UnityEngine.Vector3.Cross(rota,UnityEngine.Vector3.up) * nonDominantXZ;
            lastFreeLookOffset = flatTargetVelocityDirection;
        }
        else{
            UnityEngine.Vector3 po = currentRotation * DynamicOffset;
            po.y = 0;
            rotatedOffsetXZ = po; 
            rotatedOffsetXZ += UnityEngine.Vector3.Cross(po,UnityEngine.Vector3.up) * nonDominantXZ;
        }
    
        rotatedOffsetXZ.y = 0;
        rotatedOffsetXZ += UnityEngine.Vector3.up * dynamicHeight;

        
        UnityEngine.Vector3 ellipticalOffsetY = CalculateEllipticalOffset(rotatedOffsetY, 0f, 1f, 1f);
        UnityEngine.Vector3 horizontalOffset = new UnityEngine.Vector3(rotatedOffsetXZ.x, 0f, rotatedOffsetXZ.z);

        UnityEngine.Vector3 ellipticalOffsetXZ = CalculateEllipticalOffset(horizontalOffset, 1f, 0f, 1f);

        UnityEngine.Vector3 finalOffset = new UnityEngine.Vector3(ellipticalOffsetXZ.x, ellipticalOffsetY.y, ellipticalOffsetXZ.z);
        if(reverseCamera[0].IsPressed() || reverseCamera[1].IsPressed()){
            finalOffset.z *= -1;
            finalOffset.x *= -1;
        }
        UnityEngine.Vector3 desiredPosition = target.position + finalOffset;        
    
            
        return desiredPosition;
    }

    UnityEngine.Vector3 CalculateChasePositionOffset(float speed, UnityEngine.Vector3 offset,bool isReversed, bool isReversedPerformedThisFrame , bool isReversedReleasedThisFrame){
        bool thisFrameReversed = isReversedPerformedThisFrame || isReversedReleasedThisFrame;
        UnityEngine.Vector3 dynamicOffset = GetDynamicOffset(speed, offset, isReversed, thisFrameReversed);


        return target.position + dynamicOffset;
    }

    UnityEngine.Vector3 GetDynamicOffset(float speed, UnityEngine.Vector3 offset, bool isReversedCam, bool isReversedCamThisFrame){

        UnityEngine.Vector3 flatForward = UnityEngine.Vector3.ProjectOnPlane(transform.forward, UnityEngine.Vector3.up).normalized; // Forward without vertical tilt
        UnityEngine.Vector3 flatRight = UnityEngine.Vector3.ProjectOnPlane(transform.right, UnityEngine.Vector3.up).normalized; // right without vertical tilt
        float dynamicHeight = GetDynamicHeight(speed);
        

        UnityEngine.Vector3 flatTargetVelocity = UnityEngine.Vector3.ProjectOnPlane(targetVelocity, UnityEngine.Vector3.up); // Forward without vertical tilt
        UnityEngine.Vector3 flatTargetAcceleration = UnityEngine.Vector3.ProjectOnPlane(targetAcceleration, UnityEngine.Vector3.up); // Forward without vertical tilt
        UnityEngine.Vector3 defaultWithoutY = new UnityEngine.Vector3(offset.x, 0, offset.z);
        
        UnityEngine.Vector3 dynamicOffset;

        UnityEngine.Vector3 defaultOffset = 
            offset.x * flatRight +
            dynamicHeight * UnityEngine.Vector3.up +        
            offset.z * flatForward;         

        UnityEngine.Vector3 CamOffsetFromTargetNoY = mainCamera.transform.position - target.position;
        CamOffsetFromTargetNoY.y = 0;

        float currentYaw = UnityEngine.Vector3.SignedAngle(defaultWithoutY.normalized, CamOffsetFromTargetNoY.normalized, UnityEngine.Vector3.up);
        currentYaw = ConvertAngle(currentYaw);

        float angularYaw = ConvertAngle(Mathf.Clamp(-rb.angularVelocity.y * 10, -15, 15));
        float pitch = ConvertAngle(target.rotation.eulerAngles.x);
        float roll = ConvertAngle(target.rotation.eulerAngles.z);
        float currentYawDot = UnityEngine.Vector3.Dot(defaultWithoutY.normalized, CamOffsetFromTargetNoY.normalized);
        pitch = currentYawDot * pitch;
        roll = currentYawDot * roll;

        
        if (flatTargetVelocity.magnitude < 1)
        {
            if(!isFreelooking){
                returnToForwardTimer += Time.fixedDeltaTime;
                if (returnToForwardTimer > ReturnToForwardDelay)
                {
                    if(lastCamPos!=defaultOffset){
                        float targetYaw = 0;
                        if(isReversedCam){
                            if(Mathf.Sign(currentYaw) >=0){
                                targetYaw = 180f;
                            }
                            else{
                                targetYaw = -180f;
                            }
                        }
                        
                        float yaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref chasePosRevert,YawDampModifier);
                        if(Mathf.Abs(yaw)<0.1f){
                            yaw = 0f;
                        }
                        UnityEngine.Quaternion smoothedRotation = UnityEngine.Quaternion.Euler(pitch, yaw, roll);
                        UnityEngine.Quaternion smoothedRotationY = UnityEngine.Quaternion.Euler(pitch, 0,  roll);

                        defaultWithoutY = smoothedRotation * defaultWithoutY;
                        lastCamPos = UnityEngine.Vector3.up * dynamicHeight + defaultWithoutY;
                        UnityEngine.Vector3 dynamicOffsetYY = smoothedRotationY * lastCamPos;
                        lastCamPos = new UnityEngine.Vector3(lastCamPos.x,dynamicOffsetYY.y,lastCamPos.z);
                    }
                }
            }
            else{
                returnToForwardTimer = 0;
            }
                UnityEngine.Quaternion mYaw = UnityEngine.Quaternion.Euler(0,-angularYaw * 2 * Mathf.Sign(offset.z), 0);
                dynamicOffset = mYaw * lastCamPos;
                if(isReversedCam){
                    dynamicOffset.x *= -1;
                    dynamicOffset.z *= -1;
                }
        }
        else
        {
            float targetYaw = UnityEngine.Vector3.SignedAngle(defaultWithoutY, -flatTargetVelocity, UnityEngine.Vector3.up);
            if(isReversedCam){
                targetYaw = UnityEngine.Vector3.SignedAngle(defaultWithoutY, flatTargetVelocity, UnityEngine.Vector3.up);
            }
            targetYaw = ConvertAngle(targetYaw);
            float angleDistance = Mathf.Abs(targetYaw - currentYaw)/Time.fixedDeltaTime;
            UnityEngine.Quaternion smoothedRotation;

            if(angleDistance == 0){
                smoothedRotation = UnityEngine.Quaternion.Euler(pitch, targetYaw, roll);
            }
            else{
                float yaw = Mathf.LerpAngle(currentYaw, targetYaw, Mathf.Abs(flatTargetAcceleration.magnitude)/angleDistance * Mathf.Abs(flatTargetVelocity.magnitude));
                if(isReversedCamThisFrame){
                    smoothedRotation = UnityEngine.Quaternion.Euler(pitch, targetYaw, roll);
                }
                else{
                    smoothedRotation = UnityEngine.Quaternion.Euler(pitch, yaw, roll);
                }
            }
            UnityEngine.Quaternion smoothedRotationY = UnityEngine.Quaternion.Euler(pitch, 0, roll);
            
            defaultWithoutY =  smoothedRotation * defaultWithoutY;
            dynamicOffset = UnityEngine.Vector3.up * dynamicHeight + defaultWithoutY;
            UnityEngine.Vector3 dynamicOffsetY = smoothedRotationY * dynamicOffset;
            dynamicOffset = new UnityEngine.Vector3(dynamicOffset.x, dynamicOffsetY.y, dynamicOffset.z);
            lastCamPos = dynamicOffset;
            returnToForwardTimer = 0f;

        }
        
        
        return dynamicOffset;
    }
    float ConvertAngle(float angle){
        float newAngle;
        if(angle > 180){
            newAngle = angle - 360;
        }
        else if(angle < -180){
            newAngle = angle + 360;
        }
        else{
            newAngle = angle;
        }
        return newAngle;
    }

    float GetDynamicHeight(float speed){
        float terrainAdjustment = UnityEngine.Vector3.Dot(transform.up, UnityEngine.Vector3.up); // Dot product to detect uphill/downhill

        float dynamicHeight = Mathf.Lerp(offset.y, offset.y + Mathf.Sign(terrainAdjustment) * 3.0f, Mathf.Clamp01(Mathf.Abs(terrainAdjustment))) * Mathf.Lerp(1.0f, 1.2f, speed / 50);
        
        return dynamicHeight;
    }
    
    UnityEngine.Quaternion CalculateChaseRotation(){
        UnityEngine.Vector3 flatTargetVelocity = targetVelocity; // Forward without vertical tilt
        flatTargetVelocity.y = 0;
        if(reverseCamera[0].IsPressed() || reverseCamera[1].IsPressed()){
            flatTargetVelocity *= -1;
        }
        flatTargetVelocity.y = targetVelocity.y;

        UnityEngine.Quaternion angularRotation  = UnityEngine.Quaternion.Euler(0,Mathf.Clamp(angularVelocity.y * 2,-90,90), 0);

        UnityEngine.Vector3 lookAtPosition = target.position + angularRotation * flatTargetVelocity.normalized;

        UnityEngine.Vector3 defaultLookPosition = (target.position - mainCamera.transform.position).normalized;
;
        
        if (targetVelocity.magnitude < 0.1f)
        {
            if (lastLookPosition != defaultLookPosition)
            {
                returnToForwardRotationTimer += Time.fixedDeltaTime;
                if (returnToForwardRotationTimer > ReturnToForwardDelay)
                {
                    lastLookPosition = UnityEngine.Vector3.Lerp(lastLookPosition, defaultLookPosition, transitionBlendSpeed * Time.fixedDeltaTime);
                }
            }
            lookAtPosition = lastLookPosition; // Maintain last valid look position
        }
        else
        {
            lastLookPosition = lookAtPosition;
            returnToForwardRotationTimer = 0; // Reset timer if the target is moving
        }

        return UnityEngine.Quaternion.LookRotation(lookAtPosition - mainCamera.transform.position);
    }
    float ZoomFov(float speed){
        
        float zoomOutFactor = Mathf.Lerp(1.0f, ZoomModifierMultiplier, Mathf.Clamp01(speed / 50f)); // Gradual zoom out with speed
        return Mathf.SmoothDamp(mainCamera.fieldOfView, baseFOV * zoomOutFactor, ref FOVvelocity, smoothTimeFOV);
    }
    
private void HandleCameraTransition(UnityEngine.Vector2 freelookInput)
{
    // Compute chase and freelook target positions and rotations
    bool isReversed = reverseCamera[0].IsPressed() || reverseCamera[1].IsPressed();
    bool isReversedPressedThisFrame = reverseCamera[0].WasPressedThisFrame() || reverseCamera[1].WasPressedThisFrame();
    bool isReversedPerformedThisFrame = reverseCamera[0].WasPerformedThisFrame() || reverseCamera[1].WasPerformedThisFrame();
    bool isReversedReleasedThisFrame = reverseCamera[0].WasReleasedThisFrame() || reverseCamera[1].WasPerformedThisFrame();
    UnityEngine.Vector3 CameraToTarget = mainCamera.transform.position - target.position;
    UnityEngine.Vector3 CameraToTargetDirection = CameraToTarget.normalized;
    if (InvertXFreelook) freelookInput.x *= -1;
    if (InvertYFreelook) freelookInput.y *= -1;
    

    UnityEngine.Vector3 chasePositionOffset = CalculateChasePositionOffset(speed, offset, isReversed, isReversedPerformedThisFrame, isReversedReleasedThisFrame) - target.position;
    UnityEngine.Vector3 chaseTargetPosition = target.position + chasePositionOffset;
    UnityEngine.Vector3 chasePosition;
    UnityEngine.Quaternion chaseRotation;

    UnityEngine.Vector3 chasePositionAwayFromCamera =  UnityEngine.Vector3.SmoothDamp(mainCamera.transform.position, chaseTargetPosition, ref velocity, smoothTime) ;
    UnityEngine.Vector3 chasePositionTowardCamera = chaseTargetPosition;
    UnityEngine.Quaternion chaseTargetRotation = CalculateChaseRotation();
    float smoothTimeAdjustment = Mathf.Lerp(1f, 2f, speed / 50);
    UnityEngine.Quaternion chaseRotationAwayFromCamera = UnityEngine.Quaternion.Euler(
        Mathf.SmoothDampAngle(mainCamera.transform.rotation.eulerAngles.x, chaseTargetRotation.eulerAngles.x, ref rotVelx, 0 * camRotationSmoothTime * smoothTimeAdjustment),
        Mathf.SmoothDampAngle(mainCamera.transform.rotation.eulerAngles.y, chaseTargetRotation.eulerAngles.y, ref rotVely, 0 * camRotationSmoothTime * smoothTimeAdjustment),
        Mathf.SmoothDampAngle(mainCamera.transform.rotation.eulerAngles.z, chaseTargetRotation.eulerAngles.z, ref rotVelz, 0 * camRotationSmoothTime * smoothTimeAdjustment)
    );

    float dotProduct = UnityEngine.Vector3.Dot(velocity.normalized, CameraToTargetDirection);
    if(dotProduct > 0){
        transitionToDirection = Mathf.SmoothDamp(transitionToDirection, 1, ref ForwardtoReverseTransVel, smoothTime);
    }
    else{
        transitionToDirection = Mathf.SmoothDamp(transitionToDirection, 0, ref ReverseToForwardTransVel, smoothTime);
    }
    transitionToDirection = Mathf.Clamp01(transitionToDirection);
    

    if(isReversedPerformedThisFrame || isReversedReleasedThisFrame){
        chasePosition = chaseTargetPosition;
        chaseRotation = chaseTargetRotation;
    }
    else{
        chasePosition = UnityEngine.Vector3.Lerp(chasePositionAwayFromCamera, chasePositionTowardCamera, transitionToDirection); 
        chaseRotation = chaseRotationAwayFromCamera;
    }
    UnityEngine.Vector3 freelookPosition = CalculateFreeLookPosition(freelookInput, offset);
    UnityEngine.Quaternion freelookRotation = CalculateFreeLookRotation();

    // Blend between chase and freelook
    UnityEngine.Vector3 blendedPosition = UnityEngine.Vector3.Lerp(chasePosition, freelookPosition, transitionFactor);
    UnityEngine.Quaternion blendedRotation = UnityEngine.Quaternion.Slerp(chaseRotation, freelookRotation, transitionFactor);

    // Apply blended position and rotation
    if(!customizingCameraSettings){
        if(!pauseGame.paused){
            if(freelookOnly){
                mainCamera.transform.position = freelookPosition;
    
                mainCamera.transform.rotation = freelookRotation;
            }
            else if(chaseOnly){
                mainCamera.transform.position = chasePosition;
    
                mainCamera.transform.rotation = chaseRotation;
            }
            else{
                mainCamera.transform.position = blendedPosition;
    
                mainCamera.transform.rotation = blendedRotation;
            }
            mainCamera.fieldOfView = ZoomFov(speed);

        }
    }
    else{
        if(!freelookOnly){
            mainCamera.transform.position = CalculateChasePositionOffset(speed, offset, isReversed,isReversedPerformedThisFrame, isReversedReleasedThisFrame);
    
            mainCamera.transform.rotation = CalculateChaseRotation();
        }

        if(showMaxFOV == false){
            mainCamera.fieldOfView = baseFOV;
        }
        else{
            mainCamera.fieldOfView = maxZoomFOV;
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
    public void SaveData(ref GameData data)
    {
        data.freelookOnly = freelookOnly;
        data.distance = offset.z;
        data.height = offset.y;
        data.horrizontalOffset = offset.x;
        data.baseFOV = baseFOV;
        data.InvertXFreelook = InvertXFreelook;
        data.InvertYFreelook = InvertYFreelook;
        data.yCameraSensitivity = freelookSensitivityY;
        data.xCameraSensitivity = freelookSensitivityX;
        data.maxFOV = maxZoomFOV;

    }
    public void LoadData(GameData data)
    {
        freelookOnly = data.freelookOnly;
        offset.z = data.distance;
        offset.y = data.height;
        offset.x = data.horrizontalOffset;
        baseFOV = data.baseFOV;
        InvertXFreelook = data.InvertXFreelook;
        InvertYFreelook = data.InvertYFreelook;
        freelookSensitivityY = data.yCameraSensitivity;
        freelookSensitivityX = data.xCameraSensitivity;
        maxZoomFOV = data.maxFOV;
    }

    private void OnDestroy(){
        DisposeActions(reverseCamera);
        look.Dispose();
    }
}
