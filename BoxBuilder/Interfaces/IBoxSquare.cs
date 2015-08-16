using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxBuilder
{
    public interface IBoxSquare
    {
        double DimensionX { get; set; }
        double DimensionY { get; set; }
        double DimensionZ { get; set; }
    }
}
