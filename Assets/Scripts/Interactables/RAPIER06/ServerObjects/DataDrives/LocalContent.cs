using UnityEngine;

[CreateAssetMenu(fileName = "NewLocalLocationContent", menuName = "Disc Slot Content/Local Location")]
public class LocalContent : DiscSlotContent
{
    public enum LocalLocations
    {
        Bridge,
        LivingQuartersOne,
        LivingQuartersTwo,
        Health,
        Utility,
        ServerRoom,
        DoomsdayRoom,
        EngineRoom
    }

    public LocalLocations content;
}
