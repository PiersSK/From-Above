using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ServerHubUI : MonoBehaviour
{
    [Header("UI Containers")]
    [SerializeField] private Transform background;
    [SerializeField] private Transform foreground;

    [Header("PD UI Objects")]
    [SerializeField] private GameObject PDButtons;
    [SerializeField] private Transform PDSelector;

    [SerializeField] private PDStorage pdStorage;
    [SerializeField] private Transform pdSelectorRow;

    [Header("Data UI Objects")]
    [SerializeField] private Transform dataRow;

    [Header("FC UI Objects")]
    [SerializeField] private GameObject FCButtons;
    [SerializeField] private Transform FCSelector;

    [SerializeField] private EXEStorage fcStorage;
    [SerializeField] private Transform fcSelectorRow;

    private void Start()
    {
        PDStorage.PDStorageChanged += RefreshUIState;
        RefreshUIState();
    }

    private void RefreshUIState()
    {
        foreach (Transform obj in pdSelectorRow) obj.gameObject.SetActive(false);

        if (pdStorage.objectsStored.Count > 0) UpdateSelectorRow(pdSelectorRow, pdStorage.objectsStored, pdStorage.currentIndex);

        UpdateDataUI();
        UpdateFuncCardRow();
    }

    private void UpdateSelectorRow(Transform row, List<IServerDataObject> data, int indexSelected, List<IServerDataObject> accessibleList = null)
    {
        Vector3 SelectedPostition = row.GetComponent<RectTransform>().anchoredPosition;
        float containerWidth = row.GetComponent<RectTransform>().sizeDelta.x;

        for (int i = 0; i < data.Count; i++)
        {
            ServerHubStorageObjectUI icon = row.GetChild(i).GetComponent<ServerHubStorageObjectUI>();
            icon.SetStorageObject(data[i], accessibleList);
            icon.gameObject.SetActive(true);
            icon.transform.localScale = i == indexSelected ? Vector3.one : new Vector3(0.8f, 0.8f, 0.8f);
            icon.GetComponentInChildren<TextMeshProUGUI>().color = i == indexSelected ? UIColors.white : UIColors.grey;
        }

        SelectedPostition.x = containerWidth / 2f - row.GetChild(indexSelected).GetComponent<RectTransform>().anchoredPosition.x;
        StartCoroutine(SmoothRailSlideRotate(row.GetComponent<RectTransform>(), SelectedPostition));
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

    private void UpdateFuncCardRow()
    {
        DataDrive drive = (DataDrive)pdStorage.objectsStored[pdStorage.currentIndex];
        List<IServerDataObject> applicableExes = new();

        foreach (ServerExe exe in fcStorage.allExeReference)
        {
            if (exe.RelevantDataOnDisc(drive).Count > 0 || exe.IsApplicablePD(drive)) applicableExes.Add(exe);
        }

        foreach (Transform fcIcon in fcSelectorRow) fcIcon.gameObject.SetActive(false);
        UpdateSelectorRow(fcSelectorRow, applicableExes, 0, fcStorage.objectsStored);
    }

    private IEnumerator SmoothRailSlideRotate(RectTransform obj, Vector2 targetPosition)
    {
        float duration = 0.1f;
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

    public void ConfirmPDSelection()
    {
        PDSelector.SetParent(background);
        FCSelector.SetParent(foreground);

        PDButtons.SetActive(false);
        FCButtons.SetActive(true);


    }
}
