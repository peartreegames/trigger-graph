using System;
using UnityEngine;

namespace PeartreeGames.TriggerGraph
{
    public enum PortColor
    {
        Black,
        White,
        Cyan,
        Red,
        Green,
        Blue,
        Magenta,
        Yellow,
    }

    public enum PortOrientation
    {
        Horizontal,
        Vertical
    }

    public static class PortColorExtensions
    {
        public static Color AsColor(this PortColor color) =>
            color switch
            {
                PortColor.Black => Color.black,
                PortColor.White => Color.white,
                PortColor.Cyan => Color.cyan,
                PortColor.Red => Color.red,
                PortColor.Green => Color.green,
                PortColor.Blue => Color.blue,
                PortColor.Magenta => Color.magenta,
                PortColor.Yellow => Color.yellow,
                _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
            };
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class OutputAttribute : PropertyAttribute
    {
        public PortOrientation Orientation;
        public PortColor Color;

        public OutputAttribute(PortOrientation orientation = PortOrientation.Horizontal, PortColor color = PortColor.Cyan)
        {
            Color = color;
            Orientation = orientation;
        }
    }
}