using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;
using RTSgame.GameObjects.Economy.Buildings;
using RTSgame.GameObjects.Units;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Buildings
{
    /// <summary>
    /// A building that heals surrounding minions
    /// </summary>
    class HealingBuilding:Building, IUsableStructure
    {
        public HealingBuilding(BuildingNode bp, Boolean instantConstruction, Player owner)
            : base(bp, instantConstruction, new ModelComponent("BuildingShrine3", "BuildingShrine3" + owner.GetPlayerColorString()), owner, 100)
        {
           
            
            
            //buildCost = 1;
            InitBuilding(1000, instantConstruction, 0.16f);
            //maxHp = 100;
            angleAround.Y = -(float)Math.PI / 4;
        //    InitializeCollisionBox();
            owner.AddRespawn(this);
        }

        public override void finishedConstructing()
        {
            AddZone(new HealingZone<Minion>(this, 5, true));
            AddZone(new HealingZone<PlayerCharacter>(this, 5, true));
        }
       
        public void AttemptToUse(PlayerCharacter playerCharacter)
        {

            playerCharacter.GetGroup().Disband();
            Owner.TeleportFrom(this);
            SoundPlayer.PlaySound("teleport");
        }
        public override void HitPointsZero()
        {
            Owner.RemoveRespawn(this);
            base.HitPointsZero();

        }
        
    }
}
