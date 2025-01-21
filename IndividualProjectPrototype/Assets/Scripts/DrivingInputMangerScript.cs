using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class DrivingInputMangerScript
{
    protected CarControlls carControlls { get; private set; }
    protected InputActionAsset inputAsset { get; private set; }
    public DrivingInputMangerScript()
    {
        carControlls = new CarControlls();
        inputAsset = carControlls.asset;
    }
    public DrivingInputMangerScript(CarControlls carControlls)
    {
        this.carControlls = carControlls;
        inputAsset = carControlls.asset;
    }
    public DrivingInputMangerScript(CarControlls carControlls, InputActionAsset inputAsset)
    {
        this.carControlls = carControlls;
        this.inputAsset = inputAsset;
    }

    ~DrivingInputMangerScript()
    {

    }
}

public class DrivingManagerScriptXboxController : DrivingInputMangerScript
{
    InputActionMap XboxControls;
    List<InputAction> inputActions;
    public DrivingManagerScriptXboxController() : base()
    {
        XboxControls = inputAsset.FindActionMap("CarControlsXboxController");
        InitalizeActions();

    }

    public DrivingManagerScriptXboxController(CarControlls carControlls) : base(carControlls)
    {
        XboxControls = inputAsset.FindActionMap("CarControlsXboxController");
        InitalizeActions();

    }
    public DrivingManagerScriptXboxController(CarControlls carControlls, InputActionAsset inputAsset) : base(carControlls, inputAsset)
    {
        XboxControls = inputAsset.FindActionMap("CarControlsXboxController");
        InitalizeActions();
    }
    private void InitalizeActions()
    {
        foreach (InputAction action in XboxControls.actions)
        {
            inputActions.Add(action);
        }
    }
    bool ControlIsTypeTheSame(InputBinding currentBinding, InputBinding newBinding)
    {
        string currentControlTypePath = currentBinding.effectivePath;
        string newControlTypePath = newBinding.effectivePath;
        return ControlIsTypePathTheSame(currentControlTypePath, newControlTypePath);
    }
    bool ControlIsTypePathTheSame(string currentBindingPath, string newBindingPath)
    {
        string currentControlType = InputControlPath.TryGetControlLayout(currentBindingPath);
        string newControlType = InputControlPath.TryGetControlLayout(newBindingPath);
        return CheckControlType(currentControlType, newControlType);
    }
    bool CheckControlType(string currentDevice, string targetDevice)
    {
        if (currentDevice == null || targetDevice == null)
        {
            if (currentDevice == null)
            {
                Debug.LogError("Current device found is null");
            }
            if (targetDevice == null)
            {
                Debug.LogError("new control device found is null");
            }
            return false;
        }
        else if (currentDevice != targetDevice)
        {
            Debug.Log("control types provided were not the same");
            return false;
        }
        else
        {
            return true;
        }
    }
    private InputAction GetInputAction(string InputActionName)
    {
        foreach (InputAction action in inputActions)
        {
            if (action.name == InputActionName)
            {
                return action;
            }
        }
        return null;

    }

    private InputAction GetInputAction(Guid InputActionID)
    {
        foreach (InputAction action in inputActions)
        {
            if (action.id == InputActionID)
            {
                return action;
            }
        }
        return null;

    }

    bool CheckWithinNumOfBindings(int bindingIndex, int numOfBindings)
    {
        return (bindingIndex >= 0 && bindingIndex < numOfBindings);
    }
    public void setNewXboxControlBind(string InputActionName, int bindingIndex, string newBindingPath)
    {
        InputAction action = GetInputAction(InputActionName);
        if (CheckWithinNumOfBindings(bindingIndex, action.bindings.Count))
        {
            var currentBinding = action.bindings[bindingIndex];

            if (ControlIsTypePathTheSame(currentBinding.effectivePath, newBindingPath))
            {
                XboxControls.ApplyBindingOverride(new InputBinding
                {
                    path = currentBinding.path,
                    overridePath = newBindingPath
                });

                Debug.Log($"Binding changed from {currentBinding.effectivePath} to {newBindingPath}");
            }
            else
            {
                Debug.LogWarning("New binding path does not match the current control type.");
            }
        }
    }
    public void setNewXboxControlBind(Guid InputActionID, int bindingIndex, string newBindingPath)
    {
        InputAction action = GetInputAction(InputActionID);
        if (CheckWithinNumOfBindings(bindingIndex, action.bindings.Count))
        {
            var currentBinding = action.bindings[bindingIndex];

            if (ControlIsTypePathTheSame(currentBinding.effectivePath, newBindingPath))
            {
                XboxControls.ApplyBindingOverride(new InputBinding
                {
                    path = currentBinding.path,
                    overridePath = newBindingPath
                });

                Debug.Log($"Binding changed from {currentBinding.effectivePath} to {newBindingPath}");
            }
            else
            {
                Debug.LogWarning("New binding path does not match the current control type.");
            }
        }
    }
    public List<string> GetAllBindingPaths(string inputActionName)
    {
        InputAction action = GetInputAction(inputActionName);
        List<string> bindingPaths = new List<string>();

        if (action != null)
        {
            foreach (var binding in action.bindings)
            {
                bindingPaths.Add(binding.effectivePath);
            }
        }
        else
        {
            Debug.LogWarning($"Input action '{inputActionName}' not found.");
        }

        return bindingPaths;
    }

    public List<string> GetAllBindingPaths(Guid inputActionID)
    {
        InputAction action = GetInputAction(inputActionID);
        List<string> bindingPaths = new List<string>();

        if (action != null)
        {
            foreach (var binding in action.bindings)
            {
                bindingPaths.Add(binding.effectivePath);
            }
        }
        else
        {
            Debug.LogWarning($"Input action '{inputActionID}' not found.");
        }

        return bindingPaths;
    }

    ~DrivingManagerScriptXboxController()
    {
        inputActions.Capacity = 0;
        inputActions.Clear();
        inputActions = null;

    }
}

public class DrivingManagerScriptKeyboard : DrivingInputMangerScript
{
    InputActionMap KeyboardControls;
    List<InputAction> inputActions;


    public DrivingManagerScriptKeyboard() : base()
    {
        KeyboardControls = inputAsset.FindActionMap("CarControlsKeyboard");
        InitalizeActions();

    }
    public DrivingManagerScriptKeyboard(CarControlls carControlls) : base(carControlls)
    {
        KeyboardControls = inputAsset.FindActionMap("CarControlsKeyboard");
        InitalizeActions();

    }
    public DrivingManagerScriptKeyboard(CarControlls carControlls, InputActionAsset inputAsset) : base(carControlls, inputAsset)
    {
        KeyboardControls = this.inputAsset.FindActionMap("CarControlsKeyboard");
        InitalizeActions();
    }
    private void InitalizeActions()
    {
        foreach (InputAction action in KeyboardControls.actions)
        {
            inputActions.Add(action);
        }
    }
    bool ControlIsTypeTheSame(InputBinding currentBinding, InputBinding newBinding)
    {
        string currentControlTypePath = currentBinding.effectivePath;
        string newControlTypePath = newBinding.effectivePath;
        return ControlIsTypePathTheSame(currentControlTypePath, newControlTypePath);
    }
    bool ControlIsTypePathTheSame(string currentBindingPath, string newBindingPath)
    {
        string currentControlType = InputControlPath.TryGetControlLayout(currentBindingPath);
        string newControlType = InputControlPath.TryGetControlLayout(newBindingPath);
        return CheckControlType(currentControlType, newControlType);
    }
    bool CheckControlType(string currentDevice, string targetDevice)
    {
        if (currentDevice == null || targetDevice == null)
        {
            if (currentDevice == null)
            {
                Debug.LogError("Current device found is null");
            }
            if (targetDevice == null)
            {
                Debug.LogError("new control device found is null");
            }
            return false;
        }
        else if (currentDevice != targetDevice)
        {
            Debug.Log("control types provided were not the same");
            return false;
        }
        else
        {
            return true;
        }
    }
    private InputAction GetInputAction(string InputActionName)
    {
        foreach (InputAction action in inputActions)
        {
            if (action.name == InputActionName)
            {
                return action;
            }
        }
        return null;

    }

    private InputAction GetInputAction(Guid InputActionID)
    {
        foreach (InputAction action in inputActions)
        {
            if (action.id == InputActionID)
            {
                return action;
            }
        }
        return null;

    }

    bool CheckWithinNumOfBindings(int bindingIndex, int numOfBindings)
    {
        return (bindingIndex >= 0 && bindingIndex < numOfBindings);
    }
    public void setKeyboardControlBind(string InputActionName, int bindingIndex, string newBindingPath)
    {
        InputAction action = GetInputAction(InputActionName);
        if (CheckWithinNumOfBindings(bindingIndex, action.bindings.Count))
        {
            var currentBinding = action.bindings[bindingIndex];

            if (ControlIsTypePathTheSame(currentBinding.effectivePath, newBindingPath))
            {
                KeyboardControls.ApplyBindingOverride(new InputBinding
                {
                    path = currentBinding.path,
                    overridePath = newBindingPath
                });

                Debug.Log($"Binding changed from {currentBinding.effectivePath} to {newBindingPath}");
            }
            else
            {
                Debug.LogWarning("New binding path does not match the current control type.");
            }
        }
    }
    public void setKeyboardControlBind(Guid InputActionID, int bindingIndex, string newBindingPath)
    {
        InputAction action = GetInputAction(InputActionID);
        if (CheckWithinNumOfBindings(bindingIndex, action.bindings.Count))
        {
            var currentBinding = action.bindings[bindingIndex];

            if (ControlIsTypePathTheSame(currentBinding.effectivePath, newBindingPath))
            {
                KeyboardControls.ApplyBindingOverride(new InputBinding
                {
                    path = currentBinding.path,
                    overridePath = newBindingPath
                });

                Debug.Log($"Binding changed from {currentBinding.effectivePath} to {newBindingPath}");
            }
            else
            {
                Debug.LogWarning("New binding path does not match the current control type.");
            }
        }
    }
    public List<string> GetAllBindingPaths(string inputActionName)
    {
        InputAction action = GetInputAction(inputActionName);
        List<string> bindingPaths = new List<string>();

        if (action != null)
        {
            foreach (var binding in action.bindings)
            {
                bindingPaths.Add(binding.effectivePath);
            }
        }
        else
        {
            Debug.LogWarning($"Input action '{inputActionName}' not found.");
        }

        return bindingPaths;
    }

    public List<string> GetAllBindingPaths(Guid inputActionID)
    {
        InputAction action = GetInputAction(inputActionID);
        List<string> bindingPaths = new List<string>();

        if (action != null)
        {
            foreach (var binding in action.bindings)
            {
                bindingPaths.Add(binding.effectivePath);
            }
        }
        else
        {
            Debug.LogWarning($"Input action '{inputActionID}' not found.");
        }

        return bindingPaths;
    }
   

    ~DrivingManagerScriptKeyboard()
    {
        inputActions.Capacity = 0;
        inputActions.Clear();
        inputActions = null;
    }
}
