using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RTSgame.Utilities.Memory
{


    public class BlockMap<T> : Array2D<T> where T : IBlock , new()
    {
        private int blockMaxSizeX;
        private int blockMaxSizeY;
        private Rectangle mapBoundaries;
        private int extensionDownAndRight;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BlockMaxSize">Maximum size of each Block</param>
        /// <param name="MapBoundaries">Everything lies WITHIN these boundaries</param>
        /// <param name="ExtendDownAndRight">How much a Block should overlap with the next block</param>
        public BlockMap(int BlockMaxSize, Rectangle MapBoundaries, int ExtendDownAndRight = 0)
        {
            blockMaxSizeX = BlockMaxSize;
            blockMaxSizeY = BlockMaxSize;

            mapBoundaries = MapBoundaries;
            extensionDownAndRight = ExtendDownAndRight;

            // Set up correctly
            FillMapWithBlocks();

        }

        private void FillMapWithBlocks()
        {

            // We calculate how many blocks that fit into the range (endCorner - startCorner)
            // that we are interested in, and then we round UP, so that we get sufficient amount
            // of blocks.
            XDimension = (int)Math.Ceiling((double)(mapBoundaries.Width) / (double)blockMaxSizeX);
            YDimension = (int)Math.Ceiling((double)(mapBoundaries.Height) / (double)blockMaxSizeY);

            Data = new T[XDimension, YDimension];

            SetEveryPoint(CreateBlock(extensionDownAndRight));
        }

        private BlockMap<T>.DelSetValueGetLocation CreateBlock(int extra = 0)
        {
            return delegate(Point p)
            {
                // p is a block point

                // new block
                T t = new T();

                // first we calculate the strict coordinates
                // these coordinates are world points
                int startX = Calculations.Clamp(p.X * blockMaxSizeX + mapBoundaries.Left, mapBoundaries.Left, mapBoundaries.Right);
                int endX = Calculations.Clamp((p.X + 1) * blockMaxSizeX + mapBoundaries.Left, mapBoundaries.Left, mapBoundaries.Right);
                int startY = Calculations.Clamp(p.Y * blockMaxSizeY + mapBoundaries.Top, mapBoundaries.Top, mapBoundaries.Bottom);
                int endY = Calculations.Clamp((p.Y + 1) * blockMaxSizeY + mapBoundaries.Top, mapBoundaries.Top, mapBoundaries.Bottom);

                // world sizes
                int sizeX = endX - startX;
                int sizeY = endY - startY;

                // set this block's boundaries in the world
                t.SetStrictBoundaryRectangle(new Rectangle(startX, startY, sizeX, sizeY));

                // then we recalculate for the overlapping coordinates
                // these coordinates are world points
                endX = Calculations.Clamp((p.X + 1) * blockMaxSizeX + mapBoundaries.Left + extra, mapBoundaries.Left, mapBoundaries.Right);
                endY = Calculations.Clamp((p.Y + 1) * blockMaxSizeY + mapBoundaries.Top  + extra, mapBoundaries.Top, mapBoundaries.Bottom);

                // world sizes
                sizeX = endX - startX;
                sizeY = endY - startY;

                // set this block's boundaries in the world
                t.SetOverlappingBoundaryRectangle(new Rectangle(startX, startY, sizeX, sizeY));

                // set this block's blockpoint
                t.SetBlockPoint(new Point(p.X, p.Y));

                return t;
            };
        }

        public Rectangle GetMapBoundaries()
        {
            return mapBoundaries;
        }

        /// <summary>
        /// Finds the point of the map block, that covers
        /// this position. Argument is a world point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point GetBlockPoint(Point p)
        {
            //TODO: it would be better if this contained
            // most of the code in GetBlock, and GetBlock
            // called this, and then retrived the value.

            // -The reason that it works this way is that
            // it allows us to actually check that we
            // get the correct point

            T block = GetBlockViaWorldPoint(p);

            return block.GetBlockPoint();
        }
        

        /// <summary>
        /// Returns the block that is defined to cover this position.
        /// Argument is a world point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public T GetBlockViaWorldPoint(Point p)
        {
            // p is a world point

            // locationX/Y are block coordinates
            int locationX = (p.X - mapBoundaries.Left) / blockMaxSizeX;
            int locationY = (p.Y - mapBoundaries.Top) / blockMaxSizeY;

            locationX = Calculations.Clamp(locationX, 0, XDimension - 1);
            locationY = Calculations.Clamp(locationY, 0, YDimension - 1);

            T result = Data[locationX, locationY];

            if (p == null)
                DebugPrinter.Write("this is odd");

            if (result == null)
                DebugPrinter.Write("this is also odd");

            //Debug.Write(p.X + " . " + p.Y);
            //Debug.Write(locationX + " * " + locationY);
            //Debug.Write(result.GetBlockPoint().X + " _ " + result.GetBlockPoint().Y);

            //if (!result.CoversOverlappingPosition(p))
            //    throw new Exception("Block claims it does not contain specified position: " + p);
            return result;
        }

        /// <summary>
        /// Get all blocks that touch this region.
        /// Arguments are world points.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<T> GetBlocks(Point start, Point end)
        {
            // list with the resulting blocks
            List<T> result = new List<T>();
            
            // this gives us block
            Point firstBlockPoint = GetBlockPoint(start);
            Point lastBlockPoint = GetBlockPoint(end);
            
            for (int x = firstBlockPoint.X; x <= lastBlockPoint.X; x++)
            {
                for (int y = firstBlockPoint.Y; y <= lastBlockPoint.Y; y++)
                {
                    result.Add(Data[x,y]);
                }
            }

            return result;
        }

        /// <summary>
        /// Get all blocks that touch this region, including those
        /// that overlap into this area.
        /// Arguments are world points.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<T> GetBlocksIncludeOverlapping(Point start, Point end)
        {
            List<T> result = new List<T>();

            // We back up to the start position, to include those that might
            // overlap into the specified area.
            start.X = start.X - extensionDownAndRight;
            start.Y = start.Y - extensionDownAndRight;

            Point firstBlockPoint = GetBlockPoint(start);
            Point lastBlockPoint = GetBlockPoint(end);

            for (int x = firstBlockPoint.X; x <= lastBlockPoint.X; x++)
            {
                for (int y = firstBlockPoint.Y; y <= lastBlockPoint.Y; y++)
                {
                    result.Add(Data[x, y]);
                }
            }

            return result;
        }


        public List<T> GetBlockIncludeOverlapping(Point start)
        {
            return GetBlocksIncludeOverlapping(start, start);
        }

        /// <summary>
        /// Given a real position, get a blockmap position.
        /// Argument is a world point. Clamps without raising an error.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point GetBlockPoint2(Point p)
        {
            // gives us the block point
            int locationX = (p.X - mapBoundaries.Left) / blockMaxSizeX;
            int locationY = (p.Y - mapBoundaries.Top) / blockMaxSizeY;

            locationX = Calculations.Clamp(locationX, 0, XDimension - 1);
            locationY = Calculations.Clamp(locationY, 0, YDimension - 1);

            return new Point(locationX, locationY);
        }


        public Vector2 AdjustVectorToBlockMapVector(Vector2 v)
        {
            float x = (v.X - (float)mapBoundaries.Left) / (float)blockMaxSizeX;
            float y = (v.Y - (float)mapBoundaries.Top) / (float)blockMaxSizeY;
            return new Vector2(x, y);
        }

        /*
        public float AdjustLengthToBlockMapLength(float x)
        {
            return x  / (float)blockMaxSizeX;
        }
         */

        public T GetBlockBestFit(Point p)
        {
            return GetBlockViaWorldPoint(new Point(p.X - (extensionDownAndRight / 2), p.Y - (extensionDownAndRight / 2)));
        }

        /// <summary>
        /// Result is a world vector, argument is a blockpoint.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Vector2 GetBlockCenter(Point p)
        {
            //TODO: make sure this is correct.
            float x = (float)((p.X * blockMaxSizeX) + mapBoundaries.Left) + ((float)blockMaxSizeX / 2.0f);
            float y = (float)((p.Y * blockMaxSizeY) + mapBoundaries.Top) + ((float)blockMaxSizeY / 2.0f);
            return new Vector2(x,y);
        }
    }
}