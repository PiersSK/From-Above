using UnityEngine;

public abstract class HoldInteractable : Interactable
{
    protected override void Interact(Transform player)
    {
        PlayerMotor.Instance.SetPlayerLock(true);
        PlayerLook.Instance.SetLookLock(true, false);
        base.Interact(player);
    }

    public void baseCancelInteract(Transform player)
    {
        CancelInteract(player);
    }

    protected virtual void CancelInteract(Transform player)
    {
        PlayerMotor.Instance.SetPlayerLock(false);
        PlayerLook.Instance.SetLookLock(false, false);
    }
}