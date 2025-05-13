using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerHubDataObjectUI : MonoBehaviour
{
    [SerializeField] private Image dataTypeIcon;
    [SerializeField] private TextMeshProUGUI dataName;
    [SerializeField] private TextMeshProUGUI dataType;

    private const string EMPTY = "Empty";
    private const string NOTYPE = "-";
    private const string ENCRYPTEDICON = "DataIcons/EncryptedIcon";

    public void SetDataObject(DiscSlotContent slotContent)
    {
        if (slotContent != null)
        {
            bool encrypted = slotContent.decipherType != DiscSlotContent.DecipherType.None;

            dataName.text = encrypted ? TextEncryption.EncryptToBase64(slotContent.displayName, slotContent.decipherType) : slotContent.displayName;
            dataName.color = UIColors.white;
            dataType.text = encrypted ? TextEncryption.EncryptToBase64(slotContent.GetDisplayType(), slotContent.decipherType) : slotContent.GetDisplayType();
            dataTypeIcon.sprite = encrypted ? Resources.Load<Sprite>(ENCRYPTEDICON) : slotContent.GetIcon();
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
