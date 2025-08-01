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
        voiceLine.taskTrigger = (TaskData)EditorGUILayout.ObjectField("Task", voiceLine.taskTrigger, typeof(TaskData), false);
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
        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(voiceLine.timeConditionType != TimeConditionType.RelativeToLastVoiceLine);
        EditorGUILayout.LabelField("Relative Time Condition Settings", EditorStyles.miniBoldLabel);
        voiceLine.secondsSinceLastVoiceLine = EditorGUILayout.IntSlider("Seconds Since Last Voice Line", voiceLine.secondsSinceLastVoiceLine, 1, 1000);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(voiceLine.timeConditionType != TimeConditionType.PeriodOfInactivity);
        EditorGUILayout.LabelField("Inactivity Condition Settings", EditorStyles.miniBoldLabel);
        voiceLine.secondsOfInactivity = EditorGUILayout.IntSlider("Seconds of Inactivity", voiceLine.secondsOfInactivity, 1, 1000);
        voiceLine.inactivityStartTask = (TaskData)EditorGUILayout.ObjectField("Inactivity Start Task", voiceLine.inactivityStartTask, typeof(TaskData), false);
        voiceLine.inactivityEndTask = (TaskData)EditorGUILayout.ObjectField("Inactivity End Task", voiceLine.inactivityEndTask, typeof(TaskData), false);
        EditorGUI.EndDisabledGroup();
        EditorGUI.EndDisabledGroup();

        EditorUtility.SetDirty(voiceLine);
    }
}