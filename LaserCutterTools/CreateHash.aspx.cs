using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LaserCutterTools
{
    public partial class CreateHash : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool forceSave = chkForceDownload.Checked;
            float width = 0;
            float height = 0;
            float lineSpacing = 0;
            float gapWidth = 0;
            int hashCount = 0;

            float.TryParse(txtWidth.Text, out width);
            float.TryParse(txtHeight.Text, out height);
            float.TryParse(txtLineSpacing.Text, out lineSpacing);
            float.TryParse(txtGapWidth.Text, out gapWidth);
            int.TryParse(txtHashCount.Text, out hashCount);

            HashBuilder.HashBuilder builder = new HashBuilder.HashBuilder();

            string svgOutput = builder.BuildHash(width, height, lineSpacing, gapWidth, hashCount);

            HelperMethods.OutputSVGToResponse(svgOutput, forceSave);
        }
    }
}