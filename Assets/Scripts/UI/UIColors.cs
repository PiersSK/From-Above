using System.Collections.Generic;
using UnityEngine;

public static class UIColors
{
    public static Color white = new Color(1, 1, 1);
    public static Color grey = new Color(0.72f, 0.72f, 0.72f);
    public static Color darkGrey = new Color(0.4f, 0.4f, 0.4f);
    public static Color black = new Color(0, 0, 0);

    public static Color terminalRed = new Color(0.69f, 0.3f, 0.22f);
    public static Color terminalGreen = new Color(0.2f, 0.69f, 0.2f);
    public static Color terminalGreenTransparent = new Color(0.2f, 0.69f, 0.2f, 0.2f);

    public static Color terminalBlue = new Color(0.1f, 0.7f, 0.85f);

    public enum SubtitleColor
    {
        White,
        Grey,
        Yellow,
        Red,
        Cyan
    }
    public static List<Color> subtitleColors = new()
    {
        new Color(1, 1, 1), // White
        new Color(0.72f, 0.72f, 0.72f), // Grey
        new Color(1, 1, 0), // Yellow
        new Color(0.75f, 0.25f, 0.25f), // Red
        new Color(0.4f, 1, 1), // Cyan
    };
}
