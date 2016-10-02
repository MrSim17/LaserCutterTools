using System;
using System.Windows;
using BoxBuilder;
using LaserCutterTools.Common.Logging;
using LaserCutterTools.Common.ColorMgmt;
using LaserCutterTools.Common;
using System.Collections.Generic;
using LaserCutterTools.GearBuilder;

namespace NathanSVGTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // TODO: Add a UI function to save and load settings so I don't have to remember them.
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string output = OutputBoxBuilder();
            OutputFile(output);
        }

        private void OutputFile(string Body)
        {
            // TODO: make an option to select the location and file name for a box.
            using (System.IO.TextWriter tw = new System.IO.StreamWriter("c:\\temp\\test " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".svg"))
            {
                tw.Write(Body);
            }
        }

        private string OutputBoxBuilder()
        {
            var box = new BoxSquare
            {
                DimensionX = decimal.Parse(txtSizeX.Text),
                DimensionY = decimal.Parse(txtSizeY.Text),
                DimensionZ = decimal.Parse(txtSizeZ.Text)
            };

            var material = new Material
            {
                MaterialThickness = decimal.Parse(txtThickness.Text)
            };

            var machineSettings = new MachineSettings
            {
                ToolSpacing = decimal.Parse(txtTool.Text)
            };

            int tabsX = int.Parse(txtTabsX.Text);
            int tabsY = int.Parse(txtTabsY.Text);
            int tabsZ = int.Parse(txtTabsZ.Text);

            // TODO: make checkbox for rotating the parts to the correct orientation
            bool makeTopOpen = chkMakeOpen.IsChecked.GetValueOrDefault(false);
            int slotCount = int.Parse(txtNumSlots.Text);

            StringLogger logger = new StringLogger();

            var boxBuilder = BoxBuilderFactory.GetBoxBuilder(logger);
            string ret = string.Empty;

            if (slotCount > 0 && makeTopOpen)
            {
                decimal slotDepth = box.DimensionZ / 4; // TODO: Do not hard code slot depth
                decimal slotPadding = 0.01M; // TODO: Do not hard code slot padding

                // TODO: Make the slot depth selection either better or add something to the UI.
                // TODO: Add slot direction to the UI.
                // TODO: Add slot padding to the UI.
                ret = boxBuilder.BuildBox(box, material, machineSettings, tabsX, tabsY, tabsZ, slotDepth, slotPadding, slotCount, 0, SlotDirection.X);
            }
            else
            {
                ret = boxBuilder.BuildBox(box, material, machineSettings, tabsX, tabsY, tabsZ, makeTopOpen);
            }

            textBox1.Text = logger.Log;

            return ret;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            HashBuilder.HashBuilder builder = new HashBuilder.HashBuilder(new ColorProviderAlternating());

            float lineSpacing = 0.05f;
            float gapwidth = 0.05f;
            int hashCount = 4;
            float width = 4f;
            float height = 2f;

            string hash = builder.BuildHash(width, height, lineSpacing, gapwidth, hashCount);

            textBox1.Text = ((StringLogger)builder.Logger).Log;
            OutputFile(hash);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var gb = new PointGeneratorGear();
            // modifying only the pressure angle changes the tooth profile and changes the radius of the base circle

            int numTeeth = 30;
            double pitchDiameter = 3;
            double diametralPitch = 10;
            double pressureAngle = 20;

            var points = gb.CreateGear(numTeeth, pitchDiameter, diametralPitch, pressureAngle);//(30, 3, 10, 20, true);

            var r = new LaserCutterTools.Common.Rendering.PointRendererSVG();
            var output = r.RenderPoints(points);
            OutputFile(output);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            var material = new Material();
            material.MaterialThickness = 0.116M;

            var machineSettings = new MachineSettings();
            machineSettings.ToolSpacing = 0.012M;
            machineSettings.MaxX = 20;
            machineSettings.MaxY = 12;

            var rb = new PointGeneratorRack();

            // spur gear info
            double pitchDiameter = 3;
            var pressureAngle = 20;
            int numTeeth = 30;
            double diametralPitch = 10;

            // rack info
            double holderThickness = 0.5;
            var circularPitch = (2 * Math.PI * (pitchDiameter/2))/numTeeth;
            var backlash = 0.05;
            var clearance = 0.05;
            var addendum = 1/diametralPitch;
            var numTeethRack = 10;
            var supportBarWidth = 1;

            var rackPart = rb.CreateRackWithSlots(numTeethRack, pressureAngle, circularPitch, backlash, clearance, addendum, supportBarWidth, holderThickness, (double)material.MaterialThickness, (double)machineSettings.ToolSpacing);

            // create a sandwitch part
            var dimensions = HelperMethods.GetPolygonDimension(rackPart);
            var guardGenerator = new PointGeneratorGuard();
            var guardPart = guardGenerator.CreateGuardWithSlots(dimensions.X, dimensions.Y, holderThickness, (double)machineSettings.ToolSpacing, (double)material.MaterialThickness);

            // create part to hold the pieces together
            // TODO: Move this to a point generator
            double contentsWidth = (double)material.MaterialThickness * 3;
            //double contentsWidth = 0.5;
            double contentsDepth = 0.5;

            var holderPart = new List<PointDouble>
            {
                new PointDouble(0, (holderThickness + contentsDepth + (double)machineSettings.ToolSpacing)),
                new PointDouble(holderThickness + (double)machineSettings.ToolSpacing, holderThickness + contentsDepth + (double)machineSettings.ToolSpacing),
                new PointDouble(holderThickness + (double)machineSettings.ToolSpacing, holderThickness + (double)machineSettings.ToolSpacing),
                new PointDouble(holderThickness + contentsWidth, holderThickness + (double)machineSettings.ToolSpacing),
                new PointDouble(holderThickness + contentsWidth, holderThickness + contentsDepth + (double)machineSettings.ToolSpacing),
                new PointDouble(holderThickness + contentsWidth + holderThickness + (double)machineSettings.ToolSpacing, holderThickness + contentsDepth + (double)machineSettings.ToolSpacing),
                new PointDouble(holderThickness + contentsWidth + holderThickness + (double)machineSettings.ToolSpacing, 0),
                new PointDouble(0, 0),
            };

            // collect all the parts
            var partsToRender = new Dictionary<string, List<PointDouble>>()
            {
                { "Guard 1", guardPart },
                { "Rack", rackPart },
                { "Guard 2", guardPart },
                { "Holder 1", holderPart },
                { "Holder 2", holderPart }
            };

            var r = new LaserCutterTools.Common.Rendering.PointRendererSVG();
            var output = r.RenderPoints(partsToRender);

            OutputFile(output);
        }
    }
}