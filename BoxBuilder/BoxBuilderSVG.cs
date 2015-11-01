using Common;
using System.Collections.Generic;

namespace BoxBuilder
{
    /// <summary>
    /// Overall this class organizes all the work into one single output.
    /// 
    /// Responsibilities:
    /// 1. Manages all start configurations to make sure the pieces will fit together
    /// 2. Runs Piece point generation
    /// 3. Decides the divider size based on the slot direction
    /// 4. Runs divider point generation
    /// 5. Runs rendering
    /// 
    /// </summary>
    internal sealed class BoxBuilderSVG : IBoxBuilderSVG
    {
        IPointGeneratorBox pointGenerator;
        IPointRendererSVG pointRenderer;
        IPointGeneratorDivider dividerGenerator;
        ILogger logger;

        public BoxBuilderSVG(IPointGeneratorBox PointGenerator, IPointRendererSVG PointRenderer, IPointGeneratorDivider DividerGenerator, ILogger Logger = null)
        {
            pointGenerator = PointGenerator;
            pointRenderer = PointRenderer;
            dividerGenerator = DividerGenerator;
            logger = Logger;
        }

        public string BuildBox(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings, int TabsX, int TabsY, int TabsZ, bool MakeBoxOpen = false, bool RotateParts = false)
        {
            SideStartPositionConfiguration topBottomConfig = new SideStartPositionConfiguration
            {
                StartPositionX = TabPosition.Crest,
                StartPositionXMinus = TabPosition.Crest,
                StartPositionY = TabPosition.Crest,
                StartPositionYMinus = TabPosition.Crest
            };

            SideStartPositionConfiguration sideConfig = new SideStartPositionConfiguration
            {
                StartPositionX = TabPosition.Trough,
                StartPositionXMinus = TabPosition.Trough,
                StartPositionY = TabPosition.Trough,
                StartPositionYMinus = TabPosition.Crest
            };

            if (MakeBoxOpen)
            {
                sideConfig.StartPositionX = TabPosition.Crest;
            }

            StartPositionConfiguration startConfig = new StartPositionConfiguration(topBottomConfig, topBottomConfig, sideConfig, sideConfig, sideConfig, sideConfig);

            var pointData = pointGenerator.GeneratePoints(startConfig,
                Box,
                Material,
                MachineSettings,
                TabsX,
                TabsY,
                TabsZ,
                MakeBoxOpen);

            var renderedBox = pointRenderer.RenderPoints(pointData, null, RotateParts);

            return renderedBox;
        }

        public string BuildBox(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings, int TabsX, int TabsY, int TabsZ, decimal SlotDepth, decimal SlotPadding, int SlotCount, decimal SlotAngle, SlotDirection SlotDirection, bool RotateParts = false)
        {
            // handle all the box parts
            SideStartPositionConfiguration topBottomConfig = new SideStartPositionConfiguration
            {
                StartPositionX = TabPosition.Crest,
                StartPositionXMinus = TabPosition.Crest,
                StartPositionY = TabPosition.Crest,
                StartPositionYMinus = TabPosition.Crest
            };

            SideStartPositionConfiguration sideConfig = new SideStartPositionConfiguration
            {
                StartPositionX = TabPosition.Crest,
                StartPositionXMinus = TabPosition.Trough,
                StartPositionY = TabPosition.Trough,
                StartPositionYMinus = TabPosition.Crest
            };

            StartPositionConfiguration startConfig = new StartPositionConfiguration(topBottomConfig, topBottomConfig, sideConfig, sideConfig, sideConfig, sideConfig);

            var pointData = pointGenerator.GeneratePoints(startConfig,
                Box,
                Material,
                MachineSettings,
                TabsX,
                TabsY,
                TabsZ,
                SlotDepth, 
                SlotPadding,
                SlotCount, 
                SlotAngle,
                SlotDirection);

            // add the divider
            decimal dividerWidth = 0;
            decimal dividerHeight = Box.DimensionZ - Material.MaterialThickness;

            if (SlotDirection == SlotDirection.X)
            {
                dividerWidth = Box.DimensionY;
            }
            else
            {
                dividerWidth = Box.DimensionX;
            }

            var divider = dividerGenerator.GeneratePoints(Material, MachineSettings, dividerWidth, dividerHeight, SlotDepth, SlotAngle);
            var additionalParts = new Dictionary<PartType, List<Point>>() { { PartType.Divider, divider } };

            // Render the box
            var renderedBox = pointRenderer.RenderPoints(pointData, additionalParts, RotateParts);

            return renderedBox;
        }
    }
}
