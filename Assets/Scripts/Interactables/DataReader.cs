using NUnit;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataReader : Interactable
{
    public DataDrive insertedDrive = null;
    [SerializeField] private TextMeshProUGUI insertedDriveName;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject visiblePD;


    [SerializeField] private TextMeshProUGUI textOutput;
    [SerializeField] private MusicPlayer audioOutput;

    [SerializeField] private GameObject PDUI;
    [SerializeField] private Transform PDUIButtonContainer;

    private Transform player;
    private const string NODISK = "NONE INSERTED";

    public override bool CanInteract()
    {
        return PlayerInventory.Instance.dataDrivesHeld.Count > 0 || insertedDrive != null;
    }

    public override string GetPrompt()
    {
        string addPrefix = audioOutput != null ? "Place" : "Insert";
        return insertedDrive != null ? "Take " + insertedDrive.DiskName + " PD" : addPrefix + "PD";
    }

    protected override void Interact(Transform p)
    {
        player = p;
        PlayerInventory inv = PlayerInventory.Instance;

        if (insertedDrive != null)
        {
            UnloadDrive();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            player.GetComponent<PlayerMotor>().ToggleMovementOverride();
            player.GetComponent<PlayerLook>().ToggleLookLock();
            UIManager.Instance.ToggleCrosshairVisibility();

            foreach (Transform t in PDUIButtonContainer) Destroy(t.gameObject);

            foreach (DataDrive d in inv.dataDrivesHeld)
            {
                Button b = Instantiate(Resources.Load<Button>("PDButton"), PDUIButtonContainer);
                b.GetComponent<PDButton>().SetDrive(d, this);
            }

            PDUI.SetActive(true);
        }
    }

    public void UnlockPlayer()
    {
        Transform player = PlayerInventory.Instance.transform;

        PDUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<PlayerMotor>().ToggleMovementOverride();
        player.GetComponent<PlayerLook>().ToggleLookLock();
        UIManager.Instance.ToggleCrosshairVisibility();
    }

    public void DriveSelected(DataDrive drive)
    {
        UnlockPlayer();
        LoadDrive(drive);
    }

    public void UnloadDrive()
    {
        PlayerInventory.Instance.dataDrivesHeld.Add(insertedDrive);
        insertedDrive = null;
        insertedDriveName.text = NODISK;

        if (textOutput != null) textOutput.text = string.Empty;
        if (audioOutput != null) audioOutput.DiskRemoved();
        if (anim != null) anim.SetTrigger("Eject");
        if (visiblePD != null) visiblePD.SetActive(false);
    }

    private void LoadDrive(DataDrive drive)
    {
        insertedDrive = drive;
        PlayerInventory.Instance.dataDrivesHeld.Remove(drive);
        insertedDriveName.text = insertedDrive.DiskName;
        if (anim != null) anim.SetTrigger("Insert");
        if (visiblePD != null) visiblePD.SetActive(true);
    }
}
