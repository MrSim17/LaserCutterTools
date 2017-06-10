using LaserCutterTools.BoxBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LaserCutterTools.Common.Logging;

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
