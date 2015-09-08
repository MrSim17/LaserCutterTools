using System.Collections.Generic;

namespace BoxBuilder
{
    // TODO: It would be nice if the box point genertor supported an inside and outside box model like CSS does. Would make it easier to make boxes that fit inside of something as well as that have something fit inside of it.
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