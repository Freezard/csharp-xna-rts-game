using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.AI;
using RTSgame.AI.BasicAI;
using RTSgame.Utilities.Graphics.ParticleSystem;
using RTSgame.AI.BasicAI.Managers;
using RTSgame.GameObjects.Economy.Buildings;
using RTSgame.GameObjects.Buildings;
using SkinnedModel;
using RTSgame.GameObjects.Units;
using RTSgame.GameObjects.Components;


namespace RTSgame.GameObjects
{
    //AN ENEMY!
    class Enemy : AIControlledUnit, IAnimated
    {

        private Vector2 spreadVector = Vector2.Zero;
        private BasicEnemyAIManager aiManager;
        private int enemiesAroundCounter, enemiesAroundValue;
        //private Vector2 targetVelocity;
        private AnimationPlayer animPlayer;

        public Enemy(Vector2 pos)
            : base(pos, new SkinnedModelComponent("UltimateFBX", "SolidRed"),null,50)
        {
            SetScale(0.03f);
            animPlayer = ((SkinnedModelComponent)ModelComp).AnimationPlayer;
            animPlayer.StartClip("Take 001");
            
            velocity = Vector2.Zero;
          
            sightRange = TweakConstants.MINION_SIGHT_RANGE;
            maxSpeed = TweakConstants.MINION_MAX_SPEED;
            shoveDistanceSquared = TweakConstants.MINION_SHOVE_DISTANCE * TweakConstants.MINION_SHOVE_DISTANCE;
         
            aiManager = new BasicEnemyAIManager(this);
            InitializeCollisionBox();
        }
        public override void HitPointsZero()
        {
            GameState.GetInstance().DropMetal(Constants.DESIGN_ENEMY_METAL_DROP, this.GetPosition());
            base.HitPointsZero();
        }
        public override void HandlePossibleInteraction(IInteractable other)
        {

            if (other is Enemy && Calculations.IsWithin2DRange(this.GetPosition(), other.GetPosition(), 5))
            {
                enemiesAroundCounter++;
            }



           /* if (other is Projectile)
            {
                if (Calculations.IsWithin3DRange(this.GetPositionV3(), ((I3DCollidable)other).GetPositionV3(), GetInteractionRadius() + other.GetRadius()))
                {
                    this.Kill();
                    ParticleSystems.GetInstance().AddExplosion(this.GetPositionV3() + new Vector3(0, 0.5f, 0), Vector3.Zero, 1);
                }
            }*/

            /*
            if (other is Enemy && Calculations.IsWithin2DRange(this.GetPosition(), other.GetPosition(), 1))
            {
                ShoveAwayFrom(other.GetPosition(), 0.01f);
                ((Enemy)other).ShoveAwayFrom(new Vector2(GetPosition().X, GetPosition().Y), 0.01f);
            }
            else if (other is Building && Calculations.IsWithin2DRange(this.GetPosition(), other.GetPosition(), 2))
            {
                
                ((Building)other).damage(10);
                ParticleSystems.GetInstance().AddExplosion(GetPositionV3(), Vector3.Zero, 1);
                 SoundPlayer.Play3DSound("explosion", this.GetPositionV3());
                 this.Kill();

            }
            else if (other is PlayerCharacter && Calculations.IsWithin2DRange(this.GetPosition(), other.GetPosition(), 2))
            {
                ((Unit)other).Damage(10);
                if (EasyRandom.Next0to1() >= 0.9f)
                    ParticleSystems.GetInstance().AddExplosion(GetPositionV3() + new Vector3(0, 1.5f, 0), Vector3.Zero, 4);
                else ParticleSystems.GetInstance().AddExplosion(GetPositionV3() + new Vector3(0, 0.5f, 0), Vector3.Zero, 1);
                SoundPlayer.Play3DSound("explosion", this.GetPositionV3());
                this.Kill();
            }
            else if (!IsDead() && other is I3DCollidable)
            {
                if (Calculations.IsWithin3DRange(this.GetPositionV3(), ((I3DCollidable)other).GetPositionV3(), GetRadius() + other.GetRadius()))
                {
                    this.Kill();
                    ParticleSystems.GetInstance().AddExplosion(this.GetPositionV3() + new Vector3(0, 0.5f, 0), Vector3.Zero, 1);
                }
            }*/

        }


        public void Idle()
        {

            targetSpeed = 0;

        }
        public override void AIUpdatePreInteractions(GameTime gameTime)
        {
            if (aiManager != null)
            {

                aiManager.FindProperBehaviour(sightRange, gameTime);

            }

        }

        public override void AIInteract(IInteractable gameObject)
        {
            if(aiManager != null)
            aiManager.AIInteraction(gameObject, sightRange);
        }

        public override void AIUpdatePostInteractions(GameTime gameTime)
        {
            if (aiManager != null)
            aiManager.ChooseBehaviour();
        }

        public override void UpdateLogic(GameTime gameTime)
        {
            enemiesAroundValue = enemiesAroundCounter;
            enemiesAroundCounter = 0;
           
            //throw new NotImplementedException();
        }
        public bool IsConfidentToAttack()
        {
            return enemiesAroundValue > 5;
        }
        public override float GetSolidCollisionSize()
        {
            return base.GetSolidCollisionSize() / 1.4f;
        }
        public override CollidableType GetCollidableType()
        {
            return CollidableType.Enemy;
        }

        public override float GetMaxAIInteractionRange()
        {
            return sightRange;
        }

        public override float GetMaxInteractionRange()
        {
            return Constants.DefaultMaxInteractionRange;
        }






        public void updateAnimation(GameTime gameTime)
        {
            animPlayer.Update(gameTime.ElapsedGameTime);
        }
    }
}
