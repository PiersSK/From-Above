using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataDrive", menuName = "Scriptable Objects/DataDrive")]
public class DataDrive : IServerDataObject
{
    [Header("Disc Slots")]
    public List<DiscSlotContent> slots = new(new DiscSlotContent[5]);
    public bool revertable = false;
    public List<DiscSlotContent> revertSlots = new();
    public List<DiscSlotContent> receivedSlots = new();

    [Header("LEGACY FIELDS")]
    [TextArea(15,20)]
    public string textContent;
    public AudioClip audioContent;
}
