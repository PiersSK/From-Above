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

    private Color dailyColor = new Color(0.2f, 0.69f, 0.2f);
    private Color weaponColor = new Color(0.69f, 0.3f, 0.22f);

    public void SetTask(Task t)
    {
        task = t;

        taskTitle.text = task.taskName;
        taskLocation.text = task.taskLocation;
        taskNumber.text = task.taskNumber;

        if (task.taskType == Task.TaskType.Daily)
        {
            taskTitle.color = dailyColor;
            locationPrefix.color = dailyColor;
            taskLocation.color = dailyColor;
            taskNumberBackground.color = dailyColor;
            taskBorder.color = dailyColor;
        } else if (task.taskType == Task.TaskType.Weapon)
        {
            taskTitle.color = weaponColor;
            locationPrefix.color = weaponColor;
            taskLocation.color = weaponColor;
            taskNumberBackground.color = weaponColor;
            taskBorder.color = weaponColor;
        }
    }
}
