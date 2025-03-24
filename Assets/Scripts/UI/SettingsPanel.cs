using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Button mainNavigationButton;
    [SerializeField] private List<Selectable> selectableOptions;

    public string settingsPanelName;
    private Selectable lastSelectedOption;

    private const string BACKOUTMESSAGE = "Back";
    private const string CHANGEVALUE = "Edit Settings Value";

    public void UpdateLastSelectedOption()
    {
        foreach (Selectable option in selectableOptions)
        {
            if (option.gameObject == UIManager.Instance.GetSelectedUIObject())
            {
                lastSelectedOption = option;
            }
        }
    }

    public void SettingsPanelSelected()
    {
        if (InputManager.Instance.GamepadIsCurrentInput())
        {
            if(lastSelectedOption != null) lastSelectedOption.Select();
            else selectableOptions[0].Select();

            UIManager.Instance.ShowBackoutText(BACKOUTMESSAGE);
            UIManager.Instance.ShowLRText(CHANGEVALUE);
        }
    }

    public void SettingsPanelDeselected()
    {
        if (InputManager.Instance.GamepadIsCurrentInput())
        {
            PauseManager.Instance.CloseAllSettingsPanels();
            UIManager.Instance.HideLRText();

            mainNavigationButton.Select();
            lastSelectedOption = null;
        }
    }

}
