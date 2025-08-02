using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXEStorage : IServerHubStorage
{
    public static event OnServerStorageObjectChange EXEStorageChanged;
    public List<IServerDataObject> allExeReference = new();

    private const string NOTINSERTEDNAME = "???";

    protected override void UpdateVisualObjects()
    {
        foreach (Transform obj in visualObjects) obj.GetChild(1).gameObject.SetActive(false);
        for (int i = 0; i < allExeReference.Count; i++)
        {
            Transform obj = visualObjects.GetChild(i);
            if (objectsStored.Contains(allExeReference[i])) obj.GetChild(1).gameObject.SetActive(true);

            Quaternion targetRotation = Quaternion.Euler(0f, i == currentIndex ? 90f : 0f, 90f);
            StartCoroutine(SmoothCardRotate(obj, targetRotation));
        }
    }

    private IEnumerator SmoothCardRotate(Transform obj, Quaternion targetRotation)
    {
        float duration = 0.25f; // time to rotate
        float elapsed = 0f;
        Quaternion initialRotation = obj.localRotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            obj.localRotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            yield return null;
        }

        obj.localRotation = targetRotation; 
    }

    public override void AddNewObjectToStorage(IServerDataObject newObj)
    {
        base.AddNewObjectToStorage(newObj);
        PlayerInventory.Instance.exesHeld.Remove(newObj);
        EXEStorageChanged?.Invoke(newObj);
    }

    protected override void UpdateVisibleState()
    {
        screenDisplay.text = objectsStored.Contains(allExeReference[currentIndex]) ? allExeReference[currentIndex].objectName : NOTINSERTEDNAME;

        UpdateVisualObjects();
    }

    public override void SelectNext()
    {
        currentIndex++;
        if (currentIndex >= allExeReference.Count) currentIndex = 0;

        UpdateVisibleState();
        EXEStorageChanged?.Invoke(null);

    }

    public override void SelectPrevious()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = allExeReference.Count - 1;

        UpdateVisibleState();
        EXEStorageChanged?.Invoke(null);

    }

    public void SelectNextFiltered(List<IServerDataObject> filterList)
    {
        if (filterList.Contains(allExeReference[currentIndex]))
        {
            int indexInFilter = filterList.IndexOf(allExeReference[currentIndex]);
            indexInFilter++;
            if (indexInFilter >= filterList.Count) indexInFilter = 0;

            currentIndex = allExeReference.IndexOf(filterList[indexInFilter]);
        } else
        {
            currentIndex = allExeReference.IndexOf(filterList[0]); ;
        }

        Debug.Log("EXE Filtered (Next) selected: " + allExeReference[currentIndex].objectName);

        UpdateVisibleState();
        EXEStorageChanged?.Invoke(null);

    }

    public void SelectPreviousFiltered(List<IServerDataObject> filterList)
    {
        if (filterList.Contains(allExeReference[currentIndex]))
        {
            int indexInFilter = filterList.IndexOf(allExeReference[currentIndex]);
            indexInFilter--;
            if (indexInFilter < 0) indexInFilter = filterList.Count - 1;

            currentIndex = allExeReference.IndexOf(filterList[indexInFilter]);
        } else
        {
            currentIndex = 0;
        }

        Debug.Log("EXE Filtered (Prev) selected: " + allExeReference[currentIndex].objectName);

        UpdateVisibleState();
        EXEStorageChanged?.Invoke(null);

    }
}
