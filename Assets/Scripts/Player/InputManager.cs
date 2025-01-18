using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour 
{
    private PlayerInput playerInput;
    public PlayerInput.PlayerActions playerActions;
    private PlayerMotor motor;
    private PlayerLook look;
    private void Awake()
    {
        playerInput = new PlayerInput();
        playerActions = playerInput.Player;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        playerActions.Jump.performed += ctx => motor.Jump();
        playerActions.Crouch.performed += ctx => motor.Crouch();
        playerActions.Sprint.performed += ctx => motor.Sprint();
    }

    private void Update()
    {
        motor.ProcessMove(playerActions.Move.ReadValue<Vector2>());

    }

    private void LateUpdate()
    {
        look.ProcessLook(playerActions.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
}
