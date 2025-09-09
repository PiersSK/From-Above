using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    private bool taskPadVisible = false;
    private bool taskPadObtained = false;

    public List<TaskData> completedTasks;

    public bool pacifistEndingReached = false; //Remove from other managers before deleting

    public bool DEBUG_startOnPhaseTwo = false;
    [SerializeField] private Transform taskPadListParent;

    private Animator taskPadAnim;
    [SerializeField] private AudioClip padBeep;
    [SerializeField] private GameObject taskPadObj;
    [SerializeField] private GameObject taskPadTrigger;
    [SerializeField] private TextMeshProUGUI p2Timer;

    [SerializeField] public List<IPhase> phases;
    public IPhase currentPhase;
    private int currentPhaseIndex = 0;

    [SerializeField] private FireButton fireBtn;
    [SerializeField] private Transform player;

    [SerializeField] private Soundtrack pacifistSoundtrack;

    public delegate void OnTaskComplete(TaskData task);
    public static event OnTaskComplete TaskCompleted;

    public delegate void OnPhaseChange();
    public static event OnPhaseChange PhaseChanged;

    private const string HIDETASKPADPROMPT = "Hide TaskPad";
    private const string SHOWTASKPADPROMPT = "Show TaskPad";

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
        currentPhaseIndex = 0;
        currentPhase = phases[currentPhaseIndex];

        taskPadAnim = taskPadObj.GetComponent<Animator>();
        SoundtrackManager.SoundtrackChanged += OnSoundtrackChange;
    }

    private void Update()
    {
        if (currentPhase is DownTimePhase)
        {
            if ((currentPhase as DownTimePhase).timeLimitMins * 60 <= TimeController.Instance.GetTimeInSeconds((currentPhase as DownTimePhase).phaseTimer) && currentPhase is DownTimePhase)
            {
                MoveToNextPhase();
            }
        }

        if (currentPhase is WeaponTaskPhase)
        {
            if ((currentPhase as WeaponTaskPhase).timeLimitMins * 60 <= TimeController.Instance.GetTimeInSeconds((currentPhase as WeaponTaskPhase).phaseTimer) && !fireBtn.weaponFired)
                {
                    PacifistEnding();
                }
        }
        
    }

    private void MoveToNextPhase()
    {
        if(currentPhaseIndex == phases.Count - 1)
        {
            //We about to end the game / level so what phase you gonna fucking move to?
            return;
        }

        ++currentPhaseIndex;
        currentPhase.EndCurrentPhase();
        currentPhase = phases[currentPhaseIndex];
        currentPhase.BeginCurrentPhase();

        PhaseChanged?.Invoke();
    }

    public void ObtainTaskpad()
    {
        taskPadObtained = true;
        player.GetComponent<PlayerMotor>().TogglePlayerLock();
        taskPadObj.SetActive(true);
        taskPadTrigger.SetActive(true);

        currentPhase.BeginCurrentPhase();
        ToggleTaskPad();

        if (DEBUG_startOnPhaseTwo)
        {
            currentPhase.completedTasks.AddRange(currentPhase.tasks);
            currentPhase.tasks.Clear();

            MoveToNextPhase();
        }
    }

    public void ProgressTask(TaskData taskToComplete)
    {
        if (!currentPhase.tasks.Contains(taskToComplete)) return; //TODO: Throw an error here?

        if (currentPhase.ProgressTask(taskToComplete))
        {
            completedTasks.Add(taskToComplete);

            currentPhase.UpdateTaskPadUI();
            UIManager.Instance.CompletedTaskPopup();

            if (currentPhase.tasks.Count == 0)
            {
                MoveToNextPhase();
            }

            TaskCompleted?.Invoke(taskToComplete);
        } else
        {
            UIManager.Instance.ProgressTaskPopup();
        }

        SoundManager.Instance.PlaySFXOneShot(currentPhase.taskBeep);
        TimeController.Instance.inactivityTimer = 0f; //TODO: Should this be reset every step or only on full completion?
    }

    public void ToggleTaskPad()
    {
        if (!taskPadObtained || player.GetComponent<PlayerMotor>().movementOverridden) return;

        taskPadVisible = !taskPadVisible;
        PlayerMotor.Instance.controller.radius = taskPadVisible ? 0.6f : 0.5f;
        taskPadTrigger.layer = taskPadVisible ? 7 : 0;

        if (taskPadVisible) UIManager.Instance.ShowButtonPrompt(UIManager.ButtonPromptType.TaskPad, HIDETASKPADPROMPT);
        else UIManager.Instance.ShowButtonPrompt(UIManager.ButtonPromptType.TaskPad, SHOWTASKPADPROMPT);

        UIManager.Instance.ToggleCrosshairVisibility();
        taskPadAnim.SetBool("IsUp", taskPadVisible);
        if(taskPadVisible) SoundManager.Instance.PlaySFXOneShot(padBeep);
    }

    private void OnSoundtrackChange(Soundtrack newSoundtrack)
    {
        if (newSoundtrack == pacifistSoundtrack)
        {
            if (pacifistEndingReached)
            {
                Invoke("ShowPacifistEnding", newSoundtrack.clip.length - 12f);
            }
            else
            {
                SoundManager.Instance.PauseBgMusic();
            }

            SoundtrackManager.SoundtrackChanged -= OnSoundtrackChange;
        }
    }

    private void ShowPacifistEnding()
    {
        UIManager.Instance.ShowPacifistEnding();
    }

    private void PacifistEnding()
    {
        currentPhase.EndCurrentPhase();
        currentPhaseIndex = 0;
        currentPhase = phases[0];

        pacifistEndingReached = true;
    }
}
