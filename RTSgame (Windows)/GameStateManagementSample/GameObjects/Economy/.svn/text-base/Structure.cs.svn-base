using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Economy
{
    abstract class Structure : ModelObject, ILogic
    {
        public Structure(Vector2 newPosition, ModelComponent modelComp)
            : base(newPosition, modelComp)
        {

        }

        public override CollidableType GetCollidableType()
        {
            return CollidableType.Structure;
        }

        public override float GetMaxInteractionRange()
        {
            return Constants.DefaultMaxInteractionRange;
        }

        public abstract void UpdateLogic(GameTime gameTime);

    }
}
