using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RTSgame.Utilities.Graphics.ParticleSystem;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects
{
    // Klass för "alternativa" byggnaden
    class AlternateBuilding2 : ModelObject
    {
        public AlternateBuilding2(Vector2 position)
            : base(position, new ModelComponent("factory"))
        {
            scale = 0.025f;
            
            //texture = AssetBank.GetInstance().GetTexture("factory");
            
            ParticleManager.GetInstance().AddSmoke2(Calculations.V2ToV3(position + new Vector2(0.65f, 0.4f), 2.8f));
            ParticleManager.GetInstance().AddSmoke2(Calculations.V2ToV3(position + new Vector2(0.65f, -0.3f), 2.8f));
            InitializeCollisionBox();
        }

        public override CollidableType GetCollidableType()
        {
            return CollidableType.Structure;
        }

        public override float GetMaxInteractionRange()
        {
            throw new NotImplementedException();
        }
    }
}
