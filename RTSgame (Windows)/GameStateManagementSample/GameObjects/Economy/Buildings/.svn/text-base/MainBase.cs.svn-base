using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Buildings;
using RTSgame.Utilities;
using RTSgame.GameObjects.Components;
using RTSgame.GameObjects.Units;

namespace RTSgame.GameObjects.Economy.Buildings
{
    class MainBase : Building
    {
        public MainBase(BuildingNode bp, Boolean instantConstruction, Player owner)
            : base(bp, instantConstruction, new ModelComponent("BuildingShrine3", "SolidYellow"), owner, 100)
        {
           
            
            
            //buildCost = 1;
            InitBuilding(1000, instantConstruction, 0.16f);
            //maxHp = 100;
            angleAround.Y = -(float)Math.PI / 4;
        //    InitializeCollisionBox();
           
                owner.AddRespawn(this);
                owner.MainBase = this;
        }
        public override void finishedConstructing()
        {
            AddZone(new HealingZone<Minion>(this, 5, true));
            AddZone(new HealingZone<PlayerCharacter>(this, 5, true));
        }
        public override void HitPointsZero()
        {
            Owner.RemoveRespawn(this);
            base.HitPointsZero();

        }
    }
}
