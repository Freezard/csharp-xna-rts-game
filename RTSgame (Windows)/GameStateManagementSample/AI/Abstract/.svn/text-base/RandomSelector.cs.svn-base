using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.AI.Abstract
{
    class RandomSelector<T> :Selector<T> where T:IIntelligent
    {
        
        Random random = new Random();
        

        public RandomSelector(List<AINode<T>> choices)
        {
            this.choices = new List<AINode<T>>(choices);
            foreach (AINode<T> choice in choices)
            {
                choice.SetParent(this);
            }
        }

        public override void choose(AIManager<T> aiManager)
        {
            int index = random.Next(choices.Count);
            choices[index].choose(this.aiManager);
        }
        
    }
}
