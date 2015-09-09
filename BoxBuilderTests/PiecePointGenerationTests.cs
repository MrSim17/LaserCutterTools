using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoxBuilder;
using System.Collections.Generic;

namespace BoxBuilderTests
{
    [TestClass]
    public abstract class PiecePointGenerationTests
    {
        internal abstract IPiecePointGenerator GetPointGenerator();
        internal abstract Common.ILogger GetLogger();

        [TestMethod]
        public void PieceDimension_Default()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, 0.2M, 0, logger);

            CheckDimensions(dimensionX, dimensionY, pointData);
        }

        [TestMethod]
        public void PieceDimension_WithToolSpacing()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal toolspacing = 0.002M;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, 0.2M, toolspacing, logger);

            CheckDimensions(dimensionX + toolspacing, dimensionY + toolspacing, pointData);
        }

        [TestMethod]
        public void PieceDimension_FlatSide()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal toolspacing = 0;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, 0.2M, toolspacing, PieceSide.X, logger);

            CheckDimensions(dimensionX + toolspacing, dimensionY + toolspacing, pointData);
        }

        [TestMethod]
        public void PieceDimension_FlatSide_ToolSpacing()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal toolspacing = 0.002M;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, 0.2M, toolspacing, PieceSide.X, logger);

            CheckDimensions(dimensionX + toolspacing, dimensionY + toolspacing, pointData);
        }

        [TestMethod]
        public void PieceDimension_SlottedSide()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal toolspacing = 0;
            decimal slotWidth = 0.2M;
            int slotCount = 3;
            decimal slotDepth = 0.2M;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, slotDepth, slotWidth, slotCount, 0, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, 0.2M, toolspacing, PieceSide.X, logger);

            CheckDimensions(dimensionX + toolspacing, dimensionY + toolspacing, pointData);
        }

        [TestMethod]
        public void PieceDimension_SlottedSide_ToolSpacing()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal toolspacing = 0.02M;
            decimal slotWidth = 0.2M;
            int slotCount = 3;
            decimal slotDepth = 0.2M;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, slotDepth, slotWidth, slotCount, 0, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, 0.2M, toolspacing, PieceSide.X, logger);

            CheckDimensions(dimensionX + toolspacing, dimensionY + toolspacing, pointData);
        }

        private static void CheckDimensions(decimal XDimension, decimal YDimension, List<Point> PointData)
        {
            try
            {
                Assert.AreEqual(XDimension, TestUtilities.CalculateDimensionX(PointData), "X dimension is incorrect.");
                Assert.AreEqual(YDimension, TestUtilities.CalculateDimensionY(PointData), "Y dimension is incorrect.");
            }
            catch (AssertFailedException)
            {
                TestUtilities.RenderPiece(PointData);
                throw;
            }
        }
    }
}
