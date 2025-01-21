using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class UISound : MonoBehaviour
{
    private UIUp up;
    private UIDown down;
    private UISelectSound selectSound;
    [SerializeField] private GameObject lastSelected, currentSelected, inactiveParent;

    [SerializeField] private InputActionAsset IA_Asset;
    [SerializeField] private InputAction IA_Navigate;
    [SerializeField] private InputAction IA_Select;
    [SerializeField] private InputActionMap IAM_UI;

    
    private void Start()
    {
        down = new UIDown();
        up = new UIUp();
        selectSound = new UISelectSound();
        if (IA_Asset != null)
        {
            IA_Asset = new DefaultInputActions().asset;
        }
        IAM_UI = IA_Asset.FindActionMap("UI");
        IA_Navigate = IAM_UI.FindAction("Navigate");
        IA_Select = IAM_UI.FindAction("Submit");
        currentSelected = EventSystem.current.currentSelectedGameObject;
        IA_Navigate.Enable();
        IA_Select.Enable();



    }





    private void checkIfClassEventsAreNull()
    {
        if (up == null)
        {
            up = new UIUp();
        }
        if (down == null)
        {
            down = new UIDown();
        }
        if (selectSound == null)
        {
            selectSound = new UISelectSound();
        }
        if (IA_Asset == null)
        {
            IA_Asset = new DefaultInputActions().asset;
        }
        if (IAM_UI == null)
        {
            IAM_UI = IA_Asset.FindActionMap("UI");
        }
        if (IA_Navigate == null)
        {
            IA_Navigate = IAM_UI.FindAction("Navigate");
        }
        if (IA_Select == null)
        {
            IA_Select = IAM_UI.FindAction("Submit");
        }
    }
    // Update is called once per frame

    
    

    private void Update()
    {
        checkIfClassEventsAreNull();
        Vector2 navigation = IA_Navigate.ReadValue<Vector2>();

        currentSelected = EventSystem.current.currentSelectedGameObject;

        if (navigation.y != 0) // Upward navigation
        {
            inactiveParent = FindInactiveParent(currentSelected.gameObject.transform);

            // Check if the UI element itself or any of its inactive parents are not active
            if (inactiveParent != null && inactiveParent.activeSelf)
            {
                if (currentSelected != null && currentSelected != lastSelected)
                {
                    if (navigation.y > 0)
                    {
                        up.UIUPSound().StartEventSound();
                    }
                    else
                    {
                        down.UIDownSound().StartEventSound();
                    }
                    lastSelected = currentSelected;
                }
            }
        }
        if (currentSelected.GetComponent<RectTransform>() != null && IA_Select.WasPressedThisFrame())
        {
            inactiveParent = FindInactiveParent(currentSelected.gameObject.transform);

            // Check if the UI element itself or any of its inactive parents are not active
            if (inactiveParent != null && inactiveParent.activeSelf)
            {
                selectSound.FmodUISelectSound().StartEventSound();
            }
        }

    }

    GameObject FindInactiveParent(Transform childObj)
    {
        if (childObj == null)
        {
            return null; 
        }
        if (!childObj.gameObject.activeSelf)
        {
            return childObj.gameObject;
        }
        else
        {
            if (childObj.parent == null)
            {
                return childObj.gameObject;
            }
            else
            {
                return FindInactiveParent(childObj.parent);
            }
        }
    }
    
    private void OnDestroy()
    {
        IA_Navigate.Dispose();
        IA_Select.Dispose();
    }
}
