using System;
using System.Windows;
using BoxBuilder;
using LaserCutterTools.Common.Logging;
using LaserCutterTools.Common.ColorMgmt;

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
    }
}