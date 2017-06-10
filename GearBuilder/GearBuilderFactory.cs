using LaserCutterTools.Common.Rendering;

namespace LaserCutterTools.GearBuilder
{
    public sealed class GearBuilderFactory
    {
        public static IPointGeneratorCrossBeam GetPointGeneratorCrossBeam()
        {
            return new PointGeneratorCrossBeam();
        }

        public static IPointGeneratorGear GetPointGeneratorGear()
        {
            return new PointGeneratorGear();
        }

        public static IPointGeneratorGuard GetPointGeneratorGuard()
        {
            return new PointGeneratorGuard();
        }

        public static IPointGeneratorRack GetPointGeneratorRack()
        {
            return new PointGeneratorRack();
        }

        public static IRailBuilderSVG GetRailBuilderSVG()
        {
            var gCrossBeam = GetPointGeneratorCrossBeam();
            var gGear = GetPointGeneratorGear();
            var gGuard = GetPointGeneratorGuard();
            var gRack = GetPointGeneratorRack();
            IPointRendererSVG renderer = new PointRendererSVG();

            return new RailBuilderSVG(gGuard, gRack, gCrossBeam, renderer);
        }
    }
}
