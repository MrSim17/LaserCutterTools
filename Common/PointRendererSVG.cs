using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using LaserCutterTools.Common.ColorMgmt;

namespace LaserCutterTools.Common.Rendering
{
    /// <summary>
    /// Responsibilities:
    /// 1. Convert raw points into SVG text output
    /// 2. Manage the colors and combinations of colors for the rendered pieces
    /// 3. Output debugging point values
    /// 4. Create a document to contain all parts in SVG format.
    /// </summary>
    public sealed class PointRendererSVG : IPointRendererSVG
    {
        IColorProvider colorProvider;

        public PointRendererSVG(IColorProvider ColorProvider)
        {
            colorProvider = ColorProvider;
        }

        public PointRendererSVG()
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

            AddPolygon(svgDoc.DocumentElement, "Polygon1", PointData, UseDebugMode);

            return SerializeXMLDoc(svgDoc);
        }

        public string RenderPoints(List<Tuple<string, List<Point>>> PointData, bool UseDebugMode = false)
        {
            XmlDocument svgDoc = InitDoc();

            foreach (var poly in PointData)
            {
                AddPolygon(svgDoc.DocumentElement, poly.Item1, poly.Item2, UseDebugMode);
            }

            return SerializeXMLDoc(svgDoc);
        }

        public string RenderPoints(Dictionary<string, List<Point>> PointData, bool UseDebugMode = false)
        {
            XmlDocument svgDoc = InitDoc();

            foreach (var poly in PointData)
            {
                AddPolygon(svgDoc.DocumentElement, poly.Key, poly.Value, UseDebugMode);
            }

            return SerializeXMLDoc(svgDoc);
        }

        public string RenderPoints(List<PointDouble> PointData, bool UseDebugMode = false)
        {
            var convertedPoints = HelperMethods.ConvertDoubleToDecimal(PointData);

            return RenderPoints(convertedPoints, UseDebugMode);
        }

        public string RenderPoints(List<Tuple<string, List<PointDouble>>> PointData, bool UseDebugMode = false)
        {
            var convertedPoints = new List<Tuple<string, List<Point>>>();

            foreach (var p in PointData)
            {
                convertedPoints.Add(new Tuple<string, List<Point>>(p.Item1, HelperMethods.ConvertDoubleToDecimal(p.Item2)));
            }

            return RenderPoints(convertedPoints, UseDebugMode);
        }

        public string RenderPoints(Dictionary<string, List<PointDouble>> PointData, bool UseDebugMode = false)
        {
            var convertedPoints = new Dictionary<string, List<Point>>();

            foreach (var kvp in PointData)
            {
                convertedPoints.Add(kvp.Key, HelperMethods.ConvertDoubleToDecimal(kvp.Value));
            }

            return RenderPoints(convertedPoints, UseDebugMode);
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

        private void AddPolygon(XmlElement Parent, string Id, List<Point> PointData, bool UseDebugMode = false)
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

            if (UseDebugMode)
            {
                // output debug information
                AddPointOutput(PointData, Parent);
            }
        }
    }
}