using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;

public class WeaponTaskPhase : IPhase
{
    [Header("Canvas Items")]
    [SerializeField] private List<GameObject> taskBlocksUI;
    [SerializeField] private TextMeshProUGUI taskCount;
    [SerializeField] private TextMeshProUGUI timer;

    // Update is called once per frame
    private void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(TimeController.Instance.phase1TimeLimitMins * 60 - TimeController.Instance.GetTimeInSeconds(TimeController.Instance.time));
        timer.text = $"{time.Minutes:00}:{time.Seconds:00}";
    }

    public override void CompleteTask(TaskData completedTask)
    {
        base.CompleteTask(completedTask);
        taskCount.text = $"{completedTasks.Count}/{tasks.Count + completedTasks.Count} STEPS COMPLETED";

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

        if(tasks.Count > 0)
        {
            Transform taskUI = Instantiate(Resources.Load<Transform>("Task"), taskPadListParent);
            taskUI.GetComponent<TaskPadTask>().SetTask(GetActiveTask(tasks[0]));
        }
    }

    public override void BeginCurrentPhase()
    {
        taskCount.text = $"{completedTasks.Count}/{tasks.Count + completedTasks.Count} STEPS COMPLETED";
        taskPadHeader.color = UIColors.terminalRed;
        sequentialTaskPhase = true;

        base.BeginCurrentPhase();
        UpdateTaskPadUI();
    }

    public override void EndCurrentPhase()
    {
        base.EndCurrentPhase();
    }
}
