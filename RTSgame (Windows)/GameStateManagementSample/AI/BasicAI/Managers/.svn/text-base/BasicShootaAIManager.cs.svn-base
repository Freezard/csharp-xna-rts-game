using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Units;
using RTSgame.GameObjects;
using RTSgame.AI.BasicAI.Priorities;

namespace RTSgame.AI.BasicAI.Managers
{
    class BasicShootaAIManager: BasicAIManager<Shoota>
    {
        static private Surroundings shootaSurroundings;

        
        public BasicShootaAIManager(Shoota controlledUnit)
            : base(controlledUnit, shootaSurroundings)
        {
            //AddBehaviour(new FollowPlayerBehaviour(new LinearDistancePriority<Minion>(1, 0)));
            //AddBehaviour(new AvoidBehaviour<Enemy, PlayerCharacter>(new LinearDistancePriority<Enemy>(1, 0)));
            //AddBehaviour(new AvoidBehaviour<Enemy, Projectile>(new LinearDistancePriority<Enemy>(2, 0),1));
            AddBehaviour(new ShootBehaviour<Shoota, Minion>(new LinearDistancePriority<Shoota>(10, 1000), 500));
            //AddBehaviour(new AvoidBehaviour<Enemy, Enemy>(new ConstantPriority<Enemy>(0), 0.1f));
            AddBehaviour(new ChaseBehaviour<Shoota, Minion>(new LinearDistancePriority<Shoota>(10,0),1,1));
            //SetIdleBehaviour(new ZombieBehaviour(null));
        }

        public static void SetSurroundings(Surroundings aio)
        {
            shootaSurroundings = aio;
        }
    }
    
}
