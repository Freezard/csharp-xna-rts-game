using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects
{
    // Klass för "alternativa" trädmodellen
    class AlternateTree : ModelObject
    {

        public AlternateTree(Vector2 position)
            : base(position, new ModelComponent(""))
        {
            scale = 0.5f;

        //    texture = AssetBank.GetInstance().GetTexture("");

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
