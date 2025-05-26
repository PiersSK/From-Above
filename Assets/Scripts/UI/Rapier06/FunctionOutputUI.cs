using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FunctionOutputUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI output;
    public Button dismissButton;

    public void SetOutput(string outputText)
    {
        output.text = outputText;
    }
}
