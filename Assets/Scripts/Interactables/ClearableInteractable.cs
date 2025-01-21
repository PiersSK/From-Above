using UnityEngine;

public class ClearableInteractable : Interactable
{
    protected override void Interact(Transform player)
    {
        gameObject.SetActive(false);
    }
}
