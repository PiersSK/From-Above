using System.Linq;
using TMPro;
using UnityEngine;

public class FuncReceiveUI : FuncCardUI
{
    [SerializeField] private TextMeshProUGUI receivingMessage;
    [SerializeField] private Animator anim;

    private const string PDSUFFIX = "</b></color> has been updated with data received from remote location";
    private const string DECRYPTANIM = "Decrypt";


    protected override void ConfirmExecution()
    {
        returnButton.interactable = false;
        confirmButton.interactable = false;
        outputTitle.gameObject.SetActive(false);

        RemoteContent remote = (RemoteContent)selectedContent;

        int contentCount = remote.remoteContent.Count;
        receivingMessage.text = "...receiving " + contentCount + (contentCount > 1 ? " files" : " file") + " from remote";
        receivingMessage.gameObject.SetActive(true);

        outputTitle.text = ServerHubUI.Instance.GetFormattedDataSlotName(remote.remoteContent[0]);
        outputFileIcon.sprite = ServerHubUI.Instance.GetFormattedDataSlotIcon(remote.remoteContent[0]);

        ServerHubUI.Instance.pdStorage.receivedPDs.Add(relevantPd);

        anim.SetTrigger(DECRYPTANIM);

        float animLength = anim.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name == DECRYPTANIM.ToLower()).length;
        Invoke("GoToOutput", animLength);
    }

    private void GoToOutput()
    {
        RemoteContent remote = (RemoteContent)selectedContent;
        string output = "<b><color=white>" + relevantPd.objectName + PDSUFFIX;
        output += "\n\n\n" + remote.remoteContent.Count + " data files received:";
        foreach(DiscSlotContent slotContent in remote.remoteContent)
        {
            output += "\n\t" + ServerHubUI.Instance.GetFormattedDataSlotType(slotContent);
            output += ": " + ServerHubUI.Instance.GetFormattedDataSlotName(slotContent);
        }

        functionOutput.GetComponent<FunctionOutputUI>().SetOutput(output);
        base.ConfirmExecution();
        outputTitle.text = string.Empty;
        confirmButton.interactable = true;
        receivingMessage.gameObject.SetActive(false);
    }
}
