using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static InputManager;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    private bool taskPadVisible = false;
    private bool taskPadObtained = false;

    public List<Task> tasks;
    public List<Task> completedTasks;
    [SerializeField] private List<Task> phaseTwoTasks;
    public bool isPhaseTwo = false;
    public bool pacifistEndingReached = false;
    public int phaseTwoTasksCompleted = 0;
    public bool DEBUG_startOnPhaseTwo = false;
    [SerializeField] private Transform taskPadListParent;
    private const string TASKUIOBJECT = "Task";

    private Animator taskPadAnim;
    [SerializeField] private AudioClip padBeep;
    [SerializeField] private AudioClip task1Beep;
    [SerializeField] private AudioClip task2Beep;
    [SerializeField] private GameObject taskPadObj;
    [SerializeField] private GameObject taskPadTrigger;
    [SerializeField] private TextMeshProUGUI taskPadHeader;
    [SerializeField] private TextMeshProUGUI taskCount;
    [SerializeField] private GameObject taskCountSentence;
    [SerializeField] private GameObject phase2TaskPad;
    [SerializeField] private TextMeshProUGUI phase2taskCount;
    [SerializeField] private TextMeshProUGUI p2Timer;
    [SerializeField] private List<GameObject> phase2taskBlocks;

    [SerializeField] private FireButton fireBtn;
    [SerializeField] private Transform player;

    [SerializeField] private Soundtrack pacifistSoundtrack;

    public delegate void OnTaskComplete(Task task);
    public static event OnTaskComplete TaskCompleted;

    public delegate void OnPhaseChange();
    public static event OnPhaseChange PhaseChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        taskPadAnim = taskPadObj.GetComponent<Animator>();
        if(DEBUG_startOnPhaseTwo) MoveToPhaseTwo();
        SoundtrackManager.SoundtrackChanged += OnSoundtrackChange;
    }

    private void Update()
    {
        if(isPhaseTwo)
        {
            phase2taskCount.text = phaseTwoTasksCompleted + "/6 STEPS COMPLETED";
            TimeSpan time = TimeSpan.FromSeconds(TimeController.Instance.phase2TimeLimitMins * 60 - TimeController.Instance.GetTimeInSeconds(TimeController.Instance.time));
            p2Timer.text = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");

            foreach (GameObject t in phase2taskBlocks)
            {
                if (phase2taskBlocks.IndexOf(t) < phaseTwoTasksCompleted && !t.activeSelf) t.SetActive(true);
            }

            if (TimeController.Instance.phase2TimeLimitMins * 60 <= TimeController.Instance.GetTimeInSeconds(TimeController.Instance.time) && !fireBtn.weaponFired)
            {
                PacifistEnding();
            }
        }
    }

    private void MoveToPhaseTwo()
    {
        isPhaseTwo = true;
        completedTasks.AddRange(tasks);
        tasks.Clear();
        tasks.Add(phaseTwoTasks[0]);
        phaseTwoTasks.RemoveAt(0);

        taskCount.gameObject.SetActive(false);
        taskCountSentence.SetActive(false);
        phase2TaskPad.SetActive(true);
        phase2taskCount.text = "0/6 STEPS COMPLETED";
        taskPadHeader.color = UIColors.terminalRed;

        RefreshTaskListUI();
        PhaseChanged?.Invoke();
    }

    public void ObtainTaskpad()
    {
        taskPadObtained = true;
        player.GetComponent<PlayerMotor>().LockPlayer();
        taskPadObj.SetActive(true);
        taskPadTrigger.SetActive(true);
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

        completedTasks.Add(taskToComplete);
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

        TaskCompleted?.Invoke(taskToComplete);
    }

    public void ToggleTaskPad()
    {
        if (!taskPadObtained || player.GetComponent<PlayerMotor>().movementOverridden) return;

        taskPadVisible = !taskPadVisible;
        PlayerMotor.Instance.controller.radius = taskPadVisible ? 0.6f : 0.5f;
        taskPadTrigger.layer = taskPadVisible ? 7 : 0;

        UIManager.Instance.ToggleMenuPromptStatus();
        UIManager.Instance.ToggleCrosshairVisibility();
        taskPadAnim.SetBool("IsUp", taskPadVisible);
        if(taskPadVisible) SoundManager.Instance.PlaySFXOneShot(padBeep);
    }

    private void OnSoundtrackChange(Soundtrack newSoundtrack)
    {
        if (newSoundtrack == pacifistSoundtrack)
        {
            if (pacifistEndingReached)
                Invoke("ShowPacifistEnding", newSoundtrack.clip.length - 12f);
            else
                SoundManager.Instance.PauseBgMusic();


            SoundtrackManager.SoundtrackChanged -= OnSoundtrackChange;
        }
    }

    private void ShowPacifistEnding()
    {
        UIManager.Instance.ShowPacifistEnding();
    }

    private void PacifistEnding()
    {
        tasks.Clear();
        taskCount.gameObject.SetActive(true);
        taskCountSentence.SetActive(true);
        phase2TaskPad.SetActive(false);
        taskPadHeader.color = UIColors.terminalGreen;

        RefreshTaskListUI();
        
        isPhaseTwo = false;
        pacifistEndingReached = true;
    }
}
