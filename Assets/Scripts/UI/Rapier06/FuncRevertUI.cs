using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;

public class FuncRevertUI : FuncCardUI
{
    private const string REVERTMESSAGE = "</b></color> has been reverted to a locally found backup. This operation cannot be undone.";

    public override void OpenFuncUI(List<DiscSlotContent> validContent, DataDrive pd = null)
    {
        inputTitle.text = pd.objectName;
        relevantPd = pd;
        gameObject.SetActive(true);
    }

    protected override void ConfirmExecution()
    {
        ServerHubUI.Instance.pdStorage.revertedPDs.Add(relevantPd);

        functionOutput.GetComponent<FunctionOutputUI>().SetOutput("<b><color=white>" + relevantPd.objectName + REVERTMESSAGE);

        base.ConfirmExecution();
    }
}
