using ColorProvider;
using Common;

namespace BoxBuilder
{
    public sealed class BoxBuilder
    {
        // TODO: Deal with auto tabbing.
        //public static IBoxHandlerSquare GetBoxHandler(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings)
        //{
        //    return new BoxHandlerSquareAutoTab
        //    {
        //        ColorProvider = new ColorProviderAllDifferent(),
        //        Logger = new NullLogger()
        //    };
        //}

        internal static IBoxPointGenerator GetBoxPointGenerator(ILogger Logger)
        {
            IPiecePointGenerator piecePointGen = new PiecePointGenerator();
            IBoxPointGenerator pointGen = new BoxPointGenerator(piecePointGen, Logger);

            return pointGen;
        }

        public static IBoxHandlerSVG GetBoxHandler(int TabsX, int TabsY, int TabsZ, ILogger Logger)
        {
            IColorProvider colorProvider = new ColorProviderAllDifferent();
            IBoxPointRendererSVG pointRender = new BoxPointRendererSVG(colorProvider);
            IBoxPointGenerator pointGen = GetBoxPointGenerator(Logger);

            IBoxHandlerSVG handler = new BoxHandlerSVG(pointGen,
                pointRender,
                TabsX,
                TabsY,
                TabsZ,
                Logger);

            return handler;
        }
    }
}