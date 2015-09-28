using System;
using System.Collections.Generic;
using System.Drawing;

namespace ColorProvider
{
    public sealed class ColorProviderRandom : IColorProvider
    {
        List<Color> colors = new List<Color>();
        Color lastColor = Color.Transparent;
        Random rand;

        public ColorProviderRandom()
        {
            rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            List<Color> excludedColors = new List<Color>
            {
                Color.Transparent,
                Color.White
            };

            var props = typeof(Color).GetProperties();

            foreach (var prop in props)
            {
                if (prop.PropertyType == typeof(Color))
                {
                    Color newColor = (Color)prop.GetValue(null, null);

                    if (!excludedColors.Contains(newColor))
                    {
                        colors.Add(newColor);
                    }
                }
            }
        }

        public Color GetColor()
        {
            int i = 0;
            Color tmpColor = lastColor;

            while (i < 100 && tmpColor == lastColor)
            {
                int idx = rand.Next(colors.Count);
                tmpColor = colors[idx];
            }

            return tmpColor;
        }
    }
}
