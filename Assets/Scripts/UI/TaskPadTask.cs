using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskPadTask : MonoBehaviour
{
    public Task task;

    [SerializeField] private TextMeshProUGUI taskTitle;
    [SerializeField] private TextMeshProUGUI locationPrefix;
    [SerializeField] private TextMeshProUGUI taskLocation;
    [SerializeField] private TextMeshProUGUI taskNumber;
    [SerializeField] private Image taskNumberBackground;
    [SerializeField] private Image taskBorder;

    public void SetTask(Task t)
    {
        task = t;

        taskTitle.text = task.taskName;
        taskLocation.text = task.taskLocation;
        taskNumber.text = task.taskNumber;

        if (task.taskType == Task.TaskType.Daily)
            SetUIColors(UIColors.terminalGreen);
        else if (task.taskType == Task.TaskType.Weapon)
            SetUIColors(UIColors.terminalRed);
    }

    private void SetUIColors(Color color)
    {
        taskTitle.color = color;
        locationPrefix.color = color;
        taskLocation.color = color;
        taskNumberBackground.color = color;
        taskBorder.color = color;
    }
}
