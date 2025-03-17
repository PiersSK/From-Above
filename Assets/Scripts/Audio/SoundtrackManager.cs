using NUnit.Framework;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{
    [SerializeField] private Soundtrack currentSoundtrack;
    [SerializeField] private Soundtrack DEBUG_phase2SoundtrackStart;
    [SerializeField] private AudioSource bGMusicSource;

    private float randomLoopTimer = 0f;
    private float timeTillNextRandomLooperPlay = 0f;

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

    private void Start()
    {
        if (TaskManager.Instance.DEBUG_startOnPhaseTwo) currentSoundtrack = DEBUG_phase2SoundtrackStart;
        
        StartNewSoundtrack();
    }

    private void Update()
    {
        if (currentSoundtrack.leaveTimeBetweenLoops && !bGMusicSource.isPlaying && !SoundManager.Instance.bgPaused)
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
        if (currentSoundtrack.progressOnEnd && !bGMusicSource.isPlaying && !SoundManager.Instance.bgPaused)
            return true;

        if (currentSoundtrack.progressOnTimeCondition)
        {
            float currentTime = TimeController.Instance.GetTimeInSeconds();
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

        bGMusicSource.Play();

        if (currentSoundtrack.progressOnTaskCompletion && TaskManager.Instance.completedTasks.Contains(currentSoundtrack.progressAfter))
            MoveToNextSoundtrack(); // Instant skip if task completed prior to soundtrack start
    }

    private int GetRandomLoopTime()
    {
        int minLoop = currentSoundtrack.minSecondsBetweenLoops;
        int maxLoop = (int)Mathf.Clamp(currentSoundtrack.maxSecondsBetweenLoops, minLoop + 1, Mathf.Infinity);
        return Random.Range(minLoop, maxLoop);
    }
}
