using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.AI.BasicAI.Priorities;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.AI.BasicAI.Behaviours
{
    class RandomDirectionMove<TypeAgent> : BasicBehaviour<TypeAgent> where TypeAgent : AIControlledUnit
    {
        public RandomDirectionMove(Priority<TypeAgent> priority)
            : base(priority)
        {

        }
        public override void ApplyOn(TypeAgent me, IInteractable otherGameObject)
        {
            //me.SetTargetDirection(Vector

        }

        public override bool FulfilCriteria(TypeAgent me, IInteractable otherGameObject)
        {
            
            return true;
        }

    }
}
