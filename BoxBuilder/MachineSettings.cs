using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxBuilder
{
    public sealed class MachineSettings : IMachineSettings
    {
        public double MaxX
        {
            get;
            set;
        }

        public double MaxY
        {
            get;
            set;
        }

        public double ToolSpacing
        {
            get;
            set;
        }
    }
}
