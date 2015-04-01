using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Units;
using RTSgame.GameObjects;
using RTSgame.AI.BasicAI.Priorities;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.AI.BasicAI
{
    //A special type of chase behaviour where minions chase playerchars if they want to be followed
    class FollowPlayerBehaviour: ChaseBehaviour<Minion, PlayerCharacter>
    {

        public FollowPlayerBehaviour(Priority<Minion> priority, float speedScale)
            : base(priority, speedScale, 1)
        {

        }

        public override bool FulfilCriteria(Minion me, IInteractable otherGameObject)
        {
            
            if (base.FulfilCriteria(me, otherGameObject))
            {
                
                return true; //((PlayerCharacter)otherGameObject).Follow();
            }
            else
            {
                return false;
            }
        }
    }
}
