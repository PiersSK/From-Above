using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTaskData", menuName = "Scriptable Objects/Task Data")]
public class TaskData : ScriptableObject
{
    public enum TaskType
    {
        Daily,
        Weapon,
        DownTime
    }

    public string taskName;
    public string taskNumber;
    public TaskType taskType;

    public List<string> stepLocations = new List<string>();
    public List<string> taskSteps = new List<string>();
}
