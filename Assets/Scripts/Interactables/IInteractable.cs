using UnityEngine;

public interface IInteractable
{
    public void BaseInteract(Transform player);

    public string GetPrompt();

    public string GetRequirementMessage();

    public bool CanInteract();

    protected void Interact(Transform player);
}