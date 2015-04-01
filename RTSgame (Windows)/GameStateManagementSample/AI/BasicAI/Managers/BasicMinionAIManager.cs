using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects;
using RTSgame.AI.BasicAI.Priorities;
using RTSgame.AI.BasicAI.Behaviours;

namespace RTSgame.AI.BasicAI
{
    class BasicMinionAIManager:BasicAIManager<Minion>
    {
        static private Surroundings enemySurroundings;

        public BasicMinionAIManager(Minion controlledUnit)
            : base(controlledUnit, enemySurroundings)
        {
            //AddBehaviour(new FollowPlayerBehaviour(new LinearDistancePriority<Minion>(1, 0),1));
            //AddBehaviour(new ChaseBehaviour<Minion, Enemy>(new LinearDistancePriority<Minion>(1, 0)));
            //AddBehaviour(new ShootBehaviour<Minion, Enemy>(new LinearDistancePriority<Minion>(3, 0),1000));
            AddBehaviour(new MoveInFormationBehaviour(new ConstantPriority<Minion>(10)));
            AddBehaviour(new MinionAttackBehaviour(new ConstantPriority<Minion>(100)));
            SetIdleBehaviour(new StopBehaviour<Minion>(new ConstantPriority<Minion>(0)));
        }

        public static void SetSurroundings(Surroundings aio)
        {
            enemySurroundings = aio;
        }
    }
}
