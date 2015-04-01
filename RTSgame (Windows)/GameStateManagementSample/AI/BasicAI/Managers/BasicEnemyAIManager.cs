using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects;
using RTSgame.AI.BasicAI.Priorities;
using RTSgame.GameObjects.Abstract;
using RTSgame.GameObjects.Economy.Buildings;
using RTSgame.GameObjects.Buildings;
using RTSgame.AI.BasicAI.Behaviours;

namespace RTSgame.AI.BasicAI.Managers
{
    class BasicEnemyAIManager : BasicAIManager<Enemy>
    {
        static private Surroundings enemySurroundings;

        public BasicEnemyAIManager(Enemy controlledUnit)
            : base(controlledUnit, enemySurroundings)
        {
            //AddBehaviour(new FollowPlayerBehaviour(new LinearDistancePriority<Minion>(1, 0)));
            //AddBehaviour(new AvoidBehaviour<Enemy, PlayerCharacter>(new LinearDistancePriority<Enemy>(1, 0)));
            AddBehaviour(new AvoidBehaviour<Enemy, Projectile>(new LinearDistancePriority<Enemy>(2, 0),1));
            //AddBehaviour(new ShootBehaviour<Minion, Enemy>(new LinearDistancePriority<Minion>(10, 1000)));
            //AddBehaviour(new AvoidBehaviour<Enemy, Enemy>(new ConstantPriority<Enemy>(0), 0.1f));
            //AddBehaviour(new ChaseBehaviour<Enemy, Building>(new LinearDistancePriority<Enemy>(10,0),1,1));
            //AddBehaviour(new ConfidentAttackBehaviour(new ConstantPriority<Enemy>(10), 1000));
            AddBehaviour(new WanderBehaviour<Enemy>(new ConstantPriority<Enemy>(0),1000));
            //SetIdleBehaviour(new ZombieBehaviour(null));
        }

        public static void SetSurroundings(Surroundings aio)
        {
            enemySurroundings = aio;
        }
    }
}
