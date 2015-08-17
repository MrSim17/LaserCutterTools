using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxBuilder
{
    public sealed class Material : IMaterial
    {
        public decimal MaterialThickness
        {
            get;
            set;
        }
    }
}
