using System.Collections.Generic;

namespace BoxBuilder
{
    internal interface IPointGeneratorDivider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Material">Should be the material settings for the box you are going to place the divider in. This will
        /// allow the tabs to fit into the box and be flush on the outside.</param>
        /// <param name="MachineSettings"></param>
        /// <param name="DimensionX"></param>
        /// <param name="DimensionY"></param>
        /// <param name="SlotDepth"></param>
        /// <param name="SlotAngle"></param>
        /// <returns></returns>
        List<Point> GeneratePoints(IMaterial Material, IMachineSettings MachineSettings, decimal Width, decimal Height, decimal SlotDepth, decimal SlotAngle);
    }
}
