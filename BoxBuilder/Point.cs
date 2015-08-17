using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxBuilder
{
    public struct Point
    {
        public Point(decimal X, decimal Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public decimal X
        {
            get; set;
        }

        public decimal Y
        {
            get; set;
        }
    }
}
