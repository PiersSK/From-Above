using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected string promptMessage;
    [SerializeField] protected string requirementsNotMetMessage;
    public bool isInteractable = true;

    public delegate void OnInteract(Interactable interactable);
    public static event OnInteract PlayerInteracted;

    public void BaseInteract(Transform player)
    {
        Interact(player);
    }

    public virtual string GetPrompt()
    {
        return promptMessage;
    }

    public virtual string GetRequirementMessage()
    {
        return requirementsNotMetMessage;
    }

    public virtual bool CanInteract()
    {
        return isInteractable;
    }

    protected virtual void Interact(Transform player)
    {
        PlayerInteracted?.Invoke(this);
    }
}
