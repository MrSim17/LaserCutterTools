using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserCutterTools.Common;

namespace LaserCutterTools.GearBuilder
{
    public sealed class PointGeneratorGuard
    {
        // TODO: Account for tool width
        public List<PointDouble> CreateGuard(double DimensionX, double DimensionY)
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
