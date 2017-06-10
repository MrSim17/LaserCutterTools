using System;
using System.Collections.Generic;
using System.Linq;
using LaserCutterTools.Common;
using LaserCutterTools.Common.Rendering;

namespace LaserCutterTools.BoxBuilder
{
    /// <summary>
    /// Responsibilities:
    /// 1. Translate and rotate all parts to their final position for rendering
    /// </summary>
    public sealed class PointRendererBoxSVG : IPointRendererBoxSVG
    {
        decimal padding = 0.2M; // TODO: padding probably shouldn't be hard coded
        IPointRendererSVG SVGPointRenderer;

        public PointRendererBoxSVG(IPointRendererSVG SVGRenderer)
        {
            SVGPointRenderer = SVGRenderer;
        }

        public string RenderPoints(Dictionary<CubeSide, List<Point>> PointData, Dictionary<PartType, List<Point>> AdditionalParts, bool RotateParts = false, bool TranslateParts = true, bool UseDebugMode = false)
        {
            List<Tuple<string, List<Point>>> translatedParts = new List<Tuple<string, List<Point>>>();
            decimal dimensionX = FindDimensionX(PointData[CubeSide.Bottom]);
            decimal dimensionY = FindDimensionY(PointData[CubeSide.Bottom]);
            decimal dimensionZ = FindDimensionY(PointData[CubeSide.Left]);

            if (RotateParts)
            {
                throw new NotImplementedException("Rotating the parts during rendering is not implemented after going to manual SVG generation.");

                // TODO: add part rotation to the manual xml generation
                //HelperMethods.RotateSVG(left, 90);
                //HelperMethods.RotateSVG(right, 270);
                //HelperMethods.RotateSVG(back, 180);
            }

            // TODO: Do some better placement for the additional parts.
            if (AdditionalParts != null && AdditionalParts.Count > 0)
            {
                if(AdditionalParts.ContainsKey(PartType.Divider))
                {
                    translatedParts.Add(new Tuple<string, List<Point>>("Divider", AdditionalParts[PartType.Divider]));
                }
            }

            if (TranslateParts)
            {
                // Bottom
                decimal bottomTranslateX = padding + dimensionZ + padding;
                decimal bottomTranslateY = padding + dimensionZ + padding;

                translatedParts.Add(new Tuple<string, List<Point>>("Bottom", TranslatePolygon(PointData[CubeSide.Bottom],bottomTranslateX,bottomTranslateY)));

                // Left
                decimal leftTranslateX = padding;
                decimal leftTranslateY = padding + dimensionZ + padding;

                translatedParts.Add(new Tuple<string, List<Point>>("Left", TranslatePolygon(PointData[CubeSide.Left], leftTranslateX, leftTranslateY)));

                // Right
                decimal rightTranslateX = padding + dimensionZ + padding + dimensionX + padding;
                decimal rightTranslateY = padding + dimensionZ + padding;

                translatedParts.Add(new Tuple<string, List<Point>>("Right", TranslatePolygon(PointData[CubeSide.Right], rightTranslateX, rightTranslateY)));

                // Front
                decimal frontTranslateX = padding + dimensionZ + padding;
                decimal frontTranslateY = padding + dimensionZ + padding + dimensionY + padding;

                translatedParts.Add(new Tuple<string, List<Point>>("Front", TranslatePolygon(PointData[CubeSide.Front], frontTranslateX, frontTranslateY)));

                // Back
                decimal backTranslateX = padding + dimensionZ + padding;
                decimal backTranslateY = padding;

                translatedParts.Add(new Tuple<string, List<Point>>("Back", TranslatePolygon(PointData[CubeSide.Back], backTranslateX, backTranslateY)));

                // Top
                if (PointData.ContainsKey(CubeSide.Top))
                {
                    decimal topTranslateX = padding + dimensionZ + padding + dimensionX + padding + dimensionZ + padding;
                    decimal topTranslateY = padding + dimensionZ + padding;

                    translatedParts.Add(new Tuple<string, List<Point>>("Top", TranslatePolygon(PointData[CubeSide.Top], topTranslateX, topTranslateY)));
                }
            }
            else
            {
                translatedParts.Add(new Tuple<string, List<Point>>("Bottom", PointData[CubeSide.Bottom]));
                translatedParts.Add(new Tuple<string, List<Point>>("Left", PointData[CubeSide.Left]));
                translatedParts.Add(new Tuple<string, List<Point>>("Right", PointData[CubeSide.Right]));
                translatedParts.Add(new Tuple<string, List<Point>>("Front", PointData[CubeSide.Front]));
                translatedParts.Add(new Tuple<string, List<Point>>("Back", PointData[CubeSide.Back]));

                if (PointData.ContainsKey(CubeSide.Top))
                {
                    translatedParts.Add(new Tuple<string, List<Point>>("Top", PointData[CubeSide.Top]));
                }
            }

            return SVGPointRenderer.RenderPoints(translatedParts, UseDebugMode);
        }

        private List<Point> TranslatePolygon(List<Point> Polygon, decimal TranslateX, decimal TranslateY)
        {
            var newPoly = new List<Point>();

            foreach(var point in Polygon)
            {
                newPoly.Add(new Point(point.X + TranslateX, point.Y + TranslateY));
            }

            return newPoly;
        }

        private static decimal FindDimensionX(List<Point> PointData)
        {
            decimal minVal = 0;
            decimal maxVal = 0;

            minVal = PointData.Aggregate((curMin, newPoint) => curMin.X <= newPoint.X ? curMin : newPoint).X;
            maxVal = PointData.Aggregate((curMin, newPoint) => curMin.X >= newPoint.X ? curMin : newPoint).X;

            return Math.Abs(minVal - maxVal);
        }

        private static decimal FindDimensionY(List<Point> PointData)
        {
            decimal minVal = 0;
            decimal maxVal = 0;

            minVal = PointData.Aggregate((curMin, newPoint) => curMin.Y <= newPoint.Y ? curMin : newPoint).Y;
            maxVal = PointData.Aggregate((curMin, newPoint) => curMin.Y >= newPoint.Y ? curMin : newPoint).Y;

            return Math.Abs(minVal - maxVal);
        }
    }
}