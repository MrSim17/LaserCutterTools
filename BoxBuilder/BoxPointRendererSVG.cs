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
    /// <summary>
    /// Responsibilities:
    /// 1. Translate and rotate all parts to their final position for rendering
    /// 2. Convert raw points into SVG text output
    /// 3. Manage the colors and combinations of colors for the rendered pieces
    /// 4. Output debugging point values
    /// 5. Create a document to contain all parts in SVG format.
    /// </summary>
    public sealed class BoxPointRendererSVG : IBoxPointRendererSVG
    {
        bool translatePieces = true; // TODO: TranslatePieces variable should be eliminated when moved out to separate component
        IColorProvider colorProvider;
        decimal padding = 0.2M; // TODO: padding probably shouldn't be hard coded

        public ILogger Logger
        {
            get;
            set;
        }

        public BoxPointRendererSVG(IColorProvider ColorProvider)
        {
            Logger = new NullLogger();
        }

        public BoxPointRendererSVG()
        {
            colorProvider = new ColorProviderAllBlack();
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

        public string RenderPoints(List<Point> PointData, bool UseDebugMode = false)
        {
            XmlDocument svgDoc = InitDoc();

            AddPolygon(svgDoc, "Single Polygon", PointData, padding, padding, UseDebugMode);

            return SerializeXMLDoc(svgDoc);
        }

        public string RenderPoints(Dictionary<CubeSide, List<Point>> PointData, Dictionary<PartType, List<Point>> AdditionalParts, bool RotateParts = false, bool UseDebugMode = false)
        {
            if(RotateParts)
            {
                throw new NotImplementedException("Rotating the parts during rendering is not implemented after going to manual SVG generation.");
            }

            XmlDocument svgDoc = InitDoc();
            decimal dimensionX = FindDimensionX(PointData[CubeSide.Bottom]);
            decimal dimensionY = FindDimensionY(PointData[CubeSide.Bottom]);
            decimal dimensionZ = FindDimensionY(PointData[CubeSide.Left]);

            // TODO: Do some better placement for the additional parts. Supports the need for separating out the placement concern.
            if(AdditionalParts != null && AdditionalParts.Count > 0)
            {
                if(AdditionalParts.ContainsKey(PartType.Divider))
                {
                    AddPolygon(svgDoc, "Divider", AdditionalParts[PartType.Divider], 0, 0, UseDebugMode);
                }
            }

            if (translatePieces)
            {
                decimal bottomTranslateX = padding + dimensionZ + padding;
                decimal bottomTranslateY = padding + dimensionZ + padding;

                AddPolygon(svgDoc, "Bottom", PointData[CubeSide.Bottom], bottomTranslateX, bottomTranslateY, UseDebugMode);
            }
            else
            {
                AddPolygon(svgDoc, "Bottom", PointData[CubeSide.Bottom], 0, 0, UseDebugMode);
            }

            if (RotateParts)
            {
                throw new NotImplementedException("Rotating parts is not implemented.");
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                // TODO: add part rotation to the manual xml generation
                //HelperMethods.RotateSVG(left, 90);
            }

            if (translatePieces)
            {
                decimal leftTranslateX = padding;
                decimal leftTranslateY = padding + dimensionZ + padding;

                AddPolygon(svgDoc, "Left", PointData[CubeSide.Left], leftTranslateX, leftTranslateY, UseDebugMode);
            }
            else
            {
                AddPolygon(svgDoc, "Left", PointData[CubeSide.Left], 0, 0, UseDebugMode);
            }

            if (RotateParts)
            {
                throw new NotImplementedException("Rotating parts is not implemented.");
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                // TODO: add part rotation to the manual xml generation
                //HelperMethods.RotateSVG(right, 270);
            }

            if (translatePieces)
            {
                decimal rightTranslateX = padding + dimensionZ + padding + dimensionX + padding;
                decimal rightTranslateY = padding + dimensionZ + padding;

                AddPolygon(svgDoc, "Right", PointData[CubeSide.Right], rightTranslateX, rightTranslateY, UseDebugMode);
            }
            else
            {
                AddPolygon(svgDoc, "Right", PointData[CubeSide.Right], 0, 0, UseDebugMode);
            }

            if (translatePieces)
            {
                decimal frontTranslateX = padding + dimensionZ + padding;
                decimal frontTranslateY = padding + dimensionZ + padding + dimensionY + padding;

                AddPolygon(svgDoc, "Front", PointData[CubeSide.Front], frontTranslateX, frontTranslateY, UseDebugMode);
            }
            else
            {
                AddPolygon(svgDoc, "Front", PointData[CubeSide.Front], 0, 0, UseDebugMode);
            }

            if (RotateParts)
            {
                throw new NotImplementedException("Rotating parts is not implemented.");
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                // TODO: add part rotation to the manual xml generation
                //HelperMethods.RotateSVG(back, 180);
            }

            if (translatePieces)
            {
                decimal backTranslateX = padding + dimensionZ + padding;
                decimal backTranslateY = padding;

                AddPolygon(svgDoc, "Back", PointData[CubeSide.Back], backTranslateX, backTranslateY, UseDebugMode);
            }
            else
            {
                AddPolygon(svgDoc, "Back", PointData[CubeSide.Back], 0, 0, UseDebugMode);
            }

            if (PointData.ContainsKey(CubeSide.Top))
            {
                decimal topTranslateX = padding + dimensionZ + padding + dimensionX + padding + dimensionZ + padding;
                decimal topTranslateY = padding + dimensionZ + padding;

                if (translatePieces)
                {
                    AddPolygon(svgDoc, "Top", PointData[CubeSide.Top], topTranslateX, topTranslateY, UseDebugMode);
                }
                else
                {
                    AddPolygon(svgDoc, "Top", PointData[CubeSide.Top], 0, 0, UseDebugMode);
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

        private void AddPolygon(XmlDocument Doc, string Id, List<Point> PointData, decimal TranslateX, decimal TranslateY, bool UseDebugMode)
        {
            var group = Doc.CreateElement("g", "http://www.w3.org/2000/svg");
            Doc.DocumentElement.AppendChild(group);

            var attrTranslate = Doc.CreateAttribute("transform");
            attrTranslate.Value = string.Format("translate({0} {1})", TranslateX, TranslateY);
            group.Attributes.Append(attrTranslate);

            AddPolygon(group, Id, PointData);

            if (UseDebugMode)
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