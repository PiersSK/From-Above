using TMPro;
using UnityEngine;

public class FunctionOutputUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI output;

    public void SetOutput(string outputText)
    {
        output.text = outputText;
    }
}
