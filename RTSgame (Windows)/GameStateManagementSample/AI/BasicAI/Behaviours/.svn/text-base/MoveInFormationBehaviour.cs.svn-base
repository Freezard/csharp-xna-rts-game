using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.AI.Abstract;
using RTSgame.GameObjects;
using RTSgame.GameObjects.Abstract;
using RTSgame.AI.BasicAI.Priorities;

namespace RTSgame.AI.BasicAI.Behaviours
{
    class MoveInFormationBehaviour : BasicBehaviour<Minion>
    {

        public MoveInFormationBehaviour(Priority<Minion> priority):base(priority)
        {

            
        }
        public override bool FulfilCriteria(Minion me, IInteractable otherGameObject)
        {
           //Should execute if in group
            return me.IsInGroup();
        }

        public override void ApplyOn(Minion me, IInteractable otherGameObject)
        {
            me.MoveToPositionInFormation();
        }
    }
}
