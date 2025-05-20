using UnityEngine;

public class Door : Interactable
{
    public enum DoorType
    {
        Door,
        Vent
    }

    [Header("Door Initial State")]
    public bool isLocked;
    public bool isOpen = true;
    [SerializeField] private DoorType type;

    [Header("Sound & Animations")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AudioClip doorSfx;

    [Header("Materials")]
    [SerializeField] private Material uninteractableLight;
    [SerializeField] private Material interactableLight;

    [Header("Interaction Settings")]
    [SerializeField] private bool hasButton = true;
    [SerializeField] private Door twinButton;
    [SerializeField] private float interactionCooldown = 0.5f;

    private const string ANIMISOPEN = "IsOpen";
    private const string LOCKEDMESSAGE = "Locked";


    private void Start()
    {
        TakeOffCooldown();
        doorAnimator.SetBool(ANIMISOPEN, isOpen);
        if(hasButton) GetComponent<Renderer>().material = isLocked ? uninteractableLight : interactableLight;
    }

    public override bool CanInteract()
    {
        return isInteractable && !isLocked;
    }

    protected override void Interact(Transform player)
    {
        isOpen = !isOpen;
        if (twinButton != null && hasButton) twinButton.SyncToTwinButton();
        doorAnimator.SetBool(ANIMISOPEN, isOpen);
        SoundManager.Instance.PlaySFXOneShot(doorSfx, 0, 0.5f);

        isInteractable = false;
        requirementsNotMetMessage = string.Empty;
        Invoke("TakeOffCooldown", interactionCooldown);
    }

    private void TakeOffCooldown()
    {
        isInteractable = true;
        requirementsNotMetMessage = LOCKEDMESSAGE + " " + type.ToString();
        UpdatePromptMessage();

    }

    public void SyncToTwinButton()
    {
        isOpen = twinButton.isOpen;
        if (isLocked && !twinButton.isLocked) UnlockDoor();
        UpdatePromptMessage();

    }

    private void UpdatePromptMessage()
    {
        promptMessage = (isOpen ? "Close" : "Open") + " " + type.ToString();
    }

    private void UnlockDoor()
    {
        isLocked = false;
        GetComponent<Renderer>().material = interactableLight;
        if (twinButton != null && hasButton) twinButton.SyncToTwinButton();
    }

    public void UnlockAndOpenDoor()
    {
        UnlockDoor();
        bool playSound = !isOpen;
        isOpen = true;

        doorAnimator.SetBool(ANIMISOPEN, isOpen);
        if(playSound) SoundManager.Instance.PlaySFXOneShot(doorSfx, 0, 0.5f);
        UpdatePromptMessage();
        if (twinButton != null && hasButton) twinButton.SyncToTwinButton();
    }
}
