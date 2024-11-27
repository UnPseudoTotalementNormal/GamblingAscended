using System;

namespace Enums
{
    [Flags]
    public enum LayerMaskEnum
    {
        Default = 1 << 0,
        TransparentFX = 1 << 1,
        IgnoreRaycast = 1 << 2,
        Water = 1 << 4,
        UI = 1 << 5,
        IgnorePlaceable = 1 << 6,
        Item = 1 << 7,
        Character = 1 << 8,
        Path = 1 << 9,
        All = ~0
    }

    public static class LayerMaskEnumExtensions
    {
        public static int GetLayer(this LayerMaskEnum enumValue)
        {
            return (int)(Math.Log((int)enumValue) / Math.Log(2));
        }
    }
}