using UnityEngine;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine.Audio;

public class VoiceLineManager : MonoBehaviour
{
    public List<VoiceLine> allVoiceLines;
    private AudioSource audioSource;
    private List<VoiceLine> playedVoiceLines = new List<VoiceLine>();
    public delegate void OnVoiceLinePlayed(VoiceLine vl);
    public static event OnVoiceLinePlayed VoiceLinePlayed;
    private float timeOfLastVoiceLine;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        TaskManager.TaskCompleted += TaskCompleted;
        TaskManager.PhaseChanged += PhaseChanged;
    }

    private void Start()
    {
        EvaluateVoiceLines();
    }

    private void Update()
    {

    }

    private void OnDestroy()
    {
        TaskManager.TaskCompleted -= TaskCompleted;
        TaskManager.PhaseChanged -= PhaseChanged;
    }

    private void TaskCompleted(Task task)
    {
        EvaluateVoiceLines();
    }

    private void PhaseChanged()
    {
        EvaluateVoiceLines();
    }

    private void EvaluateVoiceLines()
    {
        foreach (var vl in allVoiceLines)
        {
            if (playedVoiceLines.Contains(vl))
                continue;

            bool shouldPlay = false;

            if (vl.playOnPhaseTransition && TaskManager.Instance.isPhaseTwo)
            {
                shouldPlay = true;
            }
            else if (vl.playOnTaskCompletion && AreTaskConditionsSatisfied(vl))
            {
                shouldPlay = true;
            }

            if (shouldPlay) {
                PlayVoiceLine(vl);
            }
            else if(vl.playOnTimeCondition)
            {
            }
        }
    }

    private bool AreTaskConditionsSatisfied(VoiceLine vl)
    {
        if (vl.taskTrigger != null)
        {
            if (!TaskManager.Instance.completedTasks.Contains(vl.taskTrigger))
                return false;
        }
        return true;
    }
    private void PlayVoiceLine(VoiceLine vl)
    {
        if (vl.clip != null)
        {
            audioSource.resource = vl.clip;
            audioSource.Play();
            playedVoiceLines.Add(vl);
            timeOfLastVoiceLine = TimeController.Instance.GetTimeInSeconds(TimeController.Instance.time);
            VoiceLinePlayed?.Invoke(vl);
        }
    }
}