using System.Drawing;

namespace LaserCutterTools.Common.ColorMgmt
{
    public sealed class ColorProviderAlternating : IColorProvider
    {
        Color CurrentColor;
        Color FirstColor;
        Color SecondColor;

        public ColorProviderAlternating(Color Color1, Color Color2)
        {
            FirstColor = Color1;
            SecondColor = Color2;
        }

        public ColorProviderAlternating()
        {
            FirstColor = Color.Red;
            SecondColor = Color.Blue;
        }

        public Color GetColor()
        {
            if (CurrentColor == FirstColor)
            {
                CurrentColor = SecondColor;
            }
            else
            {
                CurrentColor = FirstColor;
            }

            return CurrentColor;
        }
    }
}
