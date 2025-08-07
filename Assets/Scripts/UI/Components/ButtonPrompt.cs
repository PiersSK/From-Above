using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonPrompt : MonoBehaviour
{
    [SerializeField] private List<DynamicButton> buttonImages = new();
    [SerializeField] private TextMeshProUGUI promptMessage;

    public void UpdatePromptMessage(string message)
    {
        promptMessage.text = message;
    }
}
