using UnityEngine;

public class RoomTidyCounter : MonoBehaviour
{
    public bool swept = false;
    public int objectsRemoved = 0;
    [SerializeField] private int objectsToRemove = 2;

    private bool completed = false;
    [SerializeField] private Task task;

    private void Update()
    {
        if (swept && objectsRemoved == objectsToRemove && !completed)
        {
            TaskManager.Instance.CompleteTask(task);
            completed = true;
        }
    }
}
