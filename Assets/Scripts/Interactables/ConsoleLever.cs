using UnityEngine;

public class ConsoleLever : Interactable
{
    public bool isFlipped = false;

    [SerializeField] private ConsoleLever otherLever;
    [SerializeField] private Task task;

    protected override void Interact(Transform player)
    {
        GetComponent<Animation>().Play();
        isFlipped = true;

        if (otherLever.isFlipped) TaskManager.Instance.CompleteTask(task);
    }

    public override bool CanInteract()
    {
        return TaskManager.Instance.isPhaseTwo && !isFlipped && TaskManager.Instance.tasks.Contains(task);
    }
}
