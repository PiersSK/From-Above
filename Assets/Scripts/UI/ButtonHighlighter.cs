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
            ColorBlock colorBlock = new ColorBlock();
            colorBlock.normalColor = UIColors.grey;
            colorBlock.highlightedColor = UIColors.white;
            colorBlock.pressedColor = UIColors.grey;
            colorBlock.disabledColor = UIColors.darkGrey;
            colorBlock.selectedColor = UIColors.white;
            colorBlock.colorMultiplier = 1f;
            button.colors = colorBlock;

            textStandardColor = buttonText.color;
        }
    }

    private void Update()
    {
        if (button.gameObject == UIManager.Instance.GetSelectedUIObject())
        {
            buttonText.color = UIColors.white;
        } else
        {
            buttonText.color = textStandardColor;
        }
    }

}
