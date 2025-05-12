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
}
