using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.AI.BasicAI.Managers;
using RTSgame.Utilities;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Units
{
    class Shoota : AIControlledUnit
    {
        private Vector2 spreadVector = Vector2.Zero;
        private BasicShootaAIManager aiManager;
        //private Vector2 targetVelocity;


        public Shoota(Vector2 pos)
            : base(pos, new ModelComponent("Pouya2"), null, 50)
        {
           
            
            velocity = Vector2.Zero;
           // texture = AssetBank.GetInstance().GetTexture("TerrainGrass");
            sightRange = TweakConstants.MINION_SIGHT_RANGE;
            maxSpeed = TweakConstants.MINION_MAX_SPEED;
            shoveDistanceSquared = TweakConstants.MINION_SHOVE_DISTANCE * TweakConstants.MINION_SHOVE_DISTANCE;

           
            
            /*AddBehaviour(new ChaseBehaviour<Minion>());
            AddBehaviour(new AvoidBehaviour<PlayerCharacter>());

            SetIdleBehaviour(new StopBehaviour());*/
            aiManager = new BasicShootaAIManager(this);
            InitializeCollisionBox();
        }
        
        public override void HandlePossibleInteraction(IInteractable other)
        {
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
            else if (!IsDead() && other is I3DCollidable)
            {
                if (Calculations.IsWithin3DRange(this.GetPositionV3(), ((I3DCollidable)other).GetPositionV3(), GetRadius() + other.GetRadius()))
                {
                    this.Kill();
                    ParticleSystems.GetInstance().AddExplosion(this.GetPositionV3(), Vector3.Zero, 1);
                }
            }*/
              

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
            aiManager.AIInteraction(gameObject, sightRange);
        }

        public override void AIUpdatePostInteractions(GameTime gameTime)
        {
            aiManager.ChooseBehaviour();
        }

        public override void UpdateLogic(GameTime gameTime)
        {
            //throw new NotImplementedException();
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
    }
}
