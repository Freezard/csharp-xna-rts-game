using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.AI.BasicAI.Priorities
{
    class ConstantPriority<TypeAgent>: Priority<TypeAgent> where TypeAgent:AIControlledUnit
    {
        float value;
        public ConstantPriority(float value)
        {
            this.value = value;
        }

        public override float GetPriority(TypeAgent me, GameObjects.Abstract.IInteractable otherGameObject, float sightRange)
        {
            return value;
        }
    }
}
