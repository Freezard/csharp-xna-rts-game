using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Doodads
{
    abstract class DoodadModel : ModelObject, IDoodad
    {
        public DoodadModel(Vector2 newPosition, ModelComponent modelComp)
            : base(newPosition, modelComp)
        {
        }

        public void ModifySize(float SizeFactor)
        {
            SetScale(GetStandardSize() * SizeFactor);
        }

        public abstract float GetStandardSize();

        public override CollidableType GetCollidableType()
        {
            return CollidableType.Doodad;
        }

        public override float GetMaxInteractionRange()
        {
            throw new NotImplementedException();
        }
    }
}
