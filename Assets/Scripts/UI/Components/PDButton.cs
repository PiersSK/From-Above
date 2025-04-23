using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PDButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    private DataDrive drive;

    public void SetDrive(DataDrive d, Action<DataDrive> onClick)
    {
        drive = d;
        label.text = d.DiskName;
        GetComponent<Button>().onClick.AddListener(() => onClick(d));
    }
}
