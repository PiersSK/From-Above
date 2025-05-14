using UnityEngine;

[CreateAssetMenu(fileName = "NewTextContent", menuName = "Disc Slot Content/Text")]
public class TextContent : DiscSlotContent
{
    [TextArea(15, 20)]
    public string content;

    private const string TEXTICON = "DataIcons/TextIconBW";
    private const string TEXTTYPE = "Text Data";
    public override Sprite GetIcon()
    {
        return Resources.Load<Sprite>(TEXTICON);
    }

    public override string GetDisplayType()
    {
        return TEXTTYPE;
    }

    public override DataTypeSimple GetSimpleType()
    {
        return DataTypeSimple.Text;
    }
}