using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonPromptManager : MonoBehaviour
{
    private InputManager input;
    private Dictionary<string, string> displayValueOverrides = new Dictionary<string, string>()
    {
        {"Left Control","CTRL"}
    };

    private const string INTERACT = "Interact";


    [SerializeField] private ButtonPrompt interactPrompt;

    private void OnEnable()
    {
        InputManager.InputTypeChanged += UpdateButtonPrompts;
    }

    private void Start()
    {
        Debug.Log("kjhsdfkjsdfkjhsdf)");
        input = InputManager.Instance;
        UpdateButtonPrompts(input.lastInputType);
    }

    private void UpdateButtonPrompts(InputManager.LastInputType newType)
    {
        Debug.Log("updating......kdjfsdf");
        interactPrompt.UpdatePrompt(input.GetCurrentBinding(INTERACT), newType);
    }
}
