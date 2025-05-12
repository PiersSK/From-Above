using UnityEngine;

[CreateAssetMenu(fileName = "NewTextContent", menuName = "Disc Slot Content/Text")]
public class TextContent : DiscSlotContent
{
    [TextArea(15, 20)]
    public string content;
}
