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

    private const string MENUSELECT = "Select Menu Option";
    private const string SETTINGSELECT = "Edit Setting Value";
    private const string BACKOUTMESSAGE = "To Resume Game";
    private const string CANCELEXIT = "Back to Settings";

    private void Awake()
    {
        Instance = this;
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

            if (activePanel != null) UIManager.Instance.HideConfirmText();
            else UIManager.Instance.ShowConfirmText(MENUSELECT);

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
            UIManager.Instance.ShowBackoutText(CANCELEXIT);
            cancelExitGame.Select();
        }
    }

    private void CancelExit()
    {
        confirmExitScreen.SetActive(false);

        if (InputManager.Instance.GamepadIsCurrentInput())
        {
            UIManager.Instance.ShowBackoutText(BACKOUTMESSAGE);
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
        if (PlayerMotor.Instance.movementOverridden) return; // Pausing only possible outside of focus interactablesto avoid keybind clash

        gameObject.SetActive(!gameObject.activeSelf);
        Time.timeScale = gameObject.activeSelf ? 0f : 1f;
        PlayerLook.Instance.lookLocked = gameObject.activeSelf;

        if (gameObject.activeSelf)
        {
            SoundManager.Instance.PauseAllSound();
            initiallySelectedItem.Select();
            ShowDefaultPauseKeyBindings(InputManager.Instance.lastInputType);
            if(!InputManager.Instance.GamepadIsCurrentInput()) Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            SoundManager.Instance.UnpauseAllPausedSound();
            UIManager.Instance.ClearSelectedUIObject();
            UIManager.Instance.HideBackoutText();
            UIManager.Instance.HideConfirmText();
            confirmExitScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ShowDefaultPauseKeyBindings(InputManager.LastInputType inputType)
    {
        if (inputType == InputManager.LastInputType.KeyboardMouse)
        {
            UIManager.Instance.HideConfirmText();
            UIManager.Instance.HideBackoutText();
            UIManager.Instance.HideLRText();
        }
        else
        {
            UIManager.Instance.ShowConfirmText(MENUSELECT);
            UIManager.Instance.ShowBackoutText(confirmExitScreen.activeSelf ? CANCELEXIT : BACKOUTMESSAGE);
        }
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}
