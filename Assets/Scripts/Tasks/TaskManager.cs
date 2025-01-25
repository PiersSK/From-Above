using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    private bool taskPadVisible = false;
    private bool taskPadObtained = false;

    [SerializeField] private List<Task> dailyTasks;
    [SerializeField] private Transform taskPadListParent;
    private const string TASKUIOBJECT = "Task";

    private Animator taskPadAnim;
    [SerializeField] private GameObject taskPadObj;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        taskPadAnim = taskPadObj.GetComponent<Animator>();
    }

    public void ObtainTaskpad()
    {
        taskPadObtained = true;
        taskPadObj.SetActive(true);
        ToggleTaskPad();
        RefreshTaskListUI();
    }

    private void RefreshTaskListUI()
    {
        foreach (var task in dailyTasks)
        {
            Transform taskUI = Instantiate<Transform>(Resources.Load<Transform>(TASKUIOBJECT), taskPadListParent);
            taskUI.GetComponent<TaskPadTask>().SetTask(task);
        }
    }


    public void ClearDailyTask(Task taskToComplete)
    {
        dailyTasks.Remove(taskToComplete);
        RefreshTaskListUI();
    }

    public void ToggleTaskPad()
    {
        if (!taskPadObtained) return;

        taskPadVisible = !taskPadVisible;
        UIManager.Instance.ToggleMenuPromptStatus();
        UIManager.Instance.ToggleCrosshairVisibility();
        taskPadAnim.SetBool("IsUp", taskPadVisible);
    }
}
