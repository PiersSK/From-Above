using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {  get; private set; }
    [SerializeField] private GameObject crosshair;
    [SerializeField] private TextMeshProUGUI helpText;
    [SerializeField] private GameObject textPopUp;
    [SerializeField] private TextMeshProUGUI textPopUpText;
    [SerializeField] private GameObject menuPrompt;
    [SerializeField] private Animation completedTaskPopup;
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

    public void ShowHelpText(string message)
    {
        helpText.text = message;
        helpText.gameObject.SetActive(true);
    }

    public void HideHelpText()
    {
        helpText.gameObject.SetActive(false);
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

}
