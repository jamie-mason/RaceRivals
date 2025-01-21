using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    // UI Elements
    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseMenuFadeBackground;
    [SerializeField] private GameObject pauseMenuContents;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject xboxControlsMenu;
    [SerializeField] private GameObject keyboardControlsMenu;
    [SerializeField] private GameObject CameraMenu;



    [Header("Pause Menu Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button xboxControlsButton;
    [SerializeField] private Button keyboardControlsButton;
    [SerializeField] private Button controlsBackButton;
    [SerializeField] private Button keyboardControlsBackButton;
    [SerializeField] private Button xboxControlsBackButton;
    [SerializeField] private Button CameraBackButton;
    [SerializeField] private Button CameraButton;


    public InputAction pauseButtonXbox {get; private set;}
    public InputAction pauseButtonKeyboard {get; private set;}
    public InputAction backButtonXbox {get; private set;}

    public GetInputSystem getInputSystem;
    [Header("Ghost Car Script")]
    [SerializeField] private TrackPlayerPosition trackPlayerPosition;

    public KeyDetect keyDetect;

    PauseSound pauseSound;
    ResumeSound resumeSound;
    BackSound backSound;

    private TMP_InputField[] inputFields;
    private bool ClickOutOfActiveObject = false;
    [SerializeField] private GameObject firstActiveControlsMenu;
    [SerializeField] private GameObject firstActiveXboxControlsMenu;
    [SerializeField] private GameObject firstActiveKeyboardControlsMenu;

    [SerializeField] private CameraPreviewSettingsButtons managePreviewsForCameraSettings;
    public bool paused { get; private set; }
    public bool resumed;
    private void Awake()
    {
        InitializeUIElements();
        DisableAllMenus();
        paused = false;
        resumed = false;
        pauseSound = new PauseSound();
        resumeSound = new ResumeSound();
        backSound = new BackSound();

        pauseButtonKeyboard = getInputSystem.keyboard.FindAction("Pause");
        pauseButtonXbox = getInputSystem.xbox.FindAction("Pause");
        backButtonXbox = getInputSystem.xbox.FindAction("Back");

    }

    private void Start()
    {

        Time.timeScale = 1f;
        ClickOutOfActiveObject = false;
        controlsMenu.SetActive(false);
        pauseButtonKeyboard.Enable();
        backButtonXbox.Enable();
        pauseButtonXbox.Enable();

        SetUpButtonListeners();
    }

    


    private void Update()
    {
        if (trackPlayerPosition.gameover == false)
        {
            if (pauseButtonXbox.WasPressedThisFrame())
            {
                HandleMenuButtonXbox();
            }
            if (backButtonXbox.WasPressedThisFrame())
            {
                HandleBackKeyXbox();
            }
            if (pauseButtonKeyboard.WasPressedThisFrame())
            {
                HandleEscapeKey();
            }
        }
        if (pauseSound == null)
        {
            pauseSound = new PauseSound();
        }
        if(resumeSound == null)
        {
            resumeSound = new ResumeSound();
        }
        if(backSound == null)
        {
            backSound = new BackSound();
        }

    }

    private void InitializeUIElements()
    {
        pauseMenu = pauseMenu ?? GameObject.Find("PauseMenu");
        resumeButton = resumeButton ?? GameObject.Find("ResumeButton").GetComponent<Button>();

    }

    private void DisableAllMenus()
    {
        pauseMenuContents.SetActive(false);

        pauseMenu.SetActive(false);
        controlsMenu.SetActive(false);
        xboxControlsMenu.SetActive(false);
        keyboardControlsMenu.SetActive(false);
        CameraMenu.SetActive(false);
    }

    private void SetUpButtonListeners()
    {
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
        if (controlsButton != null) controlsButton.onClick.AddListener(() => TransitionToNewMenu(pauseMenuContents, controlsMenu));
        if (controlsBackButton != null) controlsBackButton.onClick.AddListener(() => TransitionToNewMenu(controlsMenu, pauseMenuContents));
        if (xboxControlsButton != null) xboxControlsButton.onClick.AddListener(() => TransitionToNewMenu(controlsMenu, xboxControlsMenu));
        if (xboxControlsBackButton != null) xboxControlsBackButton.onClick.AddListener(() => TransitionToNewMenu(xboxControlsMenu, controlsMenu));
        if (keyboardControlsButton != null) keyboardControlsButton.onClick.AddListener(() => TransitionToNewMenu(controlsMenu, keyboardControlsMenu));
        if (keyboardControlsBackButton != null) keyboardControlsBackButton.onClick.AddListener(() => TransitionToNewMenu(keyboardControlsMenu, controlsMenu));
        if (CameraBackButton != null) CameraBackButton.onClick.AddListener(() => TransitionToNewMenu(CameraMenu, pauseMenuContents));
        if (CameraButton != null) CameraButton.onClick.AddListener(() => TransitionToNewMenu(pauseMenuContents, CameraMenu));

    }

    private void HandleEscapeKey()
    {
        if (!paused)
        {
            PauseAction();
        }
        else
        {
            if (!keyDetect.isFocused && !managePreviewsForCameraSettings.previewSliderOpen)
            {
                if (pauseMenuContents.activeSelf)
                {
                    ResumeAction();
                }
                else
                {
                    // Transition between different settings menus
                    CheckAndCloseSubMenus();
                }
            }
        }
    }
    private void HandleMenuButtonXbox()
    {
        if (!paused)
        {
            PauseAction();
        }
        else
        {
            if (!keyDetect.isDetecting || !managePreviewsForCameraSettings.previewSliderOpen)
            {
                CheckAndCloseSubMenus();
                ResumeAction();
            }

        }
    }
    private void HandleBackKeyXbox()
    {
        if (paused)
        {
            if (!keyDetect.isFocused || !managePreviewsForCameraSettings.previewSliderOpen)
            {
                if (pauseMenuContents.activeSelf)
                {

                    ResumeAction();
                }
                else
                {
                    // Transition between different settings menus
                    CheckAndCloseSubMenus();
                }
            }
        }
    }

    private void CheckAndCloseSubMenus()
    {
        backSound.GetBackSound().StartEventSound();
        if(xboxControlsMenu.activeSelf){
            TransitionToNewMenu(xboxControlsMenu, controlsMenu);
        }
        else if(keyboardControlsMenu.activeSelf){
            TransitionToNewMenu(keyboardControlsMenu, controlsMenu);
        }
        else if (controlsMenu.activeSelf)
        {
            TransitionToNewMenu(controlsMenu, pauseMenuContents);
        }
        else if (CameraMenu.activeSelf){
            TransitionToNewMenu(CameraMenu, pauseMenuContents);
        }
        
    }

    private void PauseAction()
    {
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
        pauseSound.GetPauseSound().StartEventSound();
        pauseMenuContents.SetActive(true);
        paused = true;
    }

    private void ResumeAction()
    {
        pauseMenu.SetActive(false);
        resumeSound.GetResumeSound().StartEventSound();
        paused = false;
        resumed = true;
    }

    private void TransitionToNewMenu(GameObject current, GameObject nextMenu)
    {
        if (current != null && nextMenu != null)
        {
            current.SetActive(false);
            if (nextMenu == controlsMenu)
            {
                if (!nextMenu.activeSelf)
                {
                    nextMenu.SetActive(true);
                    if((current == pauseMenuContents)){
                        EventSystem.current.SetSelectedGameObject(firstActiveControlsMenu);
                    }
                    else if(current == keyboardControlsMenu){
                        EventSystem.current.SetSelectedGameObject(keyboardControlsButton.gameObject);

                    }
                    else if(current == xboxControlsMenu){
                        EventSystem.current.SetSelectedGameObject(xboxControlsButton.gameObject);
                    }

                }
            }
            else if (nextMenu == xboxControlsMenu)
            {
                if (!nextMenu.activeSelf)
                {
                    nextMenu.SetActive(true);

                    EventSystem.current.SetSelectedGameObject(firstActiveXboxControlsMenu);

                }
            }
            else if (nextMenu == keyboardControlsMenu)
            {
                if (!nextMenu.activeSelf)
                {
                    nextMenu.SetActive(true);

                    EventSystem.current.SetSelectedGameObject(firstActiveKeyboardControlsMenu);

                }
            }
            else if (nextMenu == pauseMenuContents)
            {
                if (!nextMenu.activeSelf)
                {
                    nextMenu.SetActive(true);
                    if(current == controlsMenu){
                        EventSystem.current.SetSelectedGameObject(controlsButton.gameObject);
                    }
                    else if (current == CameraMenu){
                        EventSystem.current.SetSelectedGameObject(CameraButton.gameObject);
                    }

                }
            }
            else if(nextMenu == CameraMenu){
                nextMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(CameraBackButton.gameObject);

            }

            else
            {
                if (!nextMenu.activeSelf)
                {
                    nextMenu.SetActive(true);
                    GameObject firstSelect = FindObjectOfType<Button>().gameObject;
                    EventSystem.current.SetSelectedGameObject(firstSelect);

                }
            }

        }
        else
        {
            Debug.LogError("Menu reference is missing in TransitionToNewMenu");
        }
    }

    private void ResumeGame()
    {
        if (paused)
        {
            ResumeAction();
        }
    }

    private T[] FindAnyComponentInChildren<T>(GameObject parent) where T : Component
    {
        return parent.GetComponentsInChildren<T>(true);
    }

    // Check if settings menu is open
    private bool IsPaused() => paused;

    public GameObject GetPauseMenuBackground() => pauseMenuFadeBackground;
    // Check if submenus are open
    private bool IsPauseMenuContentsOpen() => pauseMenuContents.activeSelf;
    private void OnDestroy()
    {
        pauseButtonKeyboard.Dispose();
        backButtonXbox.Dispose();
        pauseButtonXbox.Dispose();
        
    }

}