using System.Collections.Generic;
using LaserCutterTools.Common;

namespace LaserCutterTools.BoxBuilder
{
    // TODO: It would be nice if the box point genertor supported an inside and outside box model like CSS does. Would make it easier to make boxes that fit inside of something as well as that have something fit inside of it.
    internal interface IPointGeneratorBox
    {
        /// <summary>
        /// This method is used to make a box that is open or closed. This cannot generate slots for dividers.
        /// </summary>
        /// <param name="StartConfig"></param>
        /// <param name="Box">Contains the dimensions of the box to be created.</param>
        /// <param name="Material"></param>
        /// <param name="MachineSettings"></param>
        /// <param name="TabsX">Number of tabs in the X direction.</param>
        /// <param name="TabsY">Number of tabs in the Y direction.</param>
        /// <param name="TabsZ">Number of tabs in the Z direction.</param>
        /// <param name="MakeBoxOpen">Determines if the top piece of the box is generated or not. 
        /// If this is true the top edges of the front, back, left, and right pieces will be flat.</param>
        /// <returns></returns>
        Dictionary<CubeSide, List<Point>> GeneratePoints(StartPositionConfiguration StartConfig, 
            IBoxSquare Box, 
            IMaterial Material, 
            IMachineSettings MachineSettings, 
            int TabsX,
            int TabsY, 
            int TabsZ, 
            bool MakeBoxOpen);

        /// <summary>
        /// generates a box that is always open. The open end contains slots to place dividers.
        /// </summary>
        /// <param name="StartConfig"></param>
        /// <param name="Box"></param>
        /// <param name="Material"></param>
        /// <param name="MachineSettings"></param>
        /// <param name="TabsX"></param>
        /// <param name="TabsY"></param>
        /// <param name="TabsZ"></param>
        /// <param name="SlotDepth"></param>
        /// <param name="SlotPadding">Extra space around the slot so that the divider can fit looser or tighter depending on configuration.</param>
        /// <param name="SlotCount">Number of slots available for dividers.</param>
        /// <param name="SlotAngle">Angle of the slots. This will cause dividers to be angled. 0 == vertical.</param>
        /// <param name="SlotDirection">Decides whether slots run down the X or Y direction of the box.</param>
        /// <returns></returns>
        Dictionary<CubeSide, List<Point>> GeneratePoints(StartPositionConfiguration StartConfig,
            IBoxSquare Box,
            IMaterial Material,
            IMachineSettings MachineSettings,
            int TabsX,
            int TabsY,
            int TabsZ,
            decimal SlotDepth,
            decimal SlotPadding,
            int SlotCount,
            decimal SlotAngle,
            SlotDirection SlotDirection);
    }
}