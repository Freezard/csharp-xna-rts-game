using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;
using RTSgame.Utilities.Memory;
using RTSgame.Utilities.World;

namespace RTSgame.GameObjects.Abstract
{
    interface IWorld
    {
        /// <summary>
        /// Returns how many data values there are in each
        /// row and column in the world.
        /// </summary>
        /// <returns></returns>
        int GetDimension();

        /// <summary>
        /// Returns the rectangle which defines where this world begins
        /// and ends.
        /// </summary>
        /// <returns></returns>
        Rectangle GetWorldBoundaries();

        float GetScaling();

        WorldObject GetWorldPoint(Point p);

        int GetIndex(int x, int y);

        Texture2D GetTexture();

        Array2D<WorldObject> GetData();
    }
}
