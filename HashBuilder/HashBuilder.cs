using SvgNet.SvgElements;
using System.Drawing; // TODO: Get rid of useless SVG library. Generates unreliable results!
using ColorProvider;
using Common;

namespace HashBuilder
{
    // TODO: Separate out hash point generation from hash point rendering.
    public sealed class HashBuilder
    {
        IColorProvider colorProvider;
        ILogger logger;

        public ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        public HashBuilder()
        {
            colorProvider = new ColorProvider.ColorProviderAlternating();
            logger = new StringLogger();
        }

        public HashBuilder(IColorProvider ColorProvider)
        {
            colorProvider = ColorProvider;
            logger = new StringLogger();
        }

        public string BuildHash(float Width, float Height, float LineSpacing, float GapWidth, int HashCount)
        {
            Color currentColor = colorProvider.GetColor();
            float currentPosY = 0;
            SvgSvgElement root = new SvgSvgElement("20in", "12in", "0,0 20,12");

            var lines = HelperMethods.CreateHashLines(Width, Height, GapWidth, LineSpacing, HashCount, logger);

            logger.Log(string.Format("Starting First Line at y Position {0}", currentPosY));

            foreach (var line in lines)
            {
                if (line.Points[0].Y != currentPosY)
                {
                    logger.Log(string.Format("Detected Y change. Old: {0}, New: {1}", currentPosY, line.Points[0].Y));
                    Color newColor = colorProvider.GetColor();
                    logger.Log(string.Format("Changing color. Old: {0}, New: {1}", currentColor.Name, newColor.Name));
                    currentPosY = line.Points[0].Y;
                    currentColor = newColor;
                }

                SvgLineElement newLine = new SvgLineElement(line.Points[0].X, line.Points[0].Y, line.Points[1].X, line.Points[1].Y);
                newLine.Style = new SvgNet.SvgTypes.SvgStyle(new Pen(currentColor, 0.0034f));

                root.Children.Add(newLine);
            }

            return root.WriteSVGString(false);
        }
    }
}