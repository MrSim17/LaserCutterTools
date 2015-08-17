using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxBuilder
{
    public interface IMaterial
    {
        decimal MaterialThickness { get; set; }
    }
}