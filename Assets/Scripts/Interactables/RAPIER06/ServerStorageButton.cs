using UnityEngine;

public class ServerStorageButton : Interactable
{
    public enum StorageInteraction
    {
        NextPD,
        PreviousPD,
        Eject
    }

    public enum StorageType
    {
        PentaDisc,
        ExeCard
    }

    [SerializeField] private StorageType linkedStorageType;
    [SerializeField] private StorageInteraction interactionType;
    [SerializeField] private IServerHubStorage storage;

    private const string NEXTPD = "Select Next ";
    private const string PREVPD = "Select Previous ";
    private const string EJECTPD = "Eject Current ";

    private void Start()
    {
        if (interactionType == StorageInteraction.NextPD) promptMessage = NEXTPD + linkedStorageType.ToString();
        else if (interactionType == StorageInteraction.PreviousPD) promptMessage = PREVPD + linkedStorageType.ToString();
        else if (interactionType == StorageInteraction.Eject) promptMessage = EJECTPD + linkedStorageType.ToString();
    }

    public override bool CanInteract()
    {
        return (interactionType == StorageInteraction.Eject && storage.objectsStored.Count > 0)
            || (interactionType != StorageInteraction.Eject && storage.objectsStored.Count > 1);
    }

    protected override void Interact(Transform player)
    {
        if (interactionType == StorageInteraction.NextPD) storage.SelectNext();
        else if (interactionType == StorageInteraction.PreviousPD) storage.SelectPrevious();
        else if (interactionType == StorageInteraction.Eject) storage.EjectObject();
    }
}
