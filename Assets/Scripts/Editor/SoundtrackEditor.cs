using UnityEditor;
using UnityEngine;
using static Soundtrack;

[CustomEditor(typeof(Soundtrack))]
public class SoundtrackEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Soundtrack soundtrack = (Soundtrack)target;

        EditorGUILayout.LabelField("Audio Clips", EditorStyles.boldLabel);
        soundtrack.clip = (AudioClip)EditorGUILayout.ObjectField("Clip", soundtrack.clip, typeof(AudioClip), false);
        soundtrack.nextSoundtrack = (Soundtrack)EditorGUILayout.ObjectField("Next Soundtrack", soundtrack.nextSoundtrack, typeof(Soundtrack), false);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Progression Conditions", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("The soundtrack will progress if ANY of these conditions are met", MessageType.Info);
        soundtrack.progressOnEnd = EditorGUILayout.Toggle("Progess on End", soundtrack.progressOnEnd);
        soundtrack.progressOnTimeCondition = EditorGUILayout.Toggle("Progess on Time Condition", soundtrack.progressOnTimeCondition);
        soundtrack.progressOnPhaseTransition = EditorGUILayout.Toggle("Progess on Phase Transition", soundtrack.progressOnPhaseTransition);
        soundtrack.progressOnTaskCompletion = EditorGUILayout.Toggle("Progess on Task Completion", soundtrack.progressOnTaskCompletion);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Loop Settings", EditorStyles.boldLabel);
        soundtrack.shouldLoop = EditorGUILayout.Toggle("Should Loop", soundtrack.shouldLoop);
        soundtrack.leaveTimeBetweenLoops = EditorGUILayout.Toggle("Leave Time Between Loops", soundtrack.leaveTimeBetweenLoops);
        EditorGUI.BeginDisabledGroup(!soundtrack.leaveTimeBetweenLoops);
        soundtrack.minSecondsBetweenLoops = EditorGUILayout.IntField("Minimum Seconds Between Loops", soundtrack.minSecondsBetweenLoops);
        soundtrack.maxSecondsBetweenLoops = EditorGUILayout.IntField("Maximum Seconds Between Loops", soundtrack.maxSecondsBetweenLoops);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(!soundtrack.progressOnTimeCondition);
        EditorGUILayout.LabelField("Time Condition Settings", EditorStyles.boldLabel);
        soundtrack.timeConditionType = (TimeConditionType)EditorGUILayout.EnumPopup("Time Condition Type", soundtrack.timeConditionType);
        EditorGUI.BeginDisabledGroup(soundtrack.timeConditionType != TimeConditionType.AbsoluteFromStart);
        soundtrack.absoluteSecondsIntoPhase = EditorGUILayout.IntSlider("Seconds Into Phase", soundtrack.absoluteSecondsIntoPhase, 1, 1000);
        EditorGUI.EndDisabledGroup();
        EditorGUI.BeginDisabledGroup(soundtrack.timeConditionType != TimeConditionType.AbsoluteFromEnd);
        soundtrack.absoluteSecondsBeforePhaseEnd = EditorGUILayout.IntSlider("Seconds Before Phase End", soundtrack.absoluteSecondsBeforePhaseEnd, 1, 1000);
        EditorGUI.EndDisabledGroup();
        EditorGUI.BeginDisabledGroup(soundtrack.timeConditionType != TimeConditionType.Relative);
        soundtrack.percentageThroughPhase = EditorGUILayout.IntSlider("% Through Phase", soundtrack.percentageThroughPhase, 1, 99);
        EditorGUI.EndDisabledGroup();
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(!soundtrack.progressOnTaskCompletion);
        EditorGUILayout.LabelField("Task Condition Settings", EditorStyles.boldLabel);
        soundtrack.progressAfter = (TaskData)EditorGUILayout.ObjectField("Progress After", soundtrack.progressAfter, typeof(TaskData), false);
        EditorGUI.EndDisabledGroup();

        EditorUtility.SetDirty(soundtrack);
    }
}
