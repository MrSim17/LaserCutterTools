using System.Collections.Generic;
using System.Linq;
using Common;

namespace BoxBuilder
{
    /// <summary>
    /// The box point generator organizes the start configurations and other settings to generate points for each piece.
    /// The points generated here are not rendered into a visual form. They are the raw data.
    /// </summary>
    internal sealed class BoxPointGenerator : IBoxPointGenerator
    { 
        bool makeBoxOpen = false;
        ILogger logger = new NullLogger();
        PieceSide? flatSide = null;

        public IPiecePointGenerator PiecePointGenerator { get; set; }

        public ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        public BoxPointGenerator(IPiecePointGenerator PiecePointGenerator)
        {
            this.PiecePointGenerator = PiecePointGenerator;
        }

        public BoxPointGenerator(IPiecePointGenerator PiecePointGenerator, ILogger Logger)
        {
            this.PiecePointGenerator = PiecePointGenerator;
            this.logger = Logger;
        }

        public Dictionary<CubeSide, List<Point>> GeneratePoints(StartPositionConfiguration StartConfig, IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings, int TabsX, int TabsY, int TabsZ, bool MakeBoxOpen = false)
        {
            Dictionary<CubeSide, List<Point>> ret = new Dictionary<CubeSide, List<Point>>();

            if (makeBoxOpen)
            {
                flatSide = PieceSide.X;
            }

            if (!makeBoxOpen)
            {
                // omit the top if the box is open
                var pieceTop = PiecePointGenerator.CreateTabedObject(
                    Box.DimensionX,
                    Box.DimensionY,
                    TabsX,
                    TabsY,
                    StartConfig.Top.StartPositionX,
                    StartConfig.Top.StartPositionY,
                    StartConfig.Top.StartPositionXMinus,
                    StartConfig.Top.StartPositionYMinus,
                    Material.MaterialThickness,
                    MachineSettings.ToolSpacing,
                    Logger,
                    null);

                ret.Add(CubeSide.Top, pieceTop.ToList());
            }

            // Bottom
            var pieceBottom = PiecePointGenerator.CreateTabedObject(
                Box.DimensionX,
                Box.DimensionY,
                TabsX,
                TabsY,
                StartConfig.Bottom.StartPositionX,
                StartConfig.Bottom.StartPositionY,
                StartConfig.Bottom.StartPositionXMinus,
                StartConfig.Bottom.StartPositionYMinus,
                Material.MaterialThickness,
                MachineSettings.ToolSpacing,
                Logger,
                flatSide);

            // Left
            var pieceLeft = PiecePointGenerator.CreateTabedObject(
                Box.DimensionY,
                Box.DimensionZ,
                TabsY,
                TabsZ,
                StartConfig.Left.StartPositionX,
                StartConfig.Left.StartPositionY,
                StartConfig.Left.StartPositionXMinus,
                StartConfig.Left.StartPositionYMinus,
                Material.MaterialThickness,
                MachineSettings.ToolSpacing,
                Logger,
                flatSide);

            // Right
            var pieceRight = PiecePointGenerator.CreateTabedObject(
                Box.DimensionY,
                Box.DimensionZ,
                TabsY,
                TabsZ,
                StartConfig.Right.StartPositionX,
                StartConfig.Right.StartPositionY,
                StartConfig.Right.StartPositionXMinus,
                StartConfig.Right.StartPositionYMinus,
                Material.MaterialThickness,
                MachineSettings.ToolSpacing,
                Logger,
                flatSide);

            // Front
            var pieceFront = PiecePointGenerator.CreateTabedObject(
                Box.DimensionX,
                Box.DimensionZ,
                TabsX,
                TabsZ,
                StartConfig.Front.StartPositionX,
                StartConfig.Front.StartPositionY,
                StartConfig.Front.StartPositionXMinus,
                StartConfig.Front.StartPositionYMinus,
                Material.MaterialThickness,
                MachineSettings.ToolSpacing,
                Logger,
                flatSide);

            // Back
            var pieceBack = PiecePointGenerator.CreateTabedObject(
                Box.DimensionX,
                Box.DimensionZ,
                TabsX,
                TabsZ,
                StartConfig.Back.StartPositionX,
                StartConfig.Back.StartPositionY,
                StartConfig.Back.StartPositionXMinus,
                StartConfig.Back.StartPositionYMinus,
                Material.MaterialThickness,
                MachineSettings.ToolSpacing,
                Logger,
                flatSide);

            ret.Add(CubeSide.Back, pieceBack.ToList());
            ret.Add(CubeSide.Bottom, pieceBottom.ToList());
            ret.Add(CubeSide.Front, pieceFront.ToList());
            ret.Add(CubeSide.Left, pieceLeft.ToList());
            ret.Add(CubeSide.Right, pieceRight.ToList());

            return ret;
        }
    }
}
