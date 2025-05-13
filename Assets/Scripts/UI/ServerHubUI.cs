using System.Collections;
using TMPro;
using UnityEngine;

public class ServerHubUI : MonoBehaviour
{
    [SerializeField] private PDStorage pdStorage;
    [SerializeField] private Transform pdSelectorRow;
    [SerializeField] private Transform dataRow;

    private void Start()
    {
        PDStorage.PDStorageChanged += RefreshPDRowState;
        RefreshPDRowState();
    }

    private void RefreshPDRowState()
    {
        foreach (Transform obj in pdSelectorRow) obj.gameObject.SetActive(false);


        if (pdStorage.objectsStored.Count > 0)
        {
            Vector3 SelectedPostition = pdSelectorRow.GetComponent<RectTransform>().anchoredPosition;
            float containerWidth = pdSelectorRow.GetComponent<RectTransform>().sizeDelta.x;

            for (int i = 0; i < pdStorage.objectsStored.Count; i++)
            {
                ServerHubStorageObjectUI icon = pdSelectorRow.GetChild(i).GetComponent<ServerHubStorageObjectUI>();
                icon.SetStorageObject(pdStorage.objectsStored[i]);
                icon.gameObject.SetActive(true);
                icon.transform.localScale = i == pdStorage.currentIndex ? Vector3.one : new Vector3(0.8f, 0.8f, 0.8f);
                icon.GetComponentInChildren<TextMeshProUGUI>().color = i == pdStorage.currentIndex ? UIColors.white : UIColors.grey;
            }

            SelectedPostition.x = containerWidth / 2f - pdSelectorRow.GetChild(pdStorage.currentIndex).GetComponent<RectTransform>().anchoredPosition.x;
            StartCoroutine(SmoothRailSlideRotate(pdSelectorRow.GetComponent<RectTransform>(), SelectedPostition));
        }

        UpdateDataUI();
    }

    private void UpdateDataUI()
    {
        DataDrive drive = (DataDrive)pdStorage.objectsStored[pdStorage.currentIndex];
        for(int i = 0; i< dataRow.childCount; i++)
        {
            ServerHubDataObjectUI dataDisplayObject = dataRow.GetChild(i).GetComponent<ServerHubDataObjectUI>();
            dataDisplayObject.SetDataObject(drive.slots[i]);
        }
    }

    private IEnumerator SmoothRailSlideRotate(RectTransform obj, Vector2 targetPosition)
    {
        float duration = 0.1f; // time to rotate
        float elapsed = 0f;
        Vector2 initialPosition = obj.anchoredPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            obj.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, t);
            yield return null;
        }

        obj.anchoredPosition = targetPosition;
    }
}
