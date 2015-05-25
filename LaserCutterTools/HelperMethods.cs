using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaserCutterTools
{
    public sealed class HelperMethods
    {
        public static void OutputSVGToResponse(string SVGOutput, bool ForceSave)
        {
            HttpContext.Current.Response.Clear();

            if (ForceSave)
            {
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=test.svg");
            }

            HttpContext.Current.Response.ContentType = "image/svg+xml";
            HttpContext.Current.Response.Write(SVGOutput);

            HttpContext.Current.Response.End();
        }
    }
}