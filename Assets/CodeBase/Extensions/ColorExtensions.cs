using UnityEngine;

namespace CodeBase.Extensions
{
    public static class ColorExtensions
    {
        public static string ToColor(this string text, Color color)
        {
            string colorHexText = ColorUtility.ToHtmlStringRGB(color);
            return text.ToColor(colorHexText);
        }

        public static string ToColor(this string text, string colorHtmlText)
        {
#if UNITY_EDITOR
            text = $"<color=#{colorHtmlText}>{text}</color>";
#endif
            return text;
        }
    }
}