using System;
using System.Collections.Generic;
using LaserCutterTools.Common;
using LaserCutterTools.Common.Rendering;

namespace LaserCutterTools.GearBuilder
{
    internal sealed class RailBuilderSVG : IRailBuilderSVG
    {
        IPointGeneratorGuard guardGenerator;
        IPointGeneratorRack rackGenerator;
        IPointGeneratorCrossBeam crossBeamGenerator;
        IPointRendererSVG pointRendererSVG;

        public RailBuilderSVG(IPointGeneratorGuard PointGeneratorGuard, IPointGeneratorRack PointGeneratorRack, IPointGeneratorCrossBeam PointGeneratorCrossBeam, IPointRendererSVG PointRendererSVG)
        {
            guardGenerator = PointGeneratorGuard;
            rackGenerator = PointGeneratorRack;
            crossBeamGenerator = PointGeneratorCrossBeam;
            pointRendererSVG = PointRendererSVG;
        }

        public string BildRail(IMaterial Material, 
            IMachineSettings MachineSettings,
            double GearPitchDiameter,
            double GearPressureAngle,
            int GearNumTeeth,
            double GearDiametralPitch,
            double CrossBeamThickness,
            double CrossBeamContentsDepth,
            int RackNumTeeth,
            double supportBarWidth)
        {
            // rack info
            double circularPitch = (2 * Math.PI * (GearPitchDiameter / 2)) / GearNumTeeth;
            double backlash = 0.05;
            double clearance = 0.05;
            double addendum = 1 / GearDiametralPitch;

            var rackPart = rackGenerator.CreateRackWithSlots(Material, MachineSettings, RackNumTeeth, GearPressureAngle, circularPitch, backlash, clearance, addendum, supportBarWidth, CrossBeamThickness);

            // create a sandwitch part
            var dimensions = HelperMethods.GetPolygonDimension(rackPart);
            var guardPart = guardGenerator.CreateGuardWithSlots(dimensions.X, dimensions.Y, CrossBeamThickness, (double)MachineSettings.ToolSpacing, (double)Material.MaterialThickness);
            var crossBeamPart = crossBeamGenerator.CreateCrossBeam(Material, MachineSettings, CrossBeamThickness, CrossBeamContentsDepth);

            // collect all the parts
            var partsToRender = new Dictionary<string, List<PointDouble>>()
            {
                { "Guard 1", guardPart },
                { "Rack", rackPart },
                { "Guard 2", guardPart },
                { "Cross Beam 1", crossBeamPart },
                { "Cross Beam 2", crossBeamPart }
            };

            var r = new PointRendererSVG();
            var output = r.RenderPoints(partsToRender);

            return output;
        }
    }
}
