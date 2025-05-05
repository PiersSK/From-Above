using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RapierTerminal : Computer
{
    [Header("Homescreen Object References")]
    [SerializeField] private TextMeshProUGUI notifHeader;
    [SerializeField] private TextMeshProUGUI fleetNotif;
    [SerializeField] private TextMeshProUGUI shipNotif;

    [SerializeField] private Button shipStatusBtn;
    [SerializeField] private TextMeshProUGUI shipStatusHomeSubtitle;
    [SerializeField] private TextMeshProUGUI shipStatusDetailsSubtitle;

    [SerializeField] private Button readDataBtn;
    [SerializeField] private TextMeshProUGUI PDNameTitle;
    [SerializeField] private TextMeshProUGUI PDContent;

    [SerializeField] private Button adminBtn;
    [SerializeField] private Button overrideBtn;

    [SerializeField] private TextMeshProUGUI mainScreenResponse;

    [SerializeField] private GameObject mainScreen;

    [Header("Ship Status Screen Settings")]
    [SerializeField] private Button uploadStatusBtn;
    [SerializeField] private GameObject statusScreen;

    [Header("PD Screen Settings")]
    [SerializeField] private Button pdReturnBtn;
    [SerializeField] private GameObject pdScreen;

    [Header("Override Screen References")]
    [SerializeField] private Button overrideReturnBtn;
    [SerializeField] private GameObject overrideScreen;
    [SerializeField] private ServerDiscStorage overrideServer;

    [Header("Terminal & Data Settings")]
    [SerializeField] private DataReader dataReader;

    [Header("Progression Settings")]
    [SerializeField] private Task shipStatusTask;

    [Header("Admin Access Settings")]
    [SerializeField] private Button adminReturnBtn;
    [SerializeField] private TMP_InputField adminPasswordInput;
    [SerializeField] private GameObject adminUnlocked;
    [SerializeField] private GameObject adminPasswordHeader;
    [SerializeField] private List<ServerRack> adminUnlockedServers;
    [SerializeField] private string adminPassword;
    [SerializeField] private GameObject adminScreen;

    public enum Notifications
    {
        RapierFleetStatus,
        ShipStatus
    }
    private bool shipDataUploaded = false;
    private bool fleetDataUploaded = false;

    private const string ZERONOTIFICATIONS = "0 Notifications";
    private const string ONENOTIFICATION = "1 Notification (Action Needed)";

    private const string UPLOADSUCCESS = "DAILY SHIP STATUS UPLOAD COMPLETE\nCommand thanks you for your continued vigilance";
    private const string UPLOADREJECT = "Ship data was already updated today. Thank you for your continued vigilance";
    private const string SUBTITLEPOSTUPLOAD = "DAILY UPLOAD COMPLETE";
    private const string SUBTITLEPOSTUPLOADTIMER = "LAST SENT: 00 DAYS AGO";

    private const string READDATAREJECT = "NO DATA DRIVE INSERTED";

    private const string BACKTOMAINSCREEN = "Return To Home Screen";

    private void Start()
    {
        uploadStatusBtn.onClick.AddListener(UploadShipStatus);
        readDataBtn.onClick.AddListener(ReadData);
    }

    override protected void Update()
    {
        if (playerAtComputer && InputManager.Instance.playerActions.Submit.triggered)
        {
            string pass = adminPasswordInput.text;
            if(pass == adminPassword)
            {
                adminPasswordInput.gameObject.SetActive(false);
                adminPasswordHeader.gameObject.SetActive(false);
                adminUnlocked.SetActive(true);
                foreach (ServerRack r in adminUnlockedServers) r.hasDisk = true;
            } else
            {
                adminPasswordInput.text = string.Empty;
            }
        }

        //TODO: Disabled until Server Terminal Rework
        //if (overrideServer.driveInDock && !overrideBtn.gameObject.activeSelf)
        //{
        //    overrideBtn.gameObject.SetActive(true);
        //    AdjustButtonNavigation();
        //}

        //else if (!overrideServer.driveInDock && overrideBtn.gameObject.activeSelf)
        //{
        //    overrideBtn.gameObject.SetActive(false);
        //    if (overrideScreen.activeSelf)
        //    {
        //        overrideScreen.SetActive(false);
        //        mainScreen.SetActive(true);
        //    }
        //    AdjustButtonNavigation();
        //}

        if (playerAtComputer && InputManager.Instance.GamepadIsCurrentInput())
            UIManager.Instance.ShowBackoutText(mainScreen.activeSelf ? EXITTERMINAL : BACKTOMAINSCREEN);

        if (!isInteractable && InputManager.Instance.playerActions.Escape.triggered)
        {
            if (InputManager.Instance.GamepadIsCurrentInput())
            {
                if (mainScreen.activeSelf) ReleasePlayer();
                else ReturnToMainScreen();
            } else
            {
                ReleasePlayer();
            }
        }
    }

    private void ReturnToMainScreen()
    {
        if (statusScreen.activeSelf) statusScreen.SetActive(false);
        else if (pdScreen.activeSelf) pdScreen.SetActive(false);
        else if (adminScreen.activeSelf) adminScreen.SetActive(false);
        else if (overrideScreen.activeSelf) overrideScreen.SetActive(false);

        mainScreen.SetActive(true);
        SelectAppropriateButton();
    }

    private void AdjustButtonNavigation()
    {
        bool overridePresent = overrideBtn.gameObject.activeSelf;

        if (overridePresent)
        {
            shipStatusBtn.navigation = UIManager.Instance.CreateNewNavigation(null, overrideBtn, null, readDataBtn);
            readDataBtn.navigation = UIManager.Instance.CreateNewNavigation(null, overrideBtn, shipStatusBtn, null);
            adminBtn.navigation = UIManager.Instance.CreateNewNavigation(overrideBtn, null, null, null);
        } else
        {
            shipStatusBtn.navigation = UIManager.Instance.CreateNewNavigation(null, adminBtn, null, readDataBtn);
            readDataBtn.navigation = UIManager.Instance.CreateNewNavigation(null, adminBtn, shipStatusBtn, null);
            adminBtn.navigation = UIManager.Instance.CreateNewNavigation(shipStatusBtn, null, shipStatusBtn, readDataBtn);
        }
    }

    protected override void SwitchToMouseKeyboard()
    {
        base.SwitchToMouseKeyboard();
        UIManager.Instance.ShowBackoutText(EXITTERMINAL);
    }

    protected override void SwitchToGamepad()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SelectAppropriateButton();
        UIManager.Instance.ShowBackoutText(mainScreen.activeSelf ? EXITTERMINAL : BACKTOMAINSCREEN);
    }

    private void SelectAppropriateButton()
    {
        if (InputManager.Instance.GamepadIsCurrentInput())
        {
            if (mainScreen.activeSelf) shipStatusBtn.Select();
            else if (statusScreen.activeSelf) uploadStatusBtn.Select();
            else if (pdScreen.activeSelf) pdReturnBtn.Select();
            else if (adminScreen.activeSelf) adminReturnBtn.Select();
            else if (overrideScreen.activeSelf) overrideReturnBtn.Select();
        }
    }


    protected override void Interact(Transform player)
    {
        base.Interact(player);
        SelectAppropriateButton();
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

        if (fleetDataUploaded && shipDataUploaded) notifHeader.text = ZERONOTIFICATIONS;
        else if (fleetDataUploaded || shipDataUploaded) notifHeader.text = ONENOTIFICATION;
    }

    private void UploadShipStatus()
    {
        if (!shipDataUploaded)
        {
            mainScreenResponse.text = UPLOADSUCCESS; // give feedback on main screen

            shipStatusHomeSubtitle.text = SUBTITLEPOSTUPLOAD; // update main screen state
            shipStatusDetailsSubtitle.text = SUBTITLEPOSTUPLOADTIMER; // update details screen state

            ClearNotif(Notifications.ShipStatus);

            TaskManager.Instance.CompleteTask(shipStatusTask);
        } else
        {
            mainScreenResponse.text = UPLOADREJECT;
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
            PDNameTitle.text = READDATAREJECT;
            PDContent.text = string.Empty;
        }
    }
}
