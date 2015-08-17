using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxBuilder
{
    public sealed class BoxSquare : IBox, IBoxSquare
    {
        public decimal DimensionX { get; set; }
        public decimal DimensionY { get; set; }
        public decimal DimensionZ { get; set; }
    }
}