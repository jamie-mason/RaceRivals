using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.InputSystem.XInput;

public class ManageInputFields : MonoBehaviour
{
    // Start is called before the first frame update
    InputAction submitAction;
    InputActionMap submitMap;
    public GetInputSystem getInputSystem;

    private void Awake()
    {
        submitMap = getInputSystem.car.asset.FindActionMap("SubmitGamepad");
        submitAction = submitMap.actions[0];
    }
    private void OnEnable()
    {
        // Enable the submit action when the object is enabled
        submitAction.Enable();
    }

    private void OnDisable()
    {
        // Disable the submit action when the object is disabled
        submitAction.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        // Check if the selected object is not null and has a TMP_InputField component
        if (selectedObject != null && selectedObject.TryGetComponent<TMP_InputField>(out TMP_InputField inputField))
        {
            // Allow the user to press "A" (or the equivalent button) to focus on the input field
            if (submitAction.triggered) // Make sure this maps to your "A" button
            {
                // Set the input field as the selected GameObject
                EventSystem.current.SetSelectedGameObject(selectedObject);

                // Optionally activate the input field if you want to focus on it
                inputField.interactable = true;
            }

        }

    }
}
