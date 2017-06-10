using System;
using System.Collections.Generic;
using System.Linq;
using LaserCutterTools.Common;

namespace LaserCutterTools.GearBuilder
{
    // TODO: add interface for rack generation
    internal sealed class PointGeneratorRack : IPointGeneratorRack
    {
        // TODO: Account for tool width

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NumTeeth">Number of teeth on the rack. Determines the length of the rack.</param>
        /// <param name="PressureAngle"></param>
        /// <param name="circularPitch">Circumference of the pitch circle divided by the number of teeth.</param>
        /// <param name="Backlash">Minimal distance between meshing gears.</param>
        /// <param name="Clearance">Minimal distance between the apex of a tooth and the trough of the other gear.</param>
        /// <param name="Addendum"></param>
        /// <param name="SupportBarWidth">Thickness of the material attached to the rack teeth.</param>
        /// <param name="SlotDepth">Depth of the slot.</param>
        /// <param name="MaterialThickness">Thickness of the material used to make this part.</param>
        /// <param name="ToolSpacing">Width of the tool used to cut this part.</param>
        /// <returns></returns>
        public List<PointDouble> CreateRackWithSlots(IMaterial Material, IMachineSettings MachineSettings, int NumTeeth, double PressureAngle, double circularPitch, double Backlash, double Clearance, double Addendum, double SupportBarWidth, double SlotDepth)
        {
            var tmpRack = CreateRack(NumTeeth, PressureAngle, circularPitch, Backlash, Clearance, Addendum, SupportBarWidth);

            // add the slots
            var dimension = HelperMethods.GetPolygonDimension(tmpRack);

            // add slot one .5 inches from the bottom
            var slotOne = HelperMethods.TranslatePolygon(0, dimension.Y - 0.5, CreateSlot(SlotDepth, (double)MachineSettings.ToolSpacing, (double) Material.MaterialThickness));

            tmpRack.InsertRange(tmpRack.Count - 1, slotOne);

            // add slot two .5 inches from the top
            var slotTwo = HelperMethods.TranslatePolygon(0, 0.5, CreateSlot(SlotDepth, (double)MachineSettings.ToolSpacing, (double)Material.MaterialThickness));

            tmpRack.InsertRange(tmpRack.Count - 1, slotTwo);

            return tmpRack;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NumTeeth">Number of teeth on the rack. Determines the length of the rack.</param>
        /// <param name="PressureAngle"></param>
        /// <param name="circularPitch">Circumference of the pitch circle divided by the number of teeth.</param>
        /// <param name="Backlash">Minimal distance between meshing gears.</param>
        /// <param name="Clearance">Minimal distance between the apex of a tooth and the trough of the other gear.</param>
        /// <param name="Addendum"></param>
        /// <param name="SupportBarWidth">Thickness of the material attached to the rack teeth.</param>
        /// <returns></returns>
        public List<PointDouble> CreateRack(int NumTeeth, double PressureAngle, double circularPitch, double Backlash, double Clearance, double Addendum, double SupportBarWidth)
        {
            List<PointDouble> rack = new List<PointDouble>();

            var protoTooth = createRackTooth(PressureAngle, circularPitch, Clearance, Backlash, Addendum);

            // we draw one tooth in the middle and then five on either side
            for (var i = 0; i < NumTeeth; i++)
            {
                // TODO: Get rid of this drawing on both sides of the midpoint
                var tooth = HelperMethods.TranslatePolygon(0, (0.5 + -NumTeeth / 2 + i) * circularPitch, protoTooth);
                rack.AddRange(tooth);
            }

            // creating the bar backing the teeth
            var rightX = -(Addendum + Clearance);
            var width = 4 * Addendum;
            var halfHeight = NumTeeth * circularPitch / 2;

            // Create the supporting bar to hold the teeth
            var firstPoint = rack.First();
            var lastPoint = rack.Last();

            rack.Add(new PointDouble(lastPoint.X - SupportBarWidth, lastPoint.Y));
            rack.Add(new PointDouble(firstPoint.X - SupportBarWidth, firstPoint.Y));

            // move part to quadrant 1
            // TODO: This may be unnecessary if the above loop is fixed. A bit of amasking of strange calculations above.
            return HelperMethods.MovePolygonToQuadrantOne(rack);
        }

        private static List<PointDouble> CreateSlot(double SlotDepth, double ToolSpacing, double MaterialThickness)
        {
            // NOTE: Adjusting for tool spacing on the slot but nowhere else
            var slot = new List<PointDouble>
            {
                new PointDouble(0, MaterialThickness - ToolSpacing),
                new PointDouble(SlotDepth -ToolSpacing, MaterialThickness - ToolSpacing),
                new PointDouble(SlotDepth - ToolSpacing, 0),
                new PointDouble(0, 0)
            };

            return slot;
        }

        private static List<PointDouble> createRackTooth(double PressureAngle, double CircularPitch, double Clearance, double Backlash, double Addendum)
        {
            var toothWidth = CircularPitch / 2;
            var toothDepth = Addendum + Clearance;

            var sinPressureAngle = Math.Sin(PressureAngle * Math.PI / 180);
            var cosPressureAngle = Math.Cos(PressureAngle * Math.PI / 180);

            // if a positive backlash is defined then we widen the trapezoid accordingly.
            // Each side of the tooth needs to widened by a fourth of the backlash (vertical to cutter faces).
            var dx = Backlash / 4 / cosPressureAngle;

            var leftDepth = Addendum + Clearance;
            var upperLeftCorner = new PointDouble(-leftDepth, toothWidth / 2 - dx + (Addendum + Clearance) * sinPressureAngle);
            var upperRightCorner = new PointDouble(Addendum, toothWidth / 2 - dx - Addendum * sinPressureAngle);
            var lowerRightCorner = new PointDouble(upperRightCorner.X, -upperRightCorner.Y);
            var lowerLeftCorner = new PointDouble(upperLeftCorner.X, -upperLeftCorner.Y);

            //return new List<PointDouble>() { upperLeftCorner, upperRightCorner, lowerRightCorner, lowerLeftCorner };
            return new List<PointDouble>() { lowerLeftCorner, lowerRightCorner, upperRightCorner, upperLeftCorner };
        }
    }
}