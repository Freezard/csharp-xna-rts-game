using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Buildings;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using RTSgame.GameObjects.Economy.Buildings;

namespace RTSgame.GameObjects.Economy.Zones
{
    class ContributeZone: BuildingZone<ContributeBuilding>
    {
        public ContributeZone(ContributeBuilding building, float range, bool active):base(building, range, active)
        {
            
        }
        

        public override void WithinZone(IInteractable other)
        {
            
            if (active)
            {
                if (other is Minion)
                {
                    //Minion minion = (Minion)other;
                    building.Contribute();
                }
            }
        }
    }
}
