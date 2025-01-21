using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class GetInputSystem : MonoBehaviour, IDataPersistence
{
    public CarControlls car { get; private set; }
    public InputActionMap xbox;
    public InputActionMap keyboard;
    public InputActionMap look;
    [SerializeField] private InputActionAsset inputAsset;
    public KeyDetect keyDetect;
    public List<string> xboxOver;
    public List<string> keyOver;
    private void Awake()
    {
        InitializeControls();
        ApplyNewBindingsToInputActionMap(xbox, xboxOver);
        ApplyNewBindingsToInputActionMap(keyboard, keyOver);

        if (keyDetect != null)
        {
            SetUpInputFields();

        }
    }

    
    private void InitializeControls()
    {
        car = new CarControlls();
        inputAsset = car.asset;
        keyboard = inputAsset.FindActionMap("CarControlsKeyboard");
        xbox = inputAsset.FindActionMap("CarControlsXboxController");
        look = inputAsset.FindActionMap("Look");

    }

    private void SetUpInputFields()
    {
        foreach (var inputField in keyDetect.xboxControlInputFields)
        {
            if (inputField == null) continue;
            var extraInfo = inputField.GetComponent<ExtraKeyInputInfo>();
            if (extraInfo != null)
            {
                ConfigureInputField(inputField, extraInfo, xbox);
            }
        }
        foreach (var inputField in keyDetect.keyInputFields)
        {
            if (inputField == null) continue;
            var extraInfo = inputField.GetComponent<ExtraKeyInputInfo>();
            if (extraInfo != null)
            {
                ConfigureInputField(inputField, extraInfo, keyboard);
            }
        }
    }

    InputAction findAttachedAction(ExtraKeyInputInfo extraInfo, InputActionMap actionMap)
    {
        return actionMap.FindAction(extraInfo.attachedActionName);
    }
    private void ConfigureInputField(TMP_InputField inputField, ExtraKeyInputInfo extraInfo, InputActionMap actionMap)
    {
        var action = findAttachedAction(extraInfo, actionMap);
        if (action == null || action.bindings.Count == 0) return;

        if (extraInfo.isPartOfCompositeBinding)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action.bindings[i].isPartOfComposite && IsCorrectCompositePart(action.bindings[i], extraInfo))
                {
                    if (action.bindings[i].overridePath != null)
                    {
                        if (InputControlPath.TryGetDeviceLayout(action.bindings[i].overridePath) != null)
                            inputField.text = InputControlPath.ToHumanReadableString(action.bindings[i].overridePath);
                    }
                    else
                    {
                        inputField.text = InputControlPath.ToHumanReadableString(action.bindings[i].path);
                    }

                    extraInfo.actionBindingIndex = i;
                    return;
                }
            }
        }
        else
        {
            if (action.bindings[0].overridePath != null)
            {
                if (InputControlPath.TryGetDeviceLayout(action.bindings[0].overridePath) != null)
                    inputField.text = InputControlPath.ToHumanReadableString(action.bindings[0].overridePath);
            }
            else
            {
                inputField.text = InputControlPath.ToHumanReadableString(action.bindings[0].path);
            }
            extraInfo.actionBindingIndex = 0;
        }
    }

    private bool IsCorrectCompositePart(InputBinding binding, ExtraKeyInputInfo extraInfo)
    {
        return (binding.name == "positive" && extraInfo.positive) ||
               (binding.name == "negative" && !extraInfo.positive);
    }

    public void OnInputFieldEndEdit(TMP_InputField input, List<TMP_InputField> inputFields, string newText, InputAction action, int bindIndex)
    {
        ApplyNewBinding(action, bindIndex, newText);
    }

    public void ApplyNewBinding(InputAction action, int bindingIndex, string bindingPath)
    {
        action.RemoveBindingOverride(bindingIndex);
        if (action?.actionMap == null || bindingIndex >= action.bindings.Count)
        {
            Debug.LogWarning($"Invalid binding index or action for {action?.name}");
            return;
        }

        if (InputControlPath.TryGetDeviceLayout(bindingPath) == null)
        {
            return;
        }
        if (string.IsNullOrEmpty(bindingPath)){
            return;
        }

        // Apply the binding override
        action.ApplyBindingOverride(bindingIndex, bindingPath);


    }
    public void ApplyNewBindingsToInputActionMap(InputActionMap actionMap, List<string> overridePaths)
    {
        int over = 0; // Index for overridePaths
        for (int i = 0; i < actionMap.actions.Count; i++)
        {
            for (int j = 0; j < actionMap.actions[i].bindings.Count; j++)
            {
                if (over < overridePaths.Count)
                {
                    ApplyNewBinding(actionMap.actions[i], j, overridePaths[over]);
                    over++; // Increment override path index
                }
                else
                {
                    Debug.LogWarning("Not enough override paths provided for all bindings.");
                    return;
                }
            }
        }
    }

    public bool CheckIfBindingExists(InputActionMap map, string path)
    {
        foreach (var bind in map.bindings)
        {
            if (bind.path == path) return true;
        }
        return false;
    }
    public void LoadData(GameData data)
    {
        xboxOver = new List<string>(data.xboxOverridePaths);
        keyOver = new List<string>(data.keyOverridePaths);
        


    }
    List<string> FindBindingOverrides(InputActionMap actionMap){
        List<string> bindOver = new List<string>();
        foreach (InputAction actions in actionMap.actions)
        {
            foreach (InputBinding bind in actions.bindings)
            {
                bindOver.Add(bind.overridePath);
            }

        }

        return bindOver;
    }
    public void SaveData(ref GameData data)
    {
        xboxOver = new List<string>(FindBindingOverrides(xbox));
        keyOver = new List<string>(FindBindingOverrides(keyboard));
        
        
        data.xboxOverridePaths = new List<string>(xboxOver);
        data.keyOverridePaths = new List<string>(keyOver);
    }
}
