using System.Collections.Generic;
using System.Drawing;

namespace BoxBuilder
{
    internal interface IBoxPointRenderer
    {
        string RenderPoints(Dictionary<CubeSide, List<Point>> CubePoints, bool RotateParts = false);
    }
}
