using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects;
using RTSgame.GameObjects.Units;
using RTSgame.GameObjects.Abstract;
using RTSgame.AI.Abstract;
using RTSgame.Utilities;


namespace RTSgame.AI
{
    class PrioritySelectorMinion : PrioritySelector<Minion>
    {
        BehaviourA behA = new BehaviourA();
        BehaviourB behB = new BehaviourB();
        BehaviourC behC = new BehaviourC();
        public PrioritySelectorMinion()
        {
            this.choices = new List<AINode<Minion>>();
            choices.Add(behA);
            choices.Add(behB);
            choices.Add(behC);
            foreach (AINode<Minion> choice in choices)
            {
                choice.SetParent(this);
            }
        }
        public override void choose(AIManager<Minion> aiManager)
        {
            List<GameObject> surroundings = aiManager.GetSurroundings();
            int currentPriority = 0;

            foreach (GameObject gameObject in surroundings)
            {
                if (currentPriority < 1 && gameObject is Enemy)
                {
                    currentPriority = 1;
                    behA.choose(this.aiManager);

                }
                else if (currentPriority < 2 && gameObject is PlayerCharacter)
                {
                    currentPriority = 2;
                    behB.SetParametres(gameObject);
                    behB.choose(this.aiManager);
                }

            }
            if (currentPriority == 0)
            {
                behC.choose(this.aiManager);

            }

        }
    }

}
