using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using RTSgame.AI.BasicAI.Priorities;

namespace RTSgame.AI.BasicAI
{
    //A simple chase behaviour without intercept, agent moves towards objects of type TypeChaseObject at speed maxspeed*speedScale
    class ChaseBehaviour<TypeAgent, TypeChaseObject> : BasicBehaviour<TypeAgent>
        where TypeChaseObject : IInteractable
        where TypeAgent : AIControlledUnit
    {
        float speedScale;
        float minimumDistance;
        public ChaseBehaviour(Priority<TypeAgent> priority, float speedScale, float minimumDistance)
            : base(priority)
        {
            this.speedScale = speedScale;
            this.minimumDistance = minimumDistance;
        }

        public override void ApplyOn(TypeAgent me, IInteractable otherGameObject)
        {
            me.MoveToAndStop(otherGameObject.GetPosition(), speedScale);
        }

        public override bool FulfilCriteria(TypeAgent me, IInteractable otherGameObject)
        {
            return (otherGameObject is TypeChaseObject) && !Calculations.IsWithin2DRange(me.GetPosition(), otherGameObject.GetPosition(), minimumDistance);
        }
    }
}
