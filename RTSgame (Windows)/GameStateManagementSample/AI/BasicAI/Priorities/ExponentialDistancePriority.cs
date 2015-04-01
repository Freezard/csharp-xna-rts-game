using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;

namespace RTSgame.AI.BasicAI.Priorities
{
    class ExponentialDistancePriority<TypeAgent> : Priority<TypeAgent> where TypeAgent : AIControlledUnit
    {
        float factor;
        float constantAdd;
        public ExponentialDistancePriority(float factor, float constantAdd)
        {
            this.factor = factor;
            this.constantAdd = constantAdd;
        }

        public override float GetPriority(TypeAgent me, GameObjects.Abstract.IInteractable otherGameObject, float sightRange)
        {

            return factor * (float)Math.Exp(Vector2.Distance(me.GetPosition(), otherGameObject.GetPosition())) + constantAdd;
        }
    }
}
