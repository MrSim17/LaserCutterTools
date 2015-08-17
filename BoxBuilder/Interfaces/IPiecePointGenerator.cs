using System.Collections.Generic;
using Common;

namespace BoxBuilder
{
    interface IPiecePointGenerator
    {
        List<Point> CreateTabedObject
            (
            decimal DimensionX,
            decimal DimensionY,
            int NumTabsX,
            int NumTabsY,
            TabPosition StartPositionX,
            TabPosition StartPositionY,
            TabPosition StartPositionXMinus,
            TabPosition StartPositionYMinus,
            decimal MaterialThickness,
            decimal ToolSpacing,
            ILogger logger,
            PieceSide? FlatSide = null
            );
    }
}