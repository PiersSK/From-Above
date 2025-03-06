using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonPrompt : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public Image promptBackground;

    public Sprite MKBackground;
    public Sprite GamepadBackground;
    public bool circleGamepad;

    private float bgHeight = 45f;

    public void UpdatePrompt(string newPrompt, InputManager.LastInputType type)
    {
        promptText.text = newPrompt;
        promptBackground.sprite = type == InputManager.LastInputType.KeyboardMouse ? MKBackground : GamepadBackground;
        
        promptBackground.GetComponent<RectTransform>().sizeDelta = new(width, bgHeight);
    }
}
