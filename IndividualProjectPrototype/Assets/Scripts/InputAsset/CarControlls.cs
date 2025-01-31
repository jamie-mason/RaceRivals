//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/InputAsset/CarControlls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @CarControlls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @CarControlls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CarControlls"",
    ""maps"": [
        {
            ""name"": ""CarControlsXboxController"",
            ""id"": ""a66e2ccf-e839-4def-b99a-600bc808c5f2"",
            ""actions"": [
                {
                    ""name"": ""ReverseCamera"",
                    ""type"": ""Button"",
                    ""id"": ""ac9831ad-746e-40ec-92c9-5472dd52317b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Horn"",
                    ""type"": ""Button"",
                    ""id"": ""3984cc66-594a-4ee6-8f58-81cc526ad893"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HandBrake"",
                    ""type"": ""Button"",
                    ""id"": ""644b6c35-c97d-4750-9974-7e807955f588"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Drive"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3030a497-7da0-4a8d-a0fa-09c03e78cd30"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Steer"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1af34332-8efa-47b1-a383-5dce45b63c98"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""48a05ffa-609a-4034-a4d9-cd8bde00271a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""726f8cca-366d-4b5e-a961-d2fab16aac4b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9c93755a-b669-44a1-9a69-648d445b4429"",
                    ""path"": ""<XInputController>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83cbd05f-6038-422e-abe0-8f96ad5ae27e"",
                    ""path"": ""<XInputController>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53a904cc-d88c-4497-b612-234f5316e5fd"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ffead99-4d2c-4dc4-87d0-0dddb5e59a1d"",
                    ""path"": ""<XInputController>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ReverseCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ed95ed9-666b-4211-8c70-5c3cd1f1d6ab"",
                    ""path"": ""<XInputController>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HandBrake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""202b9692-8aef-4444-ba30-8d906537cab2"",
                    ""path"": ""<XInputController>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": ""AxisDeadzone"",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""ForwardReverse"",
                    ""id"": ""bbb7cf86-32ba-424f-a3a0-6292c5cdd82b"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drive"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d06dce39-525a-4621-adcc-591a12454a7d"",
                    ""path"": ""<XInputController>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drive"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c73cf73e-cd33-4799-afca-7378e57af5e4"",
                    ""path"": ""<XInputController>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drive"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""CarControlsKeyboard"",
            ""id"": ""6c0e784a-42a5-4b04-aada-2109e2d83029"",
            ""actions"": [
                {
                    ""name"": ""ReverseCamera"",
                    ""type"": ""Button"",
                    ""id"": ""9b6ffca1-614a-481c-8677-b3f3484c35aa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Horn"",
                    ""type"": ""Button"",
                    ""id"": ""43739eb0-0b1e-43ac-a304-654439ddba63"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HandBrake"",
                    ""type"": ""Button"",
                    ""id"": ""328eccbe-f8dd-4b85-8029-879f256c4a73"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Drive"",
                    ""type"": ""PassThrough"",
                    ""id"": ""dacb8618-2ef9-4541-8030-bc042282125a"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Steer"",
                    ""type"": ""PassThrough"",
                    ""id"": ""81cc1feb-e25a-4c5c-8e62-7baea054910c"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""97483cba-dcd2-4f03-af63-f3806af40d97"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""daf78dd4-7d6c-4a38-8a9e-ee3eb7ec8565"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3246d3b-e347-4206-bb77-8a2b9195b9a5"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9a5a1bf7-7ec9-474e-9bcd-37614674bcb3"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ReverseCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f87e239b-0ed9-4d83-802e-512aaf29f5e7"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ReverseCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c3ef876-1dee-4222-a09a-44a0b8dadaf5"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HandBrake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""SteerAxis"",
                    ""id"": ""47316ded-ccdb-4e61-91f5-e0683535a1c1"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ab84e349-9ef4-44fb-8bfe-9178960d8024"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""bc4f2247-fdf5-4e71-9ea9-2cb30ae201d7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ForwardReverse"",
                    ""id"": ""6d19c082-5e61-4854-b8ce-c313d6225053"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drive"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""a47fc2cb-b496-4a43-bd06-d6fd0e13acbd"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drive"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a6bea7ce-b37d-43c9-8028-edeee5c41807"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drive"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Look"",
            ""id"": ""9c7a7017-0efc-4102-ace5-0eed5cd0334d"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9c397f98-8f95-4d36-ac3f-f1f247cf5e59"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""93cb29bb-eb57-4d30-8ccd-b2919d25fed5"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15ebbe10-b617-45c9-b24d-0af58b7742af"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CarControlsXboxController
        m_CarControlsXboxController = asset.FindActionMap("CarControlsXboxController", throwIfNotFound: true);
        m_CarControlsXboxController_ReverseCamera = m_CarControlsXboxController.FindAction("ReverseCamera", throwIfNotFound: true);
        m_CarControlsXboxController_Horn = m_CarControlsXboxController.FindAction("Horn", throwIfNotFound: true);
        m_CarControlsXboxController_HandBrake = m_CarControlsXboxController.FindAction("HandBrake", throwIfNotFound: true);
        m_CarControlsXboxController_Drive = m_CarControlsXboxController.FindAction("Drive", throwIfNotFound: true);
        m_CarControlsXboxController_Steer = m_CarControlsXboxController.FindAction("Steer", throwIfNotFound: true);
        m_CarControlsXboxController_Pause = m_CarControlsXboxController.FindAction("Pause", throwIfNotFound: true);
        m_CarControlsXboxController_Back = m_CarControlsXboxController.FindAction("Back", throwIfNotFound: true);
        // CarControlsKeyboard
        m_CarControlsKeyboard = asset.FindActionMap("CarControlsKeyboard", throwIfNotFound: true);
        m_CarControlsKeyboard_ReverseCamera = m_CarControlsKeyboard.FindAction("ReverseCamera", throwIfNotFound: true);
        m_CarControlsKeyboard_Horn = m_CarControlsKeyboard.FindAction("Horn", throwIfNotFound: true);
        m_CarControlsKeyboard_HandBrake = m_CarControlsKeyboard.FindAction("HandBrake", throwIfNotFound: true);
        m_CarControlsKeyboard_Drive = m_CarControlsKeyboard.FindAction("Drive", throwIfNotFound: true);
        m_CarControlsKeyboard_Steer = m_CarControlsKeyboard.FindAction("Steer", throwIfNotFound: true);
        m_CarControlsKeyboard_Pause = m_CarControlsKeyboard.FindAction("Pause", throwIfNotFound: true);
        // Look
        m_Look = asset.FindActionMap("Look", throwIfNotFound: true);
        m_Look_Look = m_Look.FindAction("Look", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // CarControlsXboxController
    private readonly InputActionMap m_CarControlsXboxController;
    private List<ICarControlsXboxControllerActions> m_CarControlsXboxControllerActionsCallbackInterfaces = new List<ICarControlsXboxControllerActions>();
    private readonly InputAction m_CarControlsXboxController_ReverseCamera;
    private readonly InputAction m_CarControlsXboxController_Horn;
    private readonly InputAction m_CarControlsXboxController_HandBrake;
    private readonly InputAction m_CarControlsXboxController_Drive;
    private readonly InputAction m_CarControlsXboxController_Steer;
    private readonly InputAction m_CarControlsXboxController_Pause;
    private readonly InputAction m_CarControlsXboxController_Back;
    public struct CarControlsXboxControllerActions
    {
        private @CarControlls m_Wrapper;
        public CarControlsXboxControllerActions(@CarControlls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ReverseCamera => m_Wrapper.m_CarControlsXboxController_ReverseCamera;
        public InputAction @Horn => m_Wrapper.m_CarControlsXboxController_Horn;
        public InputAction @HandBrake => m_Wrapper.m_CarControlsXboxController_HandBrake;
        public InputAction @Drive => m_Wrapper.m_CarControlsXboxController_Drive;
        public InputAction @Steer => m_Wrapper.m_CarControlsXboxController_Steer;
        public InputAction @Pause => m_Wrapper.m_CarControlsXboxController_Pause;
        public InputAction @Back => m_Wrapper.m_CarControlsXboxController_Back;
        public InputActionMap Get() { return m_Wrapper.m_CarControlsXboxController; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CarControlsXboxControllerActions set) { return set.Get(); }
        public void AddCallbacks(ICarControlsXboxControllerActions instance)
        {
            if (instance == null || m_Wrapper.m_CarControlsXboxControllerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CarControlsXboxControllerActionsCallbackInterfaces.Add(instance);
            @ReverseCamera.started += instance.OnReverseCamera;
            @ReverseCamera.performed += instance.OnReverseCamera;
            @ReverseCamera.canceled += instance.OnReverseCamera;
            @Horn.started += instance.OnHorn;
            @Horn.performed += instance.OnHorn;
            @Horn.canceled += instance.OnHorn;
            @HandBrake.started += instance.OnHandBrake;
            @HandBrake.performed += instance.OnHandBrake;
            @HandBrake.canceled += instance.OnHandBrake;
            @Drive.started += instance.OnDrive;
            @Drive.performed += instance.OnDrive;
            @Drive.canceled += instance.OnDrive;
            @Steer.started += instance.OnSteer;
            @Steer.performed += instance.OnSteer;
            @Steer.canceled += instance.OnSteer;
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
            @Back.started += instance.OnBack;
            @Back.performed += instance.OnBack;
            @Back.canceled += instance.OnBack;
        }

        private void UnregisterCallbacks(ICarControlsXboxControllerActions instance)
        {
            @ReverseCamera.started -= instance.OnReverseCamera;
            @ReverseCamera.performed -= instance.OnReverseCamera;
            @ReverseCamera.canceled -= instance.OnReverseCamera;
            @Horn.started -= instance.OnHorn;
            @Horn.performed -= instance.OnHorn;
            @Horn.canceled -= instance.OnHorn;
            @HandBrake.started -= instance.OnHandBrake;
            @HandBrake.performed -= instance.OnHandBrake;
            @HandBrake.canceled -= instance.OnHandBrake;
            @Drive.started -= instance.OnDrive;
            @Drive.performed -= instance.OnDrive;
            @Drive.canceled -= instance.OnDrive;
            @Steer.started -= instance.OnSteer;
            @Steer.performed -= instance.OnSteer;
            @Steer.canceled -= instance.OnSteer;
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
            @Back.started -= instance.OnBack;
            @Back.performed -= instance.OnBack;
            @Back.canceled -= instance.OnBack;
        }

        public void RemoveCallbacks(ICarControlsXboxControllerActions instance)
        {
            if (m_Wrapper.m_CarControlsXboxControllerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICarControlsXboxControllerActions instance)
        {
            foreach (var item in m_Wrapper.m_CarControlsXboxControllerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CarControlsXboxControllerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CarControlsXboxControllerActions @CarControlsXboxController => new CarControlsXboxControllerActions(this);

    // CarControlsKeyboard
    private readonly InputActionMap m_CarControlsKeyboard;
    private List<ICarControlsKeyboardActions> m_CarControlsKeyboardActionsCallbackInterfaces = new List<ICarControlsKeyboardActions>();
    private readonly InputAction m_CarControlsKeyboard_ReverseCamera;
    private readonly InputAction m_CarControlsKeyboard_Horn;
    private readonly InputAction m_CarControlsKeyboard_HandBrake;
    private readonly InputAction m_CarControlsKeyboard_Drive;
    private readonly InputAction m_CarControlsKeyboard_Steer;
    private readonly InputAction m_CarControlsKeyboard_Pause;
    public struct CarControlsKeyboardActions
    {
        private @CarControlls m_Wrapper;
        public CarControlsKeyboardActions(@CarControlls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ReverseCamera => m_Wrapper.m_CarControlsKeyboard_ReverseCamera;
        public InputAction @Horn => m_Wrapper.m_CarControlsKeyboard_Horn;
        public InputAction @HandBrake => m_Wrapper.m_CarControlsKeyboard_HandBrake;
        public InputAction @Drive => m_Wrapper.m_CarControlsKeyboard_Drive;
        public InputAction @Steer => m_Wrapper.m_CarControlsKeyboard_Steer;
        public InputAction @Pause => m_Wrapper.m_CarControlsKeyboard_Pause;
        public InputActionMap Get() { return m_Wrapper.m_CarControlsKeyboard; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CarControlsKeyboardActions set) { return set.Get(); }
        public void AddCallbacks(ICarControlsKeyboardActions instance)
        {
            if (instance == null || m_Wrapper.m_CarControlsKeyboardActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CarControlsKeyboardActionsCallbackInterfaces.Add(instance);
            @ReverseCamera.started += instance.OnReverseCamera;
            @ReverseCamera.performed += instance.OnReverseCamera;
            @ReverseCamera.canceled += instance.OnReverseCamera;
            @Horn.started += instance.OnHorn;
            @Horn.performed += instance.OnHorn;
            @Horn.canceled += instance.OnHorn;
            @HandBrake.started += instance.OnHandBrake;
            @HandBrake.performed += instance.OnHandBrake;
            @HandBrake.canceled += instance.OnHandBrake;
            @Drive.started += instance.OnDrive;
            @Drive.performed += instance.OnDrive;
            @Drive.canceled += instance.OnDrive;
            @Steer.started += instance.OnSteer;
            @Steer.performed += instance.OnSteer;
            @Steer.canceled += instance.OnSteer;
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
        }

        private void UnregisterCallbacks(ICarControlsKeyboardActions instance)
        {
            @ReverseCamera.started -= instance.OnReverseCamera;
            @ReverseCamera.performed -= instance.OnReverseCamera;
            @ReverseCamera.canceled -= instance.OnReverseCamera;
            @Horn.started -= instance.OnHorn;
            @Horn.performed -= instance.OnHorn;
            @Horn.canceled -= instance.OnHorn;
            @HandBrake.started -= instance.OnHandBrake;
            @HandBrake.performed -= instance.OnHandBrake;
            @HandBrake.canceled -= instance.OnHandBrake;
            @Drive.started -= instance.OnDrive;
            @Drive.performed -= instance.OnDrive;
            @Drive.canceled -= instance.OnDrive;
            @Steer.started -= instance.OnSteer;
            @Steer.performed -= instance.OnSteer;
            @Steer.canceled -= instance.OnSteer;
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
        }

        public void RemoveCallbacks(ICarControlsKeyboardActions instance)
        {
            if (m_Wrapper.m_CarControlsKeyboardActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICarControlsKeyboardActions instance)
        {
            foreach (var item in m_Wrapper.m_CarControlsKeyboardActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CarControlsKeyboardActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CarControlsKeyboardActions @CarControlsKeyboard => new CarControlsKeyboardActions(this);

    // Look
    private readonly InputActionMap m_Look;
    private List<ILookActions> m_LookActionsCallbackInterfaces = new List<ILookActions>();
    private readonly InputAction m_Look_Look;
    public struct LookActions
    {
        private @CarControlls m_Wrapper;
        public LookActions(@CarControlls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_Look_Look;
        public InputActionMap Get() { return m_Wrapper.m_Look; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(LookActions set) { return set.Get(); }
        public void AddCallbacks(ILookActions instance)
        {
            if (instance == null || m_Wrapper.m_LookActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_LookActionsCallbackInterfaces.Add(instance);
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
        }

        private void UnregisterCallbacks(ILookActions instance)
        {
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
        }

        public void RemoveCallbacks(ILookActions instance)
        {
            if (m_Wrapper.m_LookActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ILookActions instance)
        {
            foreach (var item in m_Wrapper.m_LookActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_LookActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public LookActions @Look => new LookActions(this);
    public interface ICarControlsXboxControllerActions
    {
        void OnReverseCamera(InputAction.CallbackContext context);
        void OnHorn(InputAction.CallbackContext context);
        void OnHandBrake(InputAction.CallbackContext context);
        void OnDrive(InputAction.CallbackContext context);
        void OnSteer(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
    }
    public interface ICarControlsKeyboardActions
    {
        void OnReverseCamera(InputAction.CallbackContext context);
        void OnHorn(InputAction.CallbackContext context);
        void OnHandBrake(InputAction.CallbackContext context);
        void OnDrive(InputAction.CallbackContext context);
        void OnSteer(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
    public interface ILookActions
    {
        void OnLook(InputAction.CallbackContext context);
    }
}
