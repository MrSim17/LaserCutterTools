using System.Collections.Generic;
using System;

namespace BoxBuilder
{
    // TODO: Should not be creating the points based on the SVG point system. The y deltas should be inverted and it should start at a
    // higher Y value.
    internal sealed class DividerPointGeneartor : IDividerPointGenerator
    {
        public List<Point> GeneratePoints(IMaterial Material, IMachineSettings MachineSettings, decimal Width, decimal Height, decimal SlotDepth, decimal SlotAngle)
        {
            if(SlotAngle != 0)
            {
                throw new NotImplementedException("Slot angles are not implemented.");
            }

            decimal deltaX = 0;
            decimal deltaY = 0;
            decimal curX = 0;
            decimal curY = 0;
            List<Point> points = new List<Point>();

            // create the starting point
            points.Add(new Point(curX, curY));

            // create the flat top
            // TODO: Might want to add some options to create space to grab stuff instead of a flat top.
            deltaX = Width + MachineSettings.ToolSpacing;
            deltaY = 0;

            curX += deltaX;
            curY += deltaY;

            points.Add(new Point(curX, curY));

            // create the right side tab
            deltaX = 0;
            deltaY = SlotDepth + MachineSettings.ToolSpacing;

            curX += deltaX;
            curY += deltaY;

            points.Add(new Point(curX, curY));

            // Come back in the material width
            deltaX = -Material.MaterialThickness;
            deltaY = 0;

            curX += deltaX;
            curY += deltaY;

            points.Add(new Point(curX, curY));

            // Finish out the height of the divider
            deltaX = 0;
            deltaY = (Height + MachineSettings.ToolSpacing) - (SlotDepth + MachineSettings.ToolSpacing);

            curX += deltaX;
            curY += deltaY;

            points.Add(new Point(curX, curY));

            // Come back the width of the divider part that is inside the box
            deltaX = -((Width + MachineSettings.ToolSpacing) - (Material.MaterialThickness * 2));
            deltaY = 0;

            curX += deltaX;
            curY += deltaY;

            points.Add(new Point(curX, curY));

            // Come back up most of the height of the divider
            deltaX = 0;
            deltaY = -((Height + MachineSettings.ToolSpacing) - (SlotDepth + MachineSettings.ToolSpacing));

            curX += deltaX;
            curY += deltaY;

            points.Add(new Point(curX, curY));


            // Create the tab for the left side
            deltaX = -Material.MaterialThickness;
            deltaY = 0;

            curX += deltaX;
            curY += deltaY;

            points.Add(new Point(curX, curY));

            // From here the line just goes back to the origin

            return points;
        }
    }
}
