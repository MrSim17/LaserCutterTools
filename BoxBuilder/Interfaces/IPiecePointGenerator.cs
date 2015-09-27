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
                ILogger logger
            );

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
                PieceSide NonTabbedSide,
                ILogger logger
            );

        List<Point> CreateTabedObject
            (
                decimal DimensionX,
                decimal DimensionY,
                int NumTabsX,
                int NumTabsY,
                decimal SlotDepth,
                decimal Slotwidth,
                int SlotCount,
                decimal SlotAngle,
                TabPosition StartPositionX,
                TabPosition StartPositionY,
                TabPosition StartPositionXMinus,
                TabPosition StartPositionYMinus,
                decimal MaterialThickness,
                decimal ToolSpacing,
                PieceSide NonTabbedSide,
                ILogger logger
            );
    }
}