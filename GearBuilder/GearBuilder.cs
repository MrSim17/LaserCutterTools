using System;
using System.Collections.Generic;
using LaserCutterTools.Common;

namespace GearBuilder
{
    public class GearBuilder
    {
        private static PointDouble polarToLinear(PolarPointDouble p)
        {
            double r = p.R;
            double a = p.A;
            double x;
            double y;

            double AM = 180 / Math.PI;
            a = ((a + 360) % 360) / AM;

            x = Math.Cos(a) * r;
            y = -Math.Sin(a) * r;

            return new PointDouble(x, y);
        }

        private PolarPointDouble linearToPolar(PointDouble c)
        {
            double x = c.X;
            double y = c.Y;
            double r;
            double a;

            double AM = 180 / Math.PI;

            r = Math.Sqrt(x * x + y * y);
            a = Math.Asin(y / r) * AM;

            if (x < 0)
            {
                a = 180 - a;
            }

            a = (a + 360) % 360;

            return new PolarPointDouble(r, a);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NumTeeth"></param>
        /// <param name="PitchDiameter">Circle centered on and perpendicular to the axis, and passing through the pitch point. A predefined diametral position on the gear where the circular tooth thickness, pressure angle and helix angles are defined.</param>
        /// <param name="DiametralPitch">Ratio of the number of teeth to the pitch diameter</param>
        /// <param name="PressureAngle">The complement of the angle between the direction that the teeth exert force on each other, and the line joining the centers of the two gears. For involute gears, the teeth always exert force along the line of action, which, for involute gears, is a straight line; and thus, for involute gears, the pressure angle is constant.</param>
        /// <returns></returns>
        public Dictionary<string, List<Point>> createGear(double NumTeeth, double PitchDiameter, double DiametralPitch, double PressureAngle)
        {
            // Reference: http://westmichiganspline.com/theory/

            double scale = 100;
            double outerCircleDiameter = (NumTeeth + 2) / DiametralPitch;
            double RD = (NumTeeth - 2.3) / DiametralPitch;
            // base circle is used to draw the involute
            //base diameter = pitch diameter x the cosine of the pressure angle
            double baseCircleDiameter = PitchDiameter * Math.Cos(PressureAngle * (Math.PI / 180));
            double CP = Math.PI / DiametralPitch;
            double rmin = RD / 2;
            double rmax = outerCircleDiameter / 2;
            double baseCircleRadius = baseCircleDiameter / 2;
            var points = new List<PolarPointDouble>();
            double angleCurrent = 0;
            double w = Math.Ceiling(outerCircleDiameter / 2 * scale) * 2;
            double h = w;

            // calc
            points.Add(new PolarPointDouble(rmin, 0));

            int pointNum = 0;
            double step = 1;
            bool first = true;

            for (double i = 1; i < 100; i += step)
            {
                // get a point...
                var bpl = polarToLinear(new PolarPointDouble(baseCircleRadius, -i));        //base point linear
                // The length is the arc around the circle unwound (2*PI*R) divided into 360 parts then multiplied times the step we made
                var len = ((baseCircleRadius * Math.PI * 2) / 360) * i;                //length
                var opl = polarToLinear(new PolarPointDouble(len, -i + 90));      //add line
                var np = linearToPolar(new PointDouble(bpl.X + opl.X, bpl.Y + opl.Y));

                if (np.R >= rmin)
                {
                    if (first)
                    {
                        first = false;
                        step = (2 / NumTeeth) * 10;
                    }

                    if (np.R < PitchDiameter / 2)
                    {
                        angleCurrent = np.A;
                    }

                    pointNum++;

                    if (np.R > rmax)
                    {
                        np.R = rmax;
                        points.Add(np);
                    }
                    else
                    {
                        points.Add(np);
                    }
                }
            }

            // tukrozes
            double fa = 360 / NumTeeth;                       // final a
            double ma = fa / 2 + 2 * angleCurrent;               // mirror a
            double fpa = (fa - ma) > 0 ? 0 : -(fa - ma) / 2;   // first point a
            int m = points.Count;
            points[0] = new PolarPointDouble(rmin, fpa);           // fix first point a

            while (points[m - 1].A > ma / 2)
            {
                points.RemoveAt(m - 1);
                m--;
                pointNum--;
            }

            // mirror the first set of involute generated points
            for (int i = pointNum; i >= 0; i--)
            {
                var pointToMirror = points[i];
                var newAngle = ma - pointToMirror.A;
                points.Add(new PolarPointDouble(pointToMirror.R, newAngle));
            }

            // Copy the first tooth around the gear
            m = points.Count;

            for (int i = 1; i < NumTeeth; i++)
            {
                for (var p = 0; p < m; p++)
                {
                    var bp = points[p];
                    var na = bp.A + fa * i;
                    points.Add(new PolarPointDouble(bp.R, na));
                }
            }

            // Convert the points from doubles to decimals for rendering
            var gearPoly = ConvertDoubleToDecimal(ConvertPolarPointsToLinear(points));
            var outerCirclePoly = ConvertDoubleToDecimal(DrawCircle(rmax, new PointDouble(0, 0), 1000));
            var rMinCircle = ConvertDoubleToDecimal(DrawCircle(rmin, new PointDouble(0, 0), 1000));
            var baseCircle = ConvertDoubleToDecimal(DrawCircle(baseCircleRadius, new PointDouble(0, 0), 1000));
            var pitchDiameterCircle = ConvertDoubleToDecimal(DrawCircle(PitchDiameter/2, new PointDouble(0, 0), 1000));

            var ret = new Dictionary<string, List<Point>>();
            ret.Add("Gear", gearPoly);
            ret.Add("OuterCircle", outerCirclePoly);
            ret.Add("RMin", rMinCircle);
            ret.Add("BaseCircle", baseCircle);
            ret.Add("PitchDiameterCircle", pitchDiameterCircle);

            return ret;
        }

        private static List<PointDouble> DrawCircle(double Radius, PointDouble Center, int PointCount)
        {
            List<PolarPointDouble> points = new List<PolarPointDouble>();

            for(int i = 0; i < PointCount; i++)
            {
                points.Add(new PolarPointDouble(Radius, (360/(double)PointCount) * i));
            }

            return ConvertPolarPointsToLinear(points);
        }

        private static List<PointDouble> ConvertPolarPointsToLinear(List<PolarPointDouble> Points)
        {
            var output = new List<PointDouble>();

            foreach (var p in Points)
            {
                var linearPoint = polarToLinear(p);
                output.Add(new PointDouble(linearPoint.X, linearPoint.Y));
            }

            return output;
        }

        private static List<Point> ConvertDoubleToDecimal(List<PointDouble> Points)
        {
            var output = new List<Point>();

            foreach (var p in Points)
            {
                output.Add(new Point((decimal)p.X, (decimal)p.Y));
            }

            return output;
        }
    }
}
