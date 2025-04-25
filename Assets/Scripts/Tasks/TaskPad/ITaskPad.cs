using TMPro;
using UnityEngine;

public abstract class ITaskPad : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI taskPadHeader;
    [SerializeField] public Phase phaseData;
    [SerializeField] protected Transform taskPadListParent;

    protected bool hasTimer = false;
    protected const string TASKUIOBJECT = "Task";

    public virtual void UpdateTaskPadUI() 
    {
        foreach (Transform task in taskPadListParent) Destroy(task.gameObject);

        foreach (var task in phaseData.tasks)
        {
            Transform taskUI = Instantiate(Resources.Load<Transform>("Task"), taskPadListParent);
            taskUI.GetComponent<TaskPadTask>().SetTask(task);
        }
    }

    public virtual void CompleteTask(Task completedTask) 
    {
        if (!phaseData.tasks.Contains(completedTask)) return;

        phaseData.completedTasks.Add(completedTask);
        phaseData.tasks.Remove(completedTask);
    }

    public virtual void BeginCurrentPhase()
    {
        gameObject.SetActive(true);
    }

    public virtual void EndCurrentPhase()
    {
        gameObject.SetActive(false);
        phaseData.tasks = phaseData.completedTasks;
        phaseData.completedTasks.Clear();
    }
}
