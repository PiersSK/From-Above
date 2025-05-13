using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "NewVideoContent", menuName = "Disc Slot Content/Video")]
public class VideoContent : DiscSlotContent
{
    public VideoClip content;

    private const string VIDEOICON = "DataIcons/VideoIconBW";
    private const string VIDEOTYPE = "Video Data";
    public override Sprite GetIcon()
    {
        return Resources.Load<Sprite>(VIDEOICON);
    }

    public override string GetDisplayType()
    {
        return VIDEOTYPE;
    }
}
