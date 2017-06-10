using LaserCutterTools.Common;

namespace LaserCutterTools.BoxBuilder
{
    public interface IBoxBuilderSVG
    {
        string BuildBox(IBoxSquare Box, 
            IMaterial Material, 
            IMachineSettings MachineSettings, 
            int TabsX, 
            int TabsY, 
            int TabsZ, 
            bool MakeBoxOpen = false, 
            bool RotateParts = false);

        string BuildBox(IBoxSquare Box, 
            IMaterial Material, 
            IMachineSettings MachineSettings, 
            int TabsX, 
            int TabsY, 
            int TabsZ, 
            decimal SlotDepth, 
            decimal slotPadding,
            int SlotCount, 
            decimal SlotAngle, 
            SlotDirection SlotDirection,
            bool RotateParts = false);
    }
}