using System.Collections.Generic;
using UnityEngine;
using static DiscSlotContent;

[CreateAssetMenu(fileName = "NewDecipherExe", menuName = "ServerEXE/Decipher EXE")]
public class DecipherExe : ServerExe
{
    public DecipherType decipherType;

    public override List<DiscSlotContent> RelevantDataOnDisc(DataDrive drive)
    {
        List<DiscSlotContent> relevantData = new();
        List<DiscSlotContent> slots = ServerHubUI.Instance.pdStorage.receivedPDs.Contains(drive) ? drive.receivedSlots : drive.slots;

        foreach (DiscSlotContent data in slots)
        {
            if (data != null)
            {
                if (specificDataOnly && applicableData.Contains(data) && !ServerHubUI.Instance.IsContentDecrypted(data)) relevantData.Add(data);
                else if (!specificDataOnly && applicableDataTypes.Contains(data.GetSimpleType())) relevantData.Add(data);
            }
        }

        return relevantData;
    }

    public override bool DataIsRelevantToFunction(DiscSlotContent data)
    {
        if (data != null)
        {
            if (specificDataOnly && applicableData.Contains(data) && !ServerHubUI.Instance.IsContentDecrypted(data)) return true;
            else if (!specificDataOnly && applicableDataTypes.Contains(data.GetSimpleType())) return true;
        }

        return false;
    }
    public override void Perform(DiscSlotContent content)
    {
        throw new System.NotImplementedException();
    }
}
