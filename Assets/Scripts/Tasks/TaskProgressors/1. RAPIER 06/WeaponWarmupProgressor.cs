using UnityEngine;

public class WeaponWarmupProgressor : TaskProgressor
{
    protected override void OnEnable()
    {
        base.OnEnable();
        Interactable.PlayerInteracted += WarmupButtonPressed;
    }

    private void WarmupButtonPressed(Interactable interactable)
    {
        if (interactable is WarmupButton)
        {
            TaskManager.Instance.ProgressTask(task);
            Interactable.PlayerInteracted -= WarmupButtonPressed;
        }
    }
}
