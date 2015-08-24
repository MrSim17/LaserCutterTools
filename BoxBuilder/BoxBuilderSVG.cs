using Common;


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
        ILogger logger;
        int tabsX;
        int tabsY;
        int tabsZ;

        public BoxBuilderSVG(IBoxPointGenerator PointGenerator, IBoxPointRendererSVG PointRenderer, int TabsX, int TabsY, int TabsZ, ILogger Logger = null)
        {
            pointGenerator = PointGenerator;
            pointRenderer = PointRenderer;
            logger = Logger;

            tabsX = TabsX;
            tabsY = TabsY;
            tabsZ = TabsZ;
        }

        public string HandleBox(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings, bool MakeBoxOpen = false, bool RotateParts = false)
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
                tabsX,
                tabsY,
                tabsZ,
                MakeBoxOpen);

            var renderedBox = pointRenderer.RenderPoints(pointData, RotateParts);

            return renderedBox;
        }
    }
}
