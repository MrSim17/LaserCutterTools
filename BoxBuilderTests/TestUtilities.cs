using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoxBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoxBuilderTests
{
    internal sealed class TestUtilities
    {
        internal static void RenderPiece(List<Point> PointData)
        {
            IBoxPointRendererSVG renderer = new BoxPointRendererSVG(true);
            var output = renderer.RenderPoints(PointData);

            OutputFile(output);
        }

        internal static void OutputFile(string Body)
        {
            using (System.IO.TextWriter tw = new System.IO.StreamWriter("c:\\temp\\test " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".svg"))
            {
                tw.Write(Body);
            }
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

        internal static decimal CalculateDimensionX(List<Point> PointData)
        {
            return Math.Abs(GetValueMaxX(PointData) - GetValueMinX(PointData));
        }

        internal static decimal CalculateDimensionY(List<Point> PointData)
        {
            return Math.Abs(GetValueMaxY(PointData) - GetValueMinY(PointData));
        }

        internal static void CheckDimensions(decimal XDimension, decimal YDimension, List<Point> PointData)
        {
            try
            {
                Assert.AreEqual(XDimension, TestUtilities.CalculateDimensionX(PointData), "X dimension is incorrect.");
                Assert.AreEqual(YDimension, TestUtilities.CalculateDimensionY(PointData), "Y dimension is incorrect.");
            }
            catch (AssertFailedException)
            {
                TestUtilities.RenderPiece(PointData);
                throw;
            }
        }

        internal static void CheckPointsLieOnPolygons(List<Rectangle> Constraints, List<Point> PointData)
        {
            try
            {
                var retBadPoints = new List<Point>();

                foreach (Point p in PointData)
                {
                    var pointIsGood = false;

                    foreach (Rectangle rect in Constraints)
                    {
                        if (IsPointOnLine(new Point(rect.X, rect.Y), new Point(rect.X + rect.Width, rect.Y), p)) { pointIsGood = true; break; }
                        if (IsPointOnLine(new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height), p)) { pointIsGood = true; break; }
                        if (IsPointOnLine(new Point(rect.X, rect.Y), new Point(rect.X, rect.Y + rect.Height), p)) { pointIsGood = true; break; }
                        if (IsPointOnLine(new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height), p)) { pointIsGood = true; break; }
                    }

                    if (!pointIsGood)
                    {
                        retBadPoints.Add(p);
                    }
                }

                Assert.AreEqual(0, retBadPoints.Count, "Points were found that were not on the constraining polygon.");
            }
            catch
            {
                TestUtilities.RenderPiece(PointData);
                throw;
            }
        }

        internal static void CheckLinesHaveNoSlope(List<Point> PointData)
        {
            try {
                for (int i = 0; i < PointData.Count; i++)
                {
                    Point nextPoint;

                    if (i == PointData.Count - 1)
                    {
                        nextPoint = PointData.First();
                    }
                    else
                    {
                        nextPoint = PointData[i + 1];
                    }

                    var currentPoint = PointData[i];

                    Assert.IsFalse(IsLineSloped(currentPoint, nextPoint));
                }
            }
            catch (AssertFailedException)
            {
                TestUtilities.RenderPiece(PointData);
                throw;
            }
        }

        internal static bool IsPointOnLine(Point linePointA, Point linePointB, Point point)
        {
            bool pointIsGood = true;

            if (linePointA.Y - linePointB.Y == 0) // horizontal
            {
                if(point.Y != linePointA.Y)
                {
                    pointIsGood = false;
                }

                var highYValue = linePointA.X > linePointB.X ? linePointA.X : linePointB.X;
                var lowYValue = linePointA.X < linePointB.X ? linePointA.X : linePointB.X;

               if(!(point.Y >= lowYValue && point.Y <= highYValue))
                {
                    pointIsGood = false;
                }
            }
            else if (linePointA.X - linePointB.X == 0) // vertical line
            {
                if(point.X != linePointA.X)
                {
                    pointIsGood = false;
                }

                var highXValue = linePointA.Y > linePointB.Y ? linePointA.Y : linePointB.Y;
                var lowXValue = linePointA.Y < linePointB.Y ? linePointA.Y : linePointB.Y;

                if (!(point.Y >= lowXValue && point.Y <= highXValue))
                {
                    pointIsGood = false;
                }
            }
            else
            {
                const decimal EPSILON = 0.0001M;
                decimal a = (linePointB.Y - linePointA.Y) / (linePointB.X - linePointB.X);
                decimal b = linePointA.Y - a * linePointA.X;

                if (Math.Abs(point.Y - (a * point.X + b)) < EPSILON)
                {
                    return true;
                }
            }

            return pointIsGood;
        }

        internal static bool IsLineSloped(Point LinePointA, Point LinePointB)
        {
            return LinePointA.X != LinePointB.X && LinePointA.Y != LinePointB.Y;
        }
    }

    internal struct Rectangle
    {
        public Rectangle(decimal X, decimal Y, decimal Width, decimal Height)
        {
            this.X = X;
            this.Y = Y;
            this.Height = Height;
            this.Width = Width;
        }

        public decimal X { get; set; }
        public decimal Y { get; set; }

        public decimal Width { get; set; }
        public decimal Height { get; set; }
    }
}
