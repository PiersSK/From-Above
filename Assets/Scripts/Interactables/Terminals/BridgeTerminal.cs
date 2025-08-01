using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BridgeTerminal : Computer
{
    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI Header;
    [SerializeField] private TextMeshProUGUI Subheader;
    [SerializeField] private Button commanderBtn;
    [SerializeField] private TextMeshProUGUI commanderSubtitle;
    [SerializeField] private Button sendDataBtn;
    [SerializeField] private TextMeshProUGUI btnResponse;
    [SerializeField] private GameObject phase1Screen;
    [SerializeField] private GameObject phase2Screen;
    [SerializeField] private GameObject overrideImg;
    [SerializeField] private TextMeshProUGUI p2TaskCounter;
    [SerializeField] private TextMeshProUGUI p2Timer;
    [SerializeField] private List<GameObject> phase2StatusBlocks;

    [Header("Terminal & Data Settings")]
    [SerializeField] private RapierTerminal rapierTerminal;
    [SerializeField] private DataReader dataReader;
    [SerializeField] private DataDrive fleetData;

    [Header("Progression Settings")]
    [SerializeField] private Task fleetDataTask;

    private bool fleetDataUploaded = false;
    private const string UPLOADSUCCESS = "[UPLOAD OF DATA COMPLETE]";
    private const string UPLOADREPEATEDMESSAGE = "DATA ALREADY UPLOADED. PLEASE RETURN DATA TO STORAGE";
    private const string UPLOADDATAREJECT = "NO REQUEST FOUND FOR INSERTED DATA DRIVE. PLEASE RETURN DATA TO STORAGE IMMEDIATELY";

    private const string READDATAREJECT = "NO DATA DRIVE INSERTED";
    private const string COMMSREJECTION = "DENIED. Command status set to ENGAGED. Try again later.";

    private void Start()
    {
        commanderBtn.onClick.AddListener(TalkToCommand);
        sendDataBtn.onClick.AddListener(SendData);
    }

    override protected void Update()
    {
        if (phase1Screen.activeSelf && TaskManager.Instance.currentPhase is WeaponTaskPhase) ShowPhaseTwoScreen();
        if (phase2Screen.activeSelf) PhaseTwoStatusUpdate();
        if (TaskManager.Instance.pacifistEndingReached)
        {
            overrideImg.SetActive(true);
            ShowPhaseOneScreen();
        }
        base.Update();
    }

    private void TalkToCommand()
    {
        btnResponse.text = COMMSREJECTION;
    }

    public void ShowPhaseTwoScreen()
    {
        phase1Screen.SetActive(false);
        phase2Screen.SetActive(true);
    }

    public void ShowPhaseOneScreen()
    {
        phase1Screen.SetActive(true);
        phase2Screen.SetActive(false);
    }

    public void PhaseTwoStatusUpdate()
    {
        if (TaskManager.Instance.currentPhase is not WeaponTaskPhase) return;

        var weaponTasksCompleted = TaskManager.Instance.currentPhase.completedTasks.Count;

        p2TaskCounter.text = weaponTasksCompleted + "/6 STEPS COMPLETED";
        TimeSpan time = TimeSpan.FromSeconds(TimeController.Instance.phase2TimeLimitMins* 60 - TimeController.Instance.GetTimeInSeconds(TimeController.Instance.time));
        p2Timer.text = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");

        foreach (GameObject t in phase2StatusBlocks)
        {
            if (phase2StatusBlocks.IndexOf(t)  < weaponTasksCompleted && !t.activeSelf) t.SetActive(true);
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
                    btnResponse.text = UPLOADSUCCESS;
                    fleetDataUploaded = true;
                    TaskManager.Instance.ProgressTask(fleetDataTask);
                    rapierTerminal.ClearNotif(RapierTerminal.Notifications.RapierFleetStatus);
                }
                else
                    btnResponse.text = UPLOADREPEATEDMESSAGE;
            } else
            {
                btnResponse.text = UPLOADDATAREJECT;
            }
        }
        else
            btnResponse.text = READDATAREJECT;
    }
}
