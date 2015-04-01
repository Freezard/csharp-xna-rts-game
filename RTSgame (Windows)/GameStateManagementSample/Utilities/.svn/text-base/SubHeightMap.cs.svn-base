using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;

namespace RTSgame.Utilities
{
    // This is for managing the graphical representation
    // of a height map.
    // The game logical height map is a part of World, not this.

    class SubHeightMap
    {
        //Storage for all vertices (and their type defined).
        public VertexPositionNormalTexture[] HeightMapData;
        public VertexBuffer VertexBuffer;
        public IndexBuffer IndexBuffer;
        private Location StartLocation;
        private Location EndLocation;
        public int XDimension;
        public int YDimension;
        private float Scaling;
        private IWorld World;
        private float[,] heightData;

        private const bool LowerLeft = true;
        private const bool UpperRight = false;


        public SubHeightMap(IWorld world, Location startLocation, Location endLocation)
        {
            World = world;

            StartLocation = startLocation;
            EndLocation = endLocation;
            XDimension = endLocation.X - startLocation.X;
            YDimension = endLocation.Y - startLocation.Y;
            Scaling = World.GetScaling();
            
            CreateHeightMap();
            UpdateNormals();

            //SetBuffers();
        }

        // Loads height data from a heightmap
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

        //0 1 2 3 4 5 6 7 8
        //    s   e +        
        //        s2  e +
        //xd = 2
        //    0 1 2
        //        0 1 2
        //then indices are set to draw everything between 0 and 1.

        #region CreateHeightMap
        /// <summary>
        /// Fill this height map with values.
        /// </summary>
        private void CreateHeightMap()
        {
            HeightMapData = new VertexPositionNormalTexture[XDimension * YDimension];
            for (int x = 0; x < XDimension; x++)
            {
                for (int z = 0; z < YDimension; z++)
                {
                    HeightMapData[GetIndex(x, z)].Position.X = (StartLocation.X + x) * Scaling;
                    HeightMapData[GetIndex(x, z)].Position.Z = (StartLocation.Y + z) * Scaling;
                    HeightMapData[GetIndex(x, z)].Position.Y = GetWorldPoint(x, z).Height * Scaling;
                    HeightMapData[GetIndex(x, z)].Normal = Vector3.Up;

                    //TODO: Texture might not line up with texture on next subheightmap
                    switch (((x % 2) * 2 + (z % 2)))
                    {
                        case 0:
                            HeightMapData[GetIndex(x, z)].TextureCoordinate = new Vector2(0.0f, 0.0f);
                            break;
                        case 1:
                            HeightMapData[GetIndex(x, z)].TextureCoordinate = new Vector2(1.0f, 0.0f);
                            break;
                        case 2:
                            HeightMapData[GetIndex(x, z)].TextureCoordinate = new Vector2(0.0f, 1.0f);
                            break;
                        case 3:
                            HeightMapData[GetIndex(x, z)].TextureCoordinate = new Vector2(1.0f, 1.0f);
                            break;
                    }
                }
            }
        }
        #endregion

        #region CreateMapIndices (int)
        public int[] CreateMapIndices()
        {
            int[] indices;

            indices = new int[(XDimension - 1) * (YDimension - 1) * 6];
            int counter = 0;

            for (int z = 0; z < YDimension - 1; z++)
            {
                for (int x = 0; x < XDimension - 1; x++)
                {
                    int lowerLeft = (GetIndex(x, z + 1));
                    int lowerRight = (GetIndex(x + 1, z + 1));
                    int topLeft = (GetIndex(x, z));
                    int topRight = (GetIndex(x + 1, z));

                    //if (DrawManager.CullModeUsed == CullMode.CullCounterClockwiseFace)
                    {
                        indices[counter++] = topLeft;
                        indices[counter++] = lowerRight;
                        indices[counter++] = lowerLeft;

                        indices[counter++] = topLeft;
                        indices[counter++] = topRight;
                        indices[counter++] = lowerRight;
                    }
                    #region put in indices counterclock wise
                    /*
                    else
                    {
                        indices[counter++] = lowerLeft;
                        indices[counter++] = lowerRight;
                        indices[counter++] = topLeft;

                        indices[counter++] = lowerRight;
                        indices[counter++] = topRight;
                        indices[counter++] = topLeft;
                    }
                    */
                    #endregion
                }
            }

            return indices;
        }
        #endregion

        #region CreateMapIndices (short)
        public short[] CreateMapShortIndices()
        {
            short[] indices;

            indices = new short[(XDimension - 1) * (YDimension - 1) * 6];
            int counter = 0;
            for (short x = 0; x < XDimension - 1; x++)
            {
                for (short z = 0; z < YDimension - 1; z++)
                {
                    short lowerLeft = (short)(GetIndex(x, z + 1));
                    short lowerRight = (short)(GetIndex(x + 1, z + 1));
                    short topLeft = (short)(GetIndex(x, z));
                    short topRight = (short)(GetIndex(x + 1, z));

                    //if (DrawManager.CullModeUsed == CullMode.CullCounterClockwiseFace)
                    {
                        indices[counter++] = topLeft;
                        indices[counter++] = lowerRight;
                        indices[counter++] = lowerLeft;

                        indices[counter++] = topLeft;
                        indices[counter++] = topRight;
                        indices[counter++] = lowerRight;
                    }
                    #region put in indices counterclock wise
                    /*
                    else
                    {
                        indices[counter++] = lowerLeft;
                        indices[counter++] = lowerRight;
                        indices[counter++] = topLeft;

                        indices[counter++] = lowerRight;
                        indices[counter++] = topRight;
                        indices[counter++] = topLeft;
                    }*/
                    #endregion
                }
            }

            return indices;
        }
        #endregion

        #region DrawCall Parameters
        public int GetMapIndicesWalkLength()
        {
            return (XDimension - 1) * (YDimension - 1) * 6;
        }

        public int GetMapIndicesUnique()
        {
            return XDimension * YDimension;
        }

        public int GetPrimitives()
        {
            return (XDimension - 1) * (YDimension - 1) * 2;
        }
        #endregion

        #region UpdateNormals
        /// <summary>
        /// Changes all the normals of this heightmap to a more suitable value.
        /// </summary>
        private void UpdateNormals()
        {
            
            Vector3[] triangleNormals;
            triangleNormals = GetTriangleNormals();

            // Now contains all the normals, now
            // we just need to update the vertices
            // with regards to these normals

            // the float numbers are the distances
            float closeNormalWeight = 1 / 0.414213562f;
            float distantNormalWeight = 1 / 0.765366865f;

            // cut = We don't calculate normals for the outermost vertices,
            // because if we tried to do that, we would try to access values
            // which are outside the map in order to do the calculation.
            int cut = 0;

            for (int z = 0 + cut; z < YDimension - cut; z++)
            {
                for (int x = 0 + cut; x < XDimension - cut; x++)
                {
                    //int Here = 2 * GetIndex(x, z);
                    int Here = GetTriangleNormalIndex(x, z);
                    //int XBack = 2 * GetIndex(x - 1, z);
                    int XBack = GetTriangleNormalIndex(x - 1, z);
                    //int YBack = 2 * GetIndex(x, z - 1);
                    int YBack = GetTriangleNormalIndex(x, z - 1);
                    //int XYBack = 2 * GetIndex(x - 1, z - 1);
                    int XYBack = GetTriangleNormalIndex(x - 1, z - 1);

                    HeightMapData[GetIndex(x, z)].Normal =

                    // We get the avarage vector by summing
                    // the surrounding vectors and then normalizing
                    // them. (Result must be normalized regardless.)
                    Vector3.Normalize(
                        // Close normals
                        triangleNormals[XBack + 1] * closeNormalWeight +
                        triangleNormals[YBack] * closeNormalWeight +
                        // Distant normals
                        triangleNormals[Here + 1] * distantNormalWeight +
                        triangleNormals[Here]     * distantNormalWeight +
                        triangleNormals[XYBack + 1] * distantNormalWeight +
                        triangleNormals[XYBack]     * distantNormalWeight
                    );
                    #region Reversely drawn triangles
                    /*
                     * Do not remove this, this is for when triangles are drawn the other way
                    Vector3.Normalize(
                    // Close normals
                        triangleNormals[((x - 1) * XDimension + (y - 1)) * 2 + 1] * closeNormalWeight +
                        triangleNormals[(x * XDimension + y) * 2] * closeNormalWeight +
                    // Distant normals
                        triangleNormals[((x - 1) * XDimension + y) * 2 + 1] * distantNormalWeight +
                        triangleNormals[((x - 1) * XDimension + y) * 2] * distantNormalWeight +
                        triangleNormals[(x * XDimension + (y - 1)) * 2 + 1] * distantNormalWeight +
                        triangleNormals[(x * XDimension + (y - 1)) * 2] * distantNormalWeight
                    );
                     */
                    #endregion
                }
            }

        }

        // make the board smaller or larger
        private const int shrinkBorder = -1;

        // sum of how much each dimension has changed in size
        // (because of shrinkBorder)
        private const int totalBorderModification = -1 * 2 * shrinkBorder;

        /// <summary>
        /// Helper function for UpdateNormals.
        /// </summary>
        /// <returns></returns>
        private Vector3[] GetTriangleNormals()
        {
            //Vector3[] triangleNormals = new Vector3[XDimension * YDimension * 2];

            Vector3[] triangleNormals = new Vector3[
                (XDimension + totalBorderModification) * (YDimension + totalBorderModification) * 2];

            for (int x = 0 + shrinkBorder; x < XDimension - shrinkBorder; x++)
            {
                for (int z = 0 + shrinkBorder; z < YDimension - shrinkBorder; z++)
                {

                    // If you have three vertices, V1, V2 and V3, ordered in counterclockwise
                    // order, you can obtain the direction of the normal by computing
                    // (V2 - V1) x (V3 - V1), where x is the cross product of the two vectors.

                    //First we calculate some help vertices. Notice that if we ever want
                    //to do this quickly, this part can be stripped down.
                    //(Each help vertice is recreated 4 times as we calculate different normals!)

                    //These vectors represent vertices (coordinates)
                    Vector3 topLeft = new Vector3(
                        x * Scaling,
                        //HeightMapData[GetIndex(x, z)].Position.Y,
                        GetWorldPoint(x, z).Height * Scaling,
                        z * Scaling);
                    Vector3 topRight = new Vector3(
                        (x + 1) * Scaling,
                        //HeightMapData[GetIndex(x + 1, z)].Position.Y,
                        GetWorldPoint(x + 1, z).Height * Scaling,
                        z * Scaling);
                    Vector3 lowerLeft = new Vector3(
                        x * Scaling,
                        //HeightMapData[GetIndex(x, z + 1)].Position.Y,
                        GetWorldPoint(x, z + 1).Height * Scaling,
                        (z + 1) * Scaling);
                    Vector3 lowerRight = new Vector3(
                        (x + 1) * Scaling,
                        //HeightMapData[GetIndex(x + 1, z + 1)].Position.Y,
                        GetWorldPoint(x + 1, z + 1).Height * Scaling,
                        (z + 1) * Scaling);


                    // Now it's time to calculate the normals which we are interested in.
                    
                    Vector3 triangleNormal1;
                    Vector3 triangleNormal2;
                    
                    // Essentially, it seems that XNA's counterclockwise culling is dumb.
                    // http://forums.create.msdn.com/forums/p/1469/62375.aspx

                    bool triangleSeam = false;

                    if (triangleSeam)
                    {
                        triangleNormal1 = Vector3.Normalize(Vector3.Cross((lowerRight - topLeft), (lowerLeft - topLeft)));
                        triangleNormal2 = Vector3.Normalize(Vector3.Cross((topRight - topLeft), (lowerRight - topLeft)));
                    }
                    else
                    {
                        triangleNormal1 = Vector3.Normalize(Vector3.Cross((lowerRight - lowerLeft), (topLeft - lowerLeft)));
                        triangleNormal2 = Vector3.Normalize(Vector3.Cross((topRight - lowerRight), (topLeft - lowerRight)));
                    }

                    //Every normal needs to be a unit vector.
                    triangleNormals[GetTriangleNormalIndex(x, z, LowerLeft)] = triangleNormal1;
                    triangleNormals[GetTriangleNormalIndex(x, z, UpperRight)] = triangleNormal2;

                }
            }

            return triangleNormals;
        }

        private int GetTriangleNormalIndex(int X, int Y, bool LowerLeft)
        {
            if (LowerLeft)

                return GetTriangleNormalIndex(X, Y);
            //(else)
            return GetTriangleNormalIndex(X, Y) + 1;
        }

        private int GetTriangleNormalIndex(int X, int Y)
        {
            // reverse borderModifications.
            return (X + (shrinkBorder * -1)) * (YDimension + totalBorderModification) +
                Y + (shrinkBorder * -1);
        }
        #endregion

        public int GetIndex(int X, int Y)
        {
            const String error = "WARNING: SubHeightMap 0001 , tried to get value out of bounds";
            //x = Calculations.Clamp(x, 0, XDimension);
            //y = Calculations.Clamp(y, 0, YDimension);
            return X * YDimension + Y;
        }

        public RTSgame.GameObjects.WorldPoint GetWorldPoint(int X, int Y)
        {
            return World.GetWorldPoint(X + StartLocation.X, Y + StartLocation.Y);
        }
    }
}
