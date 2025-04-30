using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXEStorage : IServerHubStorage
{
    public static event OnServerStorageObjectChange EXEStorageChanged;

    protected override void UpdateVisualObjects()
    {
        foreach (Transform obj in visualObjects) obj.gameObject.SetActive(false);
        for (int i = 0; i < objectsStored.Count; i++)
        {
            Transform obj = visualObjects.GetChild(i);
            obj.gameObject.SetActive(true);

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
        EXEStorageChanged?.Invoke();
    }
}
