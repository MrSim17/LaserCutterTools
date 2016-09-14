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

        public List<Point> createGear(double NumTeeth, double Diameter, double Pitch, double PressureAngle)
        {
            double sc = 100;
            double A = 1 / Pitch;
            double B = 1.157 / Pitch;
            double OD = (NumTeeth + 2) / Pitch;
            double RD = (NumTeeth - 2.3) / Pitch;
            double BC = Diameter * Math.Cos(PressureAngle * (Math.PI / 180));
            double CP = Math.PI / Pitch;
            double rmin = RD / 2;
            double rmax = OD / 2;
            double rbase = BC / 2;
            var points = new List<PolarPointDouble>();
            double ac = 0;
            double aca = 0;
            double w = Math.Ceiling(OD / 2 * sc) * 2;
            double h = w;

            // calc
            points.Add(new PolarPointDouble(rmin, 0));

            int pn = 0;
            double step = 1;
            bool first = true;

            for (double i = 1; i < 100; i += step)
            {
                // get a point...
                var bpl = polarToLinear(new PolarPointDouble(rbase, -i));        //base point linear
                var len = ((rbase * Math.PI * 2) / 360) * i;                //length
                var opl = polarToLinear(new PolarPointDouble(len, -i + 90));      //add line
                var np = linearToPolar(new PointDouble(bpl.X + opl.X, bpl.Y + opl.Y));

                if (np.R >= rmin)
                {
                    if (first)
                    {
                        first = false;
                        step = (2 / NumTeeth) * 10;
                    }

                    if (np.R < Diameter / 2)
                    {
                        ac = np.A;
                        aca = i;
                    }

                    pn++;

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
            double ma = fa / 2 + 2 * ac;               // mirror a
            double fpa = (fa - ma) > 0 ? 0 : -(fa - ma) / 2;   // first point a
            int m = points.Count;
            points[0] = new PolarPointDouble(rmin, fpa);           // fix first point a

            while (points[m - 1].A > ma / 2)
            {
                points.RemoveAt(m - 1);
                m--;
                pn--;
            }

            for (int i = pn; i >= 0; i--)
            {
                var bp = points[i];
                var na = ma - bp.A;
                points.Add(new PolarPointDouble(bp.R, na));
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
            var output = new List<Point>();

            foreach(var p in points)
            {
                var linearPoint = polarToLinear(p);
                output.Add(new Point((decimal)linearPoint.X, (decimal)linearPoint.Y));
            }

            return output;
        }
    }
}
