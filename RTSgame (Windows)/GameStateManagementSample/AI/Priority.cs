using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.AI.BasicAI.Priorities
{
    abstract class Priority<TypeAgent> where TypeAgent : AIControlledUnit
    {
        public abstract float GetPriority(TypeAgent me, IInteractable otherGameObject, float sightRange);
    }
}
