using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using RTSgame.GameObjects.Components;


namespace RTSgame.GameObjects.Doodads
{
    abstract class BaseTree : DoodadModel
    {
        public BaseTree(Vector2 newPosition, int TreeIndex)
            : base(newPosition, new ModelComponent("DoodadTree" + TreeIndex))
        {
            
            //texture = AssetBank.GetInstance().GetTexture("DoodadTree" + TreeIndex);
            InitializeCollisionBox();
        }
    }
}
