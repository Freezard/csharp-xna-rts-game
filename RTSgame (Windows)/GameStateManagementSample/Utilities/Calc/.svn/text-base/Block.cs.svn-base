using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RTSgame.Utilities
{
    // A part of a larger plane/space/map
    abstract class Block : IBlock
    {
        // world rectangle including overlapping area
        private Rectangle OverlappingBoundaryRectangle;

        // world rectangle not including overlapping area
        private Rectangle StrictBoundaryRectangle;

        // block point
        private Point blockPoint;

        // for subclasses only, do not actually use
        // should be protected
        public Block()
        {

        }

        public Rectangle GetOverlappingBoundaryRectangle()
        {
            return OverlappingBoundaryRectangle;
        }
        
        public void SetOverlappingBoundaryRectangle(Rectangle r)
        {
            OverlappingBoundaryRectangle = r;
        }

        public Rectangle GetStrictBoundaryRectangle()
        {
            return StrictBoundaryRectangle;
        }

        public void SetStrictBoundaryRectangle(Rectangle r)
        {
            StrictBoundaryRectangle = r;
        }

        public Point GetBlockPoint()
        {
            return blockPoint;
        }

        public void SetBlockPoint(Point p)
        {
            blockPoint = p;
        }

        public bool CoversOverlappingPosition(Point p)
        {
            //return OverlappingBoundaryRectangle.Contains(p);
            return Calculations.RectangleCoversPoint(OverlappingBoundaryRectangle, p);
        }

        public bool CoversOverlappingPosition(Vector2 v)
        {
            //return OverlappingBoundaryRectangle.Contains(Calculations.Vector2ToPoint(v));
            return Calculations.RectangleCoversV2(OverlappingBoundaryRectangle, v);
        }

        public bool CoversStrictPosition(Point p)
        {
            //return StrictBoundaryRectangle.Contains(p);
            return Calculations.RectangleCoversPoint(StrictBoundaryRectangle, p);
        }

        public bool CoversStrictPosition(Vector2 v)
        {
            //return StrictBoundaryRectangle.Contains(Calculations.Vector2ToPoint(v));
            return Calculations.RectangleCoversV2(StrictBoundaryRectangle, v);
        }

    }
}
