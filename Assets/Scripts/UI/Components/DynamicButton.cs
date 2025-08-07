using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DynamicButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public Image promptBackground;

    private static string BUTTONPROMPTFPATH = "ButtonPrompts/";
    private static string KEYBOARDMOUSEFPATH = "KeyboardMouse/";
    private static string XBOXPATH = "Xbox/";
    private static string PLAYSTATIONFPATH = "Playstation/";

    private static string KEYSQUARE = "keySquare";
    private static string KEYRECTANGLE = "keyRectangle";

    private void Start()
    {
        if (buttonText == null) Debug.Log("PSK: " + gameObject.name + " has no buttonText reference");
    }

    public void UpdatePrompt(string newPrompt, InputManager.LastInputType type, bool staticKeyboardImage = false)
    {
        if (type == InputManager.LastInputType.KeyboardMouse)
        {
            DefaultButton(newPrompt);
        } else if (type == InputManager.LastInputType.Playstation)
        {
            Sprite buttonImage = Resources.Load<Sprite>(BUTTONPROMPTFPATH + PLAYSTATIONFPATH + newPrompt);
            if (buttonImage != null)
            {
                promptBackground.sprite = buttonImage;
                buttonText.text = string.Empty;
            }
            else
                DefaultButton(newPrompt);
        } else
        {
            Sprite buttonImage = Resources.Load<Sprite>(BUTTONPROMPTFPATH + XBOXPATH + newPrompt);
            if (buttonImage != null)
            {
                promptBackground.sprite = buttonImage;
                buttonText.text = string.Empty;
            }
            else
                DefaultButton(newPrompt);
        }
    }

    private void DefaultButton(string newPrompt, bool staticKeyboardImage = false)
    {
        if (staticKeyboardImage) return;

        string keyShape = newPrompt.Length > 1 ? KEYRECTANGLE : KEYSQUARE;
        promptBackground.sprite = Resources.Load<Sprite>(BUTTONPROMPTFPATH + KEYBOARDMOUSEFPATH + keyShape);
        buttonText.text = newPrompt;
    }
}
