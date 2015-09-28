using System.Collections.Generic;

namespace BoxBuilder
{
    internal interface IBoxPointRendererSVG
    {
        // TODO: Point rendering should be more generic. Instead of accepting certain parts it should just accept an array of pieces that are already arranged. Supports need to separate out arranging from rendering.
        string RenderPoints(Dictionary<CubeSide, List<Point>> CubePoints, Dictionary<PartType, List<Point>> AdditionalParts, bool RotateParts = false, bool UseDebugMode = false);
        string RenderPoints(List<Point> PointData, bool UseDebugMode = false);
    }
}
