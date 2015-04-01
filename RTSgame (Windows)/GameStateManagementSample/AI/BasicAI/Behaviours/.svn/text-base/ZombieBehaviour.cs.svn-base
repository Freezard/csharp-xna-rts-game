using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects;
using RTSgame.AI.BasicAI.Priorities;
using RTSgame.Utilities;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.AI.BasicAI.Behaviours
{
    class ZombieBehaviour:BasicBehaviour<Enemy>
    {
        public ZombieBehaviour(Priority<Enemy> priority):base(priority)
        {

           
        }

        public override bool FulfilCriteria(Enemy me, IInteractable otherGameObject)
        {
            return true;
        }

        public override void ApplyOn(Enemy me, IInteractable otherGameObject)
        {
            me.MoveToAndStop(Constants.GetCenterOfTheWorldV2(), 0.01f);
        }
    }
}
