using System.Collections.Generic;

namespace BoxBuilder
{
    public sealed class StartPositionConfiguration
    {
        private Dictionary<CubeSide, SideStartPositionConfiguration> startConfigs = new Dictionary<CubeSide, SideStartPositionConfiguration>();

        public StartPositionConfiguration(
            SideStartPositionConfiguration Top, 
            SideStartPositionConfiguration Bottom,
            SideStartPositionConfiguration Left, 
            SideStartPositionConfiguration Right,
            SideStartPositionConfiguration Front,
            SideStartPositionConfiguration Back)
        {
            startConfigs.Add(CubeSide.Top, Top);
            startConfigs.Add(CubeSide.Bottom, Bottom);
            startConfigs.Add(CubeSide.Left, Left);
            startConfigs.Add(CubeSide.Right, Right);
            startConfigs.Add(CubeSide.Front, Front);
            startConfigs.Add(CubeSide.Back, Back);
        }

        public SideStartPositionConfiguration Top
        {
            get { return startConfigs[CubeSide.Top]; }
        }

        public SideStartPositionConfiguration Bottom
        {
            get { return startConfigs[CubeSide.Bottom]; }
        }

        public SideStartPositionConfiguration Left
        {
            get { return startConfigs[CubeSide.Left]; }
        }

        public SideStartPositionConfiguration Right
        {
            get { return startConfigs[CubeSide.Right]; }
        }

        public SideStartPositionConfiguration Front
        {
            get { return startConfigs[CubeSide.Front]; }
        }

        public SideStartPositionConfiguration Back
        {
            get { return startConfigs[CubeSide.Back]; }
        }
    }

    public sealed class SideStartPositionConfiguration
    {
        public TabPosition StartPositionX { get; set; }
        public TabPosition StartPositionY { get; set; }
        public TabPosition StartPositionXMinus { get; set; }
        public TabPosition StartPositionYMinus { get; set; }

        public TabPosition GetStartPosition(PieceSide Side)
        {
            switch (Side)
            {
                case PieceSide.X:
                    return StartPositionX;
                case PieceSide.Y:
                    return StartPositionY;
                case PieceSide.XMinus:
                    return StartPositionXMinus;
                default:
                    return StartPositionYMinus;
            }
        }
    }
}