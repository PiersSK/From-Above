using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiaryTerminal : Computer
{
    private  string HEADER = "===== Rapier 06 Observation Log =====\nAwaiting daily log for operation day: 0847...";
    private List<string> QUESTIONS = new List<string>() {
        "Question 01: When you look out, what do you see?",
        "Question 02: When you look out, how do you feel?",
        "Question 03: Do you see any signs of aggression from the enemy?"
    };
    private const string FOOTER = "LOG COMPLETED. Thank you for your continued vigilance. Please return tomorrow";
    //private const string USERPREFIX = "rapier06@log-cpu: ";

    private int questionsAnswered = 0;

    [SerializeField] private List<GameObject> questionBlocks = new();
    [SerializeField] private GameObject header;
    [SerializeField] private GameObject footer;
    [SerializeField] Task task;

    private void Start()
    {
        questionBlocks[0].GetComponent<DiaryQABlock>().questionText.text = QUESTIONS[0];
        questionBlocks[0].SetActive(true);
        header.GetComponent<TextMeshProUGUI>().text = HEADER;
        footer.GetComponent<TextMeshProUGUI>().text = FOOTER;
    }

    protected override void Update()
    {
        if(input != null && input.playerActions.Submit.triggered)
        {
            string answer = questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().inputField.text;
            RevealNextText(answer);
        }

        base.Update();
    }

    public void RevealNextText(string answer)
    {
        questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().inputField.enabled = false;
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
            TaskManager.Instance.CompleteTask(task);
            footer.SetActive(true);
        }
    }

    protected override void ReleasePlayer()
    {
        if (questionsAnswered < questionBlocks.Count) 
            questionBlocks[questionsAnswered].GetComponent<DiaryQABlock>().inputField.DeactivateInputField();
        base.ReleasePlayer();
    }
}
