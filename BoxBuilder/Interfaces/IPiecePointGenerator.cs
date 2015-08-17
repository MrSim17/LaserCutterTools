using System.Collections.Generic;
using Common;

namespace BoxBuilder
{
    interface IPiecePointGenerator
    {
        List<Point> CreateTabedObject
            (
            double DimensionX,
            double DimensionY,
            int NumTabsX,
            int NumTabsY,
            TabPosition StartPositionX,
            TabPosition StartPositionY,
            TabPosition StartPositionXMinus,
            TabPosition StartPositionYMinus,
            double MaterialThickness,
            double ToolSpacing,
            ILogger logger,
            PieceSide? FlatSide = null
            );
    }
}