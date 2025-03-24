using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerMotor motor;
    private PlayerLook look;
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private InputManager inputManager;
    private PlayerInput.PlayerActions playerActions;

    private void Awake()
    {
        playerActions = inputManager.playerActions;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        playerActions.Jump.performed += ctx => motor.Jump();
        playerActions.Crouch.performed += ctx => motor.Crouch();
        playerActions.Sprint.performed += ctx => motor.Sprint();
        playerActions.Tasklist.performed += ctx => taskManager.ToggleTaskPad();
        playerActions.Pause.performed += ctx => PauseManager.Instance.TogglePauseMenu();
    }
    private void Update()
    {
        motor.ProcessMove(playerActions.Move.ReadValue<Vector2>());
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
