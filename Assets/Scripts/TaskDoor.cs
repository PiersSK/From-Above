using UnityEngine;

public class TaskDoor : MonoBehaviour
{
    [SerializeField] private Task task;

    private void Update()
    {
        if(TaskManager.Instance.tasks.Contains(task))
        {
            Destroy(gameObject);
        }
    }
}
