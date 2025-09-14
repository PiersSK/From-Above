using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BridgeTerminal : Computer
{
    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI clockTime;
   
    [SerializeField] private GameObject phase1Screen;
    [SerializeField] private GameObject phase2Screen;
    [SerializeField] private GameObject overrideImg;
    [SerializeField] private TextMeshProUGUI p2TaskCounter;
    [SerializeField] private TextMeshProUGUI p2Timer;
    [SerializeField] private List<GameObject> phase2StatusBlocks;

    public delegate void OnDataUploaded(DataDrive drive);
    public static event OnDataUploaded DataUploaded;

    override protected void Update()
    {
        clockTime.text = TimeController.Instance.GetClockTime().ToString(@"hh\:mm\:ss");

        if (phase1Screen.activeSelf && TaskManager.Instance.currentPhase is WeaponTaskPhase) ShowPhaseTwoScreen();
        if (phase2Screen.activeSelf) PhaseTwoStatusUpdate();
        if (TaskManager.Instance.pacifistEndingReached)
        {
            overrideImg.SetActive(true);
            ShowPhaseOneScreen();
        }
        base.Update();
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
}
