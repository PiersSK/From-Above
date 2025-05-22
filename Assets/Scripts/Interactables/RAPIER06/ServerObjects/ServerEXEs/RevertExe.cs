using UnityEngine;

[CreateAssetMenu(fileName = "NewRevertExe", menuName = "ServerEXE/Revert EXE")]
public class RevertExe : ServerExe
{
    public override bool PDIsRelevantToFunction(DataDrive pd)
    {
        if (!fullPDOperation) return false;
        if (fullPDOperation && !specificPdsOnly) return true;
        if (fullPDOperation && specificPdsOnly && applicablePds.Contains(pd)
            && !ServerHubUI.Instance.pdStorage.revertedPDs.Contains(pd)) return true;

        return false;
    }

    public override bool IsApplicablePD(DataDrive drive)
    {
        if (ServerHubUI.Instance.pdStorage.revertedPDs.Contains(drive)) return false;
        return base.IsApplicablePD(drive);
    }

    public override void Perform(DiscSlotContent content)
    {
        throw new System.NotImplementedException();
    }
}
