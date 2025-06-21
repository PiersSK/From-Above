using UnityEngine;

public class FuncOpenUI : FuncCardUI
{
    [SerializeField] private ShipDoorController doorController;

    protected override void ConfirmExecution()
    {
        string output = "NOT CURRENTLY SUPPORTED";
        if (selectedContent is TextContent t)
        {
            output = t.content;
        }
        else if (selectedContent is LocalContent l)
        {
            output = l.displayName + " is now opened and unlocked";
            doorController.RemoteUnlockAndOpen(l.content);
        }

        functionOutput.GetComponent<FunctionOutputUI>().SetOutput(output);
        base.ConfirmExecution();
    }
}
