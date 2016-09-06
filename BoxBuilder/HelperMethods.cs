using System;
using System.Collections.Generic;
using System.Linq;
using LaserCutterTools.Common;

namespace BoxBuilder
{
    internal sealed class HelperMethods
    {
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

        internal static Point GetPolygonDimension(List<Point> PointData)
        {
            return new Point(GetPolygonDimensionX(PointData), GetPolygonDimensionY(PointData));
        }

        internal static decimal GetPolygonDimensionX(List<Point> PointData)
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

        internal static decimal GetValueMaxX(List<Point> PointData)
        {
            return PointData.Aggregate((curMin, newPoint) => curMin.X >= newPoint.X ? curMin : newPoint).X;
        }

        internal static decimal GetValueMaxY(List<Point> PointData)
        {
            return PointData.Aggregate((curMin, newPoint) => curMin.Y >= newPoint.Y ? curMin : newPoint).Y;
        }

        internal static decimal GetValueMinX(List<Point> PointData)
        {
            return PointData.Aggregate((curMin, newPoint) => curMin.X <= newPoint.X ? curMin : newPoint).X;
        }

        internal static decimal GetValueMinY(List<Point> PointData)
        {
            return PointData.Aggregate((curMin, newPoint) => curMin.Y <= newPoint.Y ? curMin : newPoint).Y;
        }

    }
}
