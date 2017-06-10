using System.Collections.Generic;
using LaserCutterTools.Common;

namespace LaserCutterTools.GearBuilder
{
    public interface IPointGeneratorGuard
    {
        List<PointDouble> CreateGuardWithSlots(double DimensionX, double DimensionY, double SlotDepth, double ToolSpacing, double MaterialThickness);
        List<PointDouble> CreateGuard(double DimensionX, double DimensionY);
    }
}