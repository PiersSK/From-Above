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
        defaultSelectable.Select();
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

        motor.ForcePlayerToPoint(lockPoint, true);
        motor.ToggleMovementOverride();

        if (!InputManager.Instance.GamepadIsCurrentInput())
            Cursor.lockState = CursorLockMode.None;
        else
            defaultSelectable.Select();

        look.ToggleLookLock();

        UIManager.Instance.ToggleCrosshairVisibility();
        UIManager.Instance.ShowBackoutText(EXITTERMINAL);
        UIManager.Instance.HideTaskPadPrompt();
        playerAtComputer = true;

        SoundManager.Instance.PlaySFXOneShot(initiationSound, 0f, 0.1f, 0f);
    }

    protected virtual void ReleasePlayer()
    {
        motor.ToggleMovementOverride();
        Cursor.lockState = CursorLockMode.Locked;
        look.ToggleLookLock();
        isInteractable = true;

        UIManager.Instance.ToggleCrosshairVisibility();
        UIManager.Instance.HideBackoutText();
        UIManager.Instance.ShowTaskPadPrompt();
        UIManager.Instance.ClearSelectedUIObject();
        playerAtComputer = false;
    }
}