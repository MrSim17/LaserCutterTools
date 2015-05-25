using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SvgNet;
using SvgNet.SvgElements;
using System.Drawing;
using ColorProvider;
using Common;

namespace BoxBuilder
{
    public sealed class BoxHandlerSquareGeneric : IBoxHandlerSquare
    {
        bool translatePieces = true;
        ILogger logger = new NullLogger();
        IColorProvider colorProvider = new ColorProviderAllBlack();
        StartPositionConfiguration pStartConfig;
        bool makeBoxOpen = false;
        PieceSide? flatSide = null;

        public IColorProvider ColorProvider
        {
            get { return colorProvider; }
            set { colorProvider = value; }
        }

        public ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        SvgSvgElement root;
        IMaterial pMaterial;
        IBoxSquare pBox;
        IMachineSettings pMachineSettings;

        int pTabsX = 0;
        int pTabsY = 0;
        int pTabsZ = 0;

        double padding = 0.2;

        bool pRotateParts = false;

        public BoxHandlerSquareGeneric(StartPositionConfiguration StartConfig, int TabsX, int TabsY, int TabsZ, bool RotateParts = false, bool MakeBoxOpen = false)
        {
            makeBoxOpen = MakeBoxOpen;

            if (makeBoxOpen)
            {
                flatSide = PieceSide.X;
            }

            pStartConfig = StartConfig;
            pTabsX = TabsX;
            pTabsY = TabsY;
            pTabsZ = TabsZ;

            pRotateParts = RotateParts;

            root = new SvgSvgElement("20in", "12in", "0,0 20,12");
        }

        public string HandleBox(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings)
        {
            pBox = Box;
            pMaterial = Material;
            pMachineSettings = MachineSettings;

            if (!makeBoxOpen)
            {
                // omit the top if the box is open
                HandleTop();
            }

            HandleBottom();
            HandleLeft();
            HandleRight();
            HandleFront();
            HandleBack();

            return root.WriteSVGString(false);
        }

        private void HandleTop()
        {
            SideStartPositionConfiguration startConfig = pStartConfig.Top;
            var polygon = HandlePiece(startConfig, pTabsX, pTabsY, pBox.DimensionX, pBox.DimensionY, pMaterial, pMachineSettings, colorProvider, logger, null);
            polygon.Id = "Top";

            double translateX = padding + pBox.DimensionZ + padding + pBox.DimensionX + padding + pBox.DimensionZ + padding;
            double translateY = padding + pBox.DimensionZ + padding;

            if (translatePieces)
            {
                SvgGroupElement group = new SvgGroupElement();
                group.Transform.Add(string.Format("translate({0}, {1})", translateX, translateY));
                group.AddChild(polygon);

                root.AddChild(group);
            }
            else
            {
                root.AddChild(polygon);
            }
        }

        private void HandleBottom()
        {
            SideStartPositionConfiguration startConfig = pStartConfig.Bottom;
            var polygon = HandlePiece(startConfig, pTabsX, pTabsY, pBox.DimensionX, pBox.DimensionY, pMaterial, pMachineSettings, colorProvider, logger, null);
            polygon.Id = "Bottom";

            double translateX = padding + pBox.DimensionZ + padding;
            double translateY = padding + pBox.DimensionZ + padding;

            if (translatePieces)
            {
                SvgGroupElement group = new SvgGroupElement();
                group.Transform.Add(string.Format("translate({0}, {1})", translateX, translateY));
                group.AddChild(polygon);

                root.AddChild(group);
            }
            else
            {
                root.AddChild(polygon);
            }
        }

        private void HandleBack()
        {
            // TODO: instead of rotating with a transform change the parameters for generating the piece
            SideStartPositionConfiguration startConfig = pStartConfig.Back;
            var polygon = HandlePiece(startConfig, pTabsX, pTabsZ, pBox.DimensionX, pBox.DimensionZ, pMaterial, pMachineSettings, colorProvider, logger, flatSide);
            polygon.Id = "Back";

            if (pRotateParts)
            {
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                HelperMethods.RotateSVG(polygon, 180);
                //PointF actualD = HelperMethods.GetActualDimension(polygon);
                //polygon.Transform.Add(string.Format("translate({0}, {1})", -actualD.X*2, -actualD.Y*2));
            }

            double translateX = padding + pBox.DimensionZ + padding;
            double translateY = padding;

            if (translatePieces)
            {
                SvgGroupElement group = new SvgGroupElement();
                group.Transform.Add(string.Format("translate({0}, {1})", translateX, translateY));
                group.AddChild(polygon);

                root.AddChild(group);
            }
            else
            {
                root.AddChild(polygon);
            }
        }

        private void HandleFront()
        {
            SideStartPositionConfiguration startConfig = pStartConfig.Front;

            var polygon = HandlePiece(startConfig, pTabsX, pTabsZ, pBox.DimensionX, pBox.DimensionZ, pMaterial, pMachineSettings, colorProvider, logger, flatSide);
            polygon.Id = "Front";

            double translateX = padding + pBox.DimensionZ + padding;
            double translateY = padding + pBox.DimensionZ + padding + pBox.DimensionY + padding;

            if (translatePieces)
            {
                SvgGroupElement group = new SvgGroupElement();
                group.Transform.Add(string.Format("translate({0}, {1})", translateX, translateY));
                group.AddChild(polygon);

                root.AddChild(group);
            }
            else
            {
                root.AddChild(polygon);
            }
        }

        private void HandleLeft()
        {
            // TODO: instead of rotating with a transform change the parameters for generating the piece
            SideStartPositionConfiguration startConfig = pStartConfig.Left;
            var polygon = HandlePiece(startConfig, pTabsY, pTabsZ, pBox.DimensionY, pBox.DimensionZ, pMaterial, pMachineSettings, colorProvider, logger, flatSide);
            polygon.Id = "Left";

            if (pRotateParts)
            {
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                HelperMethods.RotateSVG(polygon, 90);
                //PointF actualD = HelperMethods.GetActualDimension(polygon);
                //polygon.Transform.Add(string.Format("translate({0}, {1})", -actualD.Y*2, -actualD.X*2));
            }

            double translateX = padding;
            double translateY = padding + pBox.DimensionZ + padding;

            if (translatePieces)
            {
                SvgGroupElement group = new SvgGroupElement();
                group.Transform.Add(string.Format("translate({0}, {1})", translateX, translateY));
                group.AddChild(polygon);

                root.AddChild(group);
            }
            else
            {
                root.AddChild(polygon);
            }
        }

        private void HandleRight()
        {
            // TODO: instead of rotating with a transform change the parameters for generating the piece
            SideStartPositionConfiguration startConfig = pStartConfig.Right;
            var polygon = HandlePiece(startConfig, pTabsY, pTabsZ, pBox.DimensionY, pBox.DimensionZ, pMaterial, pMachineSettings, colorProvider, logger, flatSide);
            polygon.Id = "Right";

            if (pRotateParts)
            {
                // TODO: instead of rotating with a transform change the parameters for generating the piece
                HelperMethods.RotateSVG(polygon, 270);
                //PointF actualD = HelperMethods.GetActualDimension(polygon);
                //polygon.Transform.Add(string.Format("translate({0}, {1})", -actualD.X*2, -actualD.Y*2));
            }

            double translateX = padding + pBox.DimensionZ + padding + pBox.DimensionX + padding;
            double translateY = padding + pBox.DimensionZ + padding;

            if (translatePieces)
            {
                SvgGroupElement group = new SvgGroupElement();
                group.Transform.Add(string.Format("translate({0}, {1})", translateX, translateY));
                group.AddChild(polygon);

                root.AddChild(group);
            }
            else
            {
                root.AddChild(polygon);
            }
        }

        private static SvgPolygonElement HandlePiece(
            SideStartPositionConfiguration StartConfiguration, 
            int TabsX,
            int TabsY,
            double DimensionX, 
            double DimensionY, 
            IMaterial Material, 
            IMachineSettings MachineSettings,
            IColorProvider ColorProvider,
            ILogger Logger,
            PieceSide? FlatSide
            )
        {
            PointF[] polygonPoints = HelperMethods.CreateTabedObject(
                DimensionX,
                DimensionY,
                TabsX,
                TabsY,
                StartConfiguration.StartPositionX,
                StartConfiguration.StartPositionY,
                StartConfiguration.StartPositionXMinus,
                StartConfiguration.StartPositionYMinus,
                Material.MaterialThickness,
                MachineSettings.ToolSpacing,
                Logger,
                FlatSide);

            var polygon = ConvertPointsToSVGPolygon(polygonPoints, ColorProvider.GetColor());

            return polygon;
        }

        private static SvgPolygonElement ConvertPointsToSVGPolygon(PointF[] Points, Color PieceColor)
        {
            var polygon = new SvgPolygonElement(new SvgNet.SvgTypes.SvgPoints(Points.ToArray()));
            polygon.Style = new SvgNet.SvgTypes.SvgStyle(new Pen(PieceColor, 0.0034f));

            return polygon;
        }
    }
}
