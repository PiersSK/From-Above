using UnityEngine;

public class TaskStateChange : MonoBehaviour
{
    public enum ChangeType
    {
        Disappear,
        Appear,
        LightEnable
    }
    [SerializeField] private ChangeType type;

    [SerializeField] private Task task;

    private void Update()
    {
        if(TaskManager.Instance.tasks.Contains(task))
        {
            if (type == ChangeType.Disappear) Destroy(gameObject);
            else if (type == ChangeType.Appear) GetComponent<Renderer>().enabled = true;
            else if (type == ChangeType.LightEnable) GetComponent<Light>().enabled = true;
        }
    }
}
