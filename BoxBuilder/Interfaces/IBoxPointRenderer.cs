using System.Collections.Generic;
using System.Drawing;

namespace BoxBuilder
{
    internal interface IBoxPointRenderer
    {
        string RenderPoints(Dictionary<CubeSide, List<PointF>> CubePoints, bool RotateParts = false);
    }
}
