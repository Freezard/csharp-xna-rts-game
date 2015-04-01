using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using RTSgame.AI;
using RTSgame.GameObjects;
using RTSgame.AI.Abstract;

namespace RTSgame.AI
{
    class AIManagerMinion: AIManager<Minion>
    {

        
        public AIManagerMinion(Minion associatedGameObject, Surroundings surroundings):base(associatedGameObject, surroundings)
        {



            SetEntryNode(new PrioritySelectorMinion());
            startChoose();

        }
    }
}
