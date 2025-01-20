using UnityEngine;

public class ServerDiscStorage : Interactable
{
    [SerializeField] private DataDrive driveStored;
    [SerializeField] private GameObject driveObj;
    private bool driveInDock = true;

    protected override void Interact(Transform player)
    {
        PlayerInventory inv = player.GetComponent<PlayerInventory>();

        if (!inv.dataDrivesHeld.Contains(driveStored) && driveInDock)
        {
            inv.dataDrivesHeld.Add(driveStored);
            driveInDock = false;
            driveObj.SetActive(false);
        } else if (!driveInDock)
        {
            inv.dataDrivesHeld.Remove(driveStored);
            driveInDock = true;
            driveObj.SetActive(true);
        }
    }
}
