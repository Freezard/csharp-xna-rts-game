using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.AI.Abstract
{
    //A selector chooses a sub-node (behaviour or selector) according to certain rules
    abstract class Selector<T>: AINode<T> where T:IIntelligent
    {
        //All choices available to a selector
        protected List<AINode<T>> choices;
        //AI manager that this selector belongs to
        protected AIManager<T> aiManager;
        //What a selector should do if its child node finished successfully
        public virtual void finished()
        {
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

        //What a selector should do if child node was interrupted
        //public abstract void interrupted(string error)

        //Sets the AImanager for this and all sub-selectors
        public void SetAIManager(AIManager<T> aiManager)
        {
            //Debug.Write("setting manager for " + this);
            this.aiManager = aiManager;
            foreach (AINode<T> choice in choices)
            {
                if (choice is Selector<T>)
                {
                    ((Selector<T>)choice).SetAIManager(aiManager);
                }
            }
        }

    }
}
