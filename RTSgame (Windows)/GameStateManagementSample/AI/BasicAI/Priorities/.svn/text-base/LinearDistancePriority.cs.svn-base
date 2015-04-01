using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;

namespace RTSgame.AI.BasicAI.Priorities
{
    class LinearDistancePriority<TypeAgent> : Priority<TypeAgent> where TypeAgent : AIControlledUnit
    {
        float gradientFactor;
        float constantAdd;
        public LinearDistancePriority(float gradientFactor, float constantAdd)
        {
            this.gradientFactor = gradientFactor;
            this.constantAdd = constantAdd;
        }

        public override float GetPriority(TypeAgent me, GameObjects.Abstract.IInteractable otherGameObject, float sightRange)
        {
            return gradientFactor*Vector2.Distance(me.GetPosition(), otherGameObject.GetPosition())
                 + constantAdd;
        }
    }
}
