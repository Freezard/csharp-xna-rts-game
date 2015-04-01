using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.AI.Abstract
{
    abstract class Behaviour<T> : AINode<T> where T : IIntelligent
    {
        //Runs this behaviour
        public abstract void execute(GameTime gameTime, T gameObject);
        public override void choose(AIManager<T> aiManager)
        {
            aiManager.setChosen(this);
        }
    }
}
