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

        public static string BuildBox(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings, int TabsX, int TabsY, int TabsZ, bool RotateParts, bool MakeBoxOpen, ILogger Logger)
        {
            IColorProvider colorProvider = new ColorProviderAllDifferent();
            IPiecePointGenerator piecePointGen = new PiecePointGenerator();
            IBoxPointGenerator pointGen = new BoxPointGenerator(piecePointGen);
            IBoxPointRenderer pointRender = new BoxPointRendererSVG(colorProvider);

            IBoxHandlerSquare handler = new BoxHandlerSquare(pointGen,
                pointRender,
                TabsX, 
                TabsY, 
                TabsZ, 
                Logger);

            return handler.HandleBox(Box, Material, MachineSettings, MakeBoxOpen, RotateParts);
        }
    }
}