using NUnit.Framework;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{
    [SerializeField] private Soundtrack currentSoundtrack;
    [SerializeField] private Soundtrack DEBUG_phase2SoundtrackStart;
    [SerializeField] private AudioSource bGMusicSource;

    private float randomLoopTimer = 0f;
    private float timeTillNextRandomLooperPlay = 0f;
    private bool applicationInFocus = true;

    public delegate void OnSoundtrackChange(Soundtrack newSoundtrack);
    public static event OnSoundtrackChange SoundtrackChanged;

    private void OnEnable()
    {
        TaskManager.TaskCompleted += TaskCompleted;
        TaskManager.PhaseChanged += PhaseChanged;
    }

    private void OnDisable()
    {
        TaskManager.TaskCompleted -= TaskCompleted;
        TaskManager.PhaseChanged -= PhaseChanged;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        applicationInFocus = hasFocus;
    }

    private void Start()
    {
        if (TaskManager.Instance.DEBUG_startOnPhaseTwo) currentSoundtrack = DEBUG_phase2SoundtrackStart;
        
        StartNewSoundtrack();
    }

    private void Update()
    {
        if (currentSoundtrack.leaveTimeBetweenLoops && !bGMusicSource.isPlaying && !SoundManager.Instance.bgPaused && applicationInFocus)
        {
            randomLoopTimer += Time.deltaTime;
            if(randomLoopTimer >= timeTillNextRandomLooperPlay)
            {
                bGMusicSource.Play();
                randomLoopTimer = 0f;
                timeTillNextRandomLooperPlay = GetRandomLoopTime();
            }
        }

        if (CheckSoundtrackShouldProgress()) MoveToNextSoundtrack();
    }

    private bool CheckSoundtrackShouldProgress()
    {
        if (currentSoundtrack.progressOnEnd && !bGMusicSource.isPlaying && !SoundManager.Instance.bgPaused && applicationInFocus)
            return true;

        if (currentSoundtrack.progressOnTimeCondition)
        {
            float currentTime = TimeController.Instance.GetTimeInSeconds(TimeController.Instance.time);
            float phaseTimeLimit = TimeController.Instance.phase2TimeLimitMins * 60f;

            if (currentSoundtrack.timeConditionType == Soundtrack.TimeConditionType.AbsoluteFromStart)
                return currentTime >= currentSoundtrack.absoluteSecondsIntoPhase;
            else if (currentSoundtrack.timeConditionType == Soundtrack.TimeConditionType.AbsoluteFromEnd)
                return phaseTimeLimit - currentTime <= currentSoundtrack.absoluteSecondsBeforePhaseEnd;
            else if (currentSoundtrack.timeConditionType == Soundtrack.TimeConditionType.Relative)
                return currentTime >= phaseTimeLimit * ((float)currentSoundtrack.percentageThroughPhase / 100);
        }

        return false;
    }

    private void TaskCompleted(Task task)
    {
        if (currentSoundtrack.progressOnTaskCompletion && currentSoundtrack.progressAfter == task)
        {
            MoveToNextSoundtrack();
            if (currentSoundtrack.nextSoundtrack == null) TaskManager.TaskCompleted -= TaskCompleted;

        }
    }

    private void PhaseChanged()
    {
        if (currentSoundtrack.progressOnPhaseTransition)
        {
            MoveToNextSoundtrack();
            if (currentSoundtrack.nextSoundtrack == null) TaskManager.PhaseChanged -= PhaseChanged;
        }
    }

    private void MoveToNextSoundtrack()
    {
        bGMusicSource.Stop();
        currentSoundtrack = currentSoundtrack.nextSoundtrack;
        StartNewSoundtrack();

        SoundtrackChanged?.Invoke(currentSoundtrack);
    }

    private void StartNewSoundtrack()
    {
        bGMusicSource.clip = currentSoundtrack.clip;
        bGMusicSource.loop = currentSoundtrack.shouldLoop && !currentSoundtrack.leaveTimeBetweenLoops;

        randomLoopTimer = 0f;
        timeTillNextRandomLooperPlay = GetRandomLoopTime();

        if(!SoundManager.Instance.bgPaused) bGMusicSource.Play();

        // Checks for instant skips due to to criteria being met in the past
        if ((currentSoundtrack.progressOnTaskCompletion && TaskManager.Instance.completedTasks.Contains(currentSoundtrack.progressAfter)) // Task is completed
            || CheckSoundtrackShouldProgress() // Timer has already passed
            || currentSoundtrack.progressOnPhaseTransition && TaskManager.Instance.isWeaponPhase) // Phase transition has already occurred
        {
            MoveToNextSoundtrack();
        }
    }

    private int GetRandomLoopTime()
    {
        int minLoop = currentSoundtrack.minSecondsBetweenLoops;
        int maxLoop = (int)Mathf.Clamp(currentSoundtrack.maxSecondsBetweenLoops, minLoop + 1, Mathf.Infinity);
        return Random.Range(minLoop, maxLoop);
    }
}
