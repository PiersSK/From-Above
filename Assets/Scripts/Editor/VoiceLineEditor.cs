using UnityEditor;
using UnityEngine;
using static VoiceLine;

[CustomEditor(typeof(VoiceLine))]

public class VoiceLineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        VoiceLine voiceLine = (VoiceLine)target;

        EditorGUILayout.LabelField("Audio Clips", EditorStyles.boldLabel);
        voiceLine.clip = (AudioClip)EditorGUILayout.ObjectField("Clip", voiceLine.clip, typeof(AudioClip), false);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Trigger Conditions", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("The VoiceLine will play if ANY of these conditions are met", MessageType.Info);
        voiceLine.playOnPhaseTransition = EditorGUILayout.Toggle("Play on Phase Transition", voiceLine.playOnPhaseTransition);
        voiceLine.playOnTaskCompletion = EditorGUILayout.Toggle("Play on Task Completion", voiceLine.playOnTaskCompletion);
        voiceLine.playOnTimeCondition = EditorGUILayout.Toggle("Play on Time Condition", voiceLine.playOnTimeCondition);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Task Trigger Conditions", EditorStyles.boldLabel);
        EditorGUI.BeginDisabledGroup(!voiceLine.playOnTaskCompletion);
        voiceLine.taskTrigger = (Task)EditorGUILayout.ObjectField("Task", voiceLine.taskTrigger, typeof(Task), false);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(!voiceLine.playOnTimeCondition);
        EditorGUILayout.LabelField("Time Condition Settings", EditorStyles.boldLabel);
        voiceLine.timeConditionType = (TimeConditionType)EditorGUILayout.EnumPopup("Time Condition Type", voiceLine.timeConditionType);
        EditorGUI.BeginDisabledGroup(voiceLine.timeConditionType != TimeConditionType.AbsoluteFromStart);
        voiceLine.absoluteSecondsIntoPhase = EditorGUILayout.IntSlider("Seconds Into Phase", voiceLine.absoluteSecondsIntoPhase, 1, 1000);
        EditorGUI.EndDisabledGroup();
        EditorGUI.BeginDisabledGroup(voiceLine.timeConditionType != TimeConditionType.AbsoluteFromEnd);
        voiceLine.absoluteSecondsBeforePhaseEnd = EditorGUILayout.IntSlider("Seconds Before Phase End", voiceLine.absoluteSecondsBeforePhaseEnd, 1, 1000);
        EditorGUI.EndDisabledGroup();
        EditorGUI.BeginDisabledGroup(voiceLine.timeConditionType != TimeConditionType.RelativeToLastVoiceLine);
        voiceLine.secondsSinceLastVoiceLine = EditorGUILayout.IntSlider("Seconds Since Last Voice Line", voiceLine.secondsSinceLastVoiceLine, 1, 1000);
        EditorGUI.EndDisabledGroup();
        EditorGUI.BeginDisabledGroup(voiceLine.timeConditionType != TimeConditionType.PeriodOfInactivity);
        voiceLine.secondsOfInactivity = EditorGUILayout.IntSlider("Seconds of Inactivity", voiceLine.secondsOfInactivity, 1, 1000);
        voiceLine.inactivityStartTask = (Task)EditorGUILayout.ObjectField("Task", voiceLine.inactivityStartTask, typeof(Task), false);
        voiceLine.inactivityEndTask = (Task)EditorGUILayout.ObjectField("Task", voiceLine.inactivityEndTask, typeof(Task), false);
        EditorGUI.EndDisabledGroup();
        EditorGUI.EndDisabledGroup();

        EditorUtility.SetDirty(voiceLine);
    }
}