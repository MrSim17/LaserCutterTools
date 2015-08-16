using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColorProvider;
using Common;

namespace BoxBuilder
{
    public interface IBoxHandlerSquare
    {
        string HandleBox(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings, bool MakeBoxOpen = false, bool RotateParts = false);
    }
}