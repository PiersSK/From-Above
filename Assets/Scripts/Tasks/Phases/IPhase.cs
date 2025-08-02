using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class IPhase : MonoBehaviour
{
    [SerializeField] public Image taskPadHeader;
    [SerializeField] public AudioClip taskBeep;
    [SerializeField] protected Transform taskPadListParent;

    [SerializeField] public List<TaskData> tasks;
    protected List<ActiveTask> activeTasks = new();
    public List<TaskData> completedTasks = new List<TaskData>();
    protected bool sequentialTaskPhase = false;
    protected List<TaskData> sequentialTaskHolder = new List<TaskData>();

    protected bool hasTimer = false;
    protected const string TASKUIOBJECT = "Task";

    public virtual void UpdateTaskPadUI() 
    {
        foreach (Transform task in taskPadListParent) Destroy(task.gameObject);

        foreach (var task in tasks)
        {
            Transform taskUI = Instantiate(Resources.Load<Transform>("Task"), taskPadListParent);
            taskUI.GetComponent<TaskPadTask>().SetTask(GetActiveTask(task));
        }
    }

    public virtual bool ProgressTask(TaskData task)
    {
        ActiveTask activeTask = GetActiveTask(task);
        bool taskCompleted = activeTask.ProgressTask();
        if (taskCompleted)
        {
            CompleteTask(task);
        }

        UpdateTaskPadUI();

        return taskCompleted;
    }

    public virtual void CompleteTask(TaskData completedTask) 
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
        activeTasks.Clear();
        foreach(var task in tasks) activeTasks.Add(new ActiveTask(task));

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

    public ActiveTask GetActiveTask(TaskData t)
    {
        return activeTasks.Find(x => x.task == t);
    }
}
