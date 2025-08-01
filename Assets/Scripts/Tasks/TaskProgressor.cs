using UnityEngine;

public class TaskProgressor : MonoBehaviour
{
    [SerializeField] protected TaskData task;

    protected virtual void OnEnable()
    {
        TaskManager.TaskCompleted += IsTaskCompleted;
    }

    protected virtual void OnDisable()
    {
        TaskManager.TaskCompleted -= IsTaskCompleted;
    }

    private void IsTaskCompleted(TaskData task)
    {
        if (this.task == task)
        {
            TaskManager.TaskCompleted -= IsTaskCompleted;
            Destroy(gameObject);
        }
    }
}
