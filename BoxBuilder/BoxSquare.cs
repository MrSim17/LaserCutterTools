using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxBuilder
{
    public sealed class BoxSquare : IBox, IBoxSquare
    {
        public double DimensionX { get; set; }
        public double DimensionY { get; set; }
        public double DimensionZ { get; set; }
    }
}