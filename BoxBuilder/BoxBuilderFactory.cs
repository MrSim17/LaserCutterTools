using ColorProvider;
using Common;
using LaserCutterTools.Common.Rendering;

namespace BoxBuilder
{
    public sealed class BoxBuilderFactory
    {
        internal static IPointGeneratorBox GetBoxPointGenerator(ILogger Logger)
        {
            IPointGeneratorPiece piecePointGen = new PointGeneratorPiece();
            IPointGeneratorBox pointGen = new PointGeneratorBox(piecePointGen, Logger);

            return pointGen;
        }

        public static IBoxBuilderSVG GetBoxBuilder(ILogger Logger)
        {
            IColorProvider colorProvider = new ColorProviderAllDifferent();
            IPointRendererBoxSVG pointRender = new PointRendererBoxSVG(new PointRendererSVG(colorProvider));
            IPointGeneratorBox pointGen = GetBoxPointGenerator(Logger);
            IPointGeneratorDivider dividerGenerator = new PointGeneratorDivider();

            IBoxBuilderSVG handler = new BoxBuilderSVG(pointGen,
                pointRender,
                dividerGenerator,
                Logger);

            return handler;
        }
    }
}