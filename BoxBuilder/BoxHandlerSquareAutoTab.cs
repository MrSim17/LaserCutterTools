// TODO: Consider making the auto tab just a featuer of the box handler.

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ColorProvider;
//using Common;

//namespace BoxBuilder
//{
//    internal sealed class BoxHandlerSquareAutoTab : IBoxHandlerSquare
//    {
//        public BoxHandlerSquareAutoTab()
//        {
//        }

//        public string HandleBox(IBoxSquare Box, IMaterial Material, IMachineSettings MachineSettings)
//        {
//            int tabsX = (int)Math.Floor((Box.DimensionX / 0.5) / 2);
//            int tabsY = (int)Math.Floor((Box.DimensionY / 0.5) / 2);
//            int tabsZ = (int)Math.Floor((Box.DimensionZ / 0.5) / 2);

//            BoxHandlerSquare internalBoxHandler = new BoxHandlerSquare(tabsX, tabsY, tabsZ);

//            return internalBoxHandler.HandleBox(Box, Material, MachineSettings);
//        }

//        public ILogger Logger
//        {
//            get;
//            set;
//        }

//        public IColorProvider ColorProvider
//        {
//            get;
//            set;
//        }
//    }
//}