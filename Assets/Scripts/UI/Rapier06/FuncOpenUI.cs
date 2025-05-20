using UnityEngine;

public class FuncOpenUI : FuncCardUI
{
    protected override void ConfirmExecution()
    {
        string output = "NOT CURRENTLY SUPPORTED";
        if (selectedContent is TextContent t) output = t.content;

        functionOutput.GetComponent<FunctionOutputUI>().SetOutput(output);
        base.ConfirmExecution();
    }
}
