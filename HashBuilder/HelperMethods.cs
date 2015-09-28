using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using Common;

namespace HashBuilder
{
    // TODO: Most has builder helper methods really should be their own component.
    public sealed class HelperMethods
    {
        public static List<PathData> CreateHashLines(float Width, Double height, float GapWidth, float LineSpacing, int HashCount, ILogger Logger)
        {
            Logger.Log(string.Format("Dimension: X-{0}, Y-{1}", Width, height));
            List<PathData> returnData = new List<PathData>();

            float hashWidth = (Width - (GapWidth * ((float)HashCount + 1f))) / (float)HashCount;
            //float altHashWidth = (Width - (GapWidth * HashCount) - (hashWidth * (HashCount - 1))) / 2;
            float altHashWidth = (hashWidth + GapWidth) / 2;

            Logger.Log(string.Format("Hash Width: {0}", hashWidth));
            float lineWidth = (hashWidth * (float)HashCount) + (GapWidth * ((float)HashCount + 1f));
            Logger.Log(string.Format("Line width: {0}", lineWidth));
            Logger.Log(string.Format("Alt Hash Width: {0}", altHashWidth));
            float altLineWidth = (hashWidth * ((float)HashCount - 1f)) + (GapWidth * ((float)HashCount)) + (altHashWidth * 2);
            Logger.Log(string.Format("Alt Line width: {0}", altLineWidth));

            float currentPosY = 0;
            float currentPosX = 0;
            bool isEvenRow = false;

            while (currentPosY <= height)
            {
                int contextHashCount = 0;

                if (isEvenRow)
                {
                    contextHashCount = HashCount - 1;

                    // add in the half line on the left side
                    returnData.Add(CreateHorizontalLine(new PointF(currentPosX, currentPosY), altHashWidth));
                    currentPosX += altHashWidth;
                }
                else
                {
                    contextHashCount = HashCount;
                }

                // add in the full size hashes
                for (int i = 0; i < contextHashCount; i++)
                {
                    currentPosX += GapWidth;

                    returnData.Add(CreateHorizontalLine(new PointF(currentPosX, currentPosY), hashWidth));

                    currentPosX += hashWidth;
                }

                if (isEvenRow)
                {
                    isEvenRow = false;

                    currentPosX += GapWidth;

                    // add in the half line on the right side
                    returnData.Add(CreateHorizontalLine(new PointF(currentPosX, currentPosY), altHashWidth));

                    currentPosX += altHashWidth;
                }
                else
                {
                    isEvenRow = true;
                }

                Logger.Log(string.Format("Final Line Length: {0}", currentPosX));

                currentPosY += LineSpacing;
                currentPosX = 0;
            }

            return returnData;
        }

        public static PathData CreateHorizontalLine(PointF StartingPoint, float Width)
        {
            PathData newLine = new PathData();
            PointF[] linePoints = new PointF[2];

            PointF leftLinePoint = new PointF(StartingPoint.X, StartingPoint.Y);
            linePoints[0] = leftLinePoint;

            PointF rightLinePoint = new PointF(StartingPoint.X + Width, StartingPoint.Y);
            linePoints[1] = rightLinePoint;

            newLine.Points = linePoints;

            return newLine;
        }
    }
}
