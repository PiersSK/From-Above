using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiaryQABlock : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TMP_InputField inputField;
    [SerializeField] private GameObject selectedOutline;

    private void Start()
    {
        inputField.onSelect.AddListener(ShowSelectBox);
        inputField.onDeselect.AddListener(HideSelectBox);
    }

    private void ShowSelectBox(string e)
    {
        selectedOutline.SetActive(true);
    }

    private void HideSelectBox(string e)
    {
        selectedOutline.SetActive(false);
    }
}
