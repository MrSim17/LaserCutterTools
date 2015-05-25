using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using SvgNet.SvgElements;
using SvgNet;
using System.Drawing;
using Common;

namespace BoxBuilder
{
    internal sealed class HelperMethods
    {
        public static void RotateSVG(SvgPolygonElement Polygon, int RotateDegrees)
        {
            PointF actuald = GetActualDimension(Polygon);
            // TODO: figure this out for things other than cubes
            Polygon.Transform.Add(string.Format("rotate({0} {1} {2})", RotateDegrees, actuald.X/2, actuald.Y/2));
        }

        public static PointF GetActualDimension(SvgPolygonElement Polygon)
        {
            float minX = 0;
            float minY = 0;
            float maxX = 0;
            float maxY = 0;

            for(int i = 1; i <= Polygon.Points.GetPoints().Count; i++)
            {
                float curPoint = (float)Polygon.Points.GetPoints()[i-1];

                if(i%2 == 0)
                {
                    maxY = (curPoint > maxY)? curPoint : maxY;
                    minY = (curPoint < minY)? curPoint : minY;
                }
                else
                {
                    maxX = (curPoint > maxX) ? curPoint : maxX;
                    minX = (curPoint < minX) ? curPoint : minX;
                }
            }

            return new PointF(maxX - minX, maxY - minY);
        }

        public static PointF[] CreateTabedObject
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
            )
        {
            logger.Log("=========== starting new polygon ===========");
            List<PointF> points = new List<PointF>();

            double tabSize = 0;
            double currentX = 0;
            double currentY = 0;

            TabPosition lastTabPos = TabPosition.Crest;
            PointF lastPoint = new PointF(0, 0);

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
                lastPoint.Y = (float)MaterialThickness - (float)ToolSpacing / 2;
                currentY = lastPoint.Y;
            }

            if (StartPositionYMinus == TabPosition.Trough)
            {
                lastPoint.X = (float)MaterialThickness - (float)ToolSpacing / 2;
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

                    switch(FlatSide)
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

                    lastPoint = new PointF((float)currentX, (float)currentY);
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
                            tabSize -= (MaterialThickness - (float)ToolSpacing / 2);
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

                    lastPoint = new PointF((float)currentX, (float)currentY);
                    logger.Log(string.Format("Point: ({0}, {1})", lastPoint.X, lastPoint.Y));

                    points.Add(lastPoint);

                    deltaX = 0;
                    deltaY = 0;

                    if (i < (numTabsCurrent * 2) - 2)
                    {
                        switch ((PieceSide)side)
                        {
                            case PieceSide.X:
                                deltaY = (tabPos == TabPosition.Crest) ? MaterialThickness - ((float)ToolSpacing / 2) : -(MaterialThickness - ((float)ToolSpacing / 2));
                                break;
                            case PieceSide.XMinus:
                                deltaY = (tabPos == TabPosition.Crest) ? -(MaterialThickness - ((float)ToolSpacing / 2)) : MaterialThickness - ((float)ToolSpacing / 2);
                                break;
                            case PieceSide.Y:
                                deltaX = (tabPos == TabPosition.Crest) ? -(MaterialThickness - ((float)ToolSpacing / 2)) : MaterialThickness - ((float)ToolSpacing / 2);
                                break;
                            case PieceSide.YMinus:
                                deltaX = (tabPos == TabPosition.Crest) ? MaterialThickness - ((float)ToolSpacing / 2) : -(MaterialThickness - ((float)ToolSpacing / 2));
                                break;
                        }

                        logger.Log(string.Format("Delta: ({0}, {1})", deltaX, deltaY));

                        currentX = currentX + deltaX;
                        currentY = currentY + deltaY;

                        lastPoint = new PointF((float)currentX, (float)currentY);
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
                    float adjustmentX = 0;
                    float adjustmentY = 0;

                    switch ((PieceSide)side)
                    {
                        case PieceSide.X:
                            adjustmentX = -((float)MaterialThickness - (float)ToolSpacing / 2);
                            break;
                        case PieceSide.XMinus:
                            adjustmentX = ((float)MaterialThickness - (float)ToolSpacing / 2);
                            break;
                        case PieceSide.Y:
                            adjustmentY = -((float)MaterialThickness - (float)ToolSpacing / 2);
                            break;
                        default:
                            adjustmentY = ((float)MaterialThickness - (float)ToolSpacing / 2);
                            break;
                    }

                    PointF newPoint = new PointF(points[points.Count - 1].X + (float)adjustmentX, points[points.Count - 1].Y + (float)adjustmentY);
                    PointF oldPoint = points[points.Count - 1];

                    currentX += adjustmentX;
                    currentY += adjustmentY;

                    logger.Log(string.Format("Adjusting Point: ({0},{1}) => ({2},{3})", oldPoint.X, oldPoint.Y, newPoint.X, newPoint.Y));
                    points[points.Count - 1] = newPoint;
                }
            }

            return points.ToArray();
        }
    }
}
