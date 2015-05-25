using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LaserCutterTools
{
    public partial class CreateBox : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool forceSave = chkForceDownload.Checked;
            bool rotateParts = chkRotateParts.Checked;
            bool makeBoxOpen = chkMakeBoxOpen.Checked;
            float dimensionX = 0;
            float dimensionY = 0;
            float dimensionZ = 0;
            float materialThickness = 0;
            float toolDiameter = 0;
            int tabsX = 0;
            int tabsY = 0;
            int tabsZ = 0;

            float.TryParse(txtDimensionX.Text, out dimensionX);
            float.TryParse(txtDimensionY.Text, out dimensionY);
            float.TryParse(txtDimensionZ.Text, out dimensionZ);
            float.TryParse(txtMaterialThickness.Text, out materialThickness);
            float.TryParse(txtToolDiameter.Text, out toolDiameter);

            int.TryParse(txtTabsX.Text, out tabsX);
            int.TryParse(txtTabsY.Text, out tabsY);
            int.TryParse(txtTabsZ.Text, out tabsZ);

            BoxBuilder.IBoxSquare squareBox = new BoxBuilder.BoxSquare
            {
                DimensionX = dimensionX,
                DimensionY = dimensionY,
                DimensionZ = dimensionZ
            };

            BoxBuilder.IMaterial material = new BoxBuilder.Material
            {
                MaterialThickness = materialThickness
            };

            BoxBuilder.IMachineSettings machineSettings = new BoxBuilder.MachineSettings
            {
                ToolSpacing = toolDiameter
            };

            Common.StringLogger logger = new Common.StringLogger();

            string svgOutput = BoxBuilder.BoxBuilder.BuildBox(squareBox, material, machineSettings, tabsX, tabsY, tabsZ, rotateParts, makeBoxOpen, logger);

            if (chkLogOnly.Checked)
            {
                litLog.Text = logger.Log;
            }
            else
            {
                HelperMethods.OutputSVGToResponse(svgOutput, forceSave);
            }
        }
    }
}