using System.Collections.Generic;
using LaserCutterTools.Common;

namespace LaserCutterTools.GearBuilder
{
    internal sealed class PointGeneratorCrossBeam : IPointGeneratorCrossBeam
    {
        public List<PointDouble> CreateCrossBeam(IMaterial MaterialSettings, IMachineSettings MachineSettings, double HolderThickness, double ContentsDepth)
        {
            double contentsWidth = (double)MaterialSettings.MaterialThickness * 3;

            var holderPart = new List<PointDouble>
            {
                new PointDouble(0, (HolderThickness + ContentsDepth + (double)MachineSettings.ToolSpacing)),
                new PointDouble(HolderThickness + (double)MachineSettings.ToolSpacing, HolderThickness + ContentsDepth + (double)MachineSettings.ToolSpacing),
                new PointDouble(HolderThickness + (double)MachineSettings.ToolSpacing, HolderThickness + (double)MachineSettings.ToolSpacing),
                new PointDouble(HolderThickness + contentsWidth, HolderThickness + (double)MachineSettings.ToolSpacing),
                new PointDouble(HolderThickness + contentsWidth, HolderThickness + ContentsDepth + (double)MachineSettings.ToolSpacing),
                new PointDouble(HolderThickness + contentsWidth + HolderThickness + (double)MachineSettings.ToolSpacing, HolderThickness + ContentsDepth + (double)MachineSettings.ToolSpacing),
                new PointDouble(HolderThickness + contentsWidth + HolderThickness + (double)MachineSettings.ToolSpacing, 0),
                new PointDouble(0, 0),
            };

            return holderPart;
        }
    }
}
