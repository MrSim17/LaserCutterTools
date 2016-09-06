using System;
using System.Collections.Generic;

namespace LaserCutterTools.Common.Rendering
{
    public interface IPointRendererSVG
    {
        string RenderPoints(List<Point> PointData, bool UseDebugMode = false);
        string RenderPoints(List<Tuple<string, List<Point>>> PointData, bool UseDebugMode = false);
        string RenderPoints(Dictionary<string, List<Point>> PointData, bool UserDebugMode = false);
    }
}
