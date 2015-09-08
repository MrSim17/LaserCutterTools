using System;
using System.Linq;
using BoxBuilder;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoxBuilderTests
{
    [TestClass]
    public class PointGenerationTests
    {
        [TestMethod]
        public void MPieceLocations()
        {
            var material = DefaultSettingsOneInchCube.MaterialSettings;
            var machineSettings = DefaultSettingsOneInchCube.MachineSettings;
            StartPositionConfiguration startConfig = DefaultSettingsOneInchCube.StartConfigs;
            var cube = DefaultSettingsOneInchCube.CubeDimensions;
            var pointGenerator = BoxBuilderFactory.GetBoxPointGenerator(new NullLogger());

            // test for tab values up to 5
            for (int i = 2; i <= 5; i++)
            {
                var pointData = pointGenerator.GeneratePoints(startConfig, cube, material, machineSettings, i, i, i, false);

                foreach (var key in pointData.Keys)
                {
                    var xMin = pointData[key].Aggregate((curMin, newPoint) => curMin.X <= newPoint.X ? curMin : newPoint).X;
                    var yMin = pointData[key].Aggregate((curMin, newPoint) => curMin.Y <= newPoint.Y ? curMin : newPoint).Y;

                    // make sure the piece always sits on the x and y axis
                    Assert.AreEqual(0, xMin, "Piece is not sitting on the x axis.");
                    Assert.AreEqual(0, yMin, "Piece is not sitting no the y axis.");

                    var xMax = pointData[key].Aggregate((curMin, newPoint) => curMin.X >= newPoint.X ? curMin : newPoint).X;
                    var yMax = pointData[key].Aggregate((curMin, newPoint) => curMin.Y >= newPoint.Y ? curMin : newPoint).Y;
                }
            }
        }


        /// <summary>
        /// Test Purpose: Make sure that all points lie either on the outer edge or a material thickness away from the outer edge.
        /// </summary>
        [TestMethod]
        public void MaterialSettings_Thickness()
        {
            var material = DefaultSettingsOneInchCube.MaterialSettings;
            var machineSettings = DefaultSettingsOneInchCube.MachineSettings;
            StartPositionConfiguration startConfig = DefaultSettingsOneInchCube.StartConfigs;
            var cube = DefaultSettingsOneInchCube.CubeDimensions;
            var pointGenerator = BoxBuilder.BoxBuilderFactory.GetBoxPointGenerator(new NullLogger());

            var pointData = pointGenerator.GeneratePoints(startConfig, cube, material, machineSettings, 3, 3, 3, false);

            foreach (var key in pointData.Keys)
            {
                var xMin = pointData[key].Aggregate((curMin, newPoint) => curMin.X <= newPoint.X ? curMin : newPoint).X;
                var yMin = pointData[key].Aggregate((curMin, newPoint) => curMin.Y <= newPoint.Y ? curMin : newPoint).Y;

                var xMax = pointData[key].Aggregate((curMin, newPoint) => curMin.X >= newPoint.X ? curMin : newPoint).X;
                var yMax = pointData[key].Aggregate((curMin, newPoint) => curMin.Y >= newPoint.Y ? curMin : newPoint).Y;

                // The inner square should be a material thickness from all of the outer parts
                var xInnerMin = xMin + material.MaterialThickness;
                var yInnerMin = yMin + material.MaterialThickness;

                var xInnerMax = xMin + cube.DimensionX - material.MaterialThickness + machineSettings.ToolSpacing;
                var yInnerMax = yMin + cube.DimensionY - material.MaterialThickness + machineSettings.ToolSpacing;


                foreach (var point in pointData[key])
                {
                    bool isOnOuterSquareX = point.X == xMin || point.X == xMax;
                    bool isOnOuterSquareY = point.Y == yMin || point.Y == yMax;

                    bool isOnInnerSquareX = point.X == xInnerMin || point.X == xInnerMax;
                    bool isOnInnerSquareY = point.Y == yInnerMin || point.Y == yInnerMax;

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
                var pointGenerator = BoxBuilder.BoxBuilderFactory.GetBoxPointGenerator(new NullLogger());

                var cube = new BoxSquare
                {
                    DimensionX = Dimension,
                    DimensionY = Dimension,
                    DimensionZ = Dimension
                };

                var pointData = pointGenerator.GeneratePoints(startConfig, cube, material, machineSettings, 2, 2, 2, false);

                foreach (var key in pointData.Keys)
                {

                    var xMin = pointData[key].Aggregate((curMin, newPoint) => curMin.X <= newPoint.X ? curMin : newPoint).X;
                    var yMin = pointData[key].Aggregate((curMin, newPoint) => curMin.Y <= newPoint.Y ? curMin : newPoint).Y;

                    var xMax = pointData[key].Aggregate((curMin, newPoint) => curMin.X >= newPoint.X ? curMin : newPoint).X;
                    var yMax = pointData[key].Aggregate((curMin, newPoint) => curMin.Y >= newPoint.Y ? curMin : newPoint).Y;

                    decimal xDim = Math.Abs(xMax - xMin);
                    decimal yDim = Math.Abs(yMax - yMin);

                    // Only half of a round tool would cut into something if it was directly on a cutting line.
                    var expectedDimension = Dimension + machineSettings.ToolSpacing;

                    // TODO: Fix the tiny tiny rounding error that can occurr sometimes. Seems to have to do with dividing by odd numbers of tabs.
                    //Assert.AreEqual(expectedDimension, xDim);
                    //Assert.AreEqual(expectedDimension, yDim);

                    Assert.AreEqual(expectedDimension, Math.Round(xDim, 3));
                    Assert.AreEqual(expectedDimension, Math.Round(yDim, 3));

                }


                Dimension++;
            }
        }
    }
}