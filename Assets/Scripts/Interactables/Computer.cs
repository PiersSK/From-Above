using UnityEngine;

public class Computer : Interactable
{
    private bool playerAtComputer = false;
    private PlayerMotor motor;
    private PlayerLook look;
    private InputManager input;
    [SerializeField] private Transform lockPoint;

    private void Update()
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
    }

    private void ReleasePlayer()
    {
        motor.ToggleMovementOverride();
        Cursor.lockState = CursorLockMode.Locked;
        look.ToggleLookLock();
        isInteractable = true;
    }
}