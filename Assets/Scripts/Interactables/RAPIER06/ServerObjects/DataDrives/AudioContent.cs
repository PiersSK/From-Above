using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioContent", menuName = "Disc Slot Content/Audio")]
public class AudioContent : DiscSlotContent
{
    public AudioClip content;

    private const string AUDIOICON = "DataIcons/AudioIconBW";
    private const string AUDIOTYPE = "Audio Data";
    public override Sprite GetIcon()
    {
        return Resources.Load<Sprite>(AUDIOICON);
    }

    public override string GetDisplayType()
    {
        return AUDIOTYPE;
    }

    public override DataTypeSimple GetSimpleType()
    {
        return DataTypeSimple.Audio;
    }
}
