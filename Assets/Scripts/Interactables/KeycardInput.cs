using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeycardInput : Interactable
{
    public enum KeyCardRequired
    {
        One,
        Two
    }

    [SerializeField] private KeyCardRequired key;
    [SerializeField] private Task task;
    [SerializeField] private GameObject keyObj;

    [SerializeField] private AudioClip unlockConfirmLine;
    [SerializeField] private AudioClip confirmBeep;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private List<KeycardTerminal> terminals;

    private bool keyInserted = false;

    private const string KEYCARD1INSERT = "Insert Keycard 1";
    private const string KEYCARD2INSERT = "Insert Keycard 2";
    private const string KEYCARD1NEEDED = "Requires Keycard 1";
    private const string KEYCARD2NEEDED = "Requires Keycard 2";

    public override bool CanInteract()
    {
        return TaskManager.Instance.currentPhase.tasks.Contains(task) && !keyInserted
            && ((key == KeyCardRequired.One && PlayerInventory.Instance.hasKeycard1)
               || (key == KeyCardRequired.Two && PlayerInventory.Instance.hasKeycard2));
    }

    public override string GetPrompt()
    {
        if (key == KeyCardRequired.One)
            return KEYCARD1INSERT;
        else if (key == KeyCardRequired.Two)
            return KEYCARD2INSERT;

        return string.Empty;
    }

    public override string GetRequirementMessage()
    {
        if (TaskManager.Instance.currentPhase.tasks.Contains(task) && !keyInserted)
        {
            if (key == KeyCardRequired.One)
                return KEYCARD1NEEDED;
            else if (key == KeyCardRequired.Two)
                return KEYCARD2NEEDED;
        }

        return string.Empty;
    }

    protected override void Interact(Transform player)
    {
        bool isFirstKey = DoomsdayStatusUI.Instance.keycardsInserted == 0;
        if (key == KeyCardRequired.One && PlayerInventory.Instance.hasKeycard1)
        {
            SoundManager.Instance.PlaySFXOneShot(sfx);
            keyObj.SetActive(true);
            PlayerInventory.Instance.hasKeycard1 = false;
            DoomsdayStatusUI.Instance.keycardsInserted++;
            keyInserted = true;
            if (isFirstKey)
                TimeController.Instance.StartKeycardTimer(OnTimerExpired);


        }
        if (key == KeyCardRequired.Two && PlayerInventory.Instance.hasKeycard2)
        {
            SoundManager.Instance.PlaySFXOneShot(sfx);
            keyObj.SetActive(true);
            PlayerInventory.Instance.hasKeycard2 = false;
            DoomsdayStatusUI.Instance.keycardsInserted++;
            keyInserted = true;
            if (isFirstKey)
                TimeController.Instance.StartKeycardTimer(OnTimerExpired);
        }

        if (DoomsdayStatusUI.Instance.keycardsInserted == 2)
        {
            TimeController.Instance.StopKeycardTimer();
            TaskManager.Instance.ProgressTask(task);
            SoundManager.Instance.PlaySFXOneShot(confirmBeep);
            SoundManager.Instance.PlayShipPALine(unlockConfirmLine);
            foreach (KeycardTerminal terminal in terminals)
                terminal.ShowUnlockMessage();
        }
    }

    private void OnTimerExpired()
    {
        if (keyInserted)
            keyObj.SetActive(false);
        keyInserted = false;

        if (key == KeyCardRequired.One)
            PlayerInventory.Instance.hasKeycard1 = true;
        else if (key == KeyCardRequired.Two)
            PlayerInventory.Instance.hasKeycard2 = true;

        DoomsdayStatusUI.Instance.keycardsInserted = 0;
        foreach (KeycardTerminal terminal in terminals)
            terminal.ShowFailureMessage();
    }
}
