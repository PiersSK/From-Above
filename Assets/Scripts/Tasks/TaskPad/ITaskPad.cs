using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class ITaskPad : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI taskPadHeader;
    [SerializeField] public AudioClip taskBeep;
    [SerializeField] public List<Task> tasks;
    [SerializeField] protected Transform taskPadListParent;

    public List<Task> completedTasks = new List<Task>();

    protected bool hasTimer = false;
    protected const string TASKUIOBJECT = "Task";

    public virtual void UpdateTaskPadUI() 
    {
        foreach (Transform task in taskPadListParent) Destroy(task.gameObject);

        foreach (var task in tasks)
        {
            Transform taskUI = Instantiate(Resources.Load<Transform>("Task"), taskPadListParent);
            taskUI.GetComponent<TaskPadTask>().SetTask(task);
        }
    }

    public virtual void CompleteTask(Task completedTask) 
    {
        if (!tasks.Contains(completedTask)) return;

        completedTasks.Add(completedTask);
        tasks.Remove(completedTask);
    }

    public virtual void BeginCurrentPhase()
    {
        gameObject.SetActive(true);
    }

    public virtual void EndCurrentPhase()
    {
        gameObject.SetActive(false);
        completedTasks.Clear();
    }
}
