using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHighlighter : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI buttonText;
    private Color textStandardColor;

    private void Start()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if(button == null)
        {
            Debug.LogWarning("ButtonHighlighter added to GameObject with no Button component: " + gameObject.name);
            enabled = false;
        } else
        {
            UpdateForInputType(InputManager.Instance.lastInputType);

            textStandardColor = buttonText.color;
        }
    }

    private void OnEnable()
    {
        InputManager.InputTypeChanged += UpdateForInputType;
    }

    private void OnDisable()
    {
        InputManager.InputTypeChanged -= UpdateForInputType;
    }

    private void UpdateForInputType(InputManager.LastInputType newInputType)
    {
        ColorBlock colorBlock = new ColorBlock();
        colorBlock.pressedColor = UIColors.grey;
        colorBlock.disabledColor = UIColors.darkGrey;
        colorBlock.selectedColor = UIColors.white;
        colorBlock.highlightedColor = UIColors.white;
        colorBlock.colorMultiplier = 1f;

        if (newInputType == InputManager.LastInputType.KeyboardMouse)
            colorBlock.normalColor = UIColors.white;
        else
            colorBlock.normalColor = UIColors.grey;

        button.colors = colorBlock;
    }

    private void Update()
    {
        if (UIManager.Instance.IsObjectSelected(button.gameObject) 
            && InputManager.Instance.GamepadIsCurrentInput())
        {
            buttonText.color = UIColors.white;
        } else
        {
            buttonText.color = textStandardColor;
        }
    }

}
