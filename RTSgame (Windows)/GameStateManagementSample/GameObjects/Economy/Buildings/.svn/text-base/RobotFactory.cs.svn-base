using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.GameObjects.Economy.Buildings;
using RTSgame.GameObjects.Units;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Buildings
{
    /// <summary>
    /// A robot factory manufactures robots
    /// </summary>
    class RobotFactory : Building, IUsableStructure
    {
        
        int resources;

        public RobotFactory(BuildingNode bp, Boolean instantConstruction, Player owner)
            : base(bp, instantConstruction, new ModelComponent("BuildingFactory", "BuildingFactory"+owner.GetPlayerColorString()), owner, 150)
        {
            Console.WriteLine("fac built" + owner);
            //texture = AssetBank.GetInstance().GetTexture("Bui");
            InitBuilding(1000, instantConstruction, 0.03f);
            //buildCost = 5;
            
           // maxHp = 100;
         //   InitializeCollisionBox();
        }
      

        public void AttemptToUse(PlayerCharacter playerCharacter)
        {
            //Check if enough metal, build minion
            if (Owner.HasMetalWithSignal(Constants.DESIGN_MINION_COST))
            {
                Owner.SubtractMetal(Constants.DESIGN_MINION_COST);

                const float minimumSpawnRange = 2.0f;
                const float maximumSpawnRange = 3.0f;
                const float spawnRangeSpan = maximumSpawnRange - minimumSpawnRange;

                Minion min = new Minion(this.GetPosition() +
                    Calculations.AngleToGameV2(
                        EasyRandom.Next0to1() * Calculations.DoublePi,
                        minimumSpawnRange + spawnRangeSpan * EasyRandom.Next0to1()), Owner);
                GameState.GetInstance().addGameObject(min);
                DrawManager.GetInstance().addDrawable(min);
                SoundPlayer.PlaySound("createminion");
            }
        }
        public override float GetSolidCollisionSize()
        {
            return base.GetSolidCollisionSize() / 1.4f;
        }

        public override void finishedConstructing()
        {
            ;
        }
    }
}
