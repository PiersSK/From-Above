using Unity.VisualScripting;
using UnityEngine;

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

    private bool keyInserted = false;

    public override bool CanInteract()
    {
        return TaskManager.Instance.tasks.Contains(task) && !keyInserted;
    }

    public override string GetPrompt()
    {
        if(key == KeyCardRequired.One)
            return !PlayerInventory.Instance.hasKeycard1 ? "REQUIRES KEYCARD 1" : "INSERT KEYCARD 1";
        else if(key == KeyCardRequired.Two)
            return !PlayerInventory.Instance.hasKeycard2 ? "REQUIRES KEYCARD 2" : "INSERT KEYCARD 2";

        return string.Empty;
    }

    protected override void Interact(Transform player)
    {
        if (key == KeyCardRequired.One && PlayerInventory.Instance.hasKeycard1)
        {
            SoundManager.Instance.PlaySFXOneShot(sfx);
            keyObj.SetActive(true);
            PlayerInventory.Instance.hasKeycard1 = false;
            DoomsdayStatusUI.Instance.keycardsInserted++;
            keyInserted = true;
        }
        if (key == KeyCardRequired.Two && PlayerInventory.Instance.hasKeycard2)
        {
            SoundManager.Instance.PlaySFXOneShot(sfx);
            keyObj.SetActive(true);
            PlayerInventory.Instance.hasKeycard2 = false;
            DoomsdayStatusUI.Instance.keycardsInserted++;
            keyInserted = true;
        }

        if(DoomsdayStatusUI.Instance.keycardsInserted == 2)
        {
            TaskManager.Instance.CompleteTask(task);
            SoundManager.Instance.PlaySFXOneShot(confirmBeep);
            SoundManager.Instance.PlayShipPALine(unlockConfirmLine);
        }
    }
}
