using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskPadTask : MonoBehaviour
{
    public Task task;

    [SerializeField] private TextMeshProUGUI taskTitle;
    [SerializeField] private TextMeshProUGUI taskLocation;
    [SerializeField] private TextMeshProUGUI taskNumber;
    [SerializeField] private TextMeshProUGUI taskDescription;

    [SerializeField] private Image numberBackground;
    [SerializeField] private Image locationBackground;
    [SerializeField] private Image titleBackground;

    private const string MISSINGSTEP = "No task details provided";

    public void SetTask(Task t)
    {
        task = t;

        taskTitle.text = task.taskName;
        taskLocation.text = task.taskLocation;
        taskNumber.text = task.taskNumber;
        taskDescription.text = task.taskSteps.Count > 0 ? task.taskSteps[0] : MISSINGSTEP;

        if (task.taskType == Task.TaskType.Daily)
            SetUIColors(UIColors.terminalGreen);
        else if (task.taskType == Task.TaskType.Weapon)
            SetUIColors(UIColors.terminalRed);
    }

    private void SetUIColors(Color color)
    {
        numberBackground.color = color;
        locationBackground.color = color;
        titleBackground.color = new Color(color.r, color.g, color.b, 0.5f);
    }
}
