using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private List<Selectable> selectableOptions;
    [SerializeField] private TextMeshProUGUI headerText;
    public Button headerButton;
    [SerializeField] private Image headerBackground;
    [SerializeField] private Image border;

    public string settingsPanelName;
    public bool isFocused = false;

    private const string BACKOUTMESSAGE = "Back";

    private void Start()
    {
        headerButton.onClick.AddListener(SettingsPanelSelected);
    }

    private void Update()
    {
        if (InputManager.Instance.GamepadIsCurrentInput())
        {
            bool focusedComponent = false;
            foreach (Selectable selectable in selectableOptions)
            {
                if (UIManager.Instance.IsObjectSelected(selectable.gameObject))
                {
                    focusedComponent = true;
                    break;
                }
            }
            isFocused = focusedComponent;
            if (isFocused && InputManager.Instance.playerActions.Escape.triggered)
            {
                SettingsPanelDeselected();
            }
        } else
        {
            border.color = UIColors.terminalGreen;
        }
    }

    private void SettingsPanelSelected()
    {
        if (InputManager.Instance.GamepadIsCurrentInput())
        {
            selectableOptions[0].Select();
            border.color = UIColors.terminalGreen;
            UIManager.Instance.ShowBackoutText(BACKOUTMESSAGE);
        }
    }

    private void SettingsPanelDeselected()
    {
        headerButton.Select();
        border.color = UIColors.terminalGreenTransparent;
        PauseManager.Instance.ShowDefaultPauseKeyBindings(InputManager.Instance.lastInputType);
    }
}
