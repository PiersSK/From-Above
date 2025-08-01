using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskPadTask : MonoBehaviour
{
    public TaskData task;

    [SerializeField] private TextMeshProUGUI taskTitle;
    [SerializeField] private TextMeshProUGUI taskLocation;
    [SerializeField] private TextMeshProUGUI taskNumber;
    [SerializeField] private TextMeshProUGUI taskDescription;

    [SerializeField] private Image numberBackground;
    [SerializeField] private Image locationBackground;
    [SerializeField] private Image titleBackground;

    private const string MISSINGSTEP = "No task details provided";
    private const string MISSINGLOCATION = "???";

    public void SetTask(ActiveTask t)
    {
        task = t.task;

        taskTitle.text = task.taskName;
        taskLocation.text = task.stepLocations.Count > 0 ? task.stepLocations[t.currentStep] : MISSINGLOCATION;
        taskNumber.text = task.taskNumber;
        taskDescription.text = task.taskSteps.Count > 0 ? task.taskSteps[t.currentStep] : MISSINGSTEP;

        if (task.taskType == TaskData.TaskType.Daily)
            SetUIColors(UIColors.terminalGreen);
        else if (task.taskType == TaskData.TaskType.Weapon)
            SetUIColors(UIColors.terminalRed);
    }

    private void SetUIColors(Color color)
    {
        numberBackground.color = color;
        locationBackground.color = color;
        titleBackground.color = new Color(color.r, color.g, color.b, 0.5f);
    }
}
