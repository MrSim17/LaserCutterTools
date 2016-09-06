using System.Collections.Generic;
using LaserCutterTools.Common;

namespace BoxBuilder
{
    internal interface IPointRendererBoxSVG
    {
        string RenderPoints(Dictionary<CubeSide, List<Point>> CubePoints, Dictionary<PartType, List<Point>> AdditionalParts, bool RotateParts = false, bool TranslateParts = true, bool UseDebugMode = false);
    }
}