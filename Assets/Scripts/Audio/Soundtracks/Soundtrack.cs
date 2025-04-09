using UnityEngine;

[CreateAssetMenu(fileName = "Soundtrack", menuName = "Scriptable Objects/Soundtrack")]
public class Soundtrack : ScriptableObject
{
    public AudioClip clip;
    public Soundtrack nextSoundtrack;

    public bool progressOnEnd;
    public bool progressOnTimeCondition;
    public bool progressOnPhaseTransition;
    public bool progressOnTaskCompletion;

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
    public TimeConditionType timeConditionType;
    public int absoluteSecondsIntoPhase;
    public int absoluteSecondsBeforePhaseEnd;
    public int percentageThroughPhase;

    public Task progressAfter;
}
