using UnityEngine;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine.Audio;
using UnityEditor.MPE;

public class VoiceLineManager : MonoBehaviour
{
    public List<VoiceLine> allVoiceLines;
    private AudioSource audioSource;
    private List<VoiceLine> playedVoiceLines = new List<VoiceLine>();
    public delegate void OnVoiceLinePlayed(VoiceLine vl);
    public static event OnVoiceLinePlayed VoiceLinePlayed;
    private float timeSinceLastVoiceLine;

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
        foreach(VoiceLine vl in allVoiceLines)
        {
            if (!playedVoiceLines.Contains(vl) && vl.playOnTimeCondition && AreTimeConditionsSatisfied(vl))
                PlayVoiceLine(vl);
        }
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
            timeSinceLastVoiceLine = 0f;
            VoiceLinePlayed?.Invoke(vl);
        }
    }

    private bool AreTimeConditionsSatisfied(VoiceLine vl)
    {
        switch (vl.timeConditionType)
        {
            case VoiceLine.TimeConditionType.AbsoluteFromStart:
                if(TimeController.Instance.time >= vl.absoluteSecondsIntoPhase)
                    return true;
                else return false;
            
            case VoiceLine.TimeConditionType.RelativeToLastVoiceLine:
                if(vl.name == "Rapier 6 6 - Inactivity Chaser")
                    if(vl.secondsSinceLastVoiceLine <= TimeController.Instance.getOnWithItTimer)
                        return true;
                    else return false;
                else if (vl.secondsSinceLastVoiceLine <= TimeController.Instance.radioMessageTimer)   
                        return true;
                else return false;
            
            default: return false;
        }
    }
}