using UnityEngine;
using UnityEngine.UI;

public class Computer : Interactable
{
    private bool playerAtComputer = false;
    private PlayerMotor motor;
    private PlayerLook look;
    protected InputManager input;
    [SerializeField] private Transform lockPoint;

    private const string ESCAPEUI = "[Esc] To exit terminal";

    protected virtual void Update()
    {
        if(input != null && !isInteractable && input.playerActions.Escape.triggered)
        {
            ReleasePlayer();
        }
    }

    protected override void Interact(Transform player)
    {
        motor = player.GetComponent<PlayerMotor>();
        look = player.GetComponent<PlayerLook>();
        input = player.GetComponent<InputManager>();

        isInteractable = false;

        motor.ForcePlayerToPoint(lockPoint, true);
        motor.ToggleMovementOverride();

        Cursor.lockState = CursorLockMode.None;
        look.ToggleLookLock();

        UIManager.Instance.ToggleCrosshairVisibility();
        UIManager.Instance.ShowHelpText(ESCAPEUI);
        UIManager.Instance.HideTaskPadPrompt();
    }

    protected virtual void ReleasePlayer()
    {
        motor.ToggleMovementOverride();
        Cursor.lockState = CursorLockMode.Locked;
        look.ToggleLookLock();
        isInteractable = true;

        UIManager.Instance.ToggleCrosshairVisibility();
        UIManager.Instance.HideHelpText();
        UIManager.Instance.ShowTaskPadPrompt();
    }
}