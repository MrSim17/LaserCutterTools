using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserCutterTools.Common;

namespace LaserCutterTools.GearBuilder
{
    public sealed class PointGeneratorGuard
    {
        // TODO: Account for tool width

        public List<PointDouble> CreateGuardWithSlots(double DimensionX, double DimensionY, double SlotDepth, double ToolSpacing, double MaterialThickness)
        {
            var tmpGuard = CreateGuard(DimensionX, DimensionY);

            // add slot one .5 inches from the bottom
            var slotOne = HelperMethods.TranslatePolygon(0, 0.5, CreateSlot(SlotDepth, ToolSpacing, MaterialThickness));

            tmpGuard.AddRange(slotOne);

            // add slot two .5 inches from the top
            var slotTwo = HelperMethods.TranslatePolygon(0, DimensionY - 0.5, CreateSlot(SlotDepth, ToolSpacing, MaterialThickness));

            tmpGuard.AddRange(slotTwo);

            return tmpGuard;
        }

        public List<PointDouble> CreateGuard(double DimensionX, double DimensionY)
        {
            var cornerTopLeft = new PointDouble(0, DimensionY);
            var cornerTopRight = new PointDouble(DimensionX, DimensionY);
            var cornerBottomRight = new PointDouble(DimensionX, 0);
            var cornerBottomLeft = new PointDouble(0, 0);

            var guardPart = new List<PointDouble>();

            guardPart.Add(cornerTopLeft);
            guardPart.Add(cornerTopRight);
            guardPart.Add(cornerBottomRight);
            guardPart.Add(cornerBottomLeft);

            return guardPart;
        }

        private static List<PointDouble> CreateSlot(double SlotDepth, double ToolSpacing, double MaterialThickness)
        {
            // NOTE: Adjusting for tool spacing on the slot but nowhere else
            var slot = new List<PointDouble>
            {
                new PointDouble(0, 0),
                new PointDouble(SlotDepth - ToolSpacing, 0),
                new PointDouble(SlotDepth -ToolSpacing, MaterialThickness - ToolSpacing),
                new PointDouble(0, MaterialThickness - ToolSpacing)
            };

            return slot;
        }
    }
}
