using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonPrompt : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public Image promptBackground;

    private static string BUTTONPROMPTFPATH = "ButtonPrompts/";
    private static string KEYBOARDMOUSEFPATH = "KeyboardMouse/";
    private static string XBOXPATH = "Xbox/";
    private static string PLAYSTATIONFPATH = "Playstation/";

    private static string KEYSQUARE = "keySquare";
    private static string KEYRECTANGLE = "keyRectangle";

    public void UpdatePrompt(string newPrompt, InputManager.LastInputType type)
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
                promptText.text = string.Empty;
            }
            else
                DefaultButton(newPrompt);
        } else
        {
            Sprite buttonImage = Resources.Load<Sprite>(BUTTONPROMPTFPATH + XBOXPATH + newPrompt);
            if (buttonImage != null)
            {
                promptBackground.sprite = buttonImage;
                promptText.text = string.Empty;
            }
            else
                DefaultButton(newPrompt);
        }
    }

    private void DefaultButton(string newPrompt)
    {
        string keyShape = newPrompt.Length > 1 ? KEYRECTANGLE : KEYSQUARE;
        promptBackground.sprite = Resources.Load<Sprite>(BUTTONPROMPTFPATH + KEYBOARDMOUSEFPATH + keyShape);
        promptText.text = newPrompt;
    }
}
