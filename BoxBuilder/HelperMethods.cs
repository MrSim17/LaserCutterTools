using System.Collections.Generic;
using System.Drawing;
using Common;
using SvgNet.SvgElements;
using System;

namespace BoxBuilder
{
    internal sealed class HelperMethods
    {
        public static void RotateSVG(SvgPolygonElement Polygon, int RotateDegrees)
        {
            PointF actuald = GetActualDimension(Polygon);
            // TODO: figure this out for things other than cubes
            Polygon.Transform.Add(string.Format("rotate({0} {1} {2})", RotateDegrees, actuald.X/2, actuald.Y/2));
        }

        public static PointF GetActualDimension(SvgPolygonElement Polygon)
        {
            float minX = 0;
            float minY = 0;
            float maxX = 0;
            float maxY = 0;

            for(int i = 1; i <= Polygon.Points.GetPoints().Count; i++)
            {
                float curPoint = (float)Polygon.Points.GetPoints()[i-1];

                if(i%2 == 0)
                {
                    maxY = (curPoint > maxY)? curPoint : maxY;
                    minY = (curPoint < minY)? curPoint : minY;
                }
                else
                {
                    maxX = (curPoint > maxX) ? curPoint : maxX;
                    minX = (curPoint < minX) ? curPoint : minX;
                }
            }

            return new PointF(maxX - minX, maxY - minY);
        }
    }
}
