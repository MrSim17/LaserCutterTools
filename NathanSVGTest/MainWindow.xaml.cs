using System;
using System.Windows;
using BoxBuilder;
using ColorProvider;
using Common;

namespace NathanSVGTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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

            var boxHandler = BoxBuilderFactory.GetBoxHandler(logger);
            string ret = string.Empty;

            if (slotCount > 0 && makeTopOpen)
            {
                ret = boxHandler.BuildBox(box, material, machineSettings, tabsX, tabsY, tabsZ, 1, slotCount, 0);
            }
            else
            {
                ret = boxHandler.BuildBox(box, material, machineSettings, tabsX, tabsY, tabsZ, makeTopOpen);
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