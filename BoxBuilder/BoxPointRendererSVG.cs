using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SvgNet;
using SvgNet.SvgElements;
using System.Drawing;
using ColorProvider;
using Common;
using System.Xml;
using System.IO;

namespace BoxBuilder
{
    // TODO: Completely remove the SvgNet library
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

        XmlDocument doc;
        XmlElement svgRootNode;

        SvgSvgElement rootSVGElement;
        // TODO: padding probably shouldn't be hard coded
        decimal padding = 0.2M;

        public BoxPointRendererSVG(IColorProvider ColorProvider)
        {
            colorProvider = ColorProvider;
            // TODO: the canvas size really shouldn't be hard coded
            rootSVGElement = new SvgSvgElement("20in", "12in", "0,0 20,12");
            InitDoc();
        }

        public BoxPointRendererSVG()
        {
            colorProvider = new ColorProviderAllBlack();
            // TODO: the canvas size really shouldn't be hard coded
            rootSVGElement = new SvgSvgElement("20in", "12in", "0,0 20,12");
            InitDoc();
        }

        private void InitDoc()
        {
            doc = new XmlDocument();
            doc.XmlResolver = null; // don't load external stuff
            var docType = doc.CreateDocumentType("svg", "-//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", null);
            doc.AppendChild(docType);

            svgRootNode = doc.CreateElement("svg", "http://www.w3.org/2000/svg");
            doc.AppendChild(svgRootNode);

            var attrWidth = doc.CreateAttribute("width");
            attrWidth.Value = "20.00in";
            svgRootNode.Attributes.Append(attrWidth);

            var attrHeight = doc.CreateAttribute("height");
            attrHeight.Value = "12.00in";
            svgRootNode.Attributes.Append(attrHeight);

            var attrViewBox = doc.CreateAttribute("viewBox");
            attrViewBox.Value = "0.00 0.00 20.00 12.00";
            svgRootNode.Attributes.Append(attrViewBox);
        }

        public string RenderPoints(Dictionary<CubeSide, List<Point>> PointData, bool RotateParts = false)
        {
            decimal dimensionX = FindDimensionX(PointData[CubeSide.Bottom]);
            decimal dimensionY = FindDimensionY(PointData[CubeSide.Bottom]);
            decimal dimensionZ = FindDimensionY(PointData[CubeSide.Left]);

            var bottom = ConvertPointsToSVGPolygon(PointData[CubeSide.Bottom], colorProvider.GetColor());
            bottom.Id = "Bottom";

            decimal bottomTranslateX = padding + dimensionZ + padding;
            decimal bottomTranslateY = padding + dimensionZ + padding;

            if (translatePieces)
            {
                // Manual XML
                AddPolygon("Bottom", PointData[CubeSide.Bottom], bottomTranslateX, bottomTranslateY);
            }
            else
            {
                rootSVGElement.AddChild(bottom);
                // TODO: Replace with manual XML generation
            }

            var left = ConvertPointsToSVGPolygon(PointData[CubeSide.Left], colorProvider.GetColor());
            left.Id = "Left";

            if (RotateParts)
            {
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                HelperMethods.RotateSVG(left, 90);
            }

            decimal leftTranslateX = padding;
            decimal leftTranslateY = padding + dimensionZ + padding;

            if (translatePieces)
            {
                // Manual XML
                AddPolygon("Left", PointData[CubeSide.Left], leftTranslateX, leftTranslateY);
            }
            else
            {
                rootSVGElement.AddChild(left);
                // TODO: Replace with manual XML generation
            }

            var right = ConvertPointsToSVGPolygon(PointData[CubeSide.Right], colorProvider.GetColor());
            right.Id = "Right";

            if (RotateParts)
            {
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                HelperMethods.RotateSVG(right, 270);
            }

            decimal rightTranslateX = padding + dimensionZ + padding + dimensionX + padding;
            decimal rightTranslateY = padding + dimensionZ + padding;

            if (translatePieces)
            {
                // Manual XML
                AddPolygon("Right", PointData[CubeSide.Right], rightTranslateX, rightTranslateY);
            }
            else
            {
                rootSVGElement.AddChild(right);
                // TODO: Replace with manual XML generation
            }

            var front = ConvertPointsToSVGPolygon(PointData[CubeSide.Front], colorProvider.GetColor());
            front.Id = "Front";

            decimal frontTranslateX = padding + dimensionZ + padding;
            decimal frontTranslateY = padding + dimensionZ + padding + dimensionY + padding;

            if (translatePieces)
            {
                // Manual XML
                AddPolygon("Front", PointData[CubeSide.Front], frontTranslateX, frontTranslateY);
            }
            else
            {
                rootSVGElement.AddChild(front);
                // TODO: Replace with manual XML generation
            }

            var back = ConvertPointsToSVGPolygon(PointData[CubeSide.Back], colorProvider.GetColor());
            back.Id = "Back";

            if (RotateParts)
            {
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                HelperMethods.RotateSVG(back, 180);
            }

            decimal backTranslateX = padding + dimensionZ + padding;
            decimal backTranslateY = padding;

            if (translatePieces)
            {
                // Manual XML
                AddPolygon("Back", PointData[CubeSide.Back], backTranslateX, backTranslateY);
            }
            else
            {
                rootSVGElement.AddChild(back);
                // TODO: Replace with manual XML generation
            }

            if (PointData.ContainsKey(CubeSide.Top))
            {
                var top = ConvertPointsToSVGPolygon(PointData[CubeSide.Top], colorProvider.GetColor());
                top.Id = "Top";

                decimal topTranslateX = padding + dimensionZ + padding + dimensionX + padding + dimensionZ + padding;
                decimal topTranslateY = padding + dimensionZ + padding;

                if (translatePieces)
                {
                    // Manual XML
                    AddPolygon("Top", PointData[CubeSide.Top], topTranslateX, topTranslateY);
                }
                else
                {
                    rootSVGElement.AddChild(top);
                    // TODO: Replace with manual XML generation
                }
            }

            var sb = new StringBuilder();

            using (var xmlTextWriter = XmlTextWriter.Create(sb, new XmlWriterSettings { Encoding=System.Text.Encoding.UTF8, Indent = true }))
            {
                doc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();

                // TODO: figure out the UTF-8/UTF-16 mixup here
                return sb.ToString().Replace("utf-16", "utf-8");
            }
        }

        private void AddPointOutput(List<Point> pointData, XmlElement Group)
        {
            int i = 1;
            foreach (Point p in pointData)
            {
                // manual XML
                var textEl = doc.CreateElement("text", "http://www.w3.org/2000/svg");
                textEl.InnerText = string.Format("{2}. ({0},{1})", p.X, p.Y, i++);

                var xAttrib = doc.CreateAttribute("x");
                xAttrib.Value = p.X.ToString();
                var yAttrib = doc.CreateAttribute("y");
                yAttrib.Value = p.Y.ToString();

                //var fontFamAttrib = doc.CreateAttribute("font-family");
                //fontFamAttrib.Value = "TimesNewRoman";
                //var fontSizeAttrib = doc.CreateAttribute("font-size");
                //fontSizeAttrib.Value = "0.002em";

                var styleAttrib = doc.CreateAttribute("style");
                styleAttrib.Value = "fill:#000000;font-family:TimesNewRoman;font-size:0.05px;";

                textEl.Attributes.Append(xAttrib);
                textEl.Attributes.Append(yAttrib);
                //textEl.Attributes.Append(fontFamAttrib);
                //textEl.Attributes.Append(fontSizeAttrib);
                textEl.Attributes.Append(styleAttrib);

                Group.AppendChild(textEl);
            }
        }

        private static SvgPolygonElement ConvertPointsToSVGPolygon(List<Point> Points, Color PieceColor)
        {
            StringBuilder sb = new StringBuilder();
            bool isFirst = true;

            foreach(Point p in Points)
            {
                if(isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    sb.Append(" ");
                }

                sb.Append(p.X.ToString("F3") + " " + p.Y.ToString("F3"));
            }

            var polygon = new SvgPolygonElement(new SvgNet.SvgTypes.SvgPoints(sb.ToString()));
            polygon.Style = new SvgNet.SvgTypes.SvgStyle(new Pen(PieceColor, 0.0034f));

            return polygon;
        }

        private void AddPolygon(string Id, List<Point> PointData, decimal TranslateX, decimal TranslateY)
        {
            var group = doc.CreateElement("g", "http://www.w3.org/2000/svg");
            svgRootNode.AppendChild(group);

            var attrTranslate = doc.CreateAttribute("transform");
            attrTranslate.Value = string.Format("translate({0} {1})", TranslateX, TranslateY);
            group.Attributes.Append(attrTranslate);

            AddPolygon(group, Id, PointData);

            // output debug information
            AddPointOutput(PointData, group);
        }

        private void AddPolygon(XmlElement Parent, string Id, List<Point> PointData)
        {
            var polygon = doc.CreateElement("polygon", "http://www.w3.org/2000/svg");
            Parent.AppendChild(polygon);

            var attrId = doc.CreateAttribute("id");
            attrId.Value = Id;
            polygon.Attributes.Append(attrId);

            StringBuilder sb = new StringBuilder();
            bool isFirst = true;

            foreach (Point p in PointData)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    sb.Append(" ");
                }

                sb.Append(p.X.ToString("F3") + " " + p.Y.ToString("F3"));
            }

            var attrPoints = doc.CreateAttribute("points");
            attrPoints.Value = sb.ToString();
            polygon.Attributes.Append(attrPoints);

            var attrStyle = doc.CreateAttribute("style");
            // TODO: Static color here. Should be using the provided color generator
            attrStyle.Value = "stroke-miterlimit:9;stroke-linecap:butt;opacity:1;stroke-width:0.0034;fill:none;stroke-linejoin:miter;stroke:rgb(255,0,0);";
            polygon.Attributes.Append(attrStyle);
        }

        private decimal FindDimensionX(List<Point> PointData)
        {
            decimal minVal = 0;
            decimal maxVal = 0;

            minVal = PointData.Min(p => p.X);
            maxVal = PointData.Max(p => p.X);

            return Math.Abs(minVal - maxVal);
        }

        private decimal FindDimensionY(List<Point> PointData)
        {
            decimal minVal = 0;
            decimal maxVal = 0;

            minVal = PointData.Min(p => p.Y);
            maxVal = PointData.Max(p => p.Y);

            return Math.Abs(minVal - maxVal);
        }
    }
}