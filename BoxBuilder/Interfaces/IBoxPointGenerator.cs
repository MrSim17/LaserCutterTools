using System.Collections.Generic;
using System.Drawing;


namespace BoxBuilder
{
    internal interface IBoxPointGenerator
    {
        Dictionary<CubeSide, List<Point>> GeneratePoints(StartPositionConfiguration StartConfig, IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings, int TabsX, int TabsY, int TabsZ, bool MakeBoxOpen);
    }
}