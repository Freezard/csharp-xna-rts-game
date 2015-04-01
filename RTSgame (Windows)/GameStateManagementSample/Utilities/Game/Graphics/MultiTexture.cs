using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.GameObjects;
using RTSgame.Utilities.World;

namespace RTSgame.Utilities.Graphics
{
    static class MultiTexture
    {
        public const int BorderSize = 4;
        public const int TextureDimension = 128;
        public const int TextureImageDimensionX = 512;
        public const int TextureImageDimensionY = 512;

        /// <summary>
        /// Sets the Texture coordinates for the specified VertexPositionNormalTexture,
        /// given a WorldObject and which corner of a square that this vertex lies in.
        /// Corner should have any of the values: (0,0), (0,1), (1,0), (1,1).
        /// </summary>
        /// <param name="V"></param>
        /// <param name="W"></param>
        /// <param name="Corner"></param>
        public static void DeduceTexture(ref VertexPositionNormalTexture V, WorldObject W, Point Corner)
        {
            if (W.TextureManipulation.HasFlag(TextureManipulation.HorizontalSwap))
                Corner = TextureHorizontalInvert(Corner);

            if (W.TextureManipulation.HasFlag(TextureManipulation.VerticalSwap))
                Corner = TextureVerticalInvert(Corner);

            if (W.TextureManipulation.HasFlag(TextureManipulation.DiagonalSwap))
                Corner = TextureDiagonalInvert(Corner);


            int itop = W.TextureIndexY * (TextureDimension + BorderSize) +
                        Corner.Y * TextureDimension;

            int ileft = W.TextureIndexX * (TextureDimension + BorderSize) +
                        Corner.X * TextureDimension;

            float ftop = (float)itop / (float)TextureImageDimensionY;
            float fleft = (float)ileft / (float)TextureImageDimensionX;

            V.TextureCoordinate = new Vector2(ftop, fleft);
        }

        private static Point TextureHorizontalInvert(Point P)
        {
            return new Point(1 - P.X, P.Y);
        }

        private static Point TextureVerticalInvert(Point P)
        {
            return new Point(P.X, 1 - P.Y);
        }

        private static Point TextureDiagonalInvert(Point P)
        {
            if (P == new Point(0, 0) || P == new Point(1, 1))
                return new Point(1 - P.X, 1 - P.Y);
            else
                return P;
        }
    }
}
