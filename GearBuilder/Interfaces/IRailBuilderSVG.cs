using LaserCutterTools.Common;

namespace LaserCutterTools.GearBuilder
{
    public interface IRailBuilderSVG
    {
        string BildRail(IMaterial Material, IMachineSettings MachineSettings, double GearPitchDiameter, double GearPressureAngle, int GearNumTeeth, double GearDiametralPitch, double CrossBeamThickness, double CrossBeamContentsDepth, int RackNumTeeth, double supportBarWidth);
    }
}