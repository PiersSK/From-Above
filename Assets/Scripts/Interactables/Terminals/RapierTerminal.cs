using NUnit.Framework;
using System.Collections.Generic;
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

    [SerializeField] private GameObject overrideBtn;
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject overrideScreen;
    [SerializeField] private ServerDiscStorage overrideServer;

    [SerializeField] private DataReader dataReader;

    [SerializeField] private Task shipStatusTask;

    [SerializeField] private TMP_InputField adminPasswordInput;
    [SerializeField] private GameObject adminUnlocked;
    [SerializeField] private List<ServerRack> adminUnlockedServers;
    [SerializeField] private string adminPassword;


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

    override protected void Update()
    {
        if (input != null && playerAtComputer && input.playerActions.Submit.triggered)
        {
            string pass = adminPasswordInput.text;
            if(pass == adminPassword)
            {
                adminPasswordInput.gameObject.SetActive(false);
                adminUnlocked.SetActive(true);
                foreach (ServerRack r in adminUnlockedServers) r.hasDisk = true;
            } else
            {
                adminPasswordInput.text = string.Empty;
            }
        }

        if (overrideServer.driveInDock && !overrideBtn.activeSelf) overrideBtn.SetActive(true);
        else if (!overrideServer.driveInDock && overrideBtn.activeSelf)
        {
            overrideBtn.SetActive(false);
            if(overrideScreen.activeSelf)
            {
                overrideScreen.SetActive(false);
                mainScreen.SetActive(true);
            }
        }

        base.Update();
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
