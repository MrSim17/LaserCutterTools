using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoxBuilder;
using Common;
using System.Drawing;
using System.Linq;

namespace BoxBuilderTests
{
    [TestClass]
    public class PointGenerationTests
    {
        [TestMethod]
        public void MaterialSettings_Thickness()
        {
            var material = DefaultSettingsOneInchCube.MaterialSettings;
            var machineSettings = DefaultSettingsOneInchCube.MachineSettings;
            StartPositionConfiguration startConfig = DefaultSettingsOneInchCube.StartConfigs;
            var cube = DefaultSettingsOneInchCube.CubeDimensions;
            var pointGenerator = BoxBuilder.BoxBuilder.GetBoxPointGenerator(new NullLogger());

            var pointData = pointGenerator.GeneratePoints(startConfig, cube, material, machineSettings, 2, 2, 2, false);

            foreach (var key in pointData.Keys)
            {
                var xMin = pointData[key].Min(p => p.X);
                var yMin = pointData[key].Min(p => p.Y);

                var xMax = pointData[key].Max(p => p.X);
                var yMax = pointData[key].Max(p => p.Y);

                // The inner square should be a material thickness from all of the outer parts
                var xInnerMin = xMin + material.MaterialThickness;
                var yInnerMin = yMin + material.MaterialThickness;

                var xInnerMax = xMin + cube.DimensionX - material.MaterialThickness;
                var yInnerMax = yMin + cube.DimensionY - material.MaterialThickness;


                foreach (var point in pointData[key])
                {
                    bool isOnOuterSquareX = Math.Round(point.X, 3) == Math.Round(xMin, 3) || Math.Round(point.X, 3) == Math.Round(xMax, 3);
                    bool isOnOuterSquareY = Math.Round(point.Y, 3) == Math.Round(yMin, 3) || Math.Round(point.Y, 3) == Math.Round(yMax, 3);

                    bool isOnInnerSquareX = Math.Round(point.X, 3) == Math.Round(xInnerMin, 3) || Math.Round(point.X, 3) == Math.Round(xInnerMax, 3);
                    bool isOnInnerSquareY = Math.Round(point.Y, 3) == Math.Round(yInnerMin, 3) || Math.Round(point.Y, 3) == Math.Round(yInnerMax, 3);

                    Assert.IsTrue(isOnOuterSquareX || isOnOuterSquareY || isOnInnerSquareX || isOnInnerSquareY, "Point is not on either the outer or inner square.");
                }
            }
        }

        /// <summary>
        /// Make sure that overall the piece is larger than it needs to be because of the tool size
        /// </summary>
        [TestMethod]
        public void MachineSettings_ToolSize()
        {
            var material = new Material
            {
                MaterialThickness = .2M
            };

            var machineSettings = DefaultSettingsOneInchCube.MachineSettings;
            StartPositionConfiguration startConfig = DefaultSettingsOneInchCube.StartConfigs;

            int maxDimension = 3;
            int Dimension = 1;

            while (Dimension < maxDimension)
            {
                var pointGenerator = BoxBuilder.BoxBuilder.GetBoxPointGenerator(new NullLogger());

                var cube = new BoxSquare
                {
                    DimensionX = Dimension,
                    DimensionY = Dimension,
                    DimensionZ = Dimension
                };

                var pointData = pointGenerator.GeneratePoints(startConfig, cube, material, machineSettings, 2, 2, 2, false);

                foreach (var key in pointData.Keys)
                {
                    var xDim = Math.Abs(pointData[key].Max(p => p.X) - pointData[key].Min(p => p.X));
                    var yDim = Math.Abs(pointData[key].Max(p => p.Y) - pointData[key].Min(p => p.Y));

                    // Only half of a round tool would cut into something if it was directly on a cutting line.
                    var expectedDimension = Dimension + machineSettings.ToolSpacing;

                    Assert.AreEqual(expectedDimension, Math.Round(xDim, 3));
                    Assert.AreEqual(expectedDimension, Math.Round(yDim, 3));
                }


                Dimension++;
            }
        }
    }
}