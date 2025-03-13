using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputHighlighter : MonoBehaviour
{
    public TMP_InputField inputField;
    [SerializeField] private GameObject selectedOutline;

    private void Update()
    {
        bool isSelected = UIManager.Instance.IsObjectSelected(inputField.gameObject);
        bool isFocused = inputField.isFocused;

        selectedOutline.SetActive(isSelected || isFocused);

        if (isFocused && InputManager.Instance.GamepadIsCurrentInput()) selectedOutline.GetComponent<Image>().color = UIColors.white;
        else if (isSelected) selectedOutline.GetComponent<Image>().color = UIColors.terminalGreen;
    }
}
