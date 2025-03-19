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

    public enum LastInputType { KeyboardMouse, Playstation, Xbox, Gamepad }
    public LastInputType lastInputType { get; private set; }

    public delegate void OnInputTypeChanged(LastInputType newInputType);
    public static event OnInputTypeChanged InputTypeChanged;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        playerInput = new PlayerInput();
        playerActions = playerInput.Player;

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

    public bool GamepadIsCurrentInput()
    {
        return lastInputType != LastInputType.KeyboardMouse;
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

    public string GetCurrentBinding(string actionName, string compositePart = null)
    {
        if (playerInput == null) return string.Empty;

        InputAction action = playerInput.FindAction(actionName);
        if (action == null) return string.Empty;

        foreach (var binding in action.bindings)
        {
            string bindingGroups = binding.groups ?? string.Empty;

            bool isGamepadBinding = GamepadIsCurrentInput() && bindingGroups.Contains("Gamepad");
            bool isKeyboardBinding = !GamepadIsCurrentInput() && bindingGroups.Contains("Keyboard&Mouse");

            if (compositePart != null) { 
                if (binding.isPartOfComposite && binding.name.ToUpper() == compositePart.ToUpper() && (isGamepadBinding || isKeyboardBinding))
                    return binding.ToDisplayString();
            } else
            {
                if (!binding.isPartOfComposite && (isGamepadBinding || isKeyboardBinding))
                    return binding.ToDisplayString();
            }
        }

        return string.Empty;
    }
}
