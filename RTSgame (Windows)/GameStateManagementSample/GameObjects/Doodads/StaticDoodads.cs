using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.GameObjects.Abstract;
using RTSgame.GameObjects;
using RTSgame.GameObjects.Units;

namespace RTSgame.GameObjects.Doodads
{
    public enum DoodadType : byte { None, CubeTree, CylinderTree, ConeTree };

    static class StaticDoodads
    {

        public static DoodadModel StaticDoodadObject;

        public static Tree1 CubeTree = new Tree1(Constants.GetCenterOfTheWorldV2());

        public static Tree2 CylinderTree = new Tree2(Constants.GetCenterOfTheWorldV2());

        public static Tree3 ConeTree = new Tree3(Constants.GetCenterOfTheWorldV2());

        public static void SetStaticModelObject(ref Doodad D, Point P)
        {

            SetStaticModelObjectToCorrectType(D.DoodadType);

            Vector2 position = Calculations.PointToVector2(P);
            position += new Vector2(0.5f, 0.5f);

            StaticDoodads.StaticDoodadObject.SetPosition(position);
            //StaticDoodads.StaticDoodadObject.AlignHeightToWorld(); //dynamic but slower
            StaticDoodads.StaticDoodadObject.SetHeight(D.DoodadHeight);
            StaticDoodads.StaticDoodadObject.ModifySize(D.DoodadSize);
            StaticDoodads.StaticDoodadObject.SetFacingAngleOnXZPlane(D.Rotation);

            StaticDoodads.StaticDoodadObject.UpdateCollisionBox();
        }

        public static void SetStaticModelObjectToCorrectType(DoodadType DT)
        {
            switch (DT)
            {
                case DoodadType.CubeTree:
                    StaticDoodads.StaticDoodadObject = StaticDoodads.CubeTree;
                    break;
                case DoodadType.CylinderTree:
                    StaticDoodads.StaticDoodadObject = StaticDoodads.CylinderTree;
                    break;
                case DoodadType.ConeTree:
                    StaticDoodads.StaticDoodadObject = StaticDoodads.ConeTree;
                    break;
                default:
                    StaticDoodads.StaticDoodadObject = StaticDoodads.CubeTree;
                    break;
            }
        }

        public static void SetStaticModelObjectToCorrectType(int i)
        {
            switch (i)
            {
                case 1:
                    StaticDoodads.StaticDoodadObject = StaticDoodads.CubeTree;
                    break;
                case 2:
                    StaticDoodads.StaticDoodadObject = StaticDoodads.CylinderTree;
                    break;
                case 3:
                    StaticDoodads.StaticDoodadObject = StaticDoodads.ConeTree;
                    break;
                default:
                    StaticDoodads.StaticDoodadObject = StaticDoodads.CubeTree;
                    break;
            }
        }
    }
}
