using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserCutterTools.Common;

namespace GearBuilder
{
    public sealed class PointGeneratorRack
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PressureAngle">Common values are 14.5, 20 and 25 degrees.</param>
        /// <param name="circularPitch">The circumference of the pitch circle divided by the number of teeth.</param>
        /// <returns></returns>

        public List<PointDouble> createRackShape(int NumTeeth, double PressureAngle, double circularPitch, double Backlash, double Clearance, double Addendum, double SupportBarWidth)
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
