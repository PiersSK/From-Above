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

    private const string LOCALICON = "DataIcons/LocalIconBW";
    private const string LOCALTYPE = "Local Data";
    public override Sprite GetIcon()
    {
        return Resources.Load<Sprite>(LOCALICON);
    }

    public override string GetDisplayType()
    {
        return LOCALTYPE;
    }

    public override DataTypeSimple GetSimpleType()
    {
        return DataTypeSimple.Local;
    }
}
