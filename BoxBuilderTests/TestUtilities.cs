using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoxBuilder;

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
    }
}
