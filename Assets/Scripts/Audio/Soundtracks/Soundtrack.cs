using UnityEngine;

[CreateAssetMenu(fileName = "Soundtrack", menuName = "Scriptable Objects/Soundtrack")]
public class Soundtrack : ScriptableObject
{
    public AudioClip clip;
    public Soundtrack nextClip;

    [Header("Progression Conditions")]
    public bool progressOnEnd;
    public bool progressOnTimeCondition;
    public bool progressOnPhaseTransition;
    public bool progressOnTaskCompletion;

    [Header("Loop Settings")]
    public bool shouldLoop;
    public bool leaveTimeBetweenLoops;
    public int minSecondsBetweenLoops;
    public int maxSecondsBetweenLoops;

    public enum TimeConditionType
    {
        AbsoluteFromStart,
        AbsoluteFromEnd,
        Relative
    }
    [Header("Time Condition Settings")]
    public TimeConditionType timeConditionType;
    public int absoluteSecondsIntoPhase;
    public int absoluteSecondsBeforePhaseEnd;
    public int percentageThroughPhase;

    [Header("Task Condition Settings")]
    public Task progressAfter;
}
