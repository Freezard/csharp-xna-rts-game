using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using RTSgame.AI.BasicAI.Priorities;

namespace RTSgame.AI.BasicAI
{
    //This behaviour stops the agent. Only used as idle behaviour
    class StopBehaviour<TypeAgent>:BasicBehaviour<TypeAgent> where TypeAgent:AIControlledUnit
    {
        
        public StopBehaviour(Priority<TypeAgent> priority)
            : base(priority)
        {

        }

        public override void ApplyOn(TypeAgent me, IInteractable otherGameObject)
        {
            me.Stop();
            
        }

        public override bool FulfilCriteria(TypeAgent me, IInteractable otherGameObject)
        {
            
            return true;
        }
    }
}
