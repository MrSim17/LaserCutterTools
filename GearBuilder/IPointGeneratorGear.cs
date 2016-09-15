using System.Collections.Generic;
using LaserCutterTools.Common;

namespace GearBuilder
{
    public interface IPointGeneratorGear
    {
        Dictionary<string, List<Point>> createGear(double NumTeeth, double PitchDiameter, double DiametralPitch, double PressureAngle, bool Debug = false);
    }
}
