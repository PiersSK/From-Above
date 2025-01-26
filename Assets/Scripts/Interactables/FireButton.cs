using UnityEngine;

public class FireButton : Interactable
{
    [SerializeField] private Task task;
    [SerializeField] private GameObject gameOverUI;

    public override bool CanInteract()
    {
        return TaskManager.Instance.tasks.Contains(task);
    }

    protected override void Interact(Transform player)
    {
        TaskManager.Instance.CompleteTask(task);
        gameOverUI.SetActive(true);
    }


}
