using UnityEngine;
using UnityEngine.UI;

public class Computer : Interactable
{
    protected bool playerAtComputer = false;
    protected PlayerMotor motor;
    protected PlayerLook look;
    [SerializeField] protected Transform lockPoint;
    [SerializeField] protected Selectable defaultSelectable;

    [SerializeField] protected AudioClip initiationSound;

    private Vector3 initialPos;
    private Quaternion initialRotation;

    protected const string EXITTERMINAL = "Exit Terminal";

    private void OnEnable()
    {
        InputManager.InputTypeChanged += OnInputChange;
    }

    private void OnDisable()
    {
        InputManager.InputTypeChanged -= OnInputChange;
    }

    private void OnInputChange(InputManager.LastInputType newType)
    {
        if (playerAtComputer)
        {
            if (newType == InputManager.LastInputType.KeyboardMouse)
            {
                SwitchToMouseKeyboard();
            }
            else
            {
                SwitchToGamepad();
            }
        }
    }

    protected virtual void SwitchToMouseKeyboard()
    {
        Cursor.lockState = CursorLockMode.None;
        UIManager.Instance.ClearSelectedUIObject();
    }

    protected virtual void SwitchToGamepad()
    {
        Cursor.lockState = CursorLockMode.Locked;
        defaultSelectable?.Select();
    }

    protected virtual void Update()
    {
        if(!isInteractable && InputManager.Instance.playerActions.Escape.triggered)
        {
            ReleasePlayer();
        }
    }

    protected override void Interact(Transform player)
    {
        motor = player.GetComponent<PlayerMotor>();
        look = player.GetComponent<PlayerLook>();

        isInteractable = false;

        initialPos = player.transform.position;
        initialRotation = player.transform.rotation;

        motor.ForcePlayerToPoint(lockPoint, true);
        motor.TogglePlayerLock();

        if (!InputManager.Instance.GamepadIsCurrentInput())
            Cursor.lockState = CursorLockMode.None;
        else if (defaultSelectable != null)
            defaultSelectable.Select();

        look.ToggleLookLock();

        UIManager.Instance.ToggleCrosshairVisibility();
        UIManager.Instance.ShowButtonPrompt(UIManager.ButtonPromptType.BackOut, EXITTERMINAL);
        UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.TaskPad);
        playerAtComputer = true;

        SoundManager.Instance.PlaySFXOneShot(initiationSound, 0f, 0.1f, 0f);
    }

    protected virtual void ReleasePlayer()
    {
        motor.ForcePlayerToPoint(initialPos, initialRotation);
        motor.TogglePlayerLock();
        Cursor.lockState = CursorLockMode.Locked;
        look.ToggleLookLock();
        isInteractable = true;

        UIManager.Instance.ToggleCrosshairVisibility();
        UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.BackOut);
        UIManager.Instance.ShowButtonPrompt(UIManager.ButtonPromptType.TaskPad, string.Empty, false);
        UIManager.Instance.ClearSelectedUIObject();
        playerAtComputer = false;
    }
}