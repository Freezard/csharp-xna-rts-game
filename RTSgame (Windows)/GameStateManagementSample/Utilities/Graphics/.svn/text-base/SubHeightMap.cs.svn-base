using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;

namespace RTSgame.Utilities.Graphics
{
    // This is for managing the graphical representation
    // of a height map.
    // The game logical height map is a part of World, not this.

    class SubHeightMap
    {
        //Storage for all vertices (and their type defined).
        public VertexPositionNormalTexture[] HeightMapData;
        public VertexPositionNormalTexture[] tempHeightMapData;
        public VertexBuffer VertexBuffer;
        public IndexBuffer IndexBuffer;
        public short[] Indexes;
        private short[] tempIndexes;
        private Point StartLocation;
        private Point EndLocation;
        public int XDimension;
        public int YDimension;
        private float Scaling;

        private int tempTotalIndex;
        private int tempTotalVertices;
        private const int backTrack = 10;

        private IWorld World;
        private float[,] heightData;

        private const bool LowerLeft = true;
        private const bool UpperRight = false;


        public SubHeightMap(IWorld world, Point startLocation, Point endLocation)
        {
            World = world;

            StartLocation = startLocation;
            EndLocation = endLocation;
            XDimension = endLocation.X - startLocation.X;
            YDimension = endLocation.Y - startLocation.Y;
            Scaling = World.GetScaling();

            CreateHeightMap();

        }

        // Loads height data from a heightmap texture
        private void LoadHeightData(Texture2D heightMap)
        {
            XDimension = heightMap.Width;
            YDimension = heightMap.Height;

            Color[] heightMapColors = new Color[XDimension * YDimension];
            heightMap.GetData(heightMapColors);

            heightData = new float[XDimension, YDimension];
            for (int x = 0; x < XDimension; x++)
                for (int y = 0; y < YDimension; y++)
                    heightData[x, y] = heightMapColors[x + y * XDimension].R / 5.0f;
        }

        #region CreateHeightMap
        /// <summary>
        /// Fill this height map with values.
        /// </summary>
        private void CreateHeightMap()
        {
            tempIndexes = new short[XDimension * YDimension * 6];

            // If duplication check works, then *4 is enough for a primitive
            // flatshaded heightmap here. But if duplication check does not work,
            // then it must be * 6
            tempHeightMapData = new VertexPositionNormalTexture[XDimension * YDimension * 6];

            for (int x = 0; x < XDimension; x++)
            {
                for (int z = 0; z < YDimension; z++)
                {
                    VertexPositionNormalTexture vUpLeft = GetVertice(x, z);
                    VertexPositionNormalTexture vUpRight = GetVertice(x + 1, z);
                    VertexPositionNormalTexture vDownLeft = GetVertice(x, z + 1);
                    VertexPositionNormalTexture vDownRight = GetVertice(x + 1, z + 1);

                    MultiTexture.DeduceTexture(ref vUpLeft, GetWorldPoint(x, z), new Point(0, 0));
                    MultiTexture.DeduceTexture(ref vUpRight, GetWorldPoint(x, z), new Point(1, 0));
                    MultiTexture.DeduceTexture(ref vDownLeft, GetWorldPoint(x, z), new Point(0, 1));
                    MultiTexture.DeduceTexture(ref vDownRight, GetWorldPoint(x, z), new Point(1, 1));

                    // Random light noise:
                    const float noisePower = 0.2f;
                    /*
                    Vector3 noise = new Vector3(
                        EasyRandom.NextFloat(noisePower),
                        EasyRandom.NextFloat(noisePower),
                        EasyRandom.NextFloat(noisePower));
                    */
                    float noise = EasyRandom.NextFloat(0.15f) + 0.85f;

                    if (!GetWorldPoint(x, z).DiagonalSeamIsDLtoUR)
                    {
                        // Add triangle 1
                        SetUniformNormalAndScale(ref vUpLeft, ref vDownRight, ref vDownLeft, noise);
                        AddTriangle(vUpLeft, vDownRight, vDownLeft);

                        // Add triangle 2
                        SetUniformNormalAndScale(ref vUpLeft, ref vUpRight, ref vDownRight, noise);
                        AddTriangle(vUpLeft, vUpRight, vDownRight);
                    }
                    else
                    {
                        // Add triangle 1
                        SetUniformNormalAndScale(ref vUpLeft, ref vUpRight, ref vDownLeft, noise);
                        AddTriangle(vUpLeft, vUpRight, vDownLeft);

                        // Add triangle 2
                        SetUniformNormalAndScale(ref vUpRight, ref vDownRight, ref vDownLeft, noise);
                        AddTriangle(vUpRight, vDownRight, vDownLeft);
                    }
                }
            }

            Indexes = new short[tempTotalIndex];
            HeightMapData = new VertexPositionNormalTexture[tempTotalVertices];

            for (int i = 0; i < tempTotalIndex; i++)
                Indexes[i] = tempIndexes[i];
            for (int i = 0; i < tempTotalVertices; i++)
                HeightMapData[i] = tempHeightMapData[i];
            tempIndexes = null;
            tempHeightMapData = null;
        }
        #endregion

        private VertexPositionNormalTexture GetVertice(int X, int Z)
        {
            VertexPositionNormalTexture v = new VertexPositionNormalTexture();

            v.Position.X = (StartLocation.X + X) * Scaling;
            v.Position.Z = (StartLocation.Y + Z) * Scaling;
            v.Position.Y = GetWorldPoint(X, Z).Height * Scaling;
            v.Normal = Vector3.Up;

            return v;
        }

        /// <summary>
        /// Add the necessary data for an entire triangle to be drawn.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        private void AddTriangle(VertexPositionNormalTexture v1, VertexPositionNormalTexture v2, VertexPositionNormalTexture v3)
        {
            // If two of the vertices have the same position, don't add this triangle
            if (v1.Position == v2.Position || v2.Position == v3.Position || v1.Position == v3.Position)
            {
                DebugPrinter.Write("shouldn't be here yet");
                return;
            }

            AddVerticeWithChecking(v1);
            AddVerticeWithChecking(v2);
            AddVerticeWithChecking(v3);
        }

        /// <summary>
        /// Adds a vertice to the drawing list. Does not create unnecessary
        /// new vertices.
        /// </summary>
        /// <param name="v"></param>
        private void AddVerticeWithChecking(VertexPositionNormalTexture v)
        {
            // Check if this vertex has already been added
            int startIndex = Calculations.Clamp(tempTotalVertices - backTrack, 0, 65000);
            bool foundAMatch = false;
            int i = 0;
            for (i = startIndex; i < tempTotalVertices; i++)
            {
                if (tempHeightMapData[i] == v)
                {
                    foundAMatch = true;
                    break;
                }
            }

            if (foundAMatch)
            {
                AddVerticeViaIndex(i);
            }
            else
            {
                AddVertice(v);
            }

        }

        /// <summary>
        /// Add a new vertice to the vertice data, and
        /// add an index pointing to it in the index data.
        /// </summary>
        /// <param name="v"></param>
        private void AddVertice(VertexPositionNormalTexture v)
        {
            if (tempTotalVertices >= tempHeightMapData.Length)
                DebugPrinter.Write(tempTotalVertices);
            tempHeightMapData[tempTotalVertices] = v;
            tempIndexes[tempTotalIndex] = (short)tempTotalVertices;
            tempTotalVertices++;
            tempTotalIndex++;
        }

        /// <summary>
        /// Add an index to the index data, pointing towards
        /// an already existing vertice.
        /// </summary>
        /// <param name="i"></param>
        private void AddVerticeViaIndex(int i)
        {
            tempIndexes[tempTotalIndex] = (short)i;
            tempTotalIndex++;
        }

        #region DrawCall Parameters
        /// <summary>
        /// Returns how long the indexbuffer should be
        /// </summary>
        /// <returns></returns>
        public int GetNumOfIndices()
        {
            return tempTotalIndex;
        }

        /// <summary>
        /// Returns the number of vertices
        /// </summary>
        /// <returns></returns>
        public int GetNumOfVertices()
        {
            return tempTotalVertices;
        }

        /// <summary>
        /// Returns how many triangles this SubHeightMap consists of
        /// </summary>
        /// <returns></returns>
        public int GetNumOfPrimitives()
        {
            return (tempTotalIndex / 3);
        }
        #endregion

        /// <summary>
        /// Caluclates (one of the two) normal given three points in space.
        /// </summary>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private Vector3 CalculateNormal(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            // Essentially, it seems that XNA's counterclockwise culling is dumb.
            // http://forums.create.msdn.com/forums/p/1469/62375.aspx

            Vector3 normal = Vector3.Normalize(Vector3.Cross((v0 - v1), (v2 - v1)));

            if (normal == Vector3.Zero)
            {
                new Exception("zero vector found");
                normal = Vector3.Up;
            }

            return normal;
        }

        /// <summary>
        /// Sets the normal vectors of all the arguments
        /// to the same, avarage, normal.
        /// </summary>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        private void SetUniformNormal(ref VertexPositionNormalTexture v0,
            ref VertexPositionNormalTexture v1,
            ref VertexPositionNormalTexture v2)
        {
            Vector3 normal = CalculateNormal(v0.Position, v1.Position, v2.Position);

            // set normal
            v0.Normal = normal;
            v1.Normal = normal;
            v2.Normal = normal;
        }

        /// <summary>
        /// Sets the normal vectors of all the arguments
        /// to the same, avarage, normal. Has an additional parameter
        /// for modifiying the result.
        /// </summary>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        private void SetUniformNormalAndAddV3(ref VertexPositionNormalTexture v0,
            ref VertexPositionNormalTexture v1,
            ref VertexPositionNormalTexture v2,
            Vector3 Change)
        {
            Vector3 normal = CalculateNormal(v0.Position, v1.Position, v2.Position);

            // Add change
            normal += Change;
            normal.Normalize();

            // set normal
            v0.Normal = normal;
            v1.Normal = normal;
            v2.Normal = normal;
        }

        /// <summary>
        /// Sets the normal vectors of all the arguments
        /// to the same, avarage, normal. Has an additional parameter
        /// for modifiying the result.
        /// </summary>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        private void SetUniformNormalAndScale(ref VertexPositionNormalTexture v0,
            ref VertexPositionNormalTexture v1,
            ref VertexPositionNormalTexture v2,
            float Change)
        {
            Vector3 normal = CalculateNormal(v0.Position, v1.Position, v2.Position);

            normal.Normalize();

            // Add change
            normal *= Change;

            // set normal
            v0.Normal = normal;
            v1.Normal = normal;
            v2.Normal = normal;
        }

        public RTSgame.Utilities.World.WorldObject GetWorldPoint(int X, int Y)
        {
            return World.GetWorldPoint(new Point(X + StartLocation.X, Y + StartLocation.Y));
        }
    }
}
