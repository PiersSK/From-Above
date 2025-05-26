
using System.Linq;
using UnityEngine;

public class FuncDecipherUI : FuncCardUI
{
    [SerializeField] private Animator anim;

    private const string CONFIRMMESSAGE = "Data decryption successful!";
    private const string CONFIRMMESSAGE2 = "</b></color> is now available for operation\n\nPlease return to main menu for further operations";
    private const string DECRYPTANIM = "Decrypt";


    protected override void ConfirmExecution()
    {
        returnButton.interactable = false;
        prevButton.interactable = false;
        nextButton.interactable = false;
        confirmButton.interactable = false;
        outputTitle.gameObject.SetActive(false);
        outputTitle.text = selectedContent.displayName;
        outputFileIcon.sprite = selectedContent.GetIcon();
        ServerHubUI.Instance.LogContentAsDecrypted(selectedContent);
        ServerHubUI.Instance.SetServerHubAnimationLock(true);

        anim.SetTrigger(DECRYPTANIM);

        float animLength = anim.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name == DECRYPTANIM.ToLower()).length;
        Invoke("GoToOutput", animLength);
    }

    private void GoToOutput()
    {
        string output = CONFIRMMESSAGE + "\n\n\n<b><color=white>" + selectedContent.displayName + CONFIRMMESSAGE2;

        functionOutput.GetComponent<FunctionOutputUI>().SetOutput(output);
        base.ConfirmExecution();

        prevButton.interactable = true;
        nextButton.interactable = true;
        confirmButton.interactable = true;
        outputTitle.text = "???";
        ServerHubUI.Instance.SetServerHubAnimationLock(false);
    }
}
