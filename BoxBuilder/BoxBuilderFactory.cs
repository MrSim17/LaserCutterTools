using ColorProvider;
using Common;

namespace BoxBuilder
{
    public sealed class BoxBuilderFactory
    {
        internal static IBoxPointGenerator GetBoxPointGenerator(ILogger Logger)
        {
            IPiecePointGenerator piecePointGen = new PiecePointGenerator();
            IBoxPointGenerator pointGen = new BoxPointGenerator(piecePointGen, Logger);

            return pointGen;
        }

        public static IBoxBuilderSVG GetBoxBuilder(ILogger Logger)
        {
            IColorProvider colorProvider = new ColorProviderAllDifferent();
            IBoxPointRendererSVG pointRender = new BoxPointRendererSVG(colorProvider);
            IBoxPointGenerator pointGen = GetBoxPointGenerator(Logger);
            IDividerPointGenerator dividerGenerator = new DividerPointGenerator();

            IBoxBuilderSVG handler = new BoxBuilderSVG(pointGen,
                pointRender,
                dividerGenerator,
                Logger);

            return handler;
        }
    }
}