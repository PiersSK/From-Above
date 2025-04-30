using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PDStorage : IServerHubStorage
{
    public static event OnServerStorageObjectChange PDStorageChanged;
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

    public override void AddNewObjectToStorage(IServerDataObject newObj)
    {
        base.AddNewObjectToStorage(newObj);
        PlayerInventory.Instance.dataDrivesHeld.Remove((DataDrive)newObj);
        PDStorageChanged?.Invoke();
    }

    public override void EjectObject()
    {
        if (objectsStored.Count > 0) PlayerInventory.Instance.dataDrivesHeld.Add((DataDrive)objectsStored[currentIndex]);
        base.EjectObject();
        PDStorageChanged?.Invoke();
    }
}
