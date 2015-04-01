using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Buildings;
using RTSgame.GameObjects.Abstract;
using RTSgame.GameObjects.Resources;
using RTSgame.Utilities;

namespace RTSgame.GameObjects.Economy.Zones
{
    class ResourcePickUpZone//: BuildingZone<DataMine>
    {
        /*
        public ResourcePickUpZone(DataMine building, float range, bool active):base(building, range, active)
        {
            
        }


        public override void WithinZone(IInteractable other)
        {
            if (active && Calculations.IsWithin2DRange(this.GetPosition(), other.GetPosition(), range))
            {
                if (other is Minion)
                {
                    Minion minion = (Minion)other;
                    if (!minion.IsCarryingResource() && building.hasResource())
                    {
                        minion.giveResource();
                        building.takeResource();
                    }
                }

            }
        }*/
    }
}
