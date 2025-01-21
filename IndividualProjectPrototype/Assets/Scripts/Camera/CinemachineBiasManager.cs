using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CinemachineBiasManager : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;  // Reference to your Cinemachine FreeLook camera
    public Transform followObject;               // Object to follow (your player or moving object)

    [SerializeField] private Rigidbody rb;
    [SerializeField] private PauseGame pause;    // Reference to your PauseGame class
    bool lastPauseState;
    public GetInputSystem getInputSystem;

    private InputAction reverseCameraActionXbox;
    private InputAction reverseCameraActionKeyboard;

    private void Awake()
    {
        if (getInputSystem == null)
        {
            Debug.LogError("GetInputSystem reference is missing.");
            return;
        }

        // Retrieve the ReverseCamera input action
        

        
    }

    private void OnEnable()
    {
        reverseCameraActionXbox = getInputSystem.xbox.FindAction("ReverseCamera");
        reverseCameraActionKeyboard = getInputSystem.keyboard.FindAction("ReverseCamera");
        // Enable the input action and subscribe to performed and canceled events
        if (reverseCameraActionXbox != null)
        {
            reverseCameraActionXbox.Enable();
            reverseCameraActionXbox.performed += OnReverseCameraPerformed;
            reverseCameraActionXbox.canceled += OnReverseCameraCanceled;
        }
        if (reverseCameraActionKeyboard != null)
        {
            reverseCameraActionKeyboard.Enable();
            reverseCameraActionKeyboard.performed += OnReverseCameraPerformed;
            reverseCameraActionKeyboard.canceled += OnReverseCameraCanceled;
        }

       
    }

    private void OnDisable()
    {
        // Unsubscribe from events and disable input action
        if (reverseCameraActionXbox != null)
        {
            reverseCameraActionXbox.performed -= OnReverseCameraPerformed;
            reverseCameraActionXbox.canceled -= OnReverseCameraCanceled;
            reverseCameraActionXbox.Disable();
        }
        if (reverseCameraActionKeyboard != null)
        {
            reverseCameraActionKeyboard.performed -= OnReverseCameraPerformed;
            reverseCameraActionKeyboard.canceled -= OnReverseCameraCanceled;
            reverseCameraActionKeyboard.Disable();
        }

    }

    private void Start()
    {
        followObject = freeLookCamera.Follow;

        rb = followObject.GetComponent<Rigidbody>();
        lastPauseState = pause.paused;
    }
    public void Update()
    {
        if(!pause.paused && lastPauseState)
        {
            OnGameResumed();
        }

        lastPauseState = pause.paused;
    }
    private void OnReverseCameraPerformed(InputAction.CallbackContext context)
    {
        if (!pause.paused) // Only reverse the camera if not paused
        {
            freeLookCamera.m_Heading.m_Bias = 180f; // Reverse the camera
        }
    }

    private void OnReverseCameraCanceled(InputAction.CallbackContext context)
    {
        if (!pause.paused) // Only reset if not paused
        {
            freeLookCamera.m_Heading.m_Bias = 0f; // Reset the camera bias
        }
    }

    private void OnGameResumed()
    {
        // Check if reverse button is not held when resuming
        if (!reverseCameraActionXbox.IsPressed() && !reverseCameraActionKeyboard.IsPressed())
        {
            freeLookCamera.m_Heading.m_Bias = 0f; // Reset the camera bias when resumed
        }
        else if (!reverseCameraActionXbox.IsPressed() || !reverseCameraActionKeyboard.IsPressed())
        {
            freeLookCamera.m_Heading.m_Bias = 180f; // Reverse the camera

        }
    }
}
