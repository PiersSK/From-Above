using NUnit;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ServerDiscStorage : Interactable
{
    [SerializeField] private DataDrive driveStored;
    [SerializeField] private Animator serverAnim;

    [SerializeField] private AudioClip ejectSfx;
    [SerializeField] private AudioClip insertSfx;

    public bool driveInDock = true;

    private string inDockPrompt;
    private string outDockPrompt;

    private void Start()
    {
        if (driveStored != null)
        {
            inDockPrompt = "Take " + driveStored.DiskName + " PD";
            outDockPrompt = "Return " + driveStored.DiskName + " PD";
        }
    }

    public override bool CanInteract()
    {
        List<DataDrive> playerDrives = PlayerInventory.Instance.dataDrivesHeld;
        return driveStored != null && (!playerDrives.Contains(driveStored) && driveInDock) || (playerDrives.Contains(driveStored) && !driveInDock);
    }

    public override string GetPrompt()
    {
        return driveStored != null ? (driveInDock ? inDockPrompt : outDockPrompt) : string.Empty;
    }

    protected override void Interact(Transform player)
    {
        PlayerInventory inv = player.GetComponent<PlayerInventory>();

        if (!inv.dataDrivesHeld.Contains(driveStored) && driveInDock)
        {
            inv.dataDrivesHeld.Add(driveStored);
            driveInDock = false;
            serverAnim.SetTrigger("Eject");
            SoundManager.Instance.PlaySFXOneShot(ejectSfx, 0, 0.3f);
        } else if (inv.dataDrivesHeld.Contains(driveStored) && !driveInDock)
        {
            inv.dataDrivesHeld.Remove(driveStored);
            driveInDock = true;
            serverAnim.SetTrigger("Insert");
            SoundManager.Instance.PlaySFXOneShot(insertSfx, 0, 0.3f);
        }
    }
}
