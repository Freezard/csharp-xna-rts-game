using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Economy;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using RTSgame.GameObjects.Units;

namespace RTSgame.GameObjects.Buildings
{
    class HealingZone<T>:BuildingZone<Building> where T : Unit
    {
        public HealingZone(Building building, float range, bool active):base(building, range, active)
        {
            
        }
        public override void WithinZone(Abstract.IInteractable other)
        {

                if (other is T)
                {
                    
                    if (building is Building)
                    {
                        Building b = (Building)building;
                        Unit u = (Unit)other;

                        if (!u.IsAtFullHealth() && (b.Owner == u.Owner))
                        {
                            if (b.Owner.HasEnergy(Constants.DESIGN_HEALING_COST))
                            {
                                b.Owner.SubtractEnergy(Constants.DESIGN_HEALING_COST);
                                u.Heal(Constants.DESIGN_HEALING_RATE);
                            }
                        }
                    }
                    else
                    {
                        ((T)other).Heal(Constants.DESIGN_HEALING_RATE);
                    }
                }

        }
    }
}
