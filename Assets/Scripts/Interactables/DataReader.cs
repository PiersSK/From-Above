using TMPro;
using UnityEngine;

public class DataReader : Interactable
{
    public DataDrive insertedDrive = null;
    [SerializeField] private TextMeshProUGUI insertedDriveName;
    [SerializeField] private GameObject driveObj;

    private const string NODISK = "NONE INSERTED";

    protected override void Interact(Transform player)
    {
        PlayerInventory inv = player.GetComponent<PlayerInventory>();

        if (insertedDrive == null && inv.dataDrivesHeld.Count > 0)
        {
            insertedDrive = inv.dataDrivesHeld[0];
            inv.dataDrivesHeld.RemoveAt(0);
            insertedDriveName.text = insertedDrive.DiskName;
            driveObj.SetActive(true);
        } else if (insertedDrive != null)
        {
            inv.dataDrivesHeld.Add(insertedDrive);
            insertedDrive = null;
            insertedDriveName.text = NODISK;
            driveObj.SetActive(false);

        }
    }
}
