using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PDStorage : MonoBehaviour
{
    public List<DataDrive> pDsStored;
    [SerializeField] private Transform pDsObjects;
    public int currentPDIndex = 0;

    [Header("PD UI Elements")]
    [SerializeField] private TextMeshProUGUI selectedPDName;

    private const string NOPDMESSAGE = "<i>No PentaDiscs in Storage</i>";

    public delegate void OnPDStorageChange();
    public static event OnPDStorageChange PDStorageChanged;

    private void Start()
    {
        UpdatePDVisibleState();
    }

    private void UpdatePDVisibleState()
    {
        selectedPDName.text = pDsStored.Count > 0 ? pDsStored[currentPDIndex].DiskName : NOPDMESSAGE;
        UpdatePDRackModel();
    }

    private void UpdatePDRackModel()
    {
        foreach (Transform PD in pDsObjects) PD.gameObject.SetActive(false);
        for (int i = 0; i < pDsStored.Count; i++)
        {
            Transform pdObj = pDsObjects.GetChild(i);
            pdObj.gameObject.SetActive(true);
            pdObj.localPosition = new Vector3(
                pdObj.localPosition.x,
                i == currentPDIndex ? 0.1f : 0f,
                pdObj.localPosition.z
            );
        }
    }

    public void SelectNextPD()
    {
        currentPDIndex++;
        if(currentPDIndex >= pDsStored.Count) currentPDIndex = 0;

        UpdatePDVisibleState();
    }

    public void SelectPreviousPD()
    {
        currentPDIndex--;
        if(currentPDIndex < 0) currentPDIndex = pDsStored.Count - 1;

        UpdatePDVisibleState();
    }

    public void AddPD(DataDrive newPD)
    {
        pDsStored.Add(newPD);
        UpdatePDVisibleState();
        PlayerInventory.Instance.dataDrivesHeld.Remove(newPD);
        PDStorageChanged?.Invoke();
    }

    public void EjectPD()
    {
        if (pDsStored.Count > 0)
        {
            PlayerInventory.Instance.dataDrivesHeld.Add(pDsStored[currentPDIndex]);
            pDsStored.RemoveAt(currentPDIndex);
            if (currentPDIndex >= pDsStored.Count) currentPDIndex = 0;

            UpdatePDVisibleState();
            PDStorageChanged?.Invoke();
        }
    }
}
