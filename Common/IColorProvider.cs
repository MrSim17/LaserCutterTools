using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ColorProvider
{
    public interface IColorProvider
    {
        Color GetColor();
    }

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
            
            foreach(var prop in props)
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

    public sealed class ColorProviderAllBlack : IColorProvider
    {
        public Color GetColor()
        {
            return Color.Black;
        }
    }

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
                //Color.Yellow,
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
