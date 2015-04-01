using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.AI.BasicAI;
using RTSgame.AI.BasicAI.Priorities;
using System;
using RTSgame.AI;
using RTSgame.GameObjects.Units;
using RTSgame.GameObjects.Components;
using SkinnedModel;


namespace RTSgame.GameObjects
{
    //It is a minion!!!
    class Minion : AIControlledUnit, IAnimated
    {


        private BasicMinionAIManager aiManager;
        //private AIManagerMinion aim;
        private bool carryingResource = false;
        
        private Group group = null;
        private AnimationPlayer animPlayer;

        public Minion(Vector2 pos, Player owner)
            : base(pos, new SkinnedModelComponent("UltimateFBX", "Solid" + owner.GetPlayerColorString()), owner, 30)
        {
            animPlayer = ((SkinnedModelComponent)ModelComp).AnimationPlayer;
            // texture = AssetBank.GetInstance().GetTexture("WizardSkjutarAnimation4Texture2");
            sightRange = TweakConstants.MINION_SIGHT_RANGE;
            maxSpeed = TweakConstants.MINION_MAX_SPEED;
            shoveDistanceSquared = TweakConstants.MINION_SHOVE_DISTANCE * TweakConstants.MINION_SHOVE_DISTANCE;
            // Ändrade

            SetScale(0.025f);
            animPlayer.StartClip("Take 001");
            //  animationPlayer.StartClip(getClip("Take 001"));
            aiManager = new BasicMinionAIManager(this);
            InitializeCollisionBox();
            //texture = owner.GetPlayerColor();
            //ModelComp.Texture = AssetBank.GetInstance().GetTexture("Solid" + Owner.GetPlayerColorString());
        }

        public bool IsCarryingResource()
        {
            return carryingResource;
        }
        public void dropResource()
        {
            carryingResource = false;
        }


        public override void HandlePossibleInteraction(IInteractable other)
        {
            if (other is Minion && Calculations.IsWithin2DRange(this.GetPosition(), other.GetPosition(), 1))
            {
                ShoveAwayFrom(other.GetPosition(), 0.01f);
                ((Minion)other).ShoveAwayFrom(GetPosition(), 0.01f);
            }
            if (other is MetalResource)
            {
              //  throw new Exception("LOL");
            }
            if (other is MetalResource && !carryingResource &&
                    Calculations.IsWithin2DRange(GetPosition(), other.GetPosition(), 1.0f))
            {
                //throw new Exception("LOL");
                carryingResource = true;
                SoundPlayer.PlaySound("coin");
               // ((MetalResource)other).RemoveFromGame();
               // Owner.AddMetal(((MetalResource)other).GetMetalValue());
                ((MetalResource)other).PickedUpBy(this);
            }
            /*else if (other is MetalResource && !carryingResource && Calculations.IsWithin2DRange(this.GetPosition(), other.GetPosition(), 1))
            {
                ((MetalResource)other).RemoveFromGame();
                carryingResource = true;

            }*/
        }


        public void Idle()
        {
            targetSpeed = 0;
            /*if (startOfIdle)
            {
                float angle = (float)(EasyRandom.Next0to1() * 2 * Math.PI);
                targetVelocity = Calculations.AngleToV2(angle, speed);
                startOfIdle = false;
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


        internal void giveResource()
        {
            carryingResource = true;
        }
        public override void HitPointsZero()
        {
            GameState.GetInstance().DropMetal(Constants.DESIGN_MINION_COST, this.GetPosition());
            base.HitPointsZero();
        }
        public override float GetSolidCollisionSize()
        {
            return base.GetSolidCollisionSize() / 5;
        }
        public override CollidableType GetCollidableType()
        {
            return CollidableType.Minion;
        }

        public override float GetMaxAIInteractionRange()
        {
            return sightRange;
        }

        public override float GetMaxInteractionRange()
        {
            return Constants.DefaultMaxInteractionRange;
        }

        //TODO Minor circular dependency... Changing things...
        public Group getGroup()
        {
            return this.group;
        }
        public bool IsInGroup()
        {
            if (this.group != null)
                return true;
            else
                return false;
        }


        public void MoveToPositionInFormation()
        {
            getGroup().MoveMinionInFormation(this);
        }

        internal void JoinGroup(Group group)
        {
            this.group = group; //TODO Perhaps sync better with BasicMinionAIManager?
        }

        internal void LeaveGroup()
        {
            this.group = null;
        }




        public void updateAnimation(GameTime gameTime)
        {
            animPlayer.Update(gameTime.ElapsedGameTime);
        }
    }
}
