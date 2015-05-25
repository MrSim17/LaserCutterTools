using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxBuilder
{
    public interface IMachineSettings
    {
        double MaxX { get; set; }
        double MaxY { get; set; }
        double ToolSpacing { get; set; }
    }
}
