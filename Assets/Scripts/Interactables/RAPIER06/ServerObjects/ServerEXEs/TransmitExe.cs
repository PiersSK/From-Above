using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTransmitExe", menuName = "ServerEXE/Transmit EXE")]
public class TransmitExe : ServerExe
{
    public override bool DataIsRelevantToFunction(DiscSlotContent data)
    {
        if (data != null)
        {
            if (specificDataOnly && applicableData.Contains(data)) return true;
            else if (!specificDataOnly && applicableDataTypes.Contains(data.GetSimpleType())) return true;
        }

        return false;
    }

    public override List<DiscSlotContent> RelevantDataOnDisc(DataDrive drive)
    {
        List<DiscSlotContent> relevantData = new();
        List<DiscSlotContent> slots = ServerHubUI.Instance.pdStorage.GetPDSlots(drive);

        foreach (DiscSlotContent data in slots)
        {
            if (data != null)
            {
                if (specificDataOnly && applicableData.Contains(data)) relevantData.Add(data);
                else if (!specificDataOnly && applicableDataTypes.Contains(data.GetSimpleType())) relevantData.Add(data);
            }
        }

        return relevantData;
    }
    public override void Perform(DiscSlotContent content)
    {
        throw new System.NotImplementedException();
    }
}
