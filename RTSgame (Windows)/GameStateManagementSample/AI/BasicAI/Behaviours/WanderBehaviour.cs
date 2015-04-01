using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using RTSgame.AI.BasicAI.Priorities;
using Microsoft.Xna.Framework;

namespace RTSgame.AI.BasicAI.Behaviours
{
    class WanderBehaviour<TypeAgent> : BasicBehaviour<TypeAgent> where TypeAgent : AIControlledUnit
    {
        private int coolDown;
        private int heat = 0;
        public WanderBehaviour(Priority<TypeAgent> priority, int coolDownMs)
            : base(priority)
        {
            coolDown = (int)(coolDownMs*(EasyRandom.NextFloat(0.20f)+0.9f));
        }
        public override void Update(GameTime gameTime)
        {
            heat -= gameTime.ElapsedGameTime.Milliseconds;


        }
        public override bool FulfilCriteria(TypeAgent me, IInteractable otherGameObject)
        {
            return heat <= 0;
        }

        public override void ApplyOn(TypeAgent me, IInteractable otherGameObject)
        {
            heat = coolDown;
            me.SetTargetAngle(EasyRandom.Next0to1() * Calculations.DoublePi);
            me.SetTargetSpeedScale(0.2f);
                
        }
    }
}
