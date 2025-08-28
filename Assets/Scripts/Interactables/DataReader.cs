using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataReader : Interactable
{
    public DataDrive insertedDrive = null;
    [SerializeField] private TextMeshProUGUI insertedDriveName;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject visiblePD;

    [SerializeField] private GameObject defaultScreen;
    [SerializeField] private GameObject outputScreen;
    [SerializeField] private TextMeshProUGUI textOutput;
    [SerializeField] private MusicPlayer audioOutput;

    [SerializeField] private AudioClip ejectSfx;
    [SerializeField] private AudioClip insertSfx;
    [SerializeField] private AudioClip audioDiscSfx;

    public bool canInsert = true;

    private const string NODISK = "NONE INSERTED";


    public override bool CanInteract()
    {
        return (PlayerInventory.Instance.dataDrivesHeld.Count > 0 && canInsert) || insertedDrive != null;
    }

    public override string GetPrompt()
    {
        string addPrefix = audioOutput != null ? "Place" : "Insert";
        return insertedDrive != null ? "Take " + insertedDrive.objectName + " PD" : addPrefix + "PD";
    }

    protected override void Interact(Transform p)
    {
        List<IServerDataObject> pds = audioOutput != null ? PlayerInventory.Instance.AudioDrivesHeld() : PlayerInventory.Instance.dataDrivesHeld;

        if (insertedDrive != null) UnloadDrive();
        else UIManager.Instance.ShowPDSelectUI(LoadDrive, pds);
    }

    public void UnloadDrive()
    {
        PlayerInventory.Instance.dataDrivesHeld.Add(insertedDrive);
        insertedDrive = null;
        if(insertedDriveName != null) insertedDriveName.text = NODISK;

        if (textOutput != null) textOutput.text = string.Empty;
        if (outputScreen != null)
        {
            defaultScreen.SetActive(true);
            outputScreen.SetActive(false);
        }
        if (audioOutput != null) audioOutput.DiskRemoved();
        if (anim != null) anim.SetTrigger("Eject");
        SoundManager.Instance.PlaySFXOneShot(audioOutput == null ? ejectSfx : audioDiscSfx, 0, 0.3f);
    }

    private void LoadDrive(IServerDataObject drive)
    {
        insertedDrive = (DataDrive)drive;
        PlayerInventory.Instance.dataDrivesHeld.Remove(drive);
        if (insertedDriveName != null) insertedDriveName.text = insertedDrive.objectName;
        if (anim != null) anim.SetTrigger("Insert");
        SoundManager.Instance.PlaySFXOneShot(audioOutput == null ? insertSfx : audioDiscSfx, 0, 0.3f);
        if (visiblePD != null) visiblePD.SetActive(true);
    }
}
