using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewReceiveExe", menuName = "ServerEXE/Receive EXE")]
public class ReceiveExe : ServerExe
{
    public override List<DiscSlotContent> RelevantDataOnDisc(DataDrive drive)
    {
        List<DiscSlotContent> relevantData = new();
        if (ServerHubUI.Instance.pdStorage.receivedPDs.Contains(drive)) return relevantData;

        return base.RelevantDataOnDisc(drive);
    }

    public override void Perform(DiscSlotContent content)
    {
        throw new System.NotImplementedException();
    }
}
