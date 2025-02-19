using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PDButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    private DataDrive drive;
    private DataReader reader;

    public void SetDrive(DataDrive d, DataReader reader)
    {
        drive = d;
        label.text = d.DiskName;
        GetComponent<Button>().onClick.AddListener(() => reader.DriveSelected(drive));
    }
}
