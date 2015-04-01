using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.TestObjects
{
    // Testklass för stenar
    class Stone : ModelObject
    {

        public Stone(Vector2 position)
            : base(position, new ModelComponent("DoodadStoneHeap"))
        {
            scale = 0.14f;

           // texture = AssetBank.GetInstance().GetTexture("SolidRed");

            InitializeCollisionBox();
        }

        public override CollidableType GetCollidableType()
        {
            return CollidableType.Doodad;
        }

        public override float GetMaxInteractionRange()
        {
            return 0.0f;
        }
    }
}
