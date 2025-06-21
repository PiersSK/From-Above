using System.Collections.Generic;
using UnityEngine;

public class ServerStorageDiscDrive : Interactable
{
    [SerializeField] private IServerHubStorage storage;
    public enum StorageType
    {
        PentaDisc,
        ExeCard
    }

    [SerializeField] private StorageType linkedStorageType;

    public override bool CanInteract()
    {
        if(linkedStorageType == StorageType.PentaDisc) 
            return PlayerInventory.Instance.dataDrivesHeld.Count > 0;
        else return PlayerInventory.Instance.exesHeld.Count > 0;
    }

    protected override void Interact(Transform player)
    {
        List<IServerDataObject> inventory = new();

        if (linkedStorageType == StorageType.PentaDisc)
            inventory = PlayerInventory.Instance.dataDrivesHeld;
        else
            inventory = PlayerInventory.Instance.exesHeld;

        UIManager.Instance.ShowPDSelectUI(storage.AddNewObjectToStorage, inventory);
    }
}
