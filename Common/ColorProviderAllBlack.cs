using System.Drawing;

namespace LaserCutterTools.Common.ColorMgmt
{
    public sealed class ColorProviderAllBlack : IColorProvider
    {
        public Color GetColor()
        {
            return Color.Black;
        }
    }
}
