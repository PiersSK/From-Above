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
        totalTaskCount = tasks.Count;
    }

    // Update is called once per frame
    private void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(TimeController.Instance.phase1TimeLimitMins * 60 - TimeController.Instance.GetTimeInSeconds(TimeController.Instance.time));
        timer.text = $"{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")}";
    }

    public override void CompleteTask(Task completedTask)
    {
        base.CompleteTask(completedTask);
        taskCount.text = $"{completedTasks.Count}/{totalTaskCount} STEPS COMPLETED";

        foreach (GameObject t in taskBlocksUI)
        {
            if (taskBlocksUI.IndexOf(t) < completedTasks.Count && !t.activeSelf) t.SetActive(true);
        }
    }

    public override void UpdateTaskPadUI()
    {
        foreach (Transform task in taskPadListParent)
        {
            Destroy(task.gameObject);
        }

        Transform taskUI = Instantiate(Resources.Load<Transform>("Task"), taskPadListParent);
        taskUI.GetComponent<TaskPadTask>().SetTask(tasks[0]);
        
    }

    public override void BeginCurrentPhase()
    {
        taskCount.text = $"{completedTasks.Count}/{totalTaskCount} STEPS COMPLETED";
        taskPadHeader.color = UIColors.terminalRed;
        UpdateTaskPadUI();
        base.BeginCurrentPhase();
    }

    public override void EndCurrentPhase()
    {
        base.EndCurrentPhase();
    }
}
