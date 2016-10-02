using System;
using System.Collections.Generic;
using System.Linq;

namespace LaserCutterTools.Common
{
    public sealed class HelperMethods
    {
        public static List<PointDouble> MovePolygonToQuadrantOne(List<PointDouble> Polygon)
        {
            double translateX = 0;
            double translateY = 0;

            double minX = GetValueMinX(Polygon);
            double minY = GetValueMinY(Polygon);

            if(minX < 0)
            {
                translateX = Math.Abs(minX);
            }

            if(minY < 0)
            {
                translateY = Math.Abs(minY);
            }

            return HelperMethods.TranslatePolygon(translateX, translateY, Polygon);
        }

        public static List<Point> ConvertDoubleToDecimal(List<PointDouble> Points)
        {
            var output = new List<Point>();

            foreach (var p in Points)
            {
                output.Add(new Point((decimal)p.X, (decimal)p.Y));
            }

            return output;
        }

        public static List<Point> RotatePolygon(decimal RotationDegrees)
        {
            throw new NotImplementedException();
        }

        public static List<Point> FlipPolygonVertically(List<Point> Polygon)
        {
            var newPolygon = new List<Point>();
            var maxX = GetValueMaxX(Polygon);
            var minX = GetValueMinX(Polygon);

            decimal xMidway = (maxX + minX) / 2;
            decimal distFromMidway = Math.Abs(maxX - xMidway);

            foreach (Point p in Polygon)
            {
                decimal X = p.X;
                decimal Y = p.Y;

                if (p.X > xMidway)
                {
                    X = p.X - (distFromMidway * 2);
                }
                else if (p.X < xMidway)
                {
                    X = p.X + (distFromMidway * 2);
                }

                newPolygon.Add(new Point(X, Y));
            }

            return newPolygon;
        }

        public static List<Point> FlipPolygonHorizontally(List<Point> Polygon)
        {
            var newPolygon = new List<Point>();
            var maxY = GetValueMaxY(Polygon);
            var minY = GetValueMinY(Polygon);

            decimal YMidway = (maxY + minY) / 2;
            decimal distFromMidway = Math.Abs(maxY - YMidway);

            foreach (Point p in Polygon)
            {
                decimal X = p.X;
                decimal Y = p.Y;

                if(p.Y > YMidway)
                {
                    Y = p.Y - (distFromMidway * 2);
                }
                else if(p.Y < YMidway)
                {
                    Y = p.Y + (distFromMidway * 2);
                }

                newPolygon.Add(new Point(X, Y));
            }

            return newPolygon;
        }

        public static List<Point> TranslatePolygon(decimal X, decimal Y, List<Point> Polygon)
        {
            var ret = new List<Point>();

            foreach(var p in Polygon)
            {
                ret.Add(new Point(p.X + X, p.Y + Y));
            }

            return ret;
        }

        public static List<PointDouble> TranslatePolygon(double X, double Y, List<PointDouble> Polygon)
        {
            var ret = new List<PointDouble>();

            foreach (var p in Polygon)
            {
                ret.Add(new PointDouble(p.X + X, p.Y + Y));
            }

            return ret;
        }

        internal static Point GetPolygonDimension(List<Point> PointData)
        {
            return new Point(GetPolygonDimensionX(PointData), GetPolygonDimensionY(PointData));
        }

        internal static PointDouble GetPolygonDimension(List<PointDouble> PointData)
        {
            return new PointDouble(GetPolygonDimensionX(PointData), GetPolygonDimensionY(PointData));
        }

        internal static decimal GetPolygonDimensionX(List<Point> PointData)
        {
            var minX = GetValueMinX(PointData);
            var maxX = GetValueMaxX(PointData);

            return Math.Abs(maxX - minX);
        }

        internal static double GetPolygonDimensionX(List<PointDouble> PointData)
        {
            var minX = GetValueMinX(PointData);
            var maxX = GetValueMaxX(PointData);

            return Math.Abs(maxX - minX);
        }

        internal static decimal GetPolygonDimensionY(List<Point> PointData)
        {
            var minY = GetValueMinY(PointData);
            var maxY = GetValueMaxY(PointData);

            return Math.Abs(maxY - minY);
        }

        internal static double GetPolygonDimensionY(List<PointDouble> PointData)
        {
            var minY = GetValueMinY(PointData);
            var maxY = GetValueMaxY(PointData);

            return Math.Abs(maxY - minY);
        }

        internal static decimal GetValueMaxX(List<Point> PointData)
        {
            return PointData.Aggregate((curMin, newPoint) => curMin.X >= newPoint.X ? curMin : newPoint).X;
        }

        internal static double GetValueMaxX(List<PointDouble> PointData)
        {
            return PointData.Aggregate((curMin, newPoint) => curMin.X >= newPoint.X ? curMin : newPoint).X;
        }

        internal static decimal GetValueMaxY(List<Point> PointData)
        {
            return PointData.Aggregate((curMin, newPoint) => curMin.Y >= newPoint.Y ? curMin : newPoint).Y;
        }

        internal static double GetValueMaxY(List<PointDouble> PointData)
        {
            return PointData.Aggregate((curMin, newPoint) => curMin.Y >= newPoint.Y ? curMin : newPoint).Y;
        }

        public static decimal GetValueMinX(List<Point> PointData)
        {
            return PointData.Aggregate((curMin, newPoint) => curMin.X <= newPoint.X ? curMin : newPoint).X;
        }

        public static double GetValueMinX(List<PointDouble> PointData)
        {
            return PointData.Aggregate((curMin, newPoint) => curMin.X <= newPoint.X ? curMin : newPoint).X;
        }

        public static decimal GetValueMinY(List<Point> PointData)
        {
            return PointData.Aggregate((curMin, newPoint) => curMin.Y <= newPoint.Y ? curMin : newPoint).Y;
        }

        public static double GetValueMinY(List<PointDouble> PointData)
        {
            return PointData.Aggregate((curMin, newPoint) => curMin.Y <= newPoint.Y ? curMin : newPoint).Y;
        }
    }
}
