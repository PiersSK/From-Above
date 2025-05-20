using System.Collections.Generic;
using UnityEngine;
using static DiscSlotContent;

[CreateAssetMenu(fileName = "ServerExe", menuName = "Scriptable Objects/ServerExe")]
public abstract class ServerExe : IServerDataObject
{
    public string fileName;
    [TextArea(5, 10)]
    public string terminalPreview;
    public Sprite functionImage;
    [TextArea(5, 10)]
    public string functionFlavourText;

    public bool specificDataOnly = false;
    public bool fullPDOperation = false;

    public List<DataTypeSimple> applicableDataTypes = new();
    public List<DiscSlotContent> applicableData = new();

    public bool specificPdsOnly = false;
    public List<DataDrive> applicablePds = new();

    public abstract void Perform(DiscSlotContent content);

    public bool PDIsRelevantToFunction(DataDrive pd)
    {
        if (!fullPDOperation) return false;
        if (fullPDOperation && !specificPdsOnly) return true;
        if (fullPDOperation && specificPdsOnly && applicablePds.Contains(pd)) return true;

        return false;
    }

    public virtual bool DataIsRelevantToFunction(DiscSlotContent data)
    {
        if (data != null)
        {
            if (specificDataOnly && applicableData.Contains(data)) return true;
            else if (!specificDataOnly && applicableDataTypes.Contains(data.GetSimpleType())) return true;
        }

        return false;
    }

    public virtual List<DiscSlotContent> RelevantDataOnDisc(DataDrive drive)
    {
        List<DiscSlotContent> relevantData = new();

        foreach (DiscSlotContent data in drive.slots)
        {
            if (data != null)
            {
                if (specificDataOnly && applicableData.Contains(data)) relevantData.Add(data);
                else if (!specificDataOnly && applicableDataTypes.Contains(data.GetSimpleType())) relevantData.Add(data);
            }
        }

        return relevantData;
    }

    public bool IsApplicablePD(DataDrive drive)
    {
        return fullPDOperation && (!specificPdsOnly || applicablePds.Contains(drive));
    }

}
