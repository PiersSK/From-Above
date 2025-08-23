using UnityEngine;

public class TaskPadPickup : Interactable
{
    [SerializeField] private TaskManager taskManager;

    protected override void Interact(Transform player)
    {
        taskManager.ObtainTaskpad(); 
        gameObject.SetActive(false);
        UIManager.Instance.ShowButtonPrompt(UIManager.ButtonPromptType.TaskPad, string.Empty, false);
        base.Interact(player);
    }
}
