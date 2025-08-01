using UnityEngine;

public class RoomTidyCounter : MonoBehaviour
{
    public bool swept = false;
    public int objectsRemoved = 0;
    [SerializeField] private int objectsToRemove = 2;

    private bool completed = false;
    [SerializeField] private TaskData task;

    private void Update()
    {
        if (swept && objectsRemoved == objectsToRemove && !completed)
        {
            TaskManager.Instance.ProgressTask(task);
            completed = true;
        }
    }
}
