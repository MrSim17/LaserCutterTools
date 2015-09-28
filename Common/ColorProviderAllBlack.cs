using System.Drawing;

namespace ColorProvider
{
    public sealed class ColorProviderAllBlack : IColorProvider
    {
        public Color GetColor()
        {
            return Color.Black;
        }
    }
}
