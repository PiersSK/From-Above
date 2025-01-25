using UnityEngine;

public class ClearableInteractable : Interactable
{
    [SerializeField] private RoomTidyCounter tidy;

    protected override void Interact(Transform player)
    {
        if(tidy != null) tidy.objectsRemoved++;
        gameObject.SetActive(false);
    }
}
