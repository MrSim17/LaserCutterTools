using System.Collections.Generic;
using System.Drawing;

namespace ColorProvider
{
    public sealed class ColorProviderAllDifferent : IColorProvider
    {
        List<Color> colorPool;
        int nextColorIndex = 0;

        public ColorProviderAllDifferent()
        {
            colorPool = new List<Color>
            {
                Color.Black,
                Color.Red,
                Color.Blue,
                Color.Green,
                //Color.Yellow, Yellow ends up being too light
                Color.Purple,
                Color.Pink,
                Color.Brown
            };
        }

        public Color GetColor()
        {
            while (nextColorIndex + 1 > colorPool.Count)
            {
                nextColorIndex -= colorPool.Count;
            }

            return colorPool[nextColorIndex++];
        }
    }
}
