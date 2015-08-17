using System.Collections.Generic;
using System.Drawing;
using Common;

namespace BoxBuilder
{
    internal sealed class PiecePointGenerator : IPiecePointGenerator
    {
        public List<Point> CreateTabedObject(double DimensionX, double DimensionY, int NumTabsX, int NumTabsY, TabPosition StartPositionX, TabPosition StartPositionY, TabPosition StartPositionXMinus, TabPosition StartPositionYMinus, double MaterialThickness, double ToolSpacing, ILogger logger, PieceSide? FlatSide = default(PieceSide?))
        {
            logger.Log("=========== starting new polygon ===========");
            List<Point> points = new List<Point>();

            double tabSize = 0;
            double currentX = 0;
            double currentY = 0;

            TabPosition lastTabPos = TabPosition.Crest;
            Point lastPoint = new Point(0, 0);

            if (NumTabsX % 2 == 0)
            {
                lastTabPos = StartPositionYMinus;
            }
            else
            {
                lastTabPos = (StartPositionYMinus == TabPosition.Crest) ? TabPosition.Trough : TabPosition.Crest;
            }

            if (StartPositionX == TabPosition.Trough)
            {
                lastPoint.Y = MaterialThickness - ToolSpacing / 2;
                currentY = lastPoint.Y;
            }

            if (StartPositionYMinus == TabPosition.Trough)
            {
                lastPoint.X = MaterialThickness - ToolSpacing / 2;
                currentX = lastPoint.X;
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

                if (FlatSide != null && FlatSide == (PieceSide)side)
                {
                    logger.Log("Making flat side.");
                    double deltaX = 0;
                    double deltaY = 0;

                    switch (FlatSide)
                    {
                        case PieceSide.X:
                            deltaX = DimensionX;
                            break;
                        case PieceSide.XMinus:
                            deltaX = -DimensionX;
                            break;
                        case PieceSide.Y:
                            deltaY = DimensionY;
                            break;
                        case PieceSide.YMinus:
                            deltaY = -DimensionY;
                            break;
                    }

                    currentX = currentX + deltaX;
                    currentY = currentY + deltaY;

                    lastPoint = new Point(currentX, currentY);
                    points.Add(lastPoint);
                    logger.Log(string.Format("Point: ({0}, {1})", lastPoint.X, lastPoint.Y));

                    continue;
                }

                bool isFirstTab = true;
                bool isLastTab = false;

                for (int i = 0; i < (numTabsCurrent * 2) - 1; i++)
                {
                    logger.Log(string.Format("Tab {0}, Position: {1}, Prev Pos: {2}", i + 1, tabPos, lastTabPos));

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
                            tabSize -= (MaterialThickness - ToolSpacing / 2);
                            logger.Log("Retract for prev trhough.");
                            logger.Log(string.Format("New Tab Size: {0}", tabSize));

                        }
                    }

                    double deltaX = 0;
                    double deltaY = 0;

                    switch ((PieceSide)side)
                    {
                        case PieceSide.X:
                            deltaX = tabSize;

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

                    logger.Log(string.Format("Delta: ({0}, {1})", deltaX, deltaY));

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
                                deltaY = (tabPos == TabPosition.Crest) ? MaterialThickness - (ToolSpacing / 2) : -(MaterialThickness - (ToolSpacing / 2));
                                break;
                            case PieceSide.XMinus:
                                deltaY = (tabPos == TabPosition.Crest) ? -(MaterialThickness - (ToolSpacing / 2)) : MaterialThickness - (ToolSpacing / 2);
                                break;
                            case PieceSide.Y:
                                deltaX = (tabPos == TabPosition.Crest) ? -(MaterialThickness - (ToolSpacing / 2)) : MaterialThickness - (ToolSpacing / 2);
                                break;
                            case PieceSide.YMinus:
                                deltaX = (tabPos == TabPosition.Crest) ? MaterialThickness - (ToolSpacing / 2) : -(MaterialThickness - (ToolSpacing / 2));
                                break;
                        }

                        logger.Log(string.Format("Delta: ({0}, {1})", deltaX, deltaY));

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
                    double adjustmentX = 0;
                    double adjustmentY = 0;

                    switch ((PieceSide)side)
                    {
                        case PieceSide.X:
                            adjustmentX = -(MaterialThickness - ToolSpacing / 2);
                            break;
                        case PieceSide.XMinus:
                            adjustmentX = (MaterialThickness - ToolSpacing / 2);
                            break;
                        case PieceSide.Y:
                            adjustmentY = -(MaterialThickness - ToolSpacing / 2);
                            break;
                        default:
                            adjustmentY = (MaterialThickness - ToolSpacing / 2);
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
