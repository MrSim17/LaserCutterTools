using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ColorProvider;
using Common;

namespace BoxBuilder
{
    // TODO: Contemplate separating the concerns of rotating parts and arranging them from rendering the actual XML.
    public sealed class BoxPointRendererSVG : IBoxPointRenderer
    {
        bool translatePieces = true;
        ILogger logger = new NullLogger();
        IColorProvider colorProvider;
        bool makeBoxOpen = false;
        PieceSide? flatSide = null;
        bool isInDebugMode = false;

        public ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        XmlDocument doc;
        XmlElement svgRootNode;

        // TODO: padding probably shouldn't be hard coded
        decimal padding = 0.2M;

        public BoxPointRendererSVG(IColorProvider ColorProvider, bool IsInDebugMode = false)
        {
            colorProvider = ColorProvider;
            isInDebugMode = IsInDebugMode;
            InitDoc();
        }

        public BoxPointRendererSVG(bool IsInDebugMode = false)
        {
            colorProvider = new ColorProviderAllBlack();
            isInDebugMode = IsInDebugMode;
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

            if (translatePieces)
            {
                decimal bottomTranslateX = padding + dimensionZ + padding;
                decimal bottomTranslateY = padding + dimensionZ + padding;

                AddPolygon("Bottom", PointData[CubeSide.Bottom], bottomTranslateX, bottomTranslateY);
            }
            else
            {
                AddPolygon("Bottom", PointData[CubeSide.Bottom], 0, 0);
            }

            if (RotateParts)
            {
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                // TODO: add part rotation to the manual xml generation
                //HelperMethods.RotateSVG(left, 90);
            }

            if (translatePieces)
            {
                decimal leftTranslateX = padding;
                decimal leftTranslateY = padding + dimensionZ + padding;

                AddPolygon("Left", PointData[CubeSide.Left], leftTranslateX, leftTranslateY);
            }
            else
            {
                AddPolygon("Left", PointData[CubeSide.Left], 0, 0);
            }

            if (RotateParts)
            {
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                // TODO: add part rotation to the manual xml generation
                //HelperMethods.RotateSVG(right, 270);
            }

            if (translatePieces)
            {
                decimal rightTranslateX = padding + dimensionZ + padding + dimensionX + padding;
                decimal rightTranslateY = padding + dimensionZ + padding;

                AddPolygon("Right", PointData[CubeSide.Right], rightTranslateX, rightTranslateY);
            }
            else
            {
                AddPolygon("Right", PointData[CubeSide.Right], 0, 0);
            }

            if (translatePieces)
            {
                decimal frontTranslateX = padding + dimensionZ + padding;
                decimal frontTranslateY = padding + dimensionZ + padding + dimensionY + padding;

                AddPolygon("Front", PointData[CubeSide.Front], frontTranslateX, frontTranslateY);
            }
            else
            {
                AddPolygon("Front", PointData[CubeSide.Front], 0, 0);
            }

            if (RotateParts)
            {
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                // TODO: add part rotation to the manual xml generation
                //HelperMethods.RotateSVG(back, 180);
            }

            if (translatePieces)
            {
                decimal backTranslateX = padding + dimensionZ + padding;
                decimal backTranslateY = padding;

                AddPolygon("Back", PointData[CubeSide.Back], backTranslateX, backTranslateY);
            }
            else
            {
                AddPolygon("Back", PointData[CubeSide.Back], 0, 0);
            }

            if (PointData.ContainsKey(CubeSide.Top))
            {
                decimal topTranslateX = padding + dimensionZ + padding + dimensionX + padding + dimensionZ + padding;
                decimal topTranslateY = padding + dimensionZ + padding;

                if (translatePieces)
                {
                    AddPolygon("Top", PointData[CubeSide.Top], topTranslateX, topTranslateY);
                }
                else
                {
                    AddPolygon("Top", PointData[CubeSide.Top], 0, 0);
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
                var textEl = doc.CreateElement("text", "http://www.w3.org/2000/svg");
                textEl.InnerText = string.Format("{2}. ({0},{1})", p.X, p.Y, i++);

                var xAttrib = doc.CreateAttribute("x");
                xAttrib.Value = p.X.ToString();
                var yAttrib = doc.CreateAttribute("y");
                yAttrib.Value = p.Y.ToString();

                var styleAttrib = doc.CreateAttribute("style");
                styleAttrib.Value = "fill:#000000;font-family:TimesNewRoman;font-size:0.05px;";

                textEl.Attributes.Append(xAttrib);
                textEl.Attributes.Append(yAttrib);
                textEl.Attributes.Append(styleAttrib);

                Group.AppendChild(textEl);
            }
        }

        private void AddPolygon(string Id, List<Point> PointData, decimal TranslateX, decimal TranslateY)
        {
            var group = doc.CreateElement("g", "http://www.w3.org/2000/svg");
            svgRootNode.AppendChild(group);

            var attrTranslate = doc.CreateAttribute("transform");
            attrTranslate.Value = string.Format("translate({0} {1})", TranslateX, TranslateY);
            group.Attributes.Append(attrTranslate);

            AddPolygon(group, Id, PointData);

            if (isInDebugMode)
            {
                // output debug information
                AddPointOutput(PointData, group);
            }
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
            var color = colorProvider.GetColor();
            string colorString = string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);

            attrStyle.Value = string.Format("stroke-miterlimit:9;stroke-linecap:butt;opacity:1;stroke-width:0.0034;fill:none;stroke-linejoin:miter;stroke:{0};", colorString);
            polygon.Attributes.Append(attrStyle);
        }

        private decimal FindDimensionX(List<Point> PointData)
        {
            decimal minVal = 0;
            decimal maxVal = 0;

            minVal = PointData.Aggregate((curMin, newPoint) => curMin.X <= newPoint.X ? curMin : newPoint).X;
            maxVal = PointData.Aggregate((curMin, newPoint) => curMin.X >= newPoint.X ? curMin : newPoint).X;

            return Math.Abs(minVal - maxVal);
        }

        private decimal FindDimensionY(List<Point> PointData)
        {
            decimal minVal = 0;
            decimal maxVal = 0;

            minVal = PointData.Aggregate((curMin, newPoint) => curMin.Y <= newPoint.Y ? curMin : newPoint).Y;
            maxVal = PointData.Aggregate((curMin, newPoint) => curMin.Y >= newPoint.Y ? curMin : newPoint).Y;

            return Math.Abs(minVal - maxVal);
        }
    }
}