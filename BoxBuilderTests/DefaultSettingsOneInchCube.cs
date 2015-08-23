using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoxBuilder;

namespace BoxBuilderTests
{
    internal sealed class DefaultSettingsOneInchCube
    {
        public static IBoxSquare CubeDimensions
        {
            get
            {
                return new BoxSquare
                {
                    DimensionX = 1.5m,
                    DimensionY = 1.5m,
                    DimensionZ = 1.5m
                };
            }
        }

        public static IMaterial MaterialSettings
        {
            get
            {
                return new Material
                {
                    MaterialThickness = .2M
                };
            }
        }
 
        public static IMachineSettings MachineSettings
        {
            get
            {
                return new MachineSettings
                {
                    MaxX = 20,
                    MaxY = 12,
                    ToolSpacing = .02M
                };
            }
        }

        public static StartPositionConfiguration StartConfigs
        {
            get
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

                StartPositionConfiguration startConfig = new StartPositionConfiguration(topBottomConfig, topBottomConfig, sideConfig, sideConfig, sideConfig, sideConfig);

                return startConfig;
            }
        }
    }
}
