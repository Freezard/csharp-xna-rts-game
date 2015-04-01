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
        const int BorderSize = 4;

        // how large each texture is
        const int TextureDimension = 128;

        // how large the multitexturefile is
        const int TextureImageDimension = 512;

        private static Dictionary<int, float> preCalculatedValues;

        private static void SetPrecalculatedValues()
        {
            preCalculatedValues = new Dictionary<int,float>();

            float textureImageDimension = (float)TextureImageDimension;

            for (int i = 0; i <= textureImageDimension; i += BorderSize)
            {
                preCalculatedValues.Add(i, ((float)i) / textureImageDimension);
            }
        }

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
            if (preCalculatedValues == null)
                SetPrecalculatedValues();

            if (W.TextureManipulation.HasFlag(TextureManipulation.HorizontalSwap))
                Corner = TextureHorizontalInvert(Corner);

            if (W.TextureManipulation.HasFlag(TextureManipulation.VerticalSwap))
                Corner = TextureVerticalInvert(Corner);

            if (W.TextureManipulation.HasFlag(TextureManipulation.DiagonalSwap))
                Corner = TextureDiagonalInvert(Corner);

            
            int itop = W.TextureIndexY * (TextureDimension + BorderSize) + // top side of texture
                        Corner.Y * TextureDimension; // starting point due to manioulations

            int ileft = W.TextureIndexX * (TextureDimension + BorderSize) + // left side of texture
                        Corner.X * TextureDimension; // starting point due to manioulations
           
            /*
            float ftop = (float)itop / (float)TextureImageDimension;
            float fleft = (float)ileft / (float)TextureImageDimension;
            */

            // the above calculation is not wrong, but slow as hell
            // this is nothing but a speedup, we use the above values,
            // but pre-calculated

            float ftop = preCalculatedValues[itop];
            float fleft = preCalculatedValues[ileft];

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
