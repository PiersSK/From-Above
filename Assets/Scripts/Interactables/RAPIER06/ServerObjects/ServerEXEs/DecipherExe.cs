using UnityEngine;
using static DiscSlotContent;

[CreateAssetMenu(fileName = "NewDecipherExe", menuName = "ServerEXE/Decipher EXE")]
public class DecipherExe : ServerExe
{
    public DecipherType decipherType;
    public override void Perform(DiscSlotContent content)
    {
        throw new System.NotImplementedException();
    }
}
