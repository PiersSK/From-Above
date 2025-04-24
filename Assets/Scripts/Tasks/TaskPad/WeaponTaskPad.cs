using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;

public class WeaponTaskPad : ITaskPad
{
    [Header("Canvas Items")]
    [SerializeField] private List<GameObject> taskBlocksUI;
    [SerializeField] private TextMeshProUGUI taskCount;
    [SerializeField] private TextMeshProUGUI timer;

    private int totalTaskCount;

    private void Awake()
    {
        totalTaskCount = phaseData.tasks.Count;
    }

    // Update is called once per frame
    private void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(TimeController.Instance.phase1TimeLimitMins * 60 - TimeController.Instance.GetTimeInSeconds());
        timer.text = $"{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")}";
    }

    public override void CompleteTask(Task completedTask)
    {
        base.CompleteTask(completedTask);
        taskCount.text = $"{phaseData.completedTasks.Count}/{totalTaskCount} STEPS COMPLETED";

        foreach (GameObject t in taskBlocksUI)
        {
            if (taskBlocksUI.IndexOf(t) < phaseData.completedTasks.Count && !t.activeSelf) t.SetActive(true);
        }
    }

    public override void UpdateTaskPadUI()
    {
        base.UpdateTaskPadUI();
    }

    public override void BeginCurrentPhase()
    {
        taskCount.text = $"{phaseData.completedTasks.Count}/{totalTaskCount} STEPS COMPLETED";
        taskPadHeader.color = UIColors.terminalRed;
        UpdateTaskPadUI();
        base.BeginCurrentPhase();
    }

    public override void EndCurrentPhase()
    {
        base.EndCurrentPhase();
    }
}
