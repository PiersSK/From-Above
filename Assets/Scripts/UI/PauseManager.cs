using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [SerializeField] private int mainMenuSceneIndex;
    [SerializeField] private Selectable initiallySelectedItem;
    [SerializeField] private List<SettingsPanel> settingsPanels;
    private SettingsPanel activePanel = null;

    [SerializeField] private Button exitGameFirstButton;
    [SerializeField] private Button cancelExitGame;
    [SerializeField] private GameObject confirmExitScreen;

    public bool pauseIsBlocked = false;

    private const string MENUSELECT = "Select Menu Option";
    private const string BACKOUTMESSAGE = "To Resume Game";
    private const string CANCELEXIT = "Back to Settings";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        gameObject.SetActive(false);
    }

    private void Start()
    {
        exitGameFirstButton.onClick.AddListener(GoToExitConfirm);
        cancelExitGame.onClick.AddListener(CancelExit);
    }

    private void OnEnable()
    {
        InputManager.InputTypeChanged += UpdateForNewInputType;
    }

    private void OnDisable()
    {
        InputManager.InputTypeChanged -= UpdateForNewInputType;
    }

    private void Update()
    {
        foreach (SettingsPanel panel in settingsPanels)
        {
            if (panel.gameObject.activeSelf) activePanel = panel;
        }

        if (gameObject.activeSelf && InputManager.Instance.GamepadIsCurrentInput())
        {

            if (activePanel != null) UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.Confirm);
            else UIManager.Instance.ShowButtonPrompt(UIManager.ButtonPromptType.Confirm, MENUSELECT);

            if (InputManager.Instance.playerActions.Escape.triggered)
            {
                if (confirmExitScreen.activeSelf)
                    CancelExit();
                else if (activePanel != null) {
                    activePanel.SettingsPanelDeselected();
                    ShowDefaultPauseKeyBindings(InputManager.Instance.lastInputType);
                }
                else
                    TogglePauseMenu();
            }

        }

    }

    private void UpdateForNewInputType(InputManager.LastInputType newInputType)
    {
        ShowDefaultPauseKeyBindings(newInputType);
        if(newInputType != InputManager.LastInputType.KeyboardMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;

            if (confirmExitScreen.activeSelf)
                cancelExitGame.Select();
            else if (activePanel != null)
                activePanel.SettingsPanelSelected();
            else 
                initiallySelectedItem.Select();
        } else
        {
            if (activePanel != null) activePanel.UpdateLastSelectedOption();
            UIManager.Instance.ClearSelectedUIObject();
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void GoToExitConfirm()
    {
        confirmExitScreen.SetActive(true);

        if(InputManager.Instance.GamepadIsCurrentInput())
        {
            UIManager.Instance.ShowButtonPrompt(UIManager.ButtonPromptType.BackOut, CANCELEXIT);
            cancelExitGame.Select();
        }
    }

    private void CancelExit()
    {
        confirmExitScreen.SetActive(false);

        if (InputManager.Instance.GamepadIsCurrentInput())
        {
            UIManager.Instance.ShowButtonPrompt(UIManager.ButtonPromptType.BackOut, BACKOUTMESSAGE);
            exitGameFirstButton.Select();
        }
    }

    public void CloseAllSettingsPanels()
    {
        foreach (SettingsPanel panel in settingsPanels)
        {
            panel.gameObject.SetActive(false);
        }
        activePanel = null;
    }

    public void TogglePauseMenu()
    {
        if (pauseIsBlocked) return; // Pausing only possible outside of focus interactablesto avoid keybind clash

        gameObject.SetActive(!gameObject.activeSelf);
        Time.timeScale = gameObject.activeSelf ? 0f : 1f;
        PlayerLook.Instance.lookLocked = gameObject.activeSelf;

        if (gameObject.activeSelf)
        {
            initiallySelectedItem.Select();
            SoundManager.Instance.PauseAllSound();
            ShowDefaultPauseKeyBindings(InputManager.Instance.lastInputType);
            if(!InputManager.Instance.GamepadIsCurrentInput()) Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            if (activePanel != null) activePanel.SettingsPanelDeselected();
            SoundManager.Instance.UnpauseAllPausedSound();

            UIManager.Instance.ClearSelectedUIObject();
            UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.BackOut);
            UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.Confirm);
            UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.LeftRight);
            confirmExitScreen.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ShowDefaultPauseKeyBindings(InputManager.LastInputType inputType)
    {
        if (inputType == InputManager.LastInputType.KeyboardMouse)
        {
            UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.Confirm);
            UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.BackOut);
            UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.LeftRight);
        }
        else
        {
            UIManager.Instance.ShowButtonPrompt(UIManager.ButtonPromptType.Confirm, MENUSELECT);
            UIManager.Instance.ShowButtonPrompt(UIManager.ButtonPromptType.BackOut, confirmExitScreen.activeSelf ? CANCELEXIT : BACKOUTMESSAGE);
        }
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}
