using UnityEngine;

public class FocusPickup : Interactable
{
    [SerializeField] private bool hasText;
    [TextArea(15, 20)]
    [SerializeField] private string textTranscript;
    [SerializeField] public AudioClip sfx;
    private bool textPopupVisible = false;

    private Vector3 initialPos;
    private Quaternion initialRot;

    private PlayerMotor motor;
    private PlayerLook look;

    private const string READUI = "To View Text";
    private const string ESCAPEUI = "To Put Down";

    private void Start()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

    private void Update()
    {
        if (!isInteractable && InputManager.Instance.playerActions.Escape.triggered)
        {
            ReleasePlayer();
        }

        if (!isInteractable && InputManager.Instance.playerActions.UIToggle.triggered && hasText)
        {
            ToggleTextUI();
        }
    }

    protected override void Interact(Transform player)
    {
        motor = player.GetComponent<PlayerMotor>();
        look = player.GetComponent<PlayerLook>();

        isInteractable = false;

        if(sfx != null) SoundManager.Instance.PlaySFXOneShot(sfx);

        motor.ToggleMovementOverride();
        look.ToggleLookLock();

        UIManager.Instance.ToggleCrosshairVisibility();

        if (hasText) UIManager.Instance.ShowToggleText(READUI);
        UIManager.Instance.ShowBackoutText(ESCAPEUI);

        transform.position = player.position + new Vector3(0f, 0.6f, 0f) + player.forward * 0.5f;
        transform.eulerAngles = new Vector3(0f, 180f + player.eulerAngles.y, 0f);
    }

    public virtual void ReleasePlayer()
    {
        motor.ToggleMovementOverride();
        Cursor.lockState = CursorLockMode.Locked;
        look.ToggleLookLock();
        isInteractable = true;

        UIManager.Instance.ToggleCrosshairVisibility();
        UIManager.Instance.HideBackoutText();
        UIManager.Instance.HideToggleText();
        UIManager.Instance.HidePopupText();
        textPopupVisible = false;

        transform.position = initialPos;
        transform.rotation = initialRot;
    }

    private void ToggleTextUI()
    {
        if (textPopupVisible)
            UIManager.Instance.HidePopupText();
        else
            UIManager.Instance.ShowPopupText(textTranscript);

        textPopupVisible = !textPopupVisible;
    }
}
