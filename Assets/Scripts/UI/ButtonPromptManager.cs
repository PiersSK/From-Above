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
                { "Left Control", "CTRL" }
            }
        },
    };

    private const string INTERACT = "Interact";
    private const string TASKLIST = "Tasklist";
    private const string TOGGLE = "UIToggle";
    private const string BACKOUT = "Escape";


    [SerializeField] private ButtonPrompt interactPrompt;
    [SerializeField] private ButtonPrompt taskPrompt;
    [SerializeField] private ButtonPrompt togglePrompt;
    [SerializeField] private ButtonPrompt backoutPrompt;

    private void OnEnable()
    {
        InputManager.InputTypeChanged += UpdateButtonPrompts;
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
    }

    private string GetAltText(string assignment, InputManager.LastInputType type)
    {
        if (displayValueOverrides.Keys.Contains(type))
        {
            Dictionary<string, string> overrides = displayValueOverrides[type];
            if (overrides.Keys.Contains(assignment))
            {
                Debug.Log(overrides[assignment]);
                return overrides[assignment];
            }
        }

        return assignment;
    }
}
