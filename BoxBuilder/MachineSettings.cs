﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxBuilder
{
    public sealed class MachineSettings : IMachineSettings
    {
        public decimal MaxX
        {
            get;
            set;
        }

        public decimal MaxY
        {
            get;
            set;
        }

        public decimal ToolSpacing
        {
            get;
            set;
        }
    }
}
