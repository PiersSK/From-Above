using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ButtonPromptManager : MonoBehaviour
{
    private InputManager input;
    private Dictionary<InputManager.LastInputType, Dictionary<string, string>> displayValueOverrides = new()
    {
        {
            InputManager.LastInputType.KeyboardMouse,
            new()
            {
                { "Left Control", "CTRL" },
                { "Escape", "ESC" }
            }
        },
    };

    private const string INTERACT = "Interact";
    private const string TASKLIST = "Tasklist";
    private const string TOGGLE = "UIToggle";
    private const string BACKOUT = "Escape";
    private const string CONFIRM = "Confirm";
    private const string NAVIGATE = "Navigate";
    private const string LEFT = "Left";
    private const string RIGHT = "Right";


    [SerializeField] private ButtonPrompt interactPrompt;
    [SerializeField] private ButtonPrompt taskPrompt;
    [SerializeField] private ButtonPrompt togglePrompt;
    [SerializeField] private ButtonPrompt backoutPrompt;
    [SerializeField] private ButtonPrompt confirmPrompt;
    [SerializeField] private ButtonPrompt leftPrompt;
    [SerializeField] private ButtonPrompt rightPrompt;

    private void OnEnable()
    {
        InputManager.InputTypeChanged += UpdateButtonPrompts;
    }

    private void OnDisable()
    {
        InputManager.InputTypeChanged -= UpdateButtonPrompts;
    }

    private void Start()
    {
        input = InputManager.Instance;
        UpdateButtonPrompts(input.lastInputType);
    }

    private void UpdateButtonPrompts(InputManager.LastInputType newType)
    {
        interactPrompt.UpdatePrompt(GetAltText(input.GetCurrentBinding(INTERACT), newType), newType);
        taskPrompt.UpdatePrompt(GetAltText(input.GetCurrentBinding(TASKLIST), newType), newType);
        togglePrompt.UpdatePrompt(GetAltText(input.GetCurrentBinding(TOGGLE), newType), newType);
        backoutPrompt.UpdatePrompt(GetAltText(input.GetCurrentBinding(BACKOUT), newType), newType);
        confirmPrompt.UpdatePrompt(GetAltText(input.GetCurrentBinding(CONFIRM), newType), newType);
        leftPrompt.UpdatePrompt(GetAltText(input.GetCurrentBinding(NAVIGATE, LEFT), newType), newType);
        rightPrompt.UpdatePrompt(GetAltText(input.GetCurrentBinding(NAVIGATE, RIGHT), newType), newType);
    }

    private string GetAltText(string assignment, InputManager.LastInputType type)
    {
        if (displayValueOverrides.Keys.Contains(type))
        {
            Dictionary<string, string> overrides = displayValueOverrides[type];
            if (overrides.Keys.Contains(assignment))
            {
                return overrides[assignment];
            }
        } else if (assignment.Contains("/"))
        {
            return assignment.Split("/")[1];
        }

        return assignment;
    }
}
