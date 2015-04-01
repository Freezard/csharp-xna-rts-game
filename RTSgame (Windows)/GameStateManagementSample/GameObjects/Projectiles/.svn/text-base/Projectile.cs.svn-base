using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.Utilities.Graphics.ParticleSystem;
using RTSgame.Utilities.Graphics;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Abstract
{
    abstract class Projectile : ModelObject, ICollidable, IMovable, ILogic, I3DCollidable
    {
        protected GameObject Owner;
        protected GameObject HomingTarget;
        protected Vector3 movement;
        protected bool Homing = false;
        protected float Speed = 10;
        protected ParticleEmitter trailEmitter;
        protected Vector3 lastPosition;
        //TODO: add timespan

        public Projectile(GameObject Creator, GameObject Target, Boolean homing, Vector3 originOffset, ModelComponent modelComp)
            : base(Creator.GetPosition(), modelComp)
        {
            InitShot(Creator, Target, homing, originOffset);
        }

        public Projectile(GameObject Creator, Vector3 Direction, Vector3 originOffset, ModelComponent modelComp)
            : base(Creator.GetPosition(), modelComp)
        {
            InitShot(Creator, Direction, originOffset);
        }

        public Projectile(GameObject Creator, Vector2 Direction, Vector3 originOffset, ModelComponent modelComp)
            : base(Creator.GetPosition(), modelComp)
        {
            InitShot(Creator, Direction, originOffset);
        }

        /// <summary>
        /// Can be called several times, will only happen once
        /// </summary>
        public void Explode()
        {
            if (!this.ToBeRemoved())
            {
                Vector3 explosionPosition = lastPosition + new Vector3(0, 1, 0);
                ParticleManager.GetInstance().AddExplosion(explosionPosition, movement, 1);
                SoundPlayer.Play3DSound("explosion", this.GetPositionV3());
            }
            RemoveFromGame();
        }

        public void UpdateLogic(GameTime gameTime)
        {
            lastPosition = GetPositionV3();

            if (Homing)
            {
                if (HomingTarget.ToBeRemoved())
                {
                 //   Explode();
                }
            }
            // Check Height
            if (GetHeight() < world.GetScaledHeight(GetPosition()))
            {
                Explode();
            }

            if (trailEmitter != null)
                trailEmitter.Update(gameTime, GetPositionV3() + new Vector3(0, 0.5f, 0));
        }

        public void UpdateDestination(GameTime gameTime)
        {

            if (Homing && !HomingTarget.ToBeRemoved())
            {
                // Move position towards HomingTarget with Speed Length
                movement = Calculations.DirectionV3FixedLength(this.GetPositionV3(), HomingTarget.GetPositionV3(), GetSpeed());
            }
            SetDestinationV3(GetPositionV3() + movement * Calculations.GetFractionalSecond(gameTime));

            KillIfOutsideMap();
        }

        public override void RestrictToMap()
        {
            if (VectorIsOutsideMap(GetPosition()) ||
                VectorIsOutsideMap(GetDestination()))
            {
                base.RestrictToMap();
                RemoveFromGame();
            }
        }

        public override void AlignHeightToWorld()
        {
            // This is already updated
        }

        public override float GetSolidCollisionSize()
        {
            return 0.0f;
        }

        public void InitShot(GameObject Creator, Vector3 Direction, Vector3 originOffset)
        {
            GeneralInit(Creator, originOffset);
            movement = Vector3.Normalize(Direction) * GetSpeed();
        }

        public void InitShot(GameObject Creator, Vector2 Direction, Vector3 originOffset)
        {
            GeneralInit(Creator, originOffset);
            movement = Calculations.V2ToV3(Vector2.Normalize(Direction) * GetSpeed(), 0);
        }

        public void InitShot(GameObject Creator, GameObject Target, Boolean homing, Vector3 originOffset)
        {
            GeneralInit(Creator, originOffset);
            HomingTarget = Target;
            Homing = homing;
            //FixedMovement = Calculations.OffsetTowardsTarget(Creator.GetPositionV3(), Target.GetPositionV3(), GetSpeed());
            movement = Calculations.DirectionV3FixedLength(Creator.GetPositionV3(), Target.GetPositionV3(), GetSpeed());
        }

        private float GetSpeed()
        {
            return Speed;
        }

        protected virtual void GeneralInit(GameObject Creator, Vector3 originOffset)
        {
            Owner = Creator;

            //TODO: This must probably be here if projectile is reinitialized, but not needed if new instances are created since then
            //a call will be made to base constructor
            // Update: we ignore this for now, reinitializable projectiles will be left for later
            base.AlignHeightToWorld();
            SetPositionV3(GetPositionV3() + originOffset);


            if (Creator is ModelObject)
            {
                ModelObject t = (ModelObject)Creator;
                SetHeight(GetHeight() + originOffset.Y);
            }
            InitializeCollisionBox();
            //model = AssetBank.GetInstance().GetModel("monitor");
            //scale = 0.004f;
        }

        public override void HandlePossibleInteraction(IInteractable Other)
        {
            if (shouldCollide(Other))
            {
                ModelObject other = (ModelObject)Other;
                
                if (!ToBeRemoved() &&
                    other != Owner &&
                    other.GetCollidableType() != CollidableType.Projectile &&
                    other.GetCollidableType() != CollidableType.None &&
                    Calculations.IsWithin3DRange(GetPositionV3(), other.GetPositionV3(), GetHitOtherObjectRange() + other.GetSolidCollisionSize()))
                    {
                        Explode();
                        ActionWhenCollide(Other);
                    }
            }
        }
        protected virtual float GetHitOtherObjectRange()
        {
            return 0.5f;
        }
        protected abstract void ActionWhenCollide(IInteractable Other);



        protected virtual Boolean shouldCollide(IInteractable Other)
        {
            return (Other is ModelObject);
        }

        public override TryToMoveAgain HandleSolidCollision()
        {
            Explode();
            return TryToMoveAgain.No;
        }

        public override CollidableType GetCollidableType()
        {
            return CollidableType.Projectile;
        }

        /*
        public BoundingSphere GetCollisionType()
        {
            throw new NotImplementedException();
        }
        */
    }
}
