using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    [SerializeField] private int mainMenuSceneIndex;
    [SerializeField] private List<SettingsPanel> settingsPanels;
    [SerializeField] private Selectable initiallySelectedItem;

    [SerializeField] private Button exitGameFirstButton;
    [SerializeField] private Button cancelExitGame;
    [SerializeField] private GameObject confirmExitScreen;

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
        if (gameObject.activeSelf && InputManager.Instance.GamepadIsCurrentInput())
        {
            SettingsPanel panelHeaderSelected = null;
            bool panelInFocus = false;

            foreach (SettingsPanel panel in settingsPanels)
            {
                panelInFocus = panelInFocus || panel.isFocused;

                if (UIManager.Instance.IsObjectSelected(panel.headerButton.gameObject)) panelHeaderSelected = panel;
            }

            if (panelHeaderSelected != null)
                UIManager.Instance.ShowConfirmText("Adjust " + panelHeaderSelected.settingsPanelName + " settings");
            else
                UIManager.Instance.HideConfirmText();

            if (InputManager.Instance.playerActions.Escape.triggered && !panelInFocus)
            {
                if (confirmExitScreen.activeSelf)
                    CancelExit();
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
            if(confirmExitScreen.activeSelf)
                cancelExitGame.Select();
            else 
                initiallySelectedItem.Select();
        } else
        {
            UIManager.Instance.ClearSelectedUIObject();
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

    public void TogglePauseMenu()
    {
        if (PlayerMotor.Instance.movementOverridden) return; // Pausing only possible outside of focus interactablesto avoid keybind clash

        gameObject.SetActive(!gameObject.activeSelf);
        Time.timeScale = gameObject.activeSelf ? 0f : 1f;
        PlayerLook.Instance.lookLocked = gameObject.activeSelf;
        Cursor.lockState = gameObject.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;

        if (gameObject.activeSelf)
        {
            SoundManager.Instance.PauseAllSound();
            initiallySelectedItem.Select();
            ShowDefaultPauseKeyBindings(InputManager.Instance.lastInputType);
        }
        else
        {
            SoundManager.Instance.UnpauseAllPausedSound();
            UIManager.Instance.ClearSelectedUIObject();
            UIManager.Instance.HideBackoutText();
            UIManager.Instance.HideConfirmText();
            confirmExitScreen.SetActive(false);
        }
    }

    

    public void ShowDefaultPauseKeyBindings(InputManager.LastInputType inputType)
    {
        if (inputType == InputManager.LastInputType.KeyboardMouse)
        {
            UIManager.Instance.HideConfirmText();
            UIManager.Instance.HideBackoutText();
        }
        else
        {
            UIManager.Instance.ShowBackoutText(confirmExitScreen.activeSelf ? CANCELEXIT : BACKOUTMESSAGE);
        }
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}
