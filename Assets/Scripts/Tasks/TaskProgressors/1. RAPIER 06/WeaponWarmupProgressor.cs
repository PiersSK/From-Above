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

            PowerRouterController prc = PowerRouterController.Instance;
            prc.ForcePowerToLevel(prc.lifePower, 1);
            prc.ForcePowerToLevel(prc.shipPower, 1);
            prc.ForcePowerToLevel(prc.rapierPower, 3);
        }
    }
}
