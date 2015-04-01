using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;
//using RTSgame.GameObjects.Abstract;
using RTSgame.GameObjects.Doodads;

namespace RTSgame.Utilities.World
{

    //WorldObject represents the values that we wish to store
    //on every intersection of the world.
    //Feel free to add things if you like,
    //but be cautious since there are a lot of these.

    //If this is a struct, then it is not treated by
    //the Garbage Collector, which is good.
    struct WorldObject
    {
        public float Height;

        public bool IsCompletelyBlocked;

        public byte TextureIndexX;

        public byte TextureIndexY;

        public TextureManipulation TextureManipulation;

        public SquareTerrainType SquareTerrainType;

        public bool DiagonalSeamIsDLtoUR;

        public LargeAreaType LargeAreaType;

        public Doodad Doodad;

        public WorldObject(bool occupied)
        {
            Height = 0.0f;
            IsCompletelyBlocked = occupied;
            TextureIndexX = 0;
            TextureIndexY = 0;
            TextureManipulation = 0;
            SquareTerrainType = SquareTerrainType.Grass;
            LargeAreaType = LargeAreaType.ProtectedGrass;
            DiagonalSeamIsDLtoUR = false;

            // Note that Doodad is a struct type.
            Doodad = new Doodad(DoodadType.None);

        }

        public bool HasDoodad()
        {
            return (Doodad.DoodadType != DoodadType.None);
        }
    }


}
