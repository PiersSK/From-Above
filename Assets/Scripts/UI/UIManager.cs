using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {  get; private set; }
    [SerializeField] private GameObject crosshair;

    [SerializeField] private GameObject toggleObj;
    [SerializeField] private GameObject backoutObj;
    [SerializeField] private TextMeshProUGUI toggleText;
    [SerializeField] private TextMeshProUGUI backoutText;

    [SerializeField] private GameObject textPopUp;
    [SerializeField] private TextMeshProUGUI textPopUpText;
    [SerializeField] private GameObject menuPrompt;
    [SerializeField] private Animation completedTaskPopup;

    [SerializeField] private GameObject pacifistEnding;

    [SerializeField] private GameObject pauseMenu;
    private bool taskPadVisible = false;


    [SerializeField] private AudioClip buttonBeep;
    public void ButtonBeep()
    {
        SoundManager.Instance.PlaySFXOneShot(buttonBeep);
    }

    private void Awake()
    {
        Instance = this;
    }

    public void ShowTaskPadPrompt()
    {
        menuPrompt.SetActive(true);
    }

    public void CompletedTaskPopup()
    {
        completedTaskPopup.Play();
    }

    public void HideTaskPadPrompt()
    {
        menuPrompt.SetActive(false);
    }

    public void ToggleMenuPromptStatus()
    {
        taskPadVisible = !taskPadVisible;
        menuPrompt.GetComponentInChildren<TextMeshProUGUI>().text = taskPadVisible ? "Hide TaskPad" : "View TaskPad";
    }

    public void ToggleCrosshairVisibility()
    {
        crosshair.SetActive(!crosshair.activeSelf);
    }

    public void ShowBackoutText(string message)
    {
        backoutText.text = message;
        backoutObj.SetActive(true);
    }

    public void ShowToggleText(string message)
    {
        toggleText.text = message;
        toggleObj.SetActive(true);
    }

    public void HideBackoutText()
    {
        backoutObj.SetActive(false);
    }

    public void HideToggleText()
    {
        toggleObj.SetActive(false);
    }

    public void ShowPopupText(string message)
    {
        textPopUpText.text = message;
        textPopUp.SetActive(true);
    }

    public void HidePopupText()
    {
        textPopUp.SetActive(false);
    }

    public void ShowPacifistEnding()
    {
        pacifistEnding.SetActive(true);
        Time.timeScale = 0f;
    }

    public void TogglePauseMenu()
    {
        if (PlayerMotor.Instance.movementOverridden) return; // Pausing only possible outside of focus interactablesto avoid keybind clash

        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Time.timeScale = pauseMenu.activeSelf ? 0f : 1f;
        PlayerLook.Instance.lookLocked = pauseMenu.activeSelf;
        Cursor.lockState = pauseMenu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;

        if (pauseMenu.activeSelf) SoundManager.Instance.PauseAllSound();
        else SoundManager.Instance.UnpauseAllPausedSound();
    }
}
