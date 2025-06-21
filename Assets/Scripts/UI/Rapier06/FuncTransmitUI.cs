using UnityEngine;

public class FuncTransmitUI : FuncCardUI
{
    private const string TRANSMITTED = "</b></color> has been sent to command...\n\nCommand have responded with the following:\n\t";
    private const string ALREADYSENTRESPONSE = "<Automated Message> This data has already been received by commmand. Transmission bounced.";
    private const string ENCRYPTEDALREADYSENTRESPONSE = "<Automated Message> This encrypted data has already been received by commmand. Transmission bounced.";
    private const string ENCRYPTEDRESPONSE = "<Automated Message> Command thanks you for communicating this sensitive data. Stay diligent crewmate";

    protected override void ConfirmExecution()
    {
        bool isEncrypted = !ServerHubUI.Instance.IsContentDecrypted(selectedContent);
        bool alreadyTransmitted = false;
        string response = string.Empty;
        
        if(isEncrypted)
        {
            alreadyTransmitted = ServerHubUI.Instance.pdStorage.transmittedEncryptedContent.Contains(selectedContent);
            if(!alreadyTransmitted) ServerHubUI.Instance.pdStorage.transmittedEncryptedContent.Add(selectedContent);

            response = alreadyTransmitted ? ENCRYPTEDALREADYSENTRESPONSE : ENCRYPTEDRESPONSE;
        } else
        {
            alreadyTransmitted = ServerHubUI.Instance.pdStorage.transmittedContent.Contains(selectedContent);
            if (!alreadyTransmitted) ServerHubUI.Instance.pdStorage.transmittedContent.Add(selectedContent);

            response = alreadyTransmitted ? ALREADYSENTRESPONSE : selectedContent.commandResponseMessage;
        }

        functionOutput.GetComponent<FunctionOutputUI>().SetOutput("<b><color=white>" + selectedContent.displayName + TRANSMITTED + response);
        base.ConfirmExecution();
    }
}
