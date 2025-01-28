using NUnit.Framework;
using System;
using System.Collections.Generic;
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

    [SerializeField] private GameObject phase1Screen;
    [SerializeField] private GameObject phase2Screen;
    [SerializeField] private TextMeshProUGUI p2TaskCounter;
    [SerializeField] private TextMeshProUGUI p2Timer;
    [SerializeField] private List<GameObject> phase2StatusBlocks;

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

    override protected void Update()
    {
        if (phase1Screen.activeSelf && TaskManager.Instance.isPhaseTwo) ShowPhaseTwoScreen();
        if (phase2Screen.activeSelf) PhaseTwoStatusUpdate();
        base.Update();
    }

    private void TalkToCommand()
    {
        btnResponse.text = "DENIED. Command status set to ENGAGED. Try again later.";
    }

    public void ShowPhaseTwoScreen()
    {
        phase1Screen.SetActive(false);
        phase2Screen.SetActive(true);
    }

    public void PhaseTwoStatusUpdate()
    {
        p2TaskCounter.text = TaskManager.Instance.phaseTwoTasksCompleted + "/6 STEPS COMPLETED";
        TimeSpan time = TimeSpan.FromSeconds(5*60 - TimeController.Instance.GetTimeInSeconds());
        p2Timer.text = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");

        foreach (GameObject t in phase2StatusBlocks)
        {
            if (phase2StatusBlocks.IndexOf(t)  < TaskManager.Instance.phaseTwoTasksCompleted && !t.activeSelf) t.SetActive(true);
        }
    }

    private void SendData()
    {
        if (dataReader.insertedDrive != null)
        {
            if (dataReader.insertedDrive == fleetData)
            {
                if (!fleetDataUploaded)
                {
                    btnResponse.text = fleetDataUploadedMsg;
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
