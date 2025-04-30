using UnityEngine;

public class PickupableEXE : Interactable
{
    [SerializeField] private ServerExe exe;
    private const string PICKUPPROMPT = "Pickup ";

    private void Start()
    {
        if (exe != null)
        {
            promptMessage = PICKUPPROMPT + exe.objectName;
        }
    }
    protected override void Interact(Transform player)
    {
        player.GetComponent<PlayerInventory>().exesHeld.Add(exe);
        exe = null;
        isInteractable = false;
        gameObject.SetActive(false);
    }
}