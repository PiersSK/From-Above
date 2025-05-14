using UnityEngine;

public abstract class DiscSlotContent : ScriptableObject
{
    public string displayName;
    public enum DecipherType
    {
        None,
        Alpha,
        Beta,
        Gamma
    }
    public DecipherType decipherType;

    public enum DataTypeSimple
    {
        Text,
        Audio,
        Video,
        Local,
        Remote
    }

    public abstract Sprite GetIcon();
    public abstract string GetDisplayType();
    public abstract DataTypeSimple GetSimpleType();
}
