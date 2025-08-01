using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class IPhase : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI taskPadHeader;
    [SerializeField] public AudioClip taskBeep;
    [SerializeField] protected Transform taskPadListParent;

    [SerializeField] public List<Task> tasks;
    protected List<ActiveTask> activeTasks = new();
    public List<Task> completedTasks = new List<Task>();
    protected bool sequentialTaskPhase = false;
    protected List<Task> sequentialTaskHolder = new List<Task>();

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

    public virtual bool ProgressTask(Task task)
    {
        ActiveTask activeTask = activeTasks.Find(x => x.task == task);
        bool taskCompleted = activeTask.ProgressTask();
        if (taskCompleted)
        {
            CompleteTask(task);
        }

        return taskCompleted;
    }

    public virtual void CompleteTask(Task completedTask) 
    {
        if (!tasks.Contains(completedTask)) return;

        completedTasks.Add(completedTask);
        tasks.Remove(completedTask);

        if (sequentialTaskPhase && sequentialTaskHolder.Count > 0)
        { 
            tasks.Clear();
            tasks.Add(sequentialTaskHolder[0]);
            sequentialTaskHolder.RemoveAt(0);
        }
    }

    public virtual void BeginCurrentPhase()
    {
        if(sequentialTaskPhase)
        {
            sequentialTaskHolder.AddRange(tasks);
            tasks.Clear();
            tasks.Add(sequentialTaskHolder[0]);
            sequentialTaskHolder.RemoveAt(0);
        }

        gameObject.SetActive(true);
    }

    public virtual void EndCurrentPhase()
    {
        gameObject.SetActive(false);
        completedTasks.Clear();
    }
}
