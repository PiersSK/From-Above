using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomTidyCounter : MonoBehaviour
{
    public int objectsCleaned = 0;
    [SerializeField] private List<GameObject> objectsToClean;

    private bool completed = false;
    [SerializeField] private TaskData task;

    private void Update()
    {
        if (objectsCleaned == objectsToClean.Count && !completed)
        {
            TaskManager.Instance.ProgressTask(task);
            completed = true;
        }
    }
}
