﻿using System.Collections.Generic;
using LaserCutterTools.BoxBuilder;
using LaserCutterTools.Common.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoxBuilderTests
{
    // TODO: The piece point generation tests only cover one set of start configurations. Far from complete.
    // TODO: All sloted side tests should have a slot depth different that material thickness so we can make sure they are doing their thing.
    // TODO: There are no tests for slots of a 0 width. These should be the same as the flat side config
    // TODO: There are no tests for a 0 slot count. These should be the same as the flat side config
    [TestClass]
    public abstract class PiecePointGenerationTests
    {
        internal abstract IPointGeneratorPiece GetPointGenerator();
        internal abstract ILogger GetLogger();

        // ============= Default No Tool Spacing ===============
        [TestMethod]
        public void PieceDimension_Default()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, 0.2M, 0, logger);

            TestUtilities.CheckDimensions(dimensionX, dimensionY, pointData);
        }

        [TestMethod]
        public void TabPointValidation_Default()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal materialThickness = 0.2M;
            decimal toolSpacing = 0;
            Rectangle outerRect = new Rectangle(0, 0, dimensionX + toolSpacing, dimensionY + toolSpacing);
            Rectangle innerRect = new Rectangle(materialThickness, materialThickness, dimensionX + toolSpacing - (materialThickness * 2), dimensionY + toolSpacing - (materialThickness * 2));

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, logger);

            TestUtilities.CheckPointsLieOnPolygons(new List<Rectangle> { outerRect, innerRect }, pointData);
        }

        [TestMethod]
        public void HorizontalOrVertical_Default()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal materialThickness = 0.2M;
            decimal toolSpacing = 0;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, logger);

            TestUtilities.CheckLinesHaveNoSlope(pointData);
        }

        [TestMethod]
        public void OriginCheck_Default()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal materialThickness = 0.2M;
            decimal toolSpacing = 0;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, logger);

            Assert.AreEqual(0, TestUtilities.GetValueMinX(pointData), "Piece should always sit at the origin.");
            Assert.AreEqual(0, TestUtilities.GetValueMinY(pointData), "Piece should always sit at the origin.");
        }

        // ============= Default With Tool Spacing ===============
        [TestMethod]
        public void PieceDimension_WithToolSpacing()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal toolspacing = 0.002M;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, 0.2M, toolspacing, logger);

            TestUtilities.CheckDimensions(dimensionX + toolspacing, dimensionY + toolspacing, pointData);
        }

        [TestMethod]
        public void TabPointValidation_WithToolSpacing()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal materialThickness = 0.2M;
            decimal toolSpacing = 0.02M;
            Rectangle outerRect = new Rectangle(0, 0, dimensionX + toolSpacing, dimensionY + toolSpacing);
            Rectangle innerRect = new Rectangle(materialThickness, materialThickness, dimensionX + toolSpacing - (materialThickness * 2), dimensionY + toolSpacing - (materialThickness * 2));

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, logger);

            TestUtilities.CheckPointsLieOnPolygons(new List<Rectangle> { outerRect, innerRect }, pointData);
        }

        [TestMethod]
        public void HorizontalOrVertical_WithToolSpacing()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal materialThickness = 0.2M;
            decimal toolSpacing = 0.02M;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, logger);

            TestUtilities.CheckLinesHaveNoSlope(pointData);
        }

        [TestMethod]
        public void OriginCheck_WithToolSpacing()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal materialThickness = 0.2M;
            decimal toolSpacing = 0.02M;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, logger);

            Assert.AreEqual(0, TestUtilities.GetValueMinX(pointData), "Piece should always sit at the origin.");
            Assert.AreEqual(0, TestUtilities.GetValueMinY(pointData), "Piece should always sit at the origin.");
        }


        // ============= Flat Side No Tool Spacing ===============
        [TestMethod]
        public void PieceDimension_FlatSide()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal toolspacing = 0;
            List<PieceSide> FlatSides = new List<PieceSide>
            {
                PieceSide.X,
                PieceSide.XMinus,
                PieceSide.Y,
                PieceSide.YMinus
            };

            for (int i = 0; i < FlatSides.Count; i++)
            {
                var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, 0.2M, toolspacing, FlatSides[i], logger);

                TestUtilities.CheckDimensions(dimensionX + toolspacing, dimensionY + toolspacing, pointData);
            }
        }

        [TestMethod]
        public void TabPointValidation_FlatSide()
        {
            // TODO: the tab validation should do the flat side on all sides. Currently it only does it on one side.
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal materialThickness = 0.2M;
            decimal toolSpacing = 0.0M;
            Rectangle outerRect = new Rectangle(0, 0, dimensionX + toolSpacing, dimensionY + toolSpacing);
            Rectangle innerRect = new Rectangle(materialThickness, materialThickness, dimensionX + toolSpacing - (materialThickness * 2), dimensionY + toolSpacing - (materialThickness * 2));
            List<PieceSide> FlatSides = new List<PieceSide>
            {
                PieceSide.X,
                PieceSide.XMinus,
                PieceSide.Y,
                PieceSide.YMinus
            };

            for (int i = 0; i < FlatSides.Count; i++)
            {

                var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, FlatSides[i], logger);

                TestUtilities.CheckPointsLieOnPolygons(new List<Rectangle> { outerRect, innerRect }, pointData);
            }
        }

        [TestMethod]
        public void HorizontalOrVertical_FlatSide()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal materialThickness = 0.2M;
            decimal toolSpacing = 0M;
            List<PieceSide> FlatSides = new List<PieceSide>
            {
                PieceSide.X,
                PieceSide.XMinus,
                PieceSide.Y,
                PieceSide.YMinus
            };

            for (int i = 0; i < FlatSides.Count; i++)
            {
                var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, FlatSides[i], logger);

                TestUtilities.CheckLinesHaveNoSlope(pointData);
            }
        }

        [TestMethod]
        public void OriginCheck_FlatSide()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal materialThickness = 0.2M;
            decimal toolSpacing = 0M;
            List<PieceSide> FlatSides = new List<PieceSide>
            {
                PieceSide.X,
                PieceSide.XMinus,
                PieceSide.Y,
                PieceSide.YMinus
            };

            for (int i = 0; i < FlatSides.Count; i++)
            {
                var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, FlatSides[i], logger);

                Assert.AreEqual(0, TestUtilities.GetValueMinX(pointData), "Piece should always sit at the origin.");
                Assert.AreEqual(0, TestUtilities.GetValueMinY(pointData), "Piece should always sit at the origin.");
            }
        }


        // ============= Flat Side With Tool Spacing ===============
        [TestMethod]
        public void PieceDimension_FlatSide_ToolSpacing()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal toolspacing = 0.002M;
            List<PieceSide> FlatSides = new List<PieceSide>
            {
                PieceSide.X,
                PieceSide.XMinus,
                PieceSide.Y,
                PieceSide.YMinus
            };

            for (int i = 0; i < FlatSides.Count; i++)
            {
                var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, 0.2M, toolspacing, FlatSides[i], logger);

                TestUtilities.CheckDimensions(dimensionX + toolspacing, dimensionY + toolspacing, pointData);
            }
        }

        [TestMethod]
        public void TabPointValidation_FlatSide_ToolSpacing()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal materialThickness = 0.2M;
            decimal toolSpacing = 0.0M;
            Rectangle outerRect = new Rectangle(0, 0, dimensionX + toolSpacing, dimensionY + toolSpacing);
            Rectangle innerRect = new Rectangle(materialThickness, materialThickness, dimensionX + toolSpacing - (materialThickness * 2), dimensionY + toolSpacing - (materialThickness * 2));
            List<PieceSide> FlatSides = new List<PieceSide>
            {
                PieceSide.X,
                PieceSide.XMinus,
                PieceSide.Y,
                PieceSide.YMinus
            };

            for (int i = 0; i < FlatSides.Count; i++)
            {
                var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, FlatSides[i], logger);

                TestUtilities.CheckPointsLieOnPolygons(new List<Rectangle> { outerRect, innerRect }, pointData);
            }
        }

        [TestMethod]
        public void HorizontalOrVertical_FlatSide_ToolSpacing()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal materialThickness = 0.2M;
            decimal toolSpacing = 0.02M;
            List<PieceSide> FlatSides = new List<PieceSide>
            {
                PieceSide.X,
                PieceSide.XMinus,
                PieceSide.Y,
                PieceSide.YMinus
            };

            for (int i = 0; i < FlatSides.Count; i++)
            {
                var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, FlatSides[i], logger);

                TestUtilities.CheckLinesHaveNoSlope(pointData);
            }
        }

        [TestMethod]
        public void OriginCheck_FlatSide_ToolSpacing()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal materialThickness = 0.2M;
            decimal toolSpacing = 0.02M;
            List<PieceSide> FlatSides = new List<PieceSide>
            {
                PieceSide.X,
                PieceSide.XMinus,
                PieceSide.Y,
                PieceSide.YMinus
            };

            for (int i = 0; i < FlatSides.Count; i++)
            {
                var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, FlatSides[i], logger);

                Assert.AreEqual(0, TestUtilities.GetValueMinX(pointData), "Piece should always sit at the origin.");
                Assert.AreEqual(0, TestUtilities.GetValueMinY(pointData), "Piece should always sit at the origin.");
            }
        }


        // ============= Slotted No Tool Spacing ===============
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

            TestUtilities.CheckDimensions(dimensionX + toolspacing, dimensionY + toolspacing, pointData);
        }

        [TestMethod]
        public void TabValidation_SlottedSide()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal toolSpacing = 0;
            decimal slotWidth = 0.2M;
            int slotCount = 3;
            decimal slotDepth = 0.2M;
            decimal materialThickness = 0.2M;
            Rectangle outerRect = new Rectangle(0, 0, dimensionX + toolSpacing, dimensionY + toolSpacing);
            Rectangle innerRect = new Rectangle(materialThickness, materialThickness, dimensionX + toolSpacing - (materialThickness * 2), dimensionY + toolSpacing - (materialThickness * 2));

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, slotDepth, slotWidth, slotCount, 0, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, PieceSide.X, logger);

            TestUtilities.CheckPointsLieOnPolygons(new List<Rectangle> { outerRect, innerRect }, pointData);
        }

        [TestMethod]
        public void HorizontalOrVertical_SlottedSide()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal toolSpacing = 0;
            decimal slotWidth = 0.2M;
            int slotCount = 3;
            decimal slotDepth = 0.2M;
            decimal materialThickness = 0.2M;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, slotDepth, slotWidth, slotCount, 0, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, PieceSide.X, logger);

            TestUtilities.CheckLinesHaveNoSlope(pointData);
        }

        [TestMethod]
        public void OriginCheck_SlottedSide()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal toolSpacing = 0;
            decimal slotWidth = 0.2M;
            int slotCount = 3;
            decimal slotDepth = 0.2M;
            decimal materialThickness = 0.2M;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, slotDepth, slotWidth, slotCount, 0, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, PieceSide.X, logger);

            Assert.AreEqual(0, TestUtilities.GetValueMinX(pointData), "Piece should always sit at the origin.");
            Assert.AreEqual(0, TestUtilities.GetValueMinY(pointData), "Piece should always sit at the origin.");
        }


        // ============= Slotted With Tool Spacing ===============
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

            TestUtilities.CheckDimensions(dimensionX + toolspacing, dimensionY + toolspacing, pointData);
        }

        [TestMethod]
        public void TabValidation_SlottedSide_ToolSpacing()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal toolSpacing = 0;
            decimal slotWidth = 0.2M;
            int slotCount = 3;
            decimal slotDepth = 0.2M;
            decimal materialThickness = 0.2M;
            Rectangle outerRect = new Rectangle(0, 0, dimensionX + toolSpacing, dimensionY + toolSpacing);
            Rectangle innerRect = new Rectangle(materialThickness, materialThickness, dimensionX + toolSpacing - (materialThickness * 2), dimensionY + toolSpacing - (materialThickness * 2));

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, slotDepth, slotWidth, slotCount, 0, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, PieceSide.X, logger);

            TestUtilities.CheckPointsLieOnPolygons(new List<Rectangle> { outerRect, innerRect }, pointData);
        }

        [TestMethod]
        public void HorizontalOrVertical_SlottedSide_ToolSpacing()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal toolSpacing = 0.02M;
            decimal slotWidth = 0.2M;
            int slotCount = 3;
            decimal slotDepth = 0.2M;
            decimal materialThickness = 0.2M;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, slotDepth, slotWidth, slotCount, 0, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, PieceSide.X, logger);

            TestUtilities.CheckLinesHaveNoSlope(pointData);
        }

        [TestMethod]
        public void OriginCheck_SlottedSide_ToolSpacing()
        {
            var pointGenerator = GetPointGenerator();
            var logger = GetLogger();
            decimal dimensionX = 1.5M;
            decimal dimensionY = 1.5M;
            decimal toolSpacing = 0.02M;
            decimal slotWidth = 0.2M;
            int slotCount = 3;
            decimal slotDepth = 0.2M;
            decimal materialThickness = 0.2M;

            var pointData = pointGenerator.CreateTabedObject(dimensionX, dimensionY, 3, 3, slotDepth, slotWidth, slotCount, 0, TabPosition.Crest, TabPosition.Trough, TabPosition.Crest, TabPosition.Trough, materialThickness, toolSpacing, PieceSide.X, logger);

            Assert.AreEqual(0, TestUtilities.GetValueMinX(pointData), "Piece should always sit at the origin.");
            Assert.AreEqual(0, TestUtilities.GetValueMinY(pointData), "Piece should always sit at the origin.");
        }
    }
}
