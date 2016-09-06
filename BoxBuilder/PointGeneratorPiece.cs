using System;
using System.Collections.Generic;
using Common;
using LaserCutterTools.Common;

namespace BoxBuilder
{
    internal sealed class PointGeneratorPiece : IPointGeneratorPiece
    {
        public List<Point> CreateTabedObject(decimal DimensionX, 
            decimal DimensionY, 
            int NumTabsX, 
            int NumTabsY, 
            TabPosition StartPositionX, 
            TabPosition StartPositionY, 
            TabPosition StartPositionXMinus, 
            TabPosition StartPositionYMinus,
            decimal MaterialThickness, 
            decimal ToolSpacing, 
            ILogger logger)
        {
            return CreateTabedObjectInternal(DimensionX, DimensionY, NumTabsX, NumTabsY, null, null, null, null, StartPositionX, StartPositionY, StartPositionXMinus, StartPositionYMinus, MaterialThickness, ToolSpacing, logger, CubeTopConfiguration.Closed, null);
        }

        public List<Point> CreateTabedObject(decimal DimensionX, 
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
            ILogger logger)
        {
            return CreateTabedObjectInternal(DimensionX, DimensionY, NumTabsX, NumTabsY, null, null, null, null, StartPositionX, StartPositionY, StartPositionXMinus, StartPositionYMinus, MaterialThickness, ToolSpacing, logger, CubeTopConfiguration.Open, NonTabbedSide);
        }

        public List<Point> CreateTabedObject(decimal DimensionX, 
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
            ILogger logger)
        {
            // TODO: Slot angle is not implemented here. Parameter is just ignored currently.
            if(SlotAngle != 0)
            {
                throw new NotImplementedException("Slot angle is not implemented. At this time it only accepts a 0 value.");
            }

            return CreateTabedObjectInternal(DimensionX, DimensionY, NumTabsX, NumTabsY, SlotDepth, Slotwidth, SlotCount, SlotAngle, StartPositionX, StartPositionY, StartPositionXMinus, StartPositionYMinus, MaterialThickness, ToolSpacing, logger, CubeTopConfiguration.Slotted, NonTabbedSide);
        }

        private List<Point> CreateTabedObjectInternal(decimal DimensionX, 
            decimal DimensionY, 
            int NumTabsX, 
            int NumTabsY,
            decimal? SlotDepth,
            decimal? Slotwidth,
            int? SlotCount,
            decimal? SlotAngle,
            TabPosition StartPositionX, 
            TabPosition StartPositionY, 
            TabPosition StartPositionXMinus, 
            TabPosition StartPositionYMinus,
            decimal MaterialThickness, 
            decimal ToolSpacing, 
            ILogger logger, 
            CubeTopConfiguration TopConfig,
            PieceSide? FlatSide = null)
        {
            logger.Log("=========== starting new polygon ===========");
            List<Point> points = new List<Point>();

            // flat side is always a crest. No matter what the caller says.
            if(FlatSide != null)
            {
                switch(FlatSide)
                {
                    case PieceSide.X:
                        StartPositionX = TabPosition.Crest;
                        break;
                    case PieceSide.XMinus:
                        StartPositionXMinus = TabPosition.Crest;
                        break;
                    case PieceSide.Y:
                        StartPositionY = TabPosition.Crest;
                        break;
                    case PieceSide.YMinus:
                        StartPositionYMinus = TabPosition.Crest;
                        break;
                }
            }

            decimal tabSize = 0;
            decimal currentX = 0;
            decimal currentY = 0;

            TabPosition lastTabPos = StartPositionYMinus;
            Point lastPoint = new Point(currentX, currentY);

            if (StartPositionX == TabPosition.Trough)
            {
                lastPoint.Y += MaterialThickness; // only move the material thickness because it is not changed by tool spacing
                currentY += lastPoint.Y;
            }

            if (StartPositionYMinus == TabPosition.Trough)
            {
                lastPoint.X += MaterialThickness; // only move the material thickness because it is not changed by tool spacing
                currentX += lastPoint.X;
            }

            logger.Log(string.Format("First Point: ({0}, {1})", lastPoint.X, lastPoint.Y));

            for (int side = 0; side < 4; side++)
            {
                logger.Log(string.Format("=========== starting Side {0} ===========", side + 1));

                int numTabsCurrent = 0;
                TabPosition tabPos;

                TabPosition currentStartPosition = TabPosition.Crest;
                TabPosition nextStartPosition = TabPosition.Crest;
                TabPosition prevStartPosition = TabPosition.Crest;

                switch ((PieceSide)side)
                {
                    case PieceSide.X:
                        tabPos = StartPositionX;
                        prevStartPosition = StartPositionYMinus;
                        currentStartPosition = StartPositionX;
                        nextStartPosition = StartPositionY;
                        numTabsCurrent = NumTabsX;
                        break;
                    case PieceSide.XMinus:
                        tabPos = StartPositionXMinus;
                        prevStartPosition = StartPositionY;
                        currentStartPosition = StartPositionXMinus;
                        nextStartPosition = StartPositionYMinus;
                        numTabsCurrent = NumTabsX;
                        break;
                    case PieceSide.Y:
                        tabPos = StartPositionY;
                        prevStartPosition = StartPositionX;
                        currentStartPosition = StartPositionY;
                        nextStartPosition = StartPositionXMinus;
                        numTabsCurrent = NumTabsY;
                        break;
                    default:
                        tabPos = StartPositionYMinus;
                        prevStartPosition = StartPositionXMinus;
                        currentStartPosition = StartPositionYMinus;
                        nextStartPosition = StartPositionX;
                        numTabsCurrent = NumTabsY;
                        break;
                }

                // ============== Handle the special side variations ====================
                if (
                    (TopConfig == CubeTopConfiguration.Open && FlatSide == (PieceSide)side)
                    || (TopConfig == CubeTopConfiguration.Slotted && SlotCount.GetValueOrDefault(0) == 0) // Just make the side flat if there are no slots
                    )
                {
                    logger.Log("Making flat side.");
                    decimal deltaX = 0;
                    decimal deltaY = 0;

                    switch (FlatSide)
                    {
                        case PieceSide.X:
                            deltaX = (DimensionX + ToolSpacing);

                            if(StartPositionYMinus == TabPosition.Trough)
                            {
                                deltaX -= MaterialThickness;
                            }

                            if (StartPositionY == TabPosition.Trough)
                            {
                                deltaX -= MaterialThickness;
                            }
                            break;
                        case PieceSide.XMinus:
                            deltaX = -(DimensionX + ToolSpacing);

                            if(StartPositionYMinus == TabPosition.Trough)
                            {
                                deltaX += MaterialThickness;
                            }

                            if (StartPositionY == TabPosition.Trough)
                            {
                                deltaX += MaterialThickness;
                            }
                            break;
                        case PieceSide.Y:
                            deltaY = (DimensionY + ToolSpacing);

                            if(StartPositionXMinus == TabPosition.Trough)
                            {
                                deltaY -= MaterialThickness;
                            }

                            if (StartPositionX == TabPosition.Trough)
                            {
                                deltaY -= MaterialThickness;
                            }
                            break;
                        case PieceSide.YMinus:
                            deltaY = -(DimensionY + ToolSpacing);

                            if(StartPositionXMinus ==  TabPosition.Trough)
                            {
                                deltaY += MaterialThickness;
                            }

                            if (StartPositionX == TabPosition.Trough)
                            {
                                deltaY += MaterialThickness;
                            }
                            break;
                    }

                    currentX += deltaX;
                    currentY += deltaY;

                    lastPoint = new Point(currentX, currentY);
                    points.Add(lastPoint);
                    logger.Log(string.Format("Point: ({0}, {1})", lastPoint.X, lastPoint.Y));

                    lastTabPos = TabPosition.Crest;

                    continue;
                }
                else if(TopConfig == CubeTopConfiguration.Slotted && FlatSide == (PieceSide)side)
                {
                    logger.Log("Making slotted side.");

                    decimal flatSideDimension = 0;

                    // always subtract material thickness here because we want all of the divided sections to be the same
                    // width relative to the INSIDE of the box instead of the outside.
                    switch (FlatSide)
                    {
                        case PieceSide.X:
                            flatSideDimension = DimensionX - (MaterialThickness * 2);
                            break;
                        case PieceSide.XMinus:
                            flatSideDimension = DimensionX - (MaterialThickness * 2);
                            break;
                        case PieceSide.Y:
                            flatSideDimension = DimensionY - (MaterialThickness * 2);
                            break;
                        case PieceSide.YMinus:
                            flatSideDimension = DimensionY - (MaterialThickness * 2);
                            break;
                    }

                    decimal gap = (flatSideDimension / (SlotCount.GetValueOrDefault(0) + 1)) 
                        - Slotwidth.GetValueOrDefault(0)
                        + (Slotwidth.GetValueOrDefault(0) / (SlotCount.GetValueOrDefault(0) + 1))
                        + ToolSpacing;

                    for(int ii = 0; ii < SlotCount.GetValueOrDefault(0); ii++)
                    {
                        Point topLeftP;
                        Point bottomLeftP;
                        Point bottomRightP;
                        Point topRightP;

                        // create the slot points
                        switch (FlatSide)
                        {
                            case PieceSide.X:
                                if(ii == 0 && StartPositionYMinus == TabPosition.Crest)
                                {
                                    currentX += MaterialThickness;
                                }

                                currentX += gap;
                                topLeftP = new Point(currentX, currentY);

                                currentY += SlotDepth.GetValueOrDefault(0) - (ToolSpacing / 2);
                                bottomLeftP = new Point(currentX, currentY);

                                currentX += Slotwidth.GetValueOrDefault(0) - ToolSpacing;
                                bottomRightP = new Point(currentX, currentY);

                                currentY -= SlotDepth.GetValueOrDefault(0) - (ToolSpacing / 2);
                                topRightP = new Point(currentX, currentY);
                                break;
                            case PieceSide.XMinus:
                                if (ii == 0 && StartPositionY == TabPosition.Crest)
                                {
                                    currentX += MaterialThickness;
                                }

                                currentX -= gap;
                                topLeftP = new Point(currentX, currentY);

                                currentY -= SlotDepth.GetValueOrDefault(0) - (ToolSpacing / 2);
                                bottomLeftP = new Point(currentX, currentY);

                                currentX -= Slotwidth.GetValueOrDefault(0) - ToolSpacing;
                                bottomRightP = new Point(currentX, currentY);

                                currentY += SlotDepth.GetValueOrDefault(0) - (ToolSpacing / 2);
                                topRightP = new Point(currentX, currentY);
                                break;
                            case PieceSide.Y:
                                if (ii == 0 && StartPositionX == TabPosition.Crest)
                                {
                                    currentX += MaterialThickness;
                                }

                                currentY -= gap;
                                topLeftP = new Point(currentX, currentY);

                                currentX -= SlotDepth.GetValueOrDefault(0) - (ToolSpacing / 2);
                                bottomLeftP = new Point(currentX, currentY);

                                currentY -= Slotwidth.GetValueOrDefault(0) - ToolSpacing;
                                bottomRightP = new Point(currentX, currentY);

                                currentX += SlotDepth.GetValueOrDefault(0) - (ToolSpacing / 2);
                                topRightP = new Point(currentX, currentY);
                                break;
                            case PieceSide.YMinus:
                                if (ii == 0 && StartPositionXMinus == TabPosition.Crest)
                                {
                                    currentX += MaterialThickness;
                                }

                                currentY += gap;
                                topLeftP = new Point(currentX, currentY);

                                currentX += SlotDepth.GetValueOrDefault(0) - (ToolSpacing / 2);
                                bottomLeftP = new Point(currentX, currentY);

                                currentY += Slotwidth.GetValueOrDefault(0) - ToolSpacing;
                                bottomRightP = new Point(currentX, currentY);

                                currentX -= SlotDepth.GetValueOrDefault(0) - (ToolSpacing / 2);
                                topRightP = new Point(currentX, currentY);
                                break;
                            default:
                                throw new Exception("Unknown piece side.");
                        }

                        points.Add(topLeftP);
                        points.Add(bottomLeftP);
                        points.Add(bottomRightP);
                        points.Add(topRightP);
                    }
                    
                    switch (FlatSide)
                    {
                        case PieceSide.X:
                            currentX += gap;

                            if(StartPositionY == TabPosition.Crest)
                            {
                                currentX += MaterialThickness;
                            }
                            break;
                        case PieceSide.XMinus:
                            currentX -= gap;

                            if(StartPositionYMinus == TabPosition.Crest)
                            {
                                currentX -= MaterialThickness;
                            }
                            break;
                        case PieceSide.Y:
                            currentY += gap;

                            if(StartPositionXMinus == TabPosition.Crest)
                            {
                                currentY += MaterialThickness;
                            }
                            break;
                        case PieceSide.YMinus:
                            currentY -= gap;

                            if(StartPositionX == TabPosition.Crest)
                            {
                                currentY -= MaterialThickness;
                            }
                            break;
                    }

                    // add the last gap
                    points.Add(new Point(currentX, currentY));

                    lastTabPos = TabPosition.Crest;

                    continue;
                }
                else if(TopConfig == CubeTopConfiguration.DiagonalSlotted && FlatSide == (PieceSide)side)
                {
                    logger.Log("Making diagonally slotted side.");

                    throw new NotImplementedException();
                }

                // ================== Handle Tabbing ====================
                bool isFirstTab = true;
                bool isLastTab = false;

                for (int i = 0; i < (numTabsCurrent * 2) - 1; i++)
                {
                    logger.Log(string.Format("Tab {0}, Position: {1}, Prev Pos: {2}", i + 1, tabPos, lastTabPos));

                    decimal deltaX = 0;
                    decimal deltaY = 0;

                    if (i == numTabsCurrent * 2 - 2)
                    {
                        isLastTab = true;
                    }

                    if ((PieceSide)side == PieceSide.X || (PieceSide)side == PieceSide.XMinus)
                    {
                        tabSize = DimensionX / (NumTabsX * 2 - 1);
                    }
                    else
                    {
                        tabSize = DimensionY / (NumTabsY * 2 - 1);
                    }

                    logger.Log(string.Format("Original Tab Size: {0}", tabSize));

                    if (isFirstTab)
                    {
                        if (lastTabPos == TabPosition.Trough)
                        {
                            tabSize -= MaterialThickness;
                            logger.Log("Retract for prev trhough.");
                            logger.Log(string.Format("New Tab Size: {0}", tabSize));

                        }
                    }

                    switch ((PieceSide)side)
                    {
                        case PieceSide.X:
                            deltaX += tabSize;

                            if (tabPos == TabPosition.Crest || (!isFirstTab && !isLastTab))
                            {
                                deltaX += (tabPos == TabPosition.Crest) ? ToolSpacing : -(ToolSpacing);
                            }
                            break;
                        case PieceSide.XMinus:
                            deltaX -= tabSize;

                            if (tabPos == TabPosition.Crest || (!isFirstTab && !isLastTab))
                            {
                                deltaX -= (tabPos == TabPosition.Crest) ? ToolSpacing : -(ToolSpacing);
                            }
                            break;
                        case PieceSide.Y:
                            deltaY += tabSize;

                            if (tabPos == TabPosition.Crest || (!isFirstTab && !isLastTab))
                            {
                                deltaY += (tabPos == TabPosition.Crest) ? (ToolSpacing) : -(ToolSpacing);
                            }
                            break;
                        case PieceSide.YMinus:
                            deltaY -= tabSize;

                            if (tabPos == TabPosition.Crest || (!isFirstTab && !isLastTab))
                            {
                                deltaY -= (tabPos == TabPosition.Crest) ? (ToolSpacing) : -(ToolSpacing);
                            }
                            break;
                    }

                    logger.Log(string.Format("--Delta: ({0}, {1})", deltaX, deltaY));

                    currentX = currentX + deltaX;
                    currentY = currentY + deltaY;

                    lastPoint = new Point(currentX, currentY);
                    logger.Log(string.Format("Point: ({0}, {1})", lastPoint.X, lastPoint.Y));

                    points.Add(lastPoint);

                    deltaX = 0;
                    deltaY = 0;

                    if (i < (numTabsCurrent * 2) - 2)
                    {
                        switch ((PieceSide)side)
                        {
                            case PieceSide.X:
                                deltaY = (tabPos == TabPosition.Crest) ? MaterialThickness : -(MaterialThickness);
                                break;
                            case PieceSide.XMinus:
                                deltaY = (tabPos == TabPosition.Crest) ? -(MaterialThickness) : MaterialThickness;
                                break;
                            case PieceSide.Y:
                                deltaX = (tabPos == TabPosition.Crest) ? -(MaterialThickness) : MaterialThickness;
                                break;
                            case PieceSide.YMinus:
                                deltaX = (tabPos == TabPosition.Crest) ? MaterialThickness : -(MaterialThickness);
                                break;
                        }

                        logger.Log(string.Format("--Delta: ({0}, {1})", deltaX, deltaY));

                        currentX = currentX + deltaX;
                        currentY = currentY + deltaY;

                        lastPoint = new Point(currentX, currentY);
                        logger.Log(string.Format("Point: ({0}, {1})", lastPoint.X, lastPoint.Y));

                        points.Add(lastPoint);
                    }

                    lastTabPos = tabPos;
                    tabPos = (tabPos == TabPosition.Crest) ? TabPosition.Trough : TabPosition.Crest;

                    if (isFirstTab)
                    {
                        isFirstTab = false;
                    }
                }

                if (nextStartPosition == TabPosition.Trough)
                {
                    decimal adjustmentX = 0;
                    decimal adjustmentY = 0;

                    switch ((PieceSide)side)
                    {
                        case PieceSide.X:
                            adjustmentX = -(MaterialThickness);
                            break;
                        case PieceSide.XMinus:
                            adjustmentX = (MaterialThickness);
                            break;
                        case PieceSide.Y:
                            adjustmentY = -(MaterialThickness);
                            break;
                        default:
                            adjustmentY = (MaterialThickness);
                            break;
                    }

                    Point newPoint = new Point(points[points.Count - 1].X + adjustmentX, points[points.Count - 1].Y + adjustmentY);
                    Point oldPoint = points[points.Count - 1];

                    currentX += adjustmentX;
                    currentY += adjustmentY;

                    logger.Log(string.Format("Adjusting Point: ({0},{1}) => ({2},{3})", oldPoint.X, oldPoint.Y, newPoint.X, newPoint.Y));
                    points[points.Count - 1] = newPoint;
                }
            }

            return points;
        }
    }
}
