using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using System.Collections;
using UnityEngine.InputSystem.Controls;
using TMPro.Examples;

public class KeyDetect : MonoBehaviour
{
    public TMP_InputField inputField;
    public List<TMP_InputField> keyInputFields;
    public List<TMP_InputField> xboxControlInputFields;
    public bool isFocused = false;  // Flag to track if the input field has focus
    public bool isDetecting { get; private set; }  // Flag to start detection after focus delay
    string currentControl;
    public GetInputSystem getInputSystem;
    [SerializeField] private InputActionAsset IA_Asset;

    BackSound backSound;
    ErrorSound errorSound;

    private void Awake()
    {
        keyInputFields = new List<TMP_InputField>();
        xboxControlInputFields = new List<TMP_InputField>();
        backSound = new BackSound();
        errorSound = new ErrorSound();
        isDetecting = false;
        // Get all TMP_InputField components in the scene
        TMP_InputField[] allInputFields = FindObjectsOfType<TMP_InputField>();

        // Filter by the tag "KeyInputField"
        foreach (TMP_InputField inputField in allInputFields)
        {
            if (inputField.CompareTag("KeyInputField"))
            {
                keyInputFields.Add(inputField);
                //Debug.Log(inputField.gameObject.name);
            }
            else if (inputField.CompareTag("XboxButtonInputField"))
            {
                xboxControlInputFields.Add(inputField);
            }
            inputField.interactable = false;
        }


    }

    void Update()
    {
        if (backSound == null)
        {
            backSound = new BackSound();
        }
        if (errorSound == null)
        {
            errorSound = new ErrorSound();
        }
        // Check if the selected GameObject has an Input Field
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected != null)
        {
            inputField = selected.GetComponent<TMP_InputField>();

            if (inputField != null && (compareKeyboardTag() || compareXboxInputTag()))
            {
                if (!isFocused)
                {
                    currentControl = inputField.text;
                    isFocused = true;  // Set the flag to true, now it's focused
                    if (!isDetecting) StartCoroutine(StartKeyDetectionDelay());
                    isDetecting = true;
                }
                else if (isDetecting)
                {
                    DetectKeyInput();

                }
            }
            else
            {
                // Reset focus if the input field is not the right one
                isFocused = false;
                isDetecting = false;
                inputField = null;
            }
        }
        else
        {
            // Reset focus if no GameObject is selected
            isFocused = false;
            isDetecting = false;
            inputField = null;
        }
    }

    bool compareKeyboardTag()
    {
        if (inputField.gameObject.CompareTag("KeyInputField"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool compareXboxInputTag()
    {
        if (inputField.gameObject.CompareTag("XboxButtonInputField"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private IEnumerator StartKeyDetectionDelay()
    {
        yield return new WaitForSeconds(0.1f);  // Short delay to ignore initial click
    }
    private void DetectKeyInput()
    {
        var map = IA_Asset.FindActionMap("UI");
        var actions = map.actions;
        foreach (var action in actions)
        {
            if (action.enabled)
            {
                action.Disable();
            }
        }
        foreach (KeyControl key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                string buttonPath = $"<Keyboard>/{key.name}"; // Create the path string
                if (key.name == "escape")
                {
                    EventSystem.current.SetSelectedGameObject(FindSiblingButton(inputField.transform).gameObject);
                    backSound.GetBackSound().StartEventSound();
                }
                else
                {
                    if (compareKeyboardTag())
                    {
                        inputField.text = InputControlPath.ToHumanReadableString(buttonPath); // Set the input field text to the button path
                        EventSystem.current.SetSelectedGameObject(FindSiblingButton(inputField.transform).gameObject);
                        InputAction action = getInputSystem.keyboard.FindAction(inputField.GetComponent<ExtraKeyInputInfo>().attachedActionName);
                        getInputSystem.ApplyNewBinding(action, inputField.GetComponent<ExtraKeyInputInfo>().actionBindingIndex, buttonPath);
                        inputField.interactable = false;

                    }
                    else
                    {
                        errorSound.GetErrorSound().StartEventSound();
                        inputField.text = currentControl;
                        EventSystem.current.SetSelectedGameObject(FindSiblingButton(inputField.transform).gameObject);
                        inputField.interactable = false;
                    }
                }
            }
        }

        foreach (XInputController xboxInput in XInputController.all)
        {
            if (xboxInput.startButton.wasPressedThisFrame)
            {
                backSound.GetBackSound().StartEventSound();
                EventSystem.current.SetSelectedGameObject(FindSiblingButton(inputField.transform).gameObject);
                inputField.interactable = false;

            }
            else
            {
                if (compareXboxInputTag())
                {
                    CheckXboxButtons(xboxInput);
                }

            }
        }
        foreach (var action in actions)
        {
            if (!action.enabled)
            {
                action.Enable();
            }
        }
    }
    private void CheckXboxButtons(XInputController gamepad)
    {
        // Check each button for a press
        if (gamepad.buttonSouth.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/buttonSouth");
        }
        else if (gamepad.buttonEast.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/buttonEast");
        }
        else if (gamepad.buttonWest.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/buttonWest");
        }
        else if (gamepad.buttonNorth.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/buttonNorth");
        }
        else if (gamepad.leftShoulder.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/leftShoulder");
        }
        else if (gamepad.rightShoulder.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/rightShoulder");
        }
        else if (gamepad.selectButton.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/selectButton");
        }
        else if (gamepad.startButton.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/startButton");
        }

        // Check triggers (treated as buttons when they cross a threshold)
        else if (gamepad.leftTrigger.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/leftTrigger");
        }
        else if (gamepad.rightTrigger.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/rightTrigger");
        }

        // Check D-pad directions
        else if (gamepad.dpad.up.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/dpad/up");
        }
        else if (gamepad.dpad.down.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/dpad/down");
        }
        else if (gamepad.dpad.left.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/dpad/left");
        }
        else if (gamepad.dpad.right.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/dpad/right");
        }

        // Check pressing down on the left and right sticks
        else if (gamepad.leftStickButton.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/leftStickPress");
        }
        else if (gamepad.rightStickButton.wasPressedThisFrame)
        {
            UpdateInputField("<XInputController>/rightStickPress");
        }
    }


    private void UpdateInputField(string buttonPath)
    {

        inputField.text = InputControlPath.ToHumanReadableString(buttonPath); // Set the input field text to the button path
        EventSystem.current.SetSelectedGameObject(FindSiblingButton(inputField.transform).gameObject);
        InputAction action = getInputSystem.xbox.FindAction(inputField.GetComponent<ExtraKeyInputInfo>().attachedActionName);
        getInputSystem.ApplyNewBinding(action, inputField.GetComponent<ExtraKeyInputInfo>().actionBindingIndex, buttonPath);
        inputField.interactable = false;



    }
    private void OnDestroy()
    {
        keyInputFields.Clear();
        xboxControlInputFields.Clear();
        keyInputFields.Capacity = 0;
        xboxControlInputFields.Capacity = 0;
    }
    Button FindSiblingButton(Transform inputFieldTransform)
    {
        // Loop through all siblings of the input field
        foreach (Transform sibling in inputFieldTransform.parent)
        {
            // Check if the sibling is not the input field itself
            if (sibling != inputFieldTransform)
            {
                // Try to get the Button component from the sibling
                Button button = sibling.GetComponent<Button>();
                if (button != null)
                {
                    return button; // Return the first found button sibling
                }
            }
        }

        // Return null if no button sibling was found
        return null;
    }
}
