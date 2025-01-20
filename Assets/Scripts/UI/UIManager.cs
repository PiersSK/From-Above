using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {  get; private set; }
    [SerializeField] private GameObject crosshair;
    [SerializeField] private TextMeshProUGUI helpText;

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleCrosshairVisibility()
    {
        crosshair.SetActive(!crosshair.activeSelf);
    }

    public void ShowHelpText(string message)
    {
        helpText.text = message;
        helpText.gameObject.SetActive(true);
    }

    public void HideHelpText()
    {
        helpText.gameObject.SetActive(false);
    }
}
