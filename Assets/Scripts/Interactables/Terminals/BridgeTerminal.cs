using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BridgeTerminal : Computer
{
    [SerializeField] private TextMeshProUGUI Header;
    [SerializeField] private TextMeshProUGUI Subheader;
    [SerializeField] private Button commanderBtn;
    [SerializeField] private TextMeshProUGUI commanderSubtitle;
    [SerializeField] private Button sendDataBtn;
    [SerializeField] private TextMeshProUGUI btnResponse;

    [SerializeField] private RapierTerminal rapierTerminal;
    [SerializeField] private DataReader dataReader;
    [SerializeField] private DataDrive fleetData;

    [SerializeField] private Task fleetDataTask;

    private bool fleetDataUploaded = false;
    private const string fleetDataUploadedMsg = "[UPLOAD OF DATA COMPLETE]";
    private const string fleetDataUploadedPreviouslyMsg = "DATA ALREADY UPLOADED. PLEASE RETURN DATA TO STORAGE";
    private const string dataUploadRejection = "NO REQUEST FOUND FOR INSERTED DATA DRIVE. PLEASE RETURN DATA TO STORAGE IMMEDIATELY";

    private const string readDataReject = "NO DATA DRIVE INSERTED";

    //Something about reading data

    private void Start()
    {
        commanderBtn.onClick.AddListener(TalkToCommand);
        sendDataBtn.onClick.AddListener(SendData);
    }

    private void TalkToCommand()
    {
        btnResponse.text = "Command status set to ENGAGED. Try again later.";
    }

    private void SendData()
    {
        if (dataReader.insertedDrive != null)
        {
            if (dataReader.insertedDrive == fleetData)
            {
                if (!fleetDataUploaded)
                {
                    btnResponse.text = fleetDataUploadedMsg + "\n" + dataReader.insertedDrive.DiskTextContent;
                    fleetDataUploaded = true;
                    TaskManager.Instance.CompleteTask(fleetDataTask);
                    rapierTerminal.ClearNotif(RapierTerminal.Notifications.RapierFleetStatus);
                }
                else
                    btnResponse.text = fleetDataUploadedPreviouslyMsg;
            } else
            {
                btnResponse.text = dataUploadRejection;
            }
        }
        else
            btnResponse.text = readDataReject;
    }
}
