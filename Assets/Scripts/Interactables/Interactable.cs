using UnityEngine;

public abstract class Interactable :MonoBehaviour
{
    [SerializeField] protected string promptMessage;
    [SerializeField] protected bool isInteractable = true;

    public void BaseInteract(Transform player)
    {
        Interact(player);
    }

    public virtual string GetPrompt()
    {
        return promptMessage;
    }

    public virtual bool CanInteract()
    {
        return isInteractable;
    }

    protected virtual void Interact(Transform player) {}
}
