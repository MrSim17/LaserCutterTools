using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserCutterTools.Common;

namespace GearBuilder
{
    public sealed class PointGeneratorGuard
    {
        public List<PointDouble> GeneratePoints(double DimensionX, double DimensionY)
        {
            var cornerTopLeft = new PointDouble(0, DimensionY);
            var cornerTopRight = new PointDouble(DimensionX, DimensionY);
            var cornerBottomRight = new PointDouble(DimensionX, 0);
            var cornerBottomLeft = new PointDouble(0, 0);

            var guardPart = new List<PointDouble>() { cornerTopLeft, cornerTopRight, cornerBottomRight, cornerBottomLeft };

            return guardPart;
        }
    }
}
