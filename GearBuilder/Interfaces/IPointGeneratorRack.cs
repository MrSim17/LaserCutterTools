using System.Collections.Generic;
using LaserCutterTools.Common;

namespace LaserCutterTools.GearBuilder
{
    public interface IPointGeneratorRack
    {
        List<PointDouble> CreateRackWithSlots(IMaterial Material, IMachineSettings MachineSettings, int NumTeeth, double PressureAngle, double circularPitch, double Backlash, double Clearance, double Addendum, double SupportBarWidth, double SlotDepth);
        List<PointDouble> CreateRack(int NumTeeth, double PressureAngle, double circularPitch, double Backlash, double Clearance, double Addendum, double SupportBarWidth);
    }
}
