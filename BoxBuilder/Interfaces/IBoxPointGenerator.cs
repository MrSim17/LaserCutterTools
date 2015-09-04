using System.Collections.Generic;

namespace BoxBuilder
{
    internal interface IBoxPointGenerator
    {
        Dictionary<CubeSide, List<Point>> GeneratePoints(StartPositionConfiguration StartConfig, 
            IBoxSquare Box, 
            IMaterial Material, 
            IMachineSettings MachineSettings, 
            int TabsX,
            int TabsY, 
            int TabsZ, 
            bool MakeBoxOpen);

        Dictionary<CubeSide, List<Point>> GeneratePoints(StartPositionConfiguration StartConfig,
            IBoxSquare Box,
            IMaterial Material,
            IMachineSettings MachineSettings,
            int TabsX,
            int TabsY,
            int TabsZ,
            decimal SlotDepth,
            int SlotCount,
            decimal SlotAngle,
            SlotDirection SlotDirection);
    }
}