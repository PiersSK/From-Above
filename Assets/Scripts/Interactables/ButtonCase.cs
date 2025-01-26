using UnityEngine;

public class ButtonCase : Interactable
{
    [SerializeField] private Task task;
    private bool isUp = false;

    public override bool CanInteract()
    {
        return !isUp && TaskManager.Instance.tasks.Contains(task);
    }

    protected override void Interact(Transform player)
    {
        GetComponent<Animation>().Play();
        isUp = true;
        DoomsdayStatusUI.Instance.safetyOff = true;
        TaskManager.Instance.CompleteTask(task);
    }
}
