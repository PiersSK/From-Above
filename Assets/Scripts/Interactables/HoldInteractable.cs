using UnityEngine;

public abstract class HoldInteractable : Interactable
{
    public void baseCancelInteract(Transform player)
    {
        CancelInteract(player);
    }

    protected virtual void CancelInteract(Transform player) {}
}