using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LocalContent;

public class FuncEjectUI : FuncCardUI
{
    [SerializeField] private GameObject airlockUI;
    [SerializeField] private TextMeshProUGUI pdName;
    [SerializeField] private Button ejectAirlockButton;

    [SerializeField] private AudioClip pdEjectSFX;
    //[SerializeField] private AudioClip airlockEject; TODO: Add this :)
    [SerializeField] private LocalContent airlockLocal;
    [SerializeField] private GameObject boxes;
    [SerializeField] private ShipDoorController doorController;

    private bool airlockEjected = false;

    private const string UNKNOWN = "???";
    private const string AIRLOCKBUTTON = "EJECT AIRLOCK AREA";
    private const string PDEJECT = ": Successfully ejected and returned to crewmate";
    private const string AIRLOCKEJECT = "Airlock has been opened and closed to eject any loose content in the adjacent room. To maintain ship system integrity, repeat use of this function is unavailable for the next 24 hours";

    protected override void Start()
    {
        base.Start();
        ejectAirlockButton.onClick.AddListener(EjectAirlock);
    }

    public override void OpenFuncUI(List<DiscSlotContent> validContent, DataDrive pd = null)
    {
        pdName.text = pd.objectName;
        relevantPd = pd;

        airlockUI.SetActive(!airlockEjected);

        if (!airlockEjected) {
            bool airlockPresent = (validContent.Count > 0 && validContent[0] == airlockLocal);
            inputTitle.text = airlockPresent ? airlockLocal.displayName : UNKNOWN;
            ejectAirlockButton.interactable = airlockPresent;
            ejectAirlockButton.GetComponentInChildren<TextMeshProUGUI>().text = airlockPresent ? AIRLOCKBUTTON : UNKNOWN;
            inputFileIcon.color = airlockPresent ? UIColors.terminalGreen : UIColors.darkGrey;
        }

        gameObject.SetActive(true);
    }

    protected override void ConfirmExecution()
    {
        SoundManager.Instance.PlaySFXOneShot(pdEjectSFX);
        ServerHubUI.Instance.pdStorage.objectsStored.Remove(relevantPd);
        ServerHubUI.Instance.pdStorage.currentIndex = ServerHubUI.Instance.pdStorage.objectsStored.Count - 1;
        ServerHubUI.Instance.pdStorage.SelectNext();
        PlayerInventory.Instance.dataDrivesHeld.Add(relevantPd);

        functionOutput.GetComponent<FunctionOutputUI>().SetOutput(relevantPd.objectName + PDEJECT);
        base.ConfirmExecution();
    }

    private void EjectAirlock()
    {
        boxes.SetActive(false);
        airlockEjected = true;
        doorController.RemoteClose(LocalLocations.EngineRoom);

        functionOutput.GetComponent<FunctionOutputUI>().SetOutput(AIRLOCKEJECT);
        base.ConfirmExecution();
    }

}
