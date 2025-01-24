using TMPro;
using UnityEngine;

public class DataReader : Interactable
{
    public DataDrive insertedDrive = null;
    [SerializeField] private TextMeshProUGUI insertedDriveName;
    [SerializeField] private Animator anim;


    [SerializeField] private TextMeshProUGUI textOutput;
    [SerializeField] private MusicPlayer audioOutput;

    private const string NODISK = "NONE INSERTED";

    protected override void Interact(Transform player)
    {
        PlayerInventory inv = player.GetComponent<PlayerInventory>();

        if (insertedDrive == null && inv.dataDrivesHeld.Count > 0)
        {
            insertedDrive = inv.dataDrivesHeld[0];
            inv.dataDrivesHeld.RemoveAt(0);
            insertedDriveName.text = insertedDrive.DiskName;
            if(anim != null) anim.SetTrigger("Insert");

        } else if (insertedDrive != null)
        {
            inv.dataDrivesHeld.Add(insertedDrive);
            insertedDrive = null;
            insertedDriveName.text = NODISK;

            if(textOutput != null) textOutput.text = string.Empty;
            if (audioOutput != null) audioOutput.DiskRemoved();
            if (anim != null) anim.SetTrigger("Eject");

        }
    }
}
