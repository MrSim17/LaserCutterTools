using System.Collections.Generic;
using LaserCutterTools.Common;

namespace LaserCutterTools.GearBuilder
{
    public interface IPointGeneratorCrossBeam
    {
        List<PointDouble> CreateCrossBeam(IMaterial MaterialSettings, IMachineSettings MachineSettings, double HolderThickness, double ContentsDepth);
    }
}