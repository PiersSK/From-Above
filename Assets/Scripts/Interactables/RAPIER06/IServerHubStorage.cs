using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class IServerHubStorage : MonoBehaviour
{
    [Header("Stored Elements")]
    public List<IServerDataObject> objectsStored;
    public Transform visualObjects;
    public int currentIndex = 0;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI screenDisplay;

    private const string EMPTYMESSAGE = "<i>Nothing in Storage</i>";

    public delegate void OnServerStorageObjectChange();

    private void Start()
    {
        UpdateVisibleState();
    }

    private void UpdateVisibleState()
    {
        screenDisplay.text = objectsStored.Count > 0 ? objectsStored[currentIndex].objectName : EMPTYMESSAGE;
        UpdateVisualObjects();
    }

    protected virtual void UpdateVisualObjects()
    {
        foreach (Transform obj in visualObjects) obj.gameObject.SetActive(false);
        for (int i = 0; i < objectsStored.Count; i++)
        {
            Transform obj = visualObjects.GetChild(i);
            obj.gameObject.SetActive(true);
        }
    }

    public void SelectNext()
    {
        currentIndex++;
        if (currentIndex >= objectsStored.Count) currentIndex = 0;

        UpdateVisibleState();
    }

    public void SelectPrevious()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = objectsStored.Count - 1;

        UpdateVisibleState();
    }

    public virtual void AddNewObjectToStorage(IServerDataObject newObj)
    {
        objectsStored.Add(newObj);
        UpdateVisibleState();
    }

    public virtual void EjectObject()
    {
        if (objectsStored.Count > 0)
        {
            objectsStored.RemoveAt(currentIndex);
            if (currentIndex >= objectsStored.Count) currentIndex = 0;

            UpdateVisibleState();
        }
    }
}
