using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework.Graphics;

namespace RTSgame.Utilities
{
    class HeightMap
    {
        public BasicEffect Effect;
        private IWorld World;
       
        public Array2D<SubHeightMap> HeightMaps;

        // Total size of each SubHeightMap
        private const int MaxSubHeightMapSize = 100;
        // The distance between each SubHeightMap. It's different,
        // cause the SubHeightMaps need to touch borders.
        private const int SubHeightMapDistance = MaxSubHeightMapSize - 1;

        // extra = how many additional worldPoint rows/columns this
        //         subHeightMap need in order to draw properly.
        private const int extra = 1;

        // safetyBorder = required so that we don't try to get values
        // outside the boundaries.
        private const int safetyBorder = 1;

        // Everything in world should be drawn from startLines to endLines.
        // Remember that, World.GetDimension specifies how wide in total
        // the world is, but (like an array) the last position in the world
        // is actually wideness - 1
        private int startLines;
        private int endLines;


        public HeightMap(IWorld world)
        {
            World = world;

            startLines = 0 + safetyBorder;
            endLines = World.GetDimension() - 1 - safetyBorder;
            
            // We calculate how many subheightmaps that fit into the range (endLines - startLines)
            // that we are interested in, and then we round UP, so that we get sufficient amount
            // of maps.
            HeightMaps = new Array2D<SubHeightMap>(
                (int)Math.Ceiling((double)(endLines - startLines) / (double)SubHeightMapDistance),
                (int)Math.Ceiling((double)(endLines - startLines) / (double)SubHeightMapDistance));

            HeightMaps.SetEveryPoint(CreateSubHeightMap);

        }

        private SubHeightMap CreateSubHeightMap(Location p)
        {

            int startX = Calculations.Clamp(p.X * SubHeightMapDistance, startLines, endLines);
            int endX = Calculations.Clamp((p.X + 1) * SubHeightMapDistance + extra, startLines, endLines);
            int startY = Calculations.Clamp(p.Y * SubHeightMapDistance, startLines, endLines);
            int endY = Calculations.Clamp((p.Y + 1) * SubHeightMapDistance + extra, startLines, endLines);

            return new SubHeightMap(World, new Location(startX, startY), new Location(endX, endY));
        }

    }
}
