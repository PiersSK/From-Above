using UnityEngine;

public abstract class Interactable :MonoBehaviour
{
    public string promptMessage;
    public bool isInteractable = true;

    public void BaseInteract(Transform player)
    {
        Interact(player);
    }

    protected virtual void Interact(Transform player) {}
}
