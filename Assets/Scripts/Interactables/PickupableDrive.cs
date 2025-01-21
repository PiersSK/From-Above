using UnityEngine;

public class PickupableDrive : Interactable
{
    [SerializeField] private DataDrive dataDrive;

    protected override void Interact(Transform player)
    {
        player.GetComponent<PlayerInventory>().dataDrivesHeld.Add(dataDrive);
        dataDrive = null;
        isInteractable = false;
        gameObject.SetActive(false);
    }
}
