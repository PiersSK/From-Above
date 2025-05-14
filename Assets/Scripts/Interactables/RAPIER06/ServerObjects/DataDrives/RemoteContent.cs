using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRemoteLocationContent", menuName = "Disc Slot Content/Remote Location")]
public class RemoteContent : DiscSlotContent
{
    public enum RemoteLocations
    {
        FDTCentralCommand,
        Rapier01,
        Rapier02,
        Rapier03,
        Rapier04,
        Rapier05,
        TSUCity,
        FDTCity
    }

    public RemoteLocations content;

    public List<DiscSlotContent> remoteContent = new();

    private const string REMOTEICON = "DataIcons/RemoteIconBW";
    private const string REMOTETYPE = "Remote Data";
    public override Sprite GetIcon()
    {
        return Resources.Load<Sprite>(REMOTEICON);
    }

    public override string GetDisplayType()
    {
        return REMOTETYPE;
    }

    public override DataTypeSimple GetSimpleType()
    {
        return DataTypeSimple.Remote;
    }
}
