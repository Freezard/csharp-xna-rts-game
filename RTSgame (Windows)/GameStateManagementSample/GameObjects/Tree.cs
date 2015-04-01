using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.Collision;

namespace RTSgame.GameObjects
{
    //A peaceful-looking tree
    class Tree : ModelObject
    {
        
        public Tree(Vector2 newPosition): base(newPosition)
        {
            // ORIGINAL CODE:
            
            scale = scale * (0.8f + 0.2f * EasyRandom.Next0to1());
            int modelIndex = (int)(EasyRandom.Next0to1() * 3) + 1;

            model = AssetBank.GetInstance().GetModel("tree" + modelIndex);
            texture = AssetBank.GetInstance().GetTexture("tree" + modelIndex);

            angleAround.Y = EasyRandom.Next0to1() * 2.0f * (float)Math.PI;
            

            
            // CODE FOR SIMPLENESS:
            /*
            scale = scale * (1.8f + 0.0f * EasyRandom.Next0to1());
            int modelIndex = (int)(EasyRandom.Next0to1() * 0) + 1;
            
            //model = AssetBank.GetInstance().GetModel("monitor" + modelIndex);
            model = AssetBank.GetInstance().GetModel("monitor");
            texture = AssetBank.GetInstance().GetTexture("tree" + modelIndex);

            angleAround.Y = EasyRandom.Next0to1() * 0.0f * (float)Math.PI;
            */

            InitializeCollisionBox();
        }

        public override CollidableType GetCollidableType()
        {
            return CollidableType.Doodad;
        }

        public override float GetMaxInteractionRange()
        {
            throw new NotImplementedException();
        }

        public override float GetRadius()
        {
            return base.GetRadius() / 2.0f;
        }

    }
}
