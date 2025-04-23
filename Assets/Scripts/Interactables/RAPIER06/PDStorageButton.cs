using UnityEngine;

public class PDStorageButton : Interactable
{
    public enum StorageInteraction
    {
        NextPD,
        PreviousPD,
        Eject
    }

    [SerializeField] private StorageInteraction interactionType;
    [SerializeField] private PDStorage storage;

    [SerializeField] private Material interactableMaterial;
    [SerializeField] private Material nonInteractableMaterial;

    private const string NEXTPD = "Select Next PentaDisc";
    private const string PREVPD = "Select Previous PentaDisc";
    private const string EJECTPD = "Eject Current PentaDisc";

    private void Start()
    {
        if (interactionType == StorageInteraction.NextPD) promptMessage = NEXTPD;
        else if (interactionType == StorageInteraction.PreviousPD) promptMessage = PREVPD;
        else if (interactionType == StorageInteraction.Eject) promptMessage = EJECTPD;

        PDStorage.PDStorageChanged += OnPDStorageChanged;
    }

    private void OnPDStorageChanged()
    {
        GetComponent<Renderer>().material = CanInteract() ? interactableMaterial : nonInteractableMaterial;
        foreach(Renderer renderer in transform.GetComponentsInChildren<Renderer>())
            renderer.material = CanInteract() ? interactableMaterial : nonInteractableMaterial;
    }

    public override bool CanInteract()
    {
        return (interactionType == StorageInteraction.Eject && storage.pDsStored.Count > 0)
            || (interactionType != StorageInteraction.Eject && storage.pDsStored.Count > 1);
    }

    protected override void Interact(Transform player)
    {
        if (interactionType == StorageInteraction.NextPD) storage.SelectNextPD();
        else if (interactionType == StorageInteraction.PreviousPD) storage.SelectPreviousPD();
        else if (interactionType == StorageInteraction.Eject) storage.EjectPD();
    }
}
