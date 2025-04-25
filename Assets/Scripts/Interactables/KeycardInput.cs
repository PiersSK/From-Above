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
        if(key == KeyCardRequired.One)
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
