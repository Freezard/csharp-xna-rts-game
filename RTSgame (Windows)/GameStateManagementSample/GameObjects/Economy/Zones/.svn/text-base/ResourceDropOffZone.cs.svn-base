using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.GameObjects.Economy;

namespace RTSgame.GameObjects.Buildings
{
    class ResourceDropOffZone: BuildingZone<Building>
    {
        
        public ResourceDropOffZone(Building building, float range, bool active):base(building, range, active)
        {
            
        }
        

        public override void WithinZone(IInteractable other)
        {
           
           
            if (active && Calculations.IsWithin2DRange(this.GetPosition(), other.GetPosition(), range))
            {
                
                if (other is Minion)
                {
                    Minion minion = (Minion)other;
                    if (minion.IsCarryingResource())
                    {
                        minion.dropResource();
                        building.GiveResource();
                    }
                }
            }
        }


        
    }
}
