using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserCutterTools.Common;

namespace LaserCutterTools.GearBuilder
{
    public interface IPointGeneratorRack
    {
        List<PointDouble> CreateRackWithSlots(int NumTeeth, double PressureAngle, double circularPitch, double Backlash, double Clearance, double Addendum, double SupportBarWidth, double SlotDepth, double MaterialThickness, double ToolSpacing);
        List<PointDouble> CreateRack(int NumTeeth, double PressureAngle, double circularPitch, double Backlash, double Clearance, double Addendum, double SupportBarWidth);
    }
}
