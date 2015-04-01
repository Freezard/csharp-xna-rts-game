using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.AI.Abstract
{
    class SequenceSelector<T> : Selector<T> where T : IIntelligent
    {
        int index = 0;
        public SequenceSelector(List<AINode<T>> choices)
        {
            this.choices = new List<AINode<T>>(choices);
            foreach (AINode<T> choice in choices)
            {
                choice.SetParent(this);
            }
        }
        public override void choose(AIManager<T> aiManager)
        {
            choices[index].choose(this.aiManager);
            
        }
        public override void finished()
        {
            index++;
            if (index >= choices.Count)
            {
                index = 0;
                if (parent != null)
                {
                    parent.finished();
                }
                //Root of tree, loop!
                else
                {
                    this.choose(aiManager);
                }
                
            }
            else
            {
                this.choose(aiManager);
            }

        }
    }
}
