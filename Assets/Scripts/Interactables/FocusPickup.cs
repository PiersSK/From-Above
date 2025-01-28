using UnityEngine;

public class FocusPickup : Interactable
{
    [SerializeField] private bool hasText;
    [TextArea(15, 20)]
    [SerializeField] private string textTranscript;
    private bool textPopupVisible = false;

    private Vector3 initialPos;
    private Quaternion initialRot;

    private InputManager input;
    private PlayerMotor motor;
    private PlayerLook look;

    private const string READUI = "[Tab] To View Text";
    private const string ESCAPEUI = "[Esc] To Put Down";

    private void Start()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

    private void Update()
    {
        if (input != null && !isInteractable && input.playerActions.Escape.triggered)
        {
            ReleasePlayer();
        }

        if (input != null && !isInteractable && input.playerActions.UIToggle.triggered && hasText)
        {
            ToggleTextUI();
        }
    }

    protected override void Interact(Transform player)
    {
        motor = player.GetComponent<PlayerMotor>();
        look = player.GetComponent<PlayerLook>();
        input = player.GetComponent<InputManager>();

        isInteractable = false;

        motor.ToggleMovementOverride();
        look.ToggleLookLock();

        UIManager.Instance.ToggleCrosshairVisibility();

        string help = hasText ? READUI + "\n" + ESCAPEUI : ESCAPEUI;
        UIManager.Instance.ShowHelpText(help);

        transform.position = player.position + new Vector3(0f, 0.6f, 0f) + player.forward * 0.5f;
        transform.eulerAngles = new Vector3(0f, 180f + player.eulerAngles.y, 0f);
    }

    private void ReleasePlayer()
    {
        motor.ToggleMovementOverride();
        Cursor.lockState = CursorLockMode.Locked;
        look.ToggleLookLock();
        isInteractable = true;

        UIManager.Instance.ToggleCrosshairVisibility();
        UIManager.Instance.HideHelpText();
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
