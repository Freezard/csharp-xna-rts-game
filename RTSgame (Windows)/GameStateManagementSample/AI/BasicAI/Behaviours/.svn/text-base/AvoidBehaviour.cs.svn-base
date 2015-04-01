using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using RTSgame.AI.BasicAI.Priorities;

namespace RTSgame.AI.BasicAI
{
    //A simple avoid behaviour, agent moves away from objects of type TypeAvoidObject at speed maxspeed*speedScale
    class AvoidBehaviour<TypeAgent, TypeAvoidObject> : BasicBehaviour<TypeAgent>
        where TypeAvoidObject : IInteractable
        where TypeAgent : AIControlledUnit
    {

        float speedScale;
         public AvoidBehaviour(Priority<TypeAgent> priority, float speedScale)
            : base(priority)
        {
            this.speedScale = speedScale;
        }
         public override void ApplyOn(TypeAgent me, IInteractable otherGameObject)
        {
            me.MoveAwayFrom(otherGameObject.GetPosition(), speedScale);
        }

         public override bool FulfilCriteria(TypeAgent me, IInteractable otherGameObject)
        {
            
            return (otherGameObject is TypeAvoidObject);
        }
    }
}
