using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PDButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;

    public void SetDrive(IServerDataObject d, Action<IServerDataObject> onClick)
    {
        label.text = d.objectName;
        GetComponent<Button>().onClick.AddListener(() => onClick(d));
    }
}
