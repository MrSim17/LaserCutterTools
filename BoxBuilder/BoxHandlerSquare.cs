using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SvgNet;
using SvgNet.SvgElements;
using System.Drawing;
using ColorProvider;
using Common;


namespace BoxBuilder
{
    public sealed class BoxHandlerSquare : IBoxHandlerSquare
    {
        IBoxHandlerSquare pInternalHandler;

        public IColorProvider ColorProvider
        {
            get { return pInternalHandler.ColorProvider; }
            set { pInternalHandler.ColorProvider = value; }
        }

        public ILogger Logger
        {
            get { return pInternalHandler.Logger; }
            set { pInternalHandler.Logger = value; }
        }

        public BoxHandlerSquare(int TabsX, int TabsY, int TabsZ, bool RotateParts = false, bool MakeBoxOpen = false)
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

            pInternalHandler = new BoxHandlerSquareGeneric(startConfig, TabsX, TabsY, TabsZ, RotateParts, MakeBoxOpen);
        }

        public string HandleBox(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings)
        {
            return pInternalHandler.HandleBox(Box, Material, MachineSettings);
        }
    }
}
