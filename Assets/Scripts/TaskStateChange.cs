using UnityEngine;

public class TaskStateChange : MonoBehaviour
{
    public enum ChangeType
    {
        Disappear,
        Appear,
        LightEnable,
        InvokeInteractable
    }
    [SerializeField] private ChangeType type;

    [SerializeField] private Task task;

    [SerializeField] private Interactable interactable;
    [SerializeField] private string interactableFunctionToInvoke;

    private bool triggered = false;

    private void Start()
    {
        if (type == ChangeType.InvokeInteractable && (interactable == null || interactableFunctionToInvoke == string.Empty))
            Debug.LogWarning("Object " + gameObject.name + "has TaskStateChange type of InvokeInteractable but no interactable was provided as a reference");
    }

    private void Update()
    {
        if(TaskManager.Instance.tasks.Contains(task) && !triggered)
        {
            if (type == ChangeType.Disappear) Destroy(gameObject);
            else if (type == ChangeType.Appear) GetComponent<Renderer>().enabled = true;
            else if (type == ChangeType.LightEnable) GetComponent<Light>().enabled = true;
            else if (type == ChangeType.InvokeInteractable)
            {
                if (interactable == null || interactableFunctionToInvoke == string.Empty)
                {
                    Debug.LogWarning("Object " + gameObject.name + "has TaskStateChange type of InvokeInteractable but no interactable was provided as a reference. Nothing happened when triggered");
                } else
                {
                    interactable.Invoke(interactableFunctionToInvoke, 0f);
                }
            }

            triggered = true;
        }
    }
}
