using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;

namespace BoxBuilder
{
    internal static class SVGPointsExtension
    {
        /// <summary>
        /// Hack to get around there not being a Points property to get the actual points
        /// from the SvgPoints object anymore.
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static ArrayList GetPoints(this SvgNet.SvgTypes.SvgPoints pts)
        {
            Type tInfo = pts.GetType();
            FieldInfo fInfo = tInfo.GetField("_pts", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
            return (ArrayList)fInfo.GetValue(pts);
        }
    }
}
