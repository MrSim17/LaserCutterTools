using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxBuilder
{
    public interface IBoxSquare
    {
        decimal DimensionX { get; set; }
        decimal DimensionY { get; set; }
        decimal DimensionZ { get; set; }
    }
}
