using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Buildings;
using RTSgame.Utilities.Graphics.ParticleSystem;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Projectiles
{
    class EnemyProjectile:Projectile
    {
         protected override void GeneralInit(GameObject Creator, Vector3 originOffset)
        {
            base.GeneralInit(Creator, originOffset );

            //model = AssetBank.GetInstance().GetModel("Bullet2");
            // tips på scale för Missil (Bullet1) = 0.065f
            // Tips för scale för Kanonkula (Bullet2) = 0.11f
            scale = 0.11f;

            Speed = 14;
        }
        public EnemyProjectile(GameObject Creator, GameObject Target, Boolean homing, Vector3 originOffset)
            : base(Creator, Target, homing, originOffset, new ModelComponent("Bullet2"))
        {
            //texture = AssetBank.GetInstance().GetTexture("SolidRed");

        }
        public EnemyProjectile(GameObject Creator, Vector3 Direction, Vector3 originOffset)
            : base(Creator, Direction, originOffset, new ModelComponent("Bullet2"))
        {
           // texture = AssetBank.GetInstance().GetTexture("SolidRed");
        }

        public EnemyProjectile(GameObject Creator, Vector2 Direction, Vector3 originOffset)
            : base(Creator, Direction, originOffset, new ModelComponent("Bullet2"))
        {
            //texture = AssetBank.GetInstance().GetTexture("SolidRed");
            trailEmitter = new ParticleEmitter(ParticleManager.GetInstance().smokeTrail, 100, GetPositionV3());
        }
        protected override Boolean shouldCollide(IInteractable Other)
        {
            return (Other is ModelObject) && !(Other is BuildingNode);
            //return (Other is ModelObject);
        }

        public override float GetMaxInteractionRange()
        {
            return Constants.DefaultMaxInteractionRange;
        }

        protected override void ActionWhenCollide(IInteractable Other)
        {
            if (Other is Building)
            {
                ((Building)Other).Damage(30);
            }

            DebugPrinter.Write(Other);
        }
    }
}
