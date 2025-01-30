using NUnit.Framework;
using System;
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
    public bool pacifistEndingReached = false;
    public int phaseTwoTasksCompleted = 0;
    [SerializeField] bool DEBUG_startOnPhaseTwo = false;
    [SerializeField] private Transform taskPadListParent;
    private const string TASKUIOBJECT = "Task";

    private Animator taskPadAnim;
    [SerializeField] private AudioClip padBeep;
    [SerializeField] private AudioClip task1Beep;
    [SerializeField] private AudioClip task2Beep;
    [SerializeField] private GameObject taskPadObj;
    [SerializeField] private TextMeshProUGUI taskPadHeader;
    [SerializeField] private TextMeshProUGUI taskCount;
    [SerializeField] private GameObject taskCountSentence;
    [SerializeField] private GameObject phase2TaskPad;
    [SerializeField] private TextMeshProUGUI phase2taskCount;
    [SerializeField] private TextMeshProUGUI p2Timer;
    [SerializeField] private List<GameObject> phase2taskBlocks;

    [SerializeField] private FireButton fireBtn;

    private Color dailyColor = new Color(0.2f, 0.69f, 0.2f);
    private Color weaponColor = new Color(0.69f, 0.3f, 0.22f);


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

    private void Update()
    {
        if(isPhaseTwo)
        {
            phase2taskCount.text = phaseTwoTasksCompleted + "/6 STEPS COMPLETED";
            TimeSpan time = TimeSpan.FromSeconds(TimeController.Instance.phase2TimeLimitMins * 60 - TimeController.Instance.GetTimeInSeconds());
            p2Timer.text = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");

            foreach (GameObject t in phase2taskBlocks)
            {
                if (phase2taskBlocks.IndexOf(t) < phaseTwoTasksCompleted && !t.activeSelf) t.SetActive(true);
            }

            if (TimeController.Instance.phase2TimeLimitMins * 60 <= TimeController.Instance.GetTimeInSeconds() && !fireBtn.weaponFired)
            {
                PacifistEnding();
            }
        }
    }

    private void MoveToPhaseTwo()
    {
        isPhaseTwo = true;
        tasks.Clear();
        tasks.Add(phaseTwoTasks[0]);
        phaseTwoTasks.RemoveAt(0);

        taskCount.gameObject.SetActive(false);
        taskCountSentence.SetActive(false);
        phase2TaskPad.SetActive(true);
        phase2taskCount.text = "0/6 STEPS COMPLETED";
        taskPadHeader.color = weaponColor;

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
        if (!tasks.Contains(taskToComplete)) return;

        tasks.Remove(taskToComplete);
        if(isPhaseTwo) phaseTwoTasksCompleted++;
        if (isPhaseTwo && phaseTwoTasks.Count > 0)
        {
            tasks.Add(phaseTwoTasks[0]);
            phaseTwoTasks.RemoveAt(0);
        }

        RefreshTaskListUI();
        UIManager.Instance.CompletedTaskPopup();
        SoundManager.Instance.PlaySFXOneShot(isPhaseTwo ? task2Beep : task1Beep);

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
        if(taskPadVisible) SoundManager.Instance.PlaySFXOneShot(padBeep);
    }

    private void PacifistEnding()
    {
        tasks.Clear();
        taskCount.gameObject.SetActive(true);
        taskCountSentence.SetActive(true);
        phase2TaskPad.SetActive(false);
        taskPadHeader.color = dailyColor;

        RefreshTaskListUI();
        
        isPhaseTwo = false;
        pacifistEndingReached = true;
    }
}
