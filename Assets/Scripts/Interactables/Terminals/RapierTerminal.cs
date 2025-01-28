using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RapierTerminal : Computer
{
    [SerializeField] private TextMeshProUGUI notifHeader;
    [SerializeField] private TextMeshProUGUI fleetNotif;
    [SerializeField] private TextMeshProUGUI shipNotif;

    [SerializeField] private Button shipStatusBtn;
    [SerializeField] private TextMeshProUGUI shipStatusHomeSubtitle;
    [SerializeField] private TextMeshProUGUI shipStatusDetailsSubtitle;

    [SerializeField] private Button readDataBtn;
    [SerializeField] private TextMeshProUGUI PDNameTitle;
    [SerializeField] private TextMeshProUGUI PDContent;

    [SerializeField] private TextMeshProUGUI mainScreenResponse;

    [SerializeField] private DataReader dataReader;

    [SerializeField] private Task shipStatusTask;

    public enum Notifications
    {
        RapierFleetStatus,
        ShipStatus
    }
    private bool shipDataUploaded = false;
    private bool fleetDataUploaded = false;

    private const string noNotifs = "0 Notifications";
    private const string oneNotifs = "1 Notification (Action Needed)";

    private const string shipDataUploadResponse = "DAILY SHIP STATUS UPLOAD COMPLETE\nCommand thanks you for your continued vigilance";
    private const string shipDataUploadReject = "Ship data was already updated today. Thank you for your continued vigilance";
    private const string shipDataSubtitleResponse = "DAILY UPLOAD COMPLETE";
    private const string shipDataSubtitleResponse2 = "LAST SENT: 00 DAYS AGO";

    private const string readDataReject = "NO DATA DRIVE INSERTED";

    //Something about reading data

    private void Start()
    {
        shipStatusBtn.onClick.AddListener(UploadShipStatus);
        readDataBtn.onClick.AddListener(ReadData);
    }
    
    public void ClearNotif(Notifications notif)
    {
        if(notif == Notifications.RapierFleetStatus)
        {
            fleetNotif.gameObject.SetActive(false);
            fleetDataUploaded = true;
        } else if (notif == Notifications.ShipStatus)
        {
            shipNotif.gameObject.SetActive(false);
            shipDataUploaded = true;
        }

        if (fleetDataUploaded && shipDataUploaded) notifHeader.text = noNotifs;
        else if (fleetDataUploaded || shipDataUploaded) notifHeader.text = oneNotifs;
    }

    private void UploadShipStatus()
    {
        if (!shipDataUploaded)
        {
            mainScreenResponse.text = shipDataUploadResponse; // give feedback on main screen

            shipStatusHomeSubtitle.text = shipDataSubtitleResponse; // update main screen state
            shipStatusDetailsSubtitle.text = shipDataSubtitleResponse2; // update details screen state

            ClearNotif(Notifications.ShipStatus);

            TaskManager.Instance.CompleteTask(shipStatusTask);
        } else
        {
            mainScreenResponse.text = shipDataUploadReject;
        }
    }

    private void ReadData()
    {
        if (dataReader.insertedDrive != null)
        {
            PDNameTitle.text = dataReader.insertedDrive.DiskName;
            PDContent.text = dataReader.insertedDrive.DiskTextContent;
        }
        else
        {
            PDNameTitle.text = readDataReject;
            PDContent.text = string.Empty;
        }
    }
}
