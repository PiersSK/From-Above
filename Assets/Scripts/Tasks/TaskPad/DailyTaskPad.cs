using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class DailyTaskPad : ITaskPad
{
    [Header("Canvas Items")]
    [SerializeField] private TextMeshProUGUI taskCounter;
    [SerializeField] private TextMeshProUGUI taskCounterSentence;

    public override void BeginCurrentPhase()
    {
        taskPadHeader.color = UIColors.terminalGreen;
        taskCounterSentence.text = "You have       urgent commands REMAINING";
        UpdateTaskPadUI();
        base.BeginCurrentPhase();
    }

    public override void EndCurrentPhase()
    {
        base.EndCurrentPhase();
    }

    public override void CompleteTask(Task completedTask)
    {
        base.CompleteTask(completedTask);
    }

    public override void UpdateTaskPadUI()
    {
        taskCounter.text = phaseData.tasks.Count.ToString();
        base.UpdateTaskPadUI();
    }
}
