using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {  get; private set; }
    [SerializeField] private GameObject crosshair;

    [SerializeField] private GameObject toggleObj;
    [SerializeField] private GameObject backoutObj;
    [SerializeField] private GameObject confirmObj;
    [SerializeField] private TextMeshProUGUI toggleText;
    [SerializeField] private TextMeshProUGUI backoutText;
    [SerializeField] private TextMeshProUGUI confirmText;

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

    public void ShowConfirmText(string message)
    {
        confirmText.text = message;
        confirmObj.SetActive(true);
    }

    public void HideBackoutText()
    {
        backoutObj.SetActive(false);
    }

    public void HideToggleText()
    {
        toggleObj.SetActive(false);
    }

    public void HideConfirmText()
    {
        confirmObj.SetActive(false);
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

    public void ClearSelectedUIObject()
    {
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(null);
    }

    public GameObject GetSelectedUIObject()
    {
        return GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject;
    }

    public bool IsObjectSelected(GameObject obj)
    {
        return obj == GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject;
    }

    public Navigation CreateNewNavigation(Selectable up, Selectable down, Selectable left, Selectable right)
    {
        Navigation navigation = new Navigation();
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnUp = up;
        navigation.selectOnDown = down;
        navigation.selectOnLeft = left;
        navigation.selectOnRight = right;

        return navigation;
    }
}
