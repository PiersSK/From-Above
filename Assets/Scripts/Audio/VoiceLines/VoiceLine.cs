using UnityEngine;

[CreateAssetMenu(fileName = "VoiceLine", menuName = "Scriptable Objects/VoiceLine")]
public class VoiceLine : ScriptableObject
{
    public AudioClip clip;
    public bool playOnTimeCondition;
    public bool playOnPhaseTransition;
    public bool playOnTaskCompletion;
    public enum TimeConditionType
    {
        AbsoluteFromStart,
        AbsoluteFromEnd,
        RelativeToLastVoiceLine,
        PeriodOfInactivity
    }
    public TimeConditionType timeConditionType;
    public int absoluteSecondsIntoPhase;
    public int absoluteSecondsBeforePhaseEnd;
    public int percentageThroughPhase;
    public int secondsSinceLastVoiceLine;
    public int secondsOfInactivity;
    public Task inactivityStartTask;
    public Task inactivityEndTask;
    public Task taskTrigger;
}
