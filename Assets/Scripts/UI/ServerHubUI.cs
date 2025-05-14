using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerHubUI : MonoBehaviour
{
    [Header("UI Containers")]
    [SerializeField] private Transform background;
    [SerializeField] private Transform foreground;

    [Header("PD UI Objects")]
    [SerializeField] private GameObject pdButtons;
    [SerializeField] private Transform pdSelector;

    [SerializeField] private PDStorage pdStorage;
    [SerializeField] private Transform pdSelectorRow;

    [Header("Data UI Objects")]
    [SerializeField] private Transform dataInspectorContainer;
    [SerializeField] private Transform dataRow;
    [SerializeField] private GameObject selectedPdContainer;
    [SerializeField] private ServerHubDataObjectUI selectedPd;

    [Header("FC UI Objects")]
    [SerializeField] private GameObject fcButtonContainer;
    [SerializeField] private Transform fcSelector;
    [SerializeField] private Button fcPrevButton;
    [SerializeField] private Button fcNextButton;
    [SerializeField] private Button fcConfirmButton;

    [SerializeField] private TextMeshProUGUI fcPreview;

    [SerializeField] private EXEStorage fcStorage;
    [SerializeField] private Transform fcSelectorRow;

    private List<IServerDataObject> applicableExes = new();

    private void Start()
    {
        PDStorage.PDStorageChanged += RefreshUIState;
        EXEStorage.EXEStorageChanged += RefreshUIState;
        fcNextButton.onClick.AddListener(() => fcStorage.SelectNextFiltered(applicableExes));
        fcPrevButton.onClick.AddListener(() => fcStorage.SelectPreviousFiltered(applicableExes));
        RefreshUIState();
    }

    private void RefreshUIState()
    {
        foreach (Transform obj in pdSelectorRow) obj.gameObject.SetActive(false);

        if (pdStorage.objectsStored.Count > 0) UpdateSelectorRow(pdSelectorRow, pdStorage.objectsStored, pdStorage.currentIndex);

        UpdateDataUI();
        UpdateFuncCardRow();

        UpdateFCPreview();
        UpdateFCConfirmButtonState();
    }

    private void UpdateFCConfirmButtonState()
    {
        ServerExe currentFc = (ServerExe)fcStorage.allExeReference[fcStorage.currentIndex];

        fcConfirmButton.interactable = fcStorage.objectsStored.Contains(currentFc);
    }

    private void UpdateFCPreview()
    {
        if (fcSelector.parent == foreground)
        {
            ServerExe currentFc = (ServerExe)fcStorage.allExeReference[fcStorage.currentIndex];
            fcPreview.text = fcStorage.objectsStored.Contains(currentFc) ? currentFc.terminalPreview : "???";
        } else
        {
            fcPreview.text = string.Empty;
        }
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

            bool isRelevant = ShouldHighlightData(drive.slots[i]);
            dataDisplayObject.SetHighlightState(isRelevant);
        }

        if (selectedPdContainer.activeSelf) {
            ServerExe currentFc = (ServerExe)fcStorage.allExeReference[fcStorage.currentIndex];
            selectedPd.SetHighlightState(currentFc.PDIsRelevantToFunction(drive) && fcStorage.objectsStored.Contains(currentFc));
        }
    }

    private bool ShouldHighlightData(DiscSlotContent data)
    {
        ServerExe currentFc = (ServerExe)fcStorage.allExeReference[fcStorage.currentIndex];

        if (fcSelector.parent != foreground) return false;
        else if (data == null) return false;
        else if (data.decipherType != DiscSlotContent.DecipherType.None && currentFc is not DecipherExe) return false;
        else if (!fcStorage.objectsStored.Contains(currentFc)) return false;

        return currentFc.DataIsRelevantToFunction(data);
    }

    private void UpdateFuncCardRow()
    {
        DataDrive drive = (DataDrive)pdStorage.objectsStored[pdStorage.currentIndex];
        applicableExes = new();

        foreach (ServerExe exe in fcStorage.allExeReference)
        {
            if (exe.RelevantDataOnDisc(drive).Count > 0 || exe.IsApplicablePD(drive)) applicableExes.Add(exe);
        }

        foreach (Transform fcIcon in fcSelectorRow) fcIcon.gameObject.SetActive(false);

        int selectedIndex = applicableExes.Contains(fcStorage.allExeReference[fcStorage.currentIndex])
            ? applicableExes.IndexOf(fcStorage.allExeReference[fcStorage.currentIndex])
            : 0;

        UpdateSelectorRow(fcSelectorRow, applicableExes, selectedIndex, fcStorage.objectsStored);
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
        pdSelector.SetParent(background);
        fcSelector.SetParent(foreground);

        pdButtons.SetActive(false);
        fcButtonContainer.SetActive(true);

        selectedPdContainer.SetActive(true);
        selectedPd.dataName.text = pdStorage.objectsStored[pdStorage.currentIndex].objectName;

        UpdateDataUI();
        UpdateFCPreview();
    }

    public void BackToPDSelection()
    {
        pdSelector.SetParent(foreground);
        fcSelector.SetParent(background);

        pdButtons.SetActive(true);
        fcButtonContainer.SetActive(false);
        selectedPdContainer.SetActive(false);

        UpdateDataUI();
        UpdateFCPreview();
    }

    public void ConfirmFCSelection()
    {
        fcSelector.SetParent(background);
        dataInspectorContainer.SetParent(background);
        fcButtonContainer.SetActive(false);
        selectedPdContainer.SetActive(false);

        UpdateDataUI();
        UpdateFCPreview();
    }
}
