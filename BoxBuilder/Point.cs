using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxBuilder
{
    public struct Point
    {
        public Point(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public double X
        {
            get; set;
        }

        public double Y
        {
            get; set;
        }
    }
}
