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
            if (staticKeyboardImage) SetImage(KEYBOARDMOUSEFPATH, newPrompt);
            else DefaultButton(newPrompt);
        } else if (type == InputManager.LastInputType.Playstation)
        {
            SetImage(PLAYSTATIONFPATH, newPrompt);
        } else
        {
            SetImage(XBOXPATH, newPrompt);

        }
    }

    private void SetImage(string inputPath, string newPrompt)
    {
        Sprite buttonImage = Resources.Load<Sprite>(BUTTONPROMPTFPATH + inputPath + newPrompt);
        if (buttonImage != null)
        {
            promptBackground.sprite = buttonImage;
            buttonText.text = string.Empty;
        }
        else
            DefaultButton(newPrompt);
    }

    private void DefaultButton(string newPrompt)
    {
        string keyShape = newPrompt.Length > 1 ? KEYRECTANGLE : KEYSQUARE;
        promptBackground.sprite = Resources.Load<Sprite>(BUTTONPROMPTFPATH + KEYBOARDMOUSEFPATH + keyShape);
        buttonText.text = newPrompt;
    }
}
