using UnityEngine;

public class PDStorageDiscDrive : Interactable
{
    [SerializeField] private PDStorage storage;

    public override bool CanInteract()
    {
        return PlayerInventory.Instance.dataDrivesHeld.Count > 0;
    }

    protected override void Interact(Transform player)
    {
        UIManager.Instance.ShowPDSelectUI(storage.AddPD);
    }
}
