using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.AI.Abstract
{
    //One part of the AI tree
    abstract class AINode<T> where T : IIntelligent
    {
        //The parent selector
        protected Selector<T> parent;
        //Chooses this AINode
        public abstract void choose(AIManager<T> aiManager);

        public void SetParent(Selector<T> parent)
        {
            this.parent = parent;
        }
    }
}
