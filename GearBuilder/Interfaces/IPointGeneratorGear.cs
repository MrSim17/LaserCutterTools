using System.Collections.Generic;
using LaserCutterTools.Common;

namespace LaserCutterTools.GearBuilder
{
    public interface IPointGeneratorGear
    {
        Dictionary<string, List<Point>> CreateGear(double NumTeeth, double PitchDiameter, double DiametralPitch, double PressureAngle, bool Debug = false);
    }
}
