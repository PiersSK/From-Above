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
}
