using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerHubDataObjectUI : MonoBehaviour
{
    [SerializeField] private Image dataTypeIcon;
    [SerializeField] private Image highlightBackground;
    public TextMeshProUGUI dataName;
    [SerializeField] private TextMeshProUGUI dataType;

    private const string EMPTY = "Empty";
    private const string NOTYPE = "-";
    private const string ENCRYPTEDICON = "DataIcons/EncryptedIcon";

    public void SetHighlightState(bool state)
    {
        highlightBackground.enabled = state;
    }

    public void SetDataObject(DiscSlotContent slotContent)
    {
        if (slotContent != null)
        {
            bool encrypted = slotContent.decipherType != DiscSlotContent.DecipherType.None;

            dataName.text = ServerHubUI.Instance.GetFormattedDataSlotName(slotContent);
            dataName.color = UIColors.white;
            dataType.text = ServerHubUI.Instance.GetFormattedDataSlotType(slotContent);
            dataTypeIcon.sprite = ServerHubUI.Instance.GetFormattedDataSlotIcon(slotContent);
            dataTypeIcon.color = UIColors.terminalGreen;
        } else
        {
            dataName.text = EMPTY;
            dataName.color = UIColors.grey;
            dataType.text = NOTYPE;
            dataTypeIcon.sprite = null;
            dataTypeIcon.color = UIColors.darkGrey;
        }
    }
}
