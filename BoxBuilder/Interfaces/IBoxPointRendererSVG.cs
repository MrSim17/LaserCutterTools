using System.Collections.Generic;

namespace BoxBuilder
{
    internal interface IBoxPointRendererSVG
    {
        string RenderPoints(Dictionary<CubeSide, List<Point>> CubePoints, Dictionary<PartType, List<Point>> AdditionalParts, bool RotateParts = false, bool UseDebugMode = false);
        string RenderPoints(List<Point> PointData, bool UseDebugMode = false);
    }
}
