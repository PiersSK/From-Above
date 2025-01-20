using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RapierTerminal : Computer
{
    [SerializeField] private TextMeshProUGUI notifHeader;
    [SerializeField] private TextMeshProUGUI notif1;
    [SerializeField] private TextMeshProUGUI notif2;
    [SerializeField] private Button shipStatusBtn;
    [SerializeField] private TextMeshProUGUI shipStatusSubtitle;
    [SerializeField] private Button readDataBtn;
    [SerializeField] private TextMeshProUGUI btnResponse;

    [SerializeField] private DataReader dataReader;

    private bool shipDataUploaded = false;
    private const string shipDataUploadResponse = "UPLOAD COMPLETE\nCommand thanks you for your continued vigilance";
    private const string shipDataUploadReject = "NO UPDATED FOUND\nShip data was already updated today. Thank you for your continued vigilance";
    private const string shipDataSubtitleResponse = "NO ACTION REQUIRED";

    private const string readDataReject = "NO DATA DRIVE INSERTED";

    //Something about reading data

    private void Start()
    {
        shipStatusBtn.onClick.AddListener(UploadShipStatus);
        readDataBtn.onClick.AddListener(ReadData);
    }

    private void UploadShipStatus()
    {
        if (!shipDataUploaded)
        {
            btnResponse.text = shipDataUploadResponse;
            shipStatusBtn.interactable = false;
            shipStatusSubtitle.text = shipDataSubtitleResponse;
            notif2.fontStyle = FontStyles.Strikethrough;
            shipDataUploaded = true;
        } else
        {
            btnResponse.text = shipDataUploadReject;
        }
    }

    private void ReadData()
    {
        if (dataReader.insertedDrive != null)
            btnResponse.text = dataReader.insertedDrive.DiskTextContent;
        else
            btnResponse.text = readDataReject;
    }
}
