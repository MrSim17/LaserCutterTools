using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColorProvider;
using Common;

namespace BoxBuilder
{
    public interface IBoxBuilderSVG
    {
        string HandleBox(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings, int TabsX, int TabsY, int TabsZ, bool MakeBoxOpen = false, bool RotateParts = false);
    }
}