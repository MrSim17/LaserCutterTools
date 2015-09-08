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
    public sealed class BoxPointRendererSVG : IBoxPointRendererSVG
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

        // TODO: padding probably shouldn't be hard coded
        decimal padding = 0.2M;

        public BoxPointRendererSVG(IColorProvider ColorProvider, bool IsInDebugMode = false)
        {
            colorProvider = ColorProvider;
            isInDebugMode = IsInDebugMode;
        }

        public BoxPointRendererSVG(bool IsInDebugMode = false)
        {
            colorProvider = new ColorProviderAllBlack();
            isInDebugMode = IsInDebugMode;
        }

        private static XmlDocument InitDoc()
        {
            var doc = new XmlDocument();
            doc.XmlResolver = null; // don't load external stuff
            var docType = doc.CreateDocumentType("svg", "-//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", null);
            doc.AppendChild(docType);

            var svgRootNode = doc.CreateElement("svg", "http://www.w3.org/2000/svg");
            doc.AppendChild(svgRootNode);

            // TODO: the view box and document size should not be hard coded.
            var attrWidth = doc.CreateAttribute("width");
            attrWidth.Value = "20.00in";
            svgRootNode.Attributes.Append(attrWidth);

            var attrHeight = doc.CreateAttribute("height");
            attrHeight.Value = "12.00in";
            svgRootNode.Attributes.Append(attrHeight);

            var attrViewBox = doc.CreateAttribute("viewBox");
            attrViewBox.Value = "0.00 0.00 20.00 12.00";
            svgRootNode.Attributes.Append(attrViewBox);

            return doc;
        }

        public string RenderPoints(List<Point> PointData)
        {
            XmlDocument svgDoc = InitDoc();

            AddPolygon(svgDoc, "Single Polygon", PointData, padding, padding);

            return SerializeXMLDoc(svgDoc);
        }

        public string RenderPoints(Dictionary<CubeSide, List<Point>> PointData, bool RotateParts = false)
        {
            XmlDocument svgDoc = InitDoc();
            decimal dimensionX = FindDimensionX(PointData[CubeSide.Bottom]);
            decimal dimensionY = FindDimensionY(PointData[CubeSide.Bottom]);
            decimal dimensionZ = FindDimensionY(PointData[CubeSide.Left]);

            if (translatePieces)
            {
                decimal bottomTranslateX = padding + dimensionZ + padding;
                decimal bottomTranslateY = padding + dimensionZ + padding;

                AddPolygon(svgDoc, "Bottom", PointData[CubeSide.Bottom], bottomTranslateX, bottomTranslateY);
            }
            else
            {
                AddPolygon(svgDoc, "Bottom", PointData[CubeSide.Bottom], 0, 0);
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

                AddPolygon(svgDoc, "Left", PointData[CubeSide.Left], leftTranslateX, leftTranslateY);
            }
            else
            {
                AddPolygon(svgDoc, "Left", PointData[CubeSide.Left], 0, 0);
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

                AddPolygon(svgDoc, "Right", PointData[CubeSide.Right], rightTranslateX, rightTranslateY);
            }
            else
            {
                AddPolygon(svgDoc, "Right", PointData[CubeSide.Right], 0, 0);
            }

            if (translatePieces)
            {
                decimal frontTranslateX = padding + dimensionZ + padding;
                decimal frontTranslateY = padding + dimensionZ + padding + dimensionY + padding;

                AddPolygon(svgDoc, "Front", PointData[CubeSide.Front], frontTranslateX, frontTranslateY);
            }
            else
            {
                AddPolygon(svgDoc, "Front", PointData[CubeSide.Front], 0, 0);
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

                AddPolygon(svgDoc, "Back", PointData[CubeSide.Back], backTranslateX, backTranslateY);
            }
            else
            {
                AddPolygon(svgDoc, "Back", PointData[CubeSide.Back], 0, 0);
            }

            if (PointData.ContainsKey(CubeSide.Top))
            {
                decimal topTranslateX = padding + dimensionZ + padding + dimensionX + padding + dimensionZ + padding;
                decimal topTranslateY = padding + dimensionZ + padding;

                if (translatePieces)
                {
                    AddPolygon(svgDoc, "Top", PointData[CubeSide.Top], topTranslateX, topTranslateY);
                }
                else
                {
                    AddPolygon(svgDoc, "Top", PointData[CubeSide.Top], 0, 0);
                }
            }

            return SerializeXMLDoc(svgDoc);
        }

        private static string SerializeXMLDoc(XmlDocument Doc)
        {
            var sb = new StringBuilder();

            using (var xmlTextWriter = XmlTextWriter.Create(sb, new XmlWriterSettings { Encoding = System.Text.Encoding.UTF8, Indent = true }))
            {
                Doc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();

                // TODO: figure out the UTF-8/UTF-16 mixup here
                return sb.ToString().Replace("utf-16", "utf-8");
            }
        }

        private static void AddPointOutput(List<Point> pointData, XmlElement Group)
        {
            var doc = Group.OwnerDocument;
            int i = 1;

            foreach (Point p in pointData)
            {
                var textEl = doc.CreateElement("text", "http://www.w3.org/2000/svg");
                textEl.InnerText = string.Format("{2}. ({0},{1})", p.X.ToString("F3"), p.Y.ToString("F3"), i++);

                var xAttrib = doc.CreateAttribute("x");
                xAttrib.Value = p.X.ToString("F3");
                var yAttrib = doc.CreateAttribute("y");
                yAttrib.Value = p.Y.ToString("F3");

                var transAttrib = doc.CreateAttribute("transform");
                transAttrib.Value = string.Format("rotate(20 {0} {1})", p.X.ToString("F3"), p.Y.ToString("F3"));

                var styleAttrib = doc.CreateAttribute("style");
                styleAttrib.Value = "fill:#000000;font-family:TimesNewRoman;font-size:0.05px;";

                textEl.Attributes.Append(xAttrib);
                textEl.Attributes.Append(yAttrib);
                textEl.Attributes.Append(styleAttrib);
                textEl.Attributes.Append(transAttrib);

                Group.AppendChild(textEl);
            }
        }

        private void AddPolygon(XmlDocument Doc, string Id, List<Point> PointData, decimal TranslateX, decimal TranslateY)
        {
            var group = Doc.CreateElement("g", "http://www.w3.org/2000/svg");
            Doc.DocumentElement.AppendChild(group);

            var attrTranslate = Doc.CreateAttribute("transform");
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
            var doc = Parent.OwnerDocument;
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