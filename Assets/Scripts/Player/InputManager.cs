using Adobe.Substance.Connector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

public class InputManager : MonoBehaviour 
{
    public static InputManager Instance { get; private set; }

    private PlayerInput playerInput;
    public PlayerInput.PlayerActions playerActions;
    private PlayerMotor motor;
    private PlayerLook look;

    [SerializeField] private TaskManager taskManager;

    public enum LastInputType { KeyboardMouse, Playstation, Xbox, Gamepad }
    public LastInputType lastInputType { get; private set; }

    public delegate void OnInputTypeChanged(LastInputType newInputType);
    public static event OnInputTypeChanged InputTypeChanged;

    private void Awake()
    {
        Instance = this;

        playerInput = new PlayerInput();
        playerActions = playerInput.Player;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        playerActions.Jump.performed += ctx => motor.Jump();
        playerActions.Crouch.performed += ctx => motor.Crouch();
        playerActions.Sprint.performed += ctx => motor.Sprint();
        playerActions.Tasklist.performed += ctx => taskManager.ToggleTaskPad();

        InputSystem.onAnyButtonPress.Call(OnAnyInputDetected);
        InputSystem.onEvent += OnAnyDeviceEvent;

        lastInputType = LastInputType.KeyboardMouse;
    }

    private void OnAnyInputDetected(InputControl control)
    {
        LastInputType newInputType = DetectInputDevice(control.device);
        UpdateInputType(newInputType);
    }

    private void OnAnyDeviceEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (device is Gamepad gamepad)
        {
            // Read analog stick values and trigger values
            Vector2 leftStick = gamepad.leftStick.ReadValue();
            Vector2 rightStick = gamepad.rightStick.ReadValue();
            float leftTrigger = gamepad.leftTrigger.ReadValue();
            float rightTrigger = gamepad.rightTrigger.ReadValue();

            // Only detect input if there's significant movement (prevent minor drift from triggering change)
            if (leftStick.magnitude > 0.1f || rightStick.magnitude > 0.1f ||
                leftTrigger > 0.1f || rightTrigger > 0.1f)
            {
                UpdateInputType(DetectInputDevice(gamepad));
            }
        }
        else if (device is Mouse mouse) UpdateInputType(LastInputType.KeyboardMouse);
    }

    private void UpdateInputType(LastInputType newInputType)
    {
        if (newInputType != lastInputType)
        {
            lastInputType = newInputType;
            InputTypeChanged?.Invoke(lastInputType);
        }
    }

    private LastInputType DetectInputDevice(InputDevice device)
    {
        if (device is Gamepad gamepad)
        {
            if (gamepad.displayName.Contains("Xbox"))
                return LastInputType.Xbox;
            if (gamepad.displayName.Contains("DualSense") || gamepad.displayName.Contains("DualShock"))
                return LastInputType.Playstation;

            return LastInputType.Gamepad;
        }
        return LastInputType.KeyboardMouse;
    }

    public string GetCurrentBinding(string actionName)
    {
        if (playerInput == null) return string.Empty;

        InputAction action = playerInput.FindAction(actionName);
        if (action == null) return string.Empty;

        foreach (var binding in action.bindings)
        {
            if ((lastInputType != LastInputType.KeyboardMouse && binding.groups.Contains("Gamepad")) ||
                (lastInputType == LastInputType.KeyboardMouse && binding.groups.Contains("Keyboard&Mouse")))
            {
                return binding.ToDisplayString();
            }
        }

        return string.Empty;
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
