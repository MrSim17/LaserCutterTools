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
        ILogger Logger { get; set; }
        IColorProvider ColorProvider { get; set; }
        string HandleBox(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings);
    }
}
