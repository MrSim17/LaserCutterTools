using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoxBuilder;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoxBuilderTests
{
    [TestClass]
    public sealed class PiecePointGeneratorTests : PiecePointGenerationTests
    {
        internal override ILogger GetLogger()
        {
            return new NullLogger();
        }

        internal override IPointGeneratorPiece GetPointGenerator()
        {
            return new PointGeneratorPiece();
        }
    }
}
