using System.Collections.Generic;
using UnityEngine;

public class PDStorage : IServerHubStorage
{
    public static event OnServerStorageObjectChange PDStorageChanged;
    public List<DiscSlotContent> encryptedContent = new();
    public List<DiscSlotContent> transmittedContent = new();
    public List<DiscSlotContent> transmittedEncryptedContent = new();
    public List<DataDrive> receivedPDs = new();
    public List<DataDrive> revertedPDs = new();

    protected override void UpdateVisualObjects()
    {
        foreach (Transform obj in visualObjects) obj.gameObject.SetActive(false);
        for (int i = 0; i < objectsStored.Count; i++)
        {
            Transform obj = visualObjects.GetChild(i);
            obj.gameObject.SetActive(true);
            obj.localPosition = new Vector3(
                obj.localPosition.x,
                i == currentIndex ? 0.1f : 0f,
                obj.localPosition.z
            );
        }
    }

    public override void SelectNext()
    {
        base.SelectNext();
        PDStorageChanged?.Invoke(null);
    }

    public override void SelectPrevious()
    {
        base.SelectPrevious();
        PDStorageChanged?.Invoke(null);
    }

    public override void AddNewObjectToStorage(IServerDataObject newObj)
    {
        base.AddNewObjectToStorage(newObj);
        PlayerInventory.Instance.dataDrivesHeld.Remove((DataDrive)newObj);
        PDStorageChanged?.Invoke(newObj);
    }

    public override void EjectObject()
    {
        if (objectsStored.Count > 0) PlayerInventory.Instance.dataDrivesHeld.Add((DataDrive)objectsStored[currentIndex]);
        base.EjectObject();
        PDStorageChanged?.Invoke(null);
    }

    public List<DiscSlotContent> GetPDSlots(DataDrive pd)
    {
        if (receivedPDs.Contains(pd)) return pd.receivedSlots;
        else if (revertedPDs.Contains(pd)) return pd.revertSlots;
        else return pd.slots;
    }
}
