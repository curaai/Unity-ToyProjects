using UnityEngine;

public class RainbowGradient
{
    private Gradient g;
    public RainbowGradient()
    {
        Color ToColor(string colorCode)
        {
            Color color;
            if (!colorCode.StartsWith("#"))
                colorCode = $"#{colorCode}";
            ColorUtility.TryParseHtmlString(colorCode, out color);
            return color;
        }

        g = new();
        g.colorKeys = new GradientColorKey[7]
        {
            new(ToColor("#E81416"), 0/6f),
            new(ToColor("#FFA500"), 1/6f),
            new(ToColor("#FAEB36"), 2/6f),
            new(ToColor("#79C314"), 3/6f),
            new(ToColor("#487DE7"), 4/6f),
            new(ToColor("#4B369D"), 5/6f),
            new(ToColor("#70369D"), 6/6f),
        };
    }

    public Color Get(float x) => g.Evaluate(x);
}