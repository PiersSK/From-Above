using UnityEngine;
using UnityEngine.UI;

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

    public abstract Sprite GetIcon();
    public abstract string GetDisplayType();
}
