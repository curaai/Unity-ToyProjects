using UnityEngine;
using System.Collections.Generic;

public static class UnityUtil
{
    public static Color ToColor(this string colorCode)
    {
        Color color;
        if (!colorCode.StartsWith("#"))
            colorCode = $"#{colorCode}";
        ColorUtility.TryParseHtmlString(colorCode, out color);
        return color;
    }
}