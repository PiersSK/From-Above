using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMainNavigationButton : MonoBehaviour
{
    [SerializeField] private SettingsPanel linkedSettingsPanel;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OptionSelected);
    }

    private void OptionSelected()
    {
        PauseManager.Instance.CloseAllSettingsPanels();
        if (linkedSettingsPanel != null)
        {
            linkedSettingsPanel.gameObject.SetActive(true);
            if (InputManager.Instance.GamepadIsCurrentInput()) linkedSettingsPanel.SettingsPanelSelected();
        }
    }
}
