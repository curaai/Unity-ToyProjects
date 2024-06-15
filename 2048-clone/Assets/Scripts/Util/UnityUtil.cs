using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

    public static T PickRandom<T>(this IEnumerable<T> _e)
    {
        List<T> l;
        if (_e is List<T>)
            l = (List<T>)_e;
        else
            l = _e.ToList();

        return l[UnityEngine.Random.Range(0, l.Count)];
    }
}