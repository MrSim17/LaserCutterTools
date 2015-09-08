namespace BoxBuilder
{
    public interface IBoxBuilderSVG
    {
        string BuildBox(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings, int TabsX, int TabsY, int TabsZ, bool MakeBoxOpen = false, bool RotateParts = false);
        // TODO: Box builder svg interface needs a parameter for the slot direction. Currently hardcoded internally.
        string BuildBox(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings, int TabsX, int TabsY, int TabsZ, decimal SlotDepth, int SlotCount, decimal SlotAngle, bool RotateParts = false);
    }
}