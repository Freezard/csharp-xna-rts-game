using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using RTSgame.AI.BasicAI.Priorities;
using Microsoft.Xna.Framework;

namespace RTSgame.AI.BasicAI
{
    //A basic AI-behaviour, for an agent of type TypeAgent
    abstract class BasicBehaviour<TypeAgent> where TypeAgent:AIControlledUnit

    {
        //The priority function to use
        private Priority<TypeAgent> priority;
        
        public BasicBehaviour(Priority<TypeAgent> priority)
        {

            this.priority = priority;
        }
        //Checks if this behaviour will fire at all
        public abstract Boolean FulfilCriteria(TypeAgent me, IInteractable otherGameObject);
        //Asks priority function to calculate priority
        public float GetPriority(TypeAgent me, IInteractable otherGameObject, float sightRange)
        {
            return priority.GetPriority(me, otherGameObject, sightRange);

        }
        //Apply the behaviour (modifiy both objects involved
        public abstract void ApplyOn(TypeAgent me, IInteractable otherGameObject);


        public virtual void Update(GameTime gameTime)
        {
            //Do nothing, replace in subclass if necessary
        }
    }
}
