using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SvgNet;
using SvgNet.SvgElements;
using System.Drawing;
using ColorProvider;
using Common;

namespace BoxBuilder
{
    public sealed class BoxPointRendererSVG : IBoxPointRenderer
    {
        bool translatePieces = true;
        ILogger logger = new NullLogger();
        IColorProvider colorProvider;
        bool makeBoxOpen = false;
        PieceSide? flatSide = null;

        public ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        SvgSvgElement root;
        // TODO: padding probably shouldn't be hard coded
        double padding = 0.2;

        public BoxPointRendererSVG(IColorProvider ColorProvider)
        {
            colorProvider = ColorProvider;
            // TODO: the canvas size really shouldn't be hard coded
            root = new SvgSvgElement("20in", "12in", "0,0 20,12");
        }

        public BoxPointRendererSVG()
        {
            colorProvider = new ColorProviderAllBlack();
            // TODO: the canvas size really shouldn't be hard coded
            root = new SvgSvgElement("20in", "12in", "0,0 20,12");
        }

        public string RenderPoints(Dictionary<CubeSide, List<PointF>> PointData, bool RotateParts = false)
        {
            float dimensionX = FindDimensionX(PointData[CubeSide.Bottom]);
            float dimensionY = FindDimensionY(PointData[CubeSide.Bottom]);
            float dimensionZ = FindDimensionY(PointData[CubeSide.Left]);

            // TODO: Not so great converting between arrays and lists all the time for points. Need to just pick one.
            var bottom = ConvertPointsToSVGPolygon(PointData[CubeSide.Bottom].ToArray(), colorProvider.GetColor());
            bottom.Id = "Bottom";

            double bottomTranslateX = padding + dimensionZ + padding;
            double bottomTranslateY = padding + dimensionZ + padding;

            if (translatePieces)
            {
                SvgGroupElement group = new SvgGroupElement();
                group.Transform.Add(string.Format("translate({0}, {1})", bottomTranslateX, bottomTranslateY));
                group.AddChild(bottom);

                root.AddChild(group);
            }
            else
            {
                root.AddChild(bottom);
            }

            var left = ConvertPointsToSVGPolygon(PointData[CubeSide.Left].ToArray(), colorProvider.GetColor());
            left.Id = "Left";

            if (RotateParts)
            {
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                HelperMethods.RotateSVG(left, 90);
            }

            double leftTranslateX = padding;
            double leftTranslateY = padding + dimensionZ + padding;

            if (translatePieces)
            {
                SvgGroupElement group = new SvgGroupElement();
                group.Transform.Add(string.Format("translate({0}, {1})", leftTranslateX, leftTranslateY));
                group.AddChild(left);

                root.AddChild(group);
            }
            else
            {
                root.AddChild(left);
            }

            var right = ConvertPointsToSVGPolygon(PointData[CubeSide.Right].ToArray(), colorProvider.GetColor());
            right.Id = "Right";

            if (RotateParts)
            {
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                HelperMethods.RotateSVG(right, 270);
            }

            double rightTranslateX = padding + dimensionZ + padding + dimensionX + padding;
            double rightTranslateY = padding + dimensionZ + padding;

            if (translatePieces)
            {
                SvgGroupElement group = new SvgGroupElement();
                group.Transform.Add(string.Format("translate({0}, {1})", rightTranslateX, rightTranslateY));
                group.AddChild(right);

                root.AddChild(group);
            }
            else
            {
                root.AddChild(right);
            }

            var front = ConvertPointsToSVGPolygon(PointData[CubeSide.Front].ToArray(), colorProvider.GetColor());
            front.Id = "Front";

            double frontTranslateX = padding + dimensionZ + padding;
            double frontTranslateY = padding + dimensionZ + padding + dimensionY + padding;

            if (translatePieces)
            {
                SvgGroupElement group = new SvgGroupElement();
                group.Transform.Add(string.Format("translate({0}, {1})", frontTranslateX, frontTranslateY));
                group.AddChild(front);

                root.AddChild(group);
            }
            else
            {
                root.AddChild(front);
            }

            var back = ConvertPointsToSVGPolygon(PointData[CubeSide.Back].ToArray(), colorProvider.GetColor());
            back.Id = "Back";

            if (RotateParts)
            {
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                HelperMethods.RotateSVG(back, 180);
            }

            double backTranslateX = padding + dimensionZ + padding;
            double backTranslateY = padding;

            if (translatePieces)
            {
                SvgGroupElement group = new SvgGroupElement();
                group.Transform.Add(string.Format("translate({0}, {1})", backTranslateX, backTranslateY));
                group.AddChild(back);

                root.AddChild(group);
            }
            else
            {
                root.AddChild(back);
            }

            if (PointData.ContainsKey(CubeSide.Top))
            {
                var top = ConvertPointsToSVGPolygon(PointData[CubeSide.Top].ToArray(), colorProvider.GetColor());
                top.Id = "Top";

                double topTranslateX = padding + dimensionZ + padding + dimensionX + padding + dimensionZ + padding;
                double topTranslateY = padding + dimensionZ + padding;

                if (translatePieces)
                {
                    SvgGroupElement group = new SvgGroupElement();
                    group.Transform.Add(string.Format("translate({0}, {1})", topTranslateX, topTranslateY));
                    group.AddChild(top);

                    root.AddChild(group);
                }
                else
                {
                    root.AddChild(top);
                }
            }

            // TODO: get rid of string replace hack and figure out the casing in the actual SVG library.
            return root.WriteSVGString(false).Replace("viewbox", "viewBox");
        }

        private static SvgPolygonElement ConvertPointsToSVGPolygon(PointF[] Points, Color PieceColor)
        {
            var polygon = new SvgPolygonElement(new SvgNet.SvgTypes.SvgPoints(Points.ToArray()));
            polygon.Style = new SvgNet.SvgTypes.SvgStyle(new Pen(PieceColor, 0.0034f));

            return polygon;
        }

        private float FindDimensionX(List<PointF> PointData)
        {
            float minVal = 0;
            float maxVal = 0;

            minVal = PointData.Min(p => p.X);
            maxVal = PointData.Max(p => p.X);

            return Math.Abs(minVal - maxVal);
        }

        private float FindDimensionY(List<PointF> PointData)
        {
            float minVal = 0;
            float maxVal = 0;

            minVal = PointData.Min(p => p.Y);
            maxVal = PointData.Max(p => p.Y);

            return Math.Abs(minVal - maxVal);
        }
    }
}