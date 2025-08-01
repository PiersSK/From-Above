using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiaryTerminal : Computer
{
    [Header("Object References")]
    [SerializeField] private List<GameObject> questionBlocks = new();
    [SerializeField] private GameObject header;
    [SerializeField] private GameObject footer;
    [SerializeField] private Transform lookoutPoint;

    [Header("Progression Settings")]
    [SerializeField] Task task;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip keysoundLight;
    [SerializeField] private AudioClip keysoundHeavy;

    private int questionsAnswered = 0;
    private bool isLookingOut = false;

    // String Constants
    private const string HEADER = "===== Rapier 06 Observation Log =====\nAwaiting daily log for operation day: 0847...";
    private List<string> QUESTIONS = new List<string>() {
        "Question 01: When you look out, what do you see?",
        "Question 02: When you look out, how do you feel?",
        "Question 03: Do you see any signs of aggression from the enemy?"
    };

    private const string FOOTER = "LOG COMPLETED. Thank you for your continued vigilance. Please return tomorrow";
    private const string LOOKOUTWINDOW = "To look out the window";
    private const string RETURNTODIARY = "To enter your observations";

    private void Start()
    {
        questionBlocks[0].GetComponent<DiaryQABlock>().questionText.text = QUESTIONS[0];
        questionBlocks[0].SetActive(true);
        header.GetComponent<TextMeshProUGUI>().text = HEADER;
        footer.GetComponent<TextMeshProUGUI>().text = FOOTER;
    }

    protected override void Update()
    {
        if (playerAtComputer)
        {
            if (InputManager.Instance.playerActions.Submit.triggered && questionsAnswered < questionBlocks.Count)
            {
                string answer = questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().inputField.text;
                RevealNextText(answer);
            }

            if (InputManager.Instance.playerActions.UIToggle.triggered)
                ToggleLookout();
        }

        if (playerAtComputer && SoundManager.Instance.clipPlaying != initiationSound.name)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SoundManager.Instance.PlaySFXOneShot(keysoundHeavy);
            }
            else if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.Mouse1))
            {
                SoundManager.Instance.PlaySFXOneShot(keysoundLight, 0.2f);
            }
        }

        base.Update();
    }

    protected override void Interact(Transform player)
    {
        base.Interact(player);
        UIManager.Instance.ShowToggleText(LOOKOUTWINDOW);
        questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().inputField.Select();
    }

    protected override void SwitchToMouseKeyboard()
    {
        Cursor.lockState = CursorLockMode.None;
        if(questionsAnswered < questionBlocks.Count)
            questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().inputField.ActivateInputField();
    }

    protected override void SwitchToGamepad()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (questionsAnswered < questionBlocks.Count)
            questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().inputField.ActivateInputField();
    }

    private void ToggleLookout()
    {
        isLookingOut = !isLookingOut;

        if (isLookingOut)
        {
            motor.ForcePlayerToPoint(lookoutPoint, true);
            Cursor.lockState = CursorLockMode.Locked;
            UIManager.Instance.ShowToggleText(RETURNTODIARY);
            if (questionsAnswered < questionBlocks.Count)
                questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().inputField.DeactivateInputField();
        } else
        {
            motor.ForcePlayerToPoint(lockPoint, true);
            Cursor.lockState = CursorLockMode.None;
            UIManager.Instance.ShowToggleText(LOOKOUTWINDOW);
            if (questionsAnswered < questionBlocks.Count)
                questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().inputField.ActivateInputField();
        }
    }

    public void RevealNextText(string answer)
    {
        UIManager.Instance.ClearSelectedUIObject();
        questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().inputField.readOnly = true;
        questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().inputField.interactable = false;
        questionsAnswered++;

        if (questionsAnswered < questionBlocks.Count)
        {
            if (questionsAnswered < QUESTIONS.Count)
                questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().questionText.text = QUESTIONS[questionsAnswered];

            questionBlocks[questionsAnswered].SetActive(true);
            questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().inputField.ActivateInputField();
        }
        else
        {
            TaskManager.Instance.ProgressTask(task);
            footer.SetActive(true);
        }
    }

    protected override void ReleasePlayer()
    {
        base.ReleasePlayer();
        UIManager.Instance.HideToggleText();
        if (questionsAnswered < questionBlocks.Count)
            questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().inputField.DeactivateInputField();
    }
}
