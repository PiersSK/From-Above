using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "Scriptable Objects/Task")]
public class Task : ScriptableObject
{
    public enum TaskType
    {
        Daily,
        Weapon
    }

    public string taskName;
    public string taskLocation;
    public string taskNumber;
    public TaskType taskType;
    public string weaponStatusMessage;
}
