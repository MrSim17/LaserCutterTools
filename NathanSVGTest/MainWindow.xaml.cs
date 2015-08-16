using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SvgNet;
using SvgNet.SvgElements;
using SvgNet.SvgTypes;
using System.Drawing;
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
            var box = new BoxBuilder.BoxSquare
            {
                DimensionX = double.Parse(txtSizeX.Text),
                DimensionY = double.Parse(txtSizeY.Text),
                DimensionZ = double.Parse(txtSizeZ.Text)
            };

            var material = new Material
            {
                MaterialThickness = double.Parse(txtThickness.Text)
            };

            var machineSettings = new MachineSettings
            {
                ToolSpacing = double.Parse(txtTool.Text)
            };

            int tabsX = int.Parse(txtTabsX.Text);
            int tabsY = int.Parse(txtTabsY.Text);
            int tabsZ = int.Parse(txtTabsZ.Text);

            // TODO: make checkbox for rotating the parts to the correct orientation
            bool makeTopOpen = chkMakeOpen.IsChecked.GetValueOrDefault(false);

            StringLogger logger = new StringLogger();
            var ret = BoxBuilder.BoxBuilder.BuildBox(box, material, machineSettings, tabsX, tabsY, tabsZ, true, makeTopOpen, logger);
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
            //textBox1.Text = hash;
            textBox1.Text = ((StringLogger)builder.Logger).Log;
            OutputFile(hash);
        }
    }
}