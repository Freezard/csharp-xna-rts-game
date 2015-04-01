using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.GameObjects
{
    public enum DoodadType : byte { None, Tree, Rock, Cliff };

    public struct Doodad
    {
        public DoodadType DoodadType;

        public float DoodadSize;

        public Doodad(DoodadType doodadType)
        {
            DoodadType = doodadType;
            DoodadSize = 1;
        }

        public void SetStaticModelObject(Point P)
        {

            switch (DoodadType) {
                case DoodadType.Tree:
                    Doodads.StaticModelObject = Doodads.TreeModel;
                    break;
                default:
                    Doodads.StaticModelObject = Doodads.TreeModel;
                    break;
            }

            Vector2 position = Calculations.PointToVector2(P);
            position += new Vector2(0.5f, 0.5f);

            Doodads.StaticModelObject.SetPosition(position);
            Doodads.StaticModelObject.AlignHeightToWorld();
            Doodads.StaticModelObject.SetScale(DoodadSize);

            Doodads.StaticModelObject.UpdateCollisionBox();

        }

        //public bool Intersect() {}
    }

    static class Doodads
    {
        public static ModelObject StaticModelObject;

        public static ModelObject TreeModel = new Tree(Constants.GetCenterOfTheWorldV2());
        //TODO Temp fix (?)
        public static void renewModel()
        { 
            TreeModel = new Tree(Constants.GetCenterOfTheWorldV2());
        }
    }
}
