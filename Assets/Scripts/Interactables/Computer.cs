using UnityEngine;
using UnityEngine.UI;

public class Computer : Interactable
{
    protected bool playerAtComputer = false;
    protected PlayerMotor motor;
    protected PlayerLook look;
    protected InputManager input;
    [SerializeField] protected Transform lockPoint;

    [SerializeField] protected AudioClip enterKeyclicks;

    protected string ESCAPEUI = "[CTRL] To exit terminal";

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
        playerAtComputer = true;

        SoundManager.Instance.PlaySFXOneShot(enterKeyclicks, 0f, 0.1f, 0f);
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
        playerAtComputer = false;
    }
}