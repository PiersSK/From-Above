using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class DailyTaskPhase : IPhase
{
    [Header("Canvas Items")]
    [SerializeField] private TextMeshProUGUI taskCounter;

    public override void BeginCurrentPhase()
    {
        Color c = UIColors.terminalGreen;
        c.a = 0.5f;
        taskPadHeader.color = c;
        base.BeginCurrentPhase();
        UpdateTaskPadUI();
    }

    public override void EndCurrentPhase()
    {
        base.EndCurrentPhase();
    }

    public override void CompleteTask(TaskData completedTask)
    {
        base.CompleteTask(completedTask);
    }

    public override void UpdateTaskPadUI()
    {
        taskCounter.text = tasks.Count.ToString();
        base.UpdateTaskPadUI();
    }
}
