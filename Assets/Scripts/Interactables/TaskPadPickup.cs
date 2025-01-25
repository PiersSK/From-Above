using UnityEngine;

public class TaskPadPickup : ClearableInteractable
{
    [SerializeField] private TaskManager taskManager;

    protected override void Interact(Transform player)
    {
        taskManager.ObtainTaskpad();
        UIManager.Instance.ShowTaskPadPrompt();
        base.Interact(player);
    }
}
