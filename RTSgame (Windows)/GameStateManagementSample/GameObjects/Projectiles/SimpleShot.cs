using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;
using RTSgame.Utilities.Graphics.ParticleSystem;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Projectiles
{
    class SimpleShot<FilterObject>:Projectile where FilterObject:GameObject
    {
        

        protected override void GeneralInit(GameObject Creator, Vector3 originOffset)
        {
            base.GeneralInit(Creator, originOffset );

            //model = AssetBank.GetInstance().GetModel("Bullet2");
            // tips på scale för Missil (Bullet1) = 0.065f
            // Tips för scale för Kanonkula (Bullet2) = 0.11f
            scale = 0.0011f;

            Speed = 14;
        }
        public SimpleShot(GameObject Creator, GameObject Target, Boolean homing, Vector3 originOffset)
            : base(Creator, Target, homing, originOffset, new ModelComponent("Bullet2"))
        {
            //texture = AssetBank.GetInstance().GetTexture("SolidRed");

        }
        public SimpleShot(GameObject Creator, Vector3 Direction, Vector3 originOffset)
            : base(Creator, Direction, originOffset, new ModelComponent("Bullet2"))
        {
            //texture = AssetBank.GetInstance().GetTexture("SolidRed");
        }

        public SimpleShot(GameObject Creator, Vector2 Direction, Vector3 originOffset)
            : base(Creator, Direction, originOffset, new ModelComponent("Bullet2"))
        {
            //texture = AssetBank.GetInstance().GetTexture("SolidRed");
            trailEmitter = new ParticleEmitter(ParticleManager.GetInstance().smokeTrail, 100, GetPositionV3());
        }
        protected override Boolean shouldCollide(IInteractable Other)
        {
            return (Other is ModelObject && Other is FilterObject);
            //return (Other is ModelObject);
        }

        public override float GetMaxInteractionRange()
        {
            return Constants.DefaultMaxInteractionRange;
        }

        protected override void ActionWhenCollide(IInteractable Other)
        {
            if (Other is Enemy)
            {
                ((Enemy)Other).Damage(30);
            }
        }
    }
}
