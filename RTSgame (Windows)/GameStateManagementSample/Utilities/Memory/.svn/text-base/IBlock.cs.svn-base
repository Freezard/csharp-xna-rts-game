using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RTSgame.Utilities.Memory
{
    public interface IBlock
    {

        Boolean CoversOverlappingPosition(Point p);
        Boolean CoversOverlappingPosition(Vector2 v);

        Boolean CoversStrictPosition(Point p);
        Boolean CoversStrictPosition(Vector2 v);


        void SetBlockPoint(Point p);
        Point GetBlockPoint();


        void SetOverlappingBoundaryRectangle(Rectangle r);
        Rectangle GetOverlappingBoundaryRectangle();

        void SetStrictBoundaryRectangle(Rectangle r);
        Rectangle GetStrictBoundaryRectangle();

    }
}
