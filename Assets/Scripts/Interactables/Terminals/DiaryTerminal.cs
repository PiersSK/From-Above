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

    [Header("Terminal & Data Settings")]
    [SerializeField] private DataReader dataReader;
    [SerializeField] private DataDrive logsDrive;

    private int questionsAnswered = 0;
    private bool isLookingOut = false;
    public bool logCompleted = false;

    private const string LOOKOUTWINDOW = "To look out the window";
    private const string RETURNTODIARY = "To enter your observations";

    private void Start()
    {
        questionBlocks[0].SetActive(true);
    }

    protected override void Update()
    {
        if (playerAtComputer && InputManager.Instance.playerActions.UIToggle.triggered)
        {
            ToggleLookout();
        }

        base.Update();
    }

    protected override void Interact(Transform player)
    {
        base.Interact(player);
        UIManager.Instance.ShowToggleText(LOOKOUTWINDOW);
        if (questionsAnswered < questionBlocks.Count)
            questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().answerButtons[0].Select();
    }

    protected override void SwitchToMouseKeyboard()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    protected override void SwitchToGamepad()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (questionsAnswered < questionBlocks.Count)
            questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().answerButtons[0].Select();
    }

    private void ToggleLookout()
    {
        isLookingOut = !isLookingOut;

        if (isLookingOut)
        {
            motor.ForcePlayerToPoint(lookoutPoint, true);
            Cursor.lockState = CursorLockMode.Locked;
            UIManager.Instance.ShowToggleText(RETURNTODIARY);
        } else
        {
            motor.ForcePlayerToPoint(lockPoint, true);
            Cursor.lockState = CursorLockMode.None;
            UIManager.Instance.ShowToggleText(LOOKOUTWINDOW);
            if (questionsAnswered < questionBlocks.Count)
                questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().answerButtons[0].Select();
        }
    }

    public void RevealNextQuestion()
    {
        UIManager.Instance.ClearSelectedUIObject();
        questionsAnswered++;

        if (questionsAnswered < questionBlocks.Count)
        {
            questionBlocks[questionsAnswered].SetActive(true);
            questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().answerButtons[0].Select();
        }
        else
        {
            logCompleted = true;
            dataReader.insertedDrive = logsDrive;
            footer.SetActive(true);
        }
    }

    protected override void ReleasePlayer()
    {
        base.ReleasePlayer();
        UIManager.Instance.HideToggleText();
    }
}
