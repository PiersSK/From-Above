using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RapierTerminal : Computer
{
    [SerializeField] private TextMeshProUGUI notifHeader;
    [SerializeField] private TextMeshProUGUI notif1;
    [SerializeField] private TextMeshProUGUI notif2;
    [SerializeField] private Button shipStatusBtn;
    [SerializeField] private TextMeshProUGUI shipStatusSubtitle;
    [SerializeField] private Button readDataBtn;
    [SerializeField] private TextMeshProUGUI readDataSubtitle;
    [SerializeField] private TextMeshProUGUI btnResponse;

    private bool shipDataUploaded = false;
    private const string shipDataUploadResponse = "UPLOAD COMPLETE\nCommand thanks you for your continued vigilance";
    private const string shipDataUploadReject = "NO UPDATED FOUND\nShip data was already updated today. Thank you for your continued vigilance";
    private const string shipDataSubtitleResponse = "NO ACTION REQUIRED";

    //Something about reading data

    private void Start()
    {
        shipStatusBtn.onClick.AddListener(UploadShipStatus);
        readDataBtn.onClick.AddListener(ReadData);
    }

    private void UploadShipStatus()
    {
        if (!shipDataUploaded)
        {
            btnResponse.text = shipDataUploadResponse;
            shipStatusBtn.interactable = false;
            shipStatusSubtitle.text = shipDataSubtitleResponse;
            notif2.fontStyle = FontStyles.Strikethrough;
            shipDataUploaded = true;
        } else
        {
            btnResponse.text = shipDataUploadReject;
        }
    }

    private void ReadData()
    {
        // Something here later
    }
}
