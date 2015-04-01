using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects
{
    /// <summary>
    /// A piece of metal the player can collect
    /// </summary>
    class MetalResource : ModelObject, ILogic
    {
        //The value of this piece of metal
        public int metalValue;
        private Minion carriedBy;
        
        public MetalResource(Vector2 newPosition):this(newPosition, Constants.DESIGN_RESOURCE_PILE_MAX_AMOUNT)
        {
            
        }
        public MetalResource(Vector2 newPosition, int value)
            : base(newPosition + new Vector2(EasyRandom.Next0to1()-0.5f,EasyRandom.Next0to1()-0.5f) , new ModelComponent("EconomyMetalResource"))
        {
            scale = 0.010f * (float)value/Constants.DESIGN_RESOURCE_PILE_MAX_AMOUNT;
            metalValue = value;
            //texture = AssetBank.GetInstance().GetTexture("resource");
            angleAround.Y = EasyRandom.Next0to1() * 2.0f *(float)Math.PI;
            InitializeCollisionBox();
        }

        public override CollidableType GetCollidableType()
        {
            return CollidableType.Resource;
        }

        public override float GetMaxInteractionRange()
        {
            return 0.0f;
        }
        public int GetMetalValue(){
            return metalValue;
        }


        internal void PickedUpBy(Minion minion)
        {
            carriedBy = minion;
           
        }

        public void UpdateLogic(GameTime gameTime)
        {
            if (carriedBy != null)
            {
                this.SetPositionV3(carriedBy.GetPositionV3() + new Vector3(0,6,0));
                this.SetDestinationV3(carriedBy.GetPositionV3() + new Vector3(0, 6, 0));
            }
        }
    }
}
