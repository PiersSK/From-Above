using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    private bool taskPadVisible = false;
    private bool taskPadObtained = false;

    public List<Task> tasks;
    [SerializeField] private List<Task> phaseTwoTasks;
    public bool isPhaseTwo = false;
    public int phaseTwoTasksCompleted = 0;
    [SerializeField] bool DEBUG_startOnPhaseTwo = false;
    [SerializeField] private Transform taskPadListParent;
    private const string TASKUIOBJECT = "Task";

    private Animator taskPadAnim;
    [SerializeField] private GameObject taskPadObj;
    [SerializeField] private TextMeshProUGUI taskCount;

    [SerializeField] private Transform player;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        taskPadAnim = taskPadObj.GetComponent<Animator>();
        if(DEBUG_startOnPhaseTwo) MoveToPhaseTwo();
    }

    private void MoveToPhaseTwo()
    {
        isPhaseTwo = true;
        tasks.Clear();
        tasks.Add(phaseTwoTasks[0]);
        phaseTwoTasks.RemoveAt(0);

        RefreshTaskListUI();
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
        taskCount.text = tasks.Count.ToString();

        foreach (Transform task in taskPadListParent) Destroy(task.gameObject);

        foreach (var task in tasks)
        {
            Transform taskUI = Instantiate<Transform>(Resources.Load<Transform>(TASKUIOBJECT), taskPadListParent);
            taskUI.GetComponent<TaskPadTask>().SetTask(task);
        }
    }


    public void CompleteTask(Task taskToComplete)
    {
        tasks.Remove(taskToComplete);

        if(isPhaseTwo && phaseTwoTasks.Count > 0)
        {
            tasks.Add(phaseTwoTasks[0]);
            phaseTwoTasks.RemoveAt(0);
            phaseTwoTasksCompleted++;
        }

        RefreshTaskListUI();
        UIManager.Instance.CompletedTaskPopup();

        if(!isPhaseTwo && tasks.Count == 0)
        {
            MoveToPhaseTwo();
        }
    }

    public void ToggleTaskPad()
    {
        if (!taskPadObtained || player.GetComponent<PlayerMotor>().movementOverridden) return;

        taskPadVisible = !taskPadVisible;
        UIManager.Instance.ToggleMenuPromptStatus();
        UIManager.Instance.ToggleCrosshairVisibility();
        taskPadAnim.SetBool("IsUp", taskPadVisible);
    }
}
