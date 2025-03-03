using System.Collections.Generic;
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
        {
            InputManager.LastInputType.Gamepad,
            new()
            {
                { "", "" }
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
        interactPrompt.UpdatePrompt(input.GetCurrentBinding(INTERACT), newType);
        taskPrompt.UpdatePrompt(input.GetCurrentBinding(TASKLIST), newType);
        togglePrompt.UpdatePrompt(input.GetCurrentBinding(TOGGLE), newType);
        backoutPrompt.UpdatePrompt(input.GetCurrentBinding(BACKOUT), newType);
    }
}
