using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultBindingsHandler : MonoBehaviour , IDataPersistence
{
    private CarControlls controls;
    private InputActionAsset asset;
    private InputActionMap KeyboardMap;
    private InputActionMap XboxMap;
    public List<InputBinding> keyboardInputBindingPaths { get; private set; }
    public List<InputBinding> xboxInputBindingPaths { get; private set; }

    private void Awake()
    {
        controls = new CarControlls();
        asset = controls.asset;
        keyboardInputBindingPaths = new List<InputBinding>(); // Initialize the list
        xboxInputBindingPaths = new List<InputBinding>();     // Initialize the list

        KeyboardMap = asset.FindActionMap("CarControlsKeyboard");
        XboxMap = asset.FindActionMap("CarControlsXboxController");


        StoreDefaultKeyboardBindingPaths();
        StoreDefaultXboxBindingPaths();
    }
    public void LoadData(GameData data)
    {
       
    }
    public void SaveData(ref GameData data)
    {
       
    }
    private void StoreDefaultKeyboardBindingPaths()
    {
        if (KeyboardMap != null)
        {
            foreach (InputBinding bind in KeyboardMap.bindings)
            {
                keyboardInputBindingPaths.Add(bind);
            }
        }
        else
        {
            Debug.LogWarning("Keyboard action map not found.");
        }
    }

    private void StoreDefaultXboxBindingPaths()
    {
        if (XboxMap != null)
        {
            foreach (InputBinding bind in XboxMap.bindings)
            {
                xboxInputBindingPaths.Add(bind);
            }
        }
        else
        {
            Debug.LogWarning("Xbox action map not found.");
        }
    }
}
