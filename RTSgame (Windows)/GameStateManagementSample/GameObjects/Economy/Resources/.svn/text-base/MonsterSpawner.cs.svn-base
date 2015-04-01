using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Economy.Zones;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Economy.Resources
{
    class MonsterSpawner : Structure
    {
        public int cool = 60;

        public MonsterSpawner(Vector2 newPosition)
            : base(newPosition, new ModelComponent("DoodadStoneHeap"))
        {



          
            
            scale = 0.2f;

            InitializeCollisionBox();

            
        }
        public override void UpdateLogic(GameTime gameTime)
        {
            if (GameState.GetInstance().enemies < Constants.MaxNumOfMonsters)
            {
                cool--;
                if (cool <= 0)
                {
                    // TODO: change cooldown to time measurement
                    cool = 8 * Constants.FRAMES_PER_SECOND;
                    //cool = 1; // for debugging

                    const float minimumSpawnRange = 1.5f;
                    const float maximumSpawnRange = 2.5f;
                    const float spawnRangeSpan = maximumSpawnRange - minimumSpawnRange;
                    Enemy enemy = new Enemy(this.GetPosition() +
                        Calculations.AngleToGameV2(
                            EasyRandom.Next0to1() * Calculations.DoublePi,
                            minimumSpawnRange + spawnRangeSpan * EasyRandom.Next0to1()));

                    GameState.GetInstance().addGameObject(enemy);
                    DrawManager.GetInstance().addDrawable(enemy);

                }
            }
        }

    }
}
