using System.Collections.Generic;

namespace BoxBuilder
{
    internal interface IBoxPointRendererSVG
    {
        string RenderPoints(Dictionary<CubeSide, List<Point>> CubePoints, bool RotateParts = false);
        string RenderPoints(List<Point> PointData);
    }
}
