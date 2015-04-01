using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.Utilities.Graphics.ParticleSystem;
using RTSgame.GameObjects.Buildings;
using RTSgame.GameObjects.Units;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Projectiles
{
    class PlayerCodedProjectile:Projectile
    {
        private Player player;
        private int damage;
        private PlayerCharacter playerCharacter;
        private Player controllingPlayer;
        private GameObject autoAimTarget;
        private Vector3 vector3;
        private int shotCharge;
        protected override void GeneralInit(GameObject Creator, Vector3 originOffset)
        {
            base.GeneralInit(Creator, originOffset );

            //model = AssetBank.GetInstance().GetModel("Bullet2");
            // tips på scale för Missil (Bullet1) = 0.065f
            // Tips för scale för Kanonkula (Bullet2) = 0.11f
            scale = 1.11f;

            Speed = 14;
        }
        
        public PlayerCodedProjectile(GameObject Creator, Player player, GameObject Target, Boolean homing, Vector3 originOffset, int damage)
            : base(Creator, Target, homing, originOffset, new ModelComponent("Bullet2"))
        {
           // texture = AssetBank.GetInstance().GetTexture("SolidRed");
            this.player = player;
            this.damage = damage;
            trailEmitter = new ParticleEmitter(ParticleManager.GetInstance().smokeTrail, 100, GetPositionV3());

        }
        public PlayerCodedProjectile(GameObject Creator, Player player, Vector3 Direction, Vector3 originOffset, int damage)
            : base(Creator, Direction, originOffset, new ModelComponent("Bullet2"))
        {
           // texture = AssetBank.GetInstance().GetTexture("SolidRed");
            this.player = player;
            this.damage = damage;
            trailEmitter = new ParticleEmitter(ParticleManager.GetInstance().smokeTrail, 100, GetPositionV3());
        }

        public PlayerCodedProjectile(GameObject Creator, Player player, Vector2 Direction, Vector3 originOffset, int damage)
            : base(Creator, Direction, originOffset, new ModelComponent("Bullet2"))
        {
           // texture = AssetBank.GetInstance().GetTexture("SolidRed");
            this.player = player;
            this.damage = damage;
            trailEmitter = new ParticleEmitter(ParticleManager.GetInstance().smokeTrail, 100, GetPositionV3());
        }

        

        protected override Boolean shouldCollide(IInteractable Other)
        {
            if (Other is PlayerOwnedObject)
            {
                return !(((PlayerOwnedObject)Other).Owner == player);
            }
            else
            {
                return false;
            }
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
                ((Building)Other).Damage(damage);
            }
            if (Other is Unit)
            {
                ((Unit)Other).Damage(damage);
            }

            DebugPrinter.Write(Other);
        }
    }
}
