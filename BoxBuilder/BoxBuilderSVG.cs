using Common;
using System.Collections.Generic;

namespace BoxBuilder
{
    /// <summary>
    /// This class handles organizing the generation of the points and then passing them to the rendering engine to produce the final result.
    /// Tihs class also has the responsibility of determining the start configurations for all the pieces to be rendered.
    /// </summary>
    internal sealed class BoxBuilderSVG : IBoxBuilderSVG
    {
        IBoxPointGenerator pointGenerator;
        IBoxPointRendererSVG pointRenderer;
        IDividerPointGenerator dividerGenerator;
        ILogger logger;

        public BoxBuilderSVG(IBoxPointGenerator PointGenerator, IBoxPointRendererSVG PointRenderer, IDividerPointGenerator DividerGenerator, ILogger Logger = null)
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

        public string BuildBox(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings, int TabsX, int TabsY, int TabsZ, decimal SlotDepth, int SlotCount, decimal SlotAngle, bool RotateParts = false)
        {
            // handle all the box parts
            var slotDirection = SlotDirection.X; // TODO: no hardcoded slot direction MUST ADD UI OPTION!

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
                SlotCount, 
                SlotAngle,
                slotDirection);

            // add the divider
            decimal dividerWidth = 0;
            decimal dividerHeight = Box.DimensionZ - Material.MaterialThickness;

            if (slotDirection == SlotDirection.X)
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
