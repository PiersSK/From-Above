using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonPrompt : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public Image promptBackground;

    public void UpdatePrompt(string newPrompt, InputManager.LastInputType type)
    {
        promptText.text = newPrompt;
    }
}
