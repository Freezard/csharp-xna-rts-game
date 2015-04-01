using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using RTSgame.GameObjects.Economy.Zones;
using RTSgame.GameObjects;
using RTSgame.GameObjects.Buildings;
using RTSgame.AI.BasicAI.Priorities;

namespace RTSgame.AI.BasicAI
{
    //A special type of chase behaviour where minions will move to resource drop off zones if they are carrying resources
    class DropOffResourceBehaviour : ChaseBehaviour<Minion, ResourceDropOffZone>
    {
         public DropOffResourceBehaviour(Priority<Minion> priority, float speedScale)
            : base(priority, speedScale,0)
        {

        }
         public override bool FulfilCriteria(Minion me, IInteractable otherGameObject)
        {
            if (base.FulfilCriteria(me, otherGameObject))
            {

                    return me.IsCarryingResource() && ((ResourceDropOffZone)otherGameObject).IsActive();

            }
            else
            {
                return false;
            }
        }
    }
}
