using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ServerHubUI : MonoBehaviour
{
    public static ServerHubUI Instance { get; private set; }

    [Header("UI Containers")]
    [SerializeField] private Transform background;
    [SerializeField] private Transform foreground;

    [Header("PD UI Objects")]
    [SerializeField] private GameObject pdButtons;
    [SerializeField] private Button pdPrevButton;
    [SerializeField] private Button pdNextButton;
    [SerializeField] private Button pdConfirmButton;
    [SerializeField] private Transform pdSelector;
    [SerializeField] private GameObject pdEmptyMessage;
    [SerializeField] public PDStorage pdStorage;
    [SerializeField] private Transform pdSelectorRow;

    [Header("Data UI Objects")]
    [SerializeField] private Transform dataInspectorContainer;
    [SerializeField] private Transform dataRow;
    [SerializeField] private GameObject selectedPdContainer;
    [SerializeField] private GameObject inspectorEmptyMessage;
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

    [SerializeField] private Transform functionWindows;
    [SerializeField] private GameObject outputWindow;

    private List<IServerDataObject> applicableExes = new();
    public FuncCardUI currentFcUi = null;
    public bool longerAnimationPlaying = false;

    private const string ENCRYPTEDICON = "DataIcons/EncryptedIcon";
    protected const string EXITTERMINAL = "Exit Terminal";
    private const string BACKTOPD = "Return to PD Select";
    private const string BACKTOFC = "Retrun to FC Select";

    private void Awake()
    {
        if(Instance!=null) Debug.Log("SERVERHUB: Instance = " + Instance.name + " | IsDestroyed(): " + Instance.IsDestroyed());

        if (Instance != null && Instance != this && !Instance.IsDestroyed())
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        PDStorage.PDStorageChanged += PDStateChange;
        EXEStorage.EXEStorageChanged += RefreshUIState;
        fcNextButton.onClick.AddListener(() => fcStorage.SelectNextFiltered(applicableExes));
        fcPrevButton.onClick.AddListener(() => fcStorage.SelectPreviousFiltered(applicableExes));
        RefreshUIState();
    }
    public void SelectRelevantStartButton()
    {
        if (pdSelector.parent == foreground) pdPrevButton.Select();
        else if (fcSelector.parent == foreground) fcPrevButton.Select();
        else if (currentFcUi != null && !outputWindow.activeSelf) currentFcUi.confirmButton.Select();
        else outputWindow.GetComponent<FunctionOutputUI>().dismissButton.Select();
    }

    public string GetGamepadBackoutPrompt()
    {
        if (pdSelector.parent == foreground) return EXITTERMINAL;
        else if (fcSelector.parent == foreground) return BACKTOPD;
        else if (currentFcUi != null && !outputWindow.activeSelf) return BACKTOFC;
        else return BACKTOPD;
    }

    private void PDStateChange()
    {
        if (pdStorage.objectsStored.Count == 0) BackToPDSelection();
        RefreshUIState();
    }

    private void SetNoPDState()
    {
        bool pdsInStorage = pdStorage.objectsStored.Count > 0;

        pdPrevButton.interactable = pdsInStorage;
        pdNextButton.interactable = pdsInStorage;
        pdConfirmButton.interactable = pdsInStorage;

        pdEmptyMessage.SetActive(!pdsInStorage);
        inspectorEmptyMessage.SetActive(!pdsInStorage);
        dataRow.gameObject.SetActive(pdsInStorage);
        pdSelectorRow.gameObject.SetActive(pdsInStorage);
    }

    private void RefreshUIState()
    {
        foreach (Transform obj in pdSelectorRow) obj.gameObject.SetActive(false);

        if (pdStorage.objectsStored.Count > 0)
        {
            UpdateSelectorRow(pdSelectorRow, pdStorage.objectsStored, pdStorage.currentIndex);
            UpdateDataUI();
            UpdateFuncCardRow();
            UpdateFCPreview();
            UpdateFCConfirmButtonState();
        }

        SetNoPDState();

        if (InputManager.Instance.GamepadIsCurrentInput()) UIManager.Instance.ShowBackoutText(GetGamepadBackoutPrompt());
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
        Vector3 selectedPosition = row.GetComponent<RectTransform>().anchoredPosition;
        float containerWidth = row.GetComponent<RectTransform>().sizeDelta.x;
        float iconWidth = row.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
        float iconSpacing = row.GetComponent<HorizontalLayoutGroup>().spacing;

        for (int i = 0; i < data.Count; i++)
        {
            ServerHubStorageObjectUI icon = row.GetChild(i).GetComponent<ServerHubStorageObjectUI>();
            icon.SetStorageObject(data[i], accessibleList);
            icon.gameObject.SetActive(true);
            icon.transform.localScale = i == indexSelected ? Vector3.one : new Vector3(0.8f, 0.8f, 0.8f);
            icon.GetComponentInChildren<TextMeshProUGUI>().color = i == indexSelected ? UIColors.white : UIColors.grey;
        }

        float spacingFactor = (data.Count + 1) * 0.5f - indexSelected - 1f;
        selectedPosition.x = spacingFactor * (iconWidth + iconSpacing);

        //selectedPosition.x = containerWidth / 2f - row.GetChild(indexSelected).GetComponent<RectTransform>().anchoredPosition.x;
        StartCoroutine(SmoothRailSlideRotate(row.GetComponent<RectTransform>(), selectedPosition));
    }

    private void UpdateDataUI()
    {
        if (pdStorage.objectsStored.Count == 0) return;

        DataDrive drive = (DataDrive)pdStorage.objectsStored[pdStorage.currentIndex];
        List<DiscSlotContent> slots = pdStorage.GetPDSlots(drive);

        for(int i = 0; i< dataRow.childCount; i++)
        {
            ServerHubDataObjectUI dataDisplayObject = dataRow.GetChild(i).GetComponent<ServerHubDataObjectUI>();
            dataDisplayObject.SetDataObject(slots[i]);

            bool isRelevant = ShouldHighlightData(slots[i]);
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
        else if (!IsContentDecrypted(data) && currentFc is not DecipherExe && currentFc is not TransmitExe) return false;
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

        int selectedIndex = 0;

        if(applicableExes.Contains(fcStorage.allExeReference[fcStorage.currentIndex]))
        {
            selectedIndex = applicableExes.IndexOf(fcStorage.allExeReference[fcStorage.currentIndex]);
        } else
        {
            fcStorage.currentIndex = fcStorage.allExeReference.IndexOf(applicableExes[0]);
        }

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

    public void SetServerHubAnimationLock(bool isLocked)
    {
        longerAnimationPlaying = isLocked;
        if (longerAnimationPlaying) UIManager.Instance.HideBackoutText();
        else UIManager.Instance.ShowBackoutText(InputManager.Instance.GamepadIsCurrentInput() ? GetGamepadBackoutPrompt() : EXITTERMINAL);
    }

    public bool GamepadReturnPressed()
    {
        if (longerAnimationPlaying) return false;

        if (fcSelector.parent == foreground)
        {
            BackToPDSelection();
            return true;
        }
        else if (currentFcUi != null && !outputWindow.activeSelf)
        {
            ConfirmPDSelection();
            return true;
        }
        else if (outputWindow.activeSelf)
        {
            BackToPDSelection();
            return true;
        }

        return false;
    }

    public void ConfirmPDSelection()
    {
        pdSelector.SetParent(background);
        dataInspectorContainer.SetParent(foreground);
        fcSelector.SetParent(foreground);
        foreach (Transform functionWindow in functionWindows) functionWindow.gameObject.SetActive(false);
        outputWindow.SetActive(false);
        currentFcUi = null;

        pdButtons.SetActive(false);
        fcButtonContainer.SetActive(true);

        selectedPdContainer.SetActive(true);
        selectedPd.dataName.text = pdStorage.objectsStored[pdStorage.currentIndex].objectName;

        fcPrevButton.Select();

        RefreshUIState();
    }

    public void BackToPDSelection()
    {
        pdSelector.SetParent(foreground);
        dataInspectorContainer.SetParent(foreground);
        fcSelector.SetParent(background);
        foreach(Transform functionWindow in functionWindows) functionWindow.gameObject.SetActive(false);
        outputWindow.SetActive(false);
        currentFcUi = null;

        pdButtons.SetActive(true);
        fcButtonContainer.SetActive(false);
        selectedPdContainer.SetActive(false);

        pdPrevButton.Select();

        RefreshUIState();
    }

    public void ConfirmFCSelection()
    {
        fcSelector.SetParent(background);
        dataInspectorContainer.SetParent(background);
        fcButtonContainer.SetActive(false);
        selectedPdContainer.SetActive(false);
        outputWindow.SetActive(false);

        DataDrive pd = (DataDrive)pdStorage.objectsStored[pdStorage.currentIndex];
        ServerExe fc = (ServerExe)fcStorage.allExeReference[fcStorage.currentIndex];
        List<DiscSlotContent> relevantContent = fc.RelevantDataOnDisc(pd);

        currentFcUi = functionWindows.Find(fc.name).GetComponent<FuncCardUI>();
        currentFcUi.OpenFuncUI(relevantContent, pd);
        currentFcUi.confirmButton.Select();

        RefreshUIState();
    }

    public bool IsContentDecrypted(DiscSlotContent content)
    {
        return content.decipherType == DiscSlotContent.DecipherType.None || pdStorage.encryptedContent.Contains(content);
    }

    public string GetFormattedDataSlotName(DiscSlotContent content)
    {
        return !IsContentDecrypted(content) ? TextEncryption.EncryptToBase64(content.displayName, content.decipherType) : content.displayName;
    }

    public string GetFormattedDataSlotType(DiscSlotContent content)
    {
        return !IsContentDecrypted(content) ? TextEncryption.EncryptToBase64(content.GetDisplayType(), content.decipherType) : content.GetDisplayType();
    }

    public Sprite GetFormattedDataSlotIcon(DiscSlotContent content)
    {
        return !IsContentDecrypted(content) ? Resources.Load<Sprite>(ENCRYPTEDICON) : content.GetIcon();
    }

    public void LogContentAsDecrypted(DiscSlotContent content)
    {
        pdStorage.encryptedContent.Add(content);
    }
}
