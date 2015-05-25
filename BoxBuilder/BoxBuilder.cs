using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SvgNet;
using SvgNet.SvgElements;
using System.Drawing;
using System.Drawing.Drawing2D;
using ColorProvider;
using Common;

namespace BoxBuilder
{
    public sealed class BoxBuilder
    {
        public static IBoxHandlerSquare GetBoxHandler(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings)
        {
            return new BoxHandlerSquareAutoTab
            {
                ColorProvider = new ColorProviderAllDifferent(),
                Logger = new NullLogger()
            };
        }

        public static string BuildBox(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings, int TabsX, int TabsY, int TabsZ, bool RotateParts, bool MakeBoxOpen, ILogger Logger)
        {
            IBoxHandlerSquare handler = null;

            handler = new BoxHandlerSquare(TabsX, TabsY, TabsZ, RotateParts, MakeBoxOpen)
            {
                Logger = Logger,
                ColorProvider = new ColorProviderAllDifferent()
            };

            return handler.HandleBox(Box, Material, MachineSettings);
        }
    }
}