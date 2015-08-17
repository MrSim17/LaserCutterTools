﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxBuilder
{
    public interface IMachineSettings
    {
        decimal MaxX { get; set; }
        decimal MaxY { get; set; }
        decimal ToolSpacing { get; set; }
    }
}
