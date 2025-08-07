using System;
using System.Collections.Generic;
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
    [SerializeField] private GameObject lRObj;

    public enum ButtonPromptType
    {
        TaskPad,
        BackOut,
        Confirm,
        LeftRight,
        Toggle,
        Move,
        Interact
    }
    [SerializeField] private List<ButtonPromptType> promptOrder = new();
    [SerializeField] private List<ButtonPrompt> prompts = new();
    private Dictionary<ButtonPromptType, ButtonPrompt> promptDict = new();

    [SerializeField] private TextMeshProUGUI toggleText;
    [SerializeField] private TextMeshProUGUI backoutText;
    [SerializeField] private TextMeshProUGUI confirmText;
    [SerializeField] private TextMeshProUGUI lRText;

    [SerializeField] private GameObject textPopUp;
    [SerializeField] private TextMeshProUGUI textPopUpText;
    [SerializeField] private GameObject menuPrompt;
    [SerializeField] private Animation completedTaskPopup;
    [SerializeField] private Animation progressTaskPopup;

    [SerializeField] private GameObject pacifistEnding;

    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private PDSelectUI pdSelectUI;
    private bool taskPadVisible = false;


    [SerializeField] private AudioClip buttonBeep;
    public void ButtonBeep()
    {
        SoundManager.Instance.PlaySFXOneShot(buttonBeep);
    }

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
    }

    private void Start()
    {
        for(int i = 0; i < promptOrder.Count; i++)
        {
            if (i >= prompts.Count)
            {
                Debug.LogWarning("Prompt referencing shorter than order list. Check to ensure all references have been made correctly");
                break;
            }
            promptDict.Add(promptOrder[i], prompts[i]);
        }
    }

    public void ShowButtonPrompt(ButtonPromptType type, string message, bool updateMessage = true)
    {
        if(updateMessage) promptDict[type].UpdatePromptMessage(message);
        promptDict[type].gameObject.SetActive(true);
    }

    public void HideButtonPrompt(ButtonPromptType type)
    {
        promptDict[type].gameObject.SetActive(false);
    }

    public void CompletedTaskPopup()
    {
        completedTaskPopup.Play();
    }

    public void ProgressTaskPopup()
    {
        progressTaskPopup.Play();
    }

    public void ToggleCrosshairVisibility()
    {
        crosshair.SetActive(!crosshair.activeSelf);
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

    public void ShowPDSelectUI(Action<IServerDataObject> onClick, List<IServerDataObject> inventory)
    {
        pdSelectUI.ShowUI(onClick, inventory);
        pdSelectUI.gameObject.SetActive(true);
    }
}
