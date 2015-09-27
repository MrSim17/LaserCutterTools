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

        public static IBoxBuilderSVG GetBoxHandler(ILogger Logger)
        {
            IColorProvider colorProvider = new ColorProviderAllDifferent();
            IBoxPointRendererSVG pointRender = new BoxPointRendererSVG(colorProvider, true);
            IBoxPointGenerator pointGen = GetBoxPointGenerator(Logger);
            IDividerPointGenerator dividerGenerator = new DividerPointGeneartor();

            IBoxBuilderSVG handler = new BoxBuilderSVG(pointGen,
                pointRender,
                dividerGenerator,
                Logger);

            return handler;
        }
    }
}