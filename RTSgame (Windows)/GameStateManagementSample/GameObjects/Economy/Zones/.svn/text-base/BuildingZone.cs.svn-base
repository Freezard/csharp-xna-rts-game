using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.GameObjects.Economy;
using RTSgame.Collision;

namespace RTSgame.GameObjects.Buildings
{
    abstract class BuildingZone<T> : GameObject, IInteractable where T : Building
    {
        protected T building;
        protected Boolean active;
        protected float range = 2;
        protected BoundingSphere collisionSphere;

        public BuildingZone(T building, float range, bool active)
            : base(building.GetPosition())
        {
            this.building = building;
            this.range = range;
            SetActive(active);
            collisionSphere = CollisionShape.NewBoundingSphere(this.GetPositionV3(), range);
        }


        public abstract void WithinZone(IInteractable other);

        public override Vector2 GetPosition()
        {
            return building.GetPosition();
        }
        
        public virtual void HandlePossibleInteraction(IInteractable otherObject)
        {

            //TODO: range should be:
            // This object's ai radius + otherObjects.size
            if (active && Calculations.IsWithin2DRange(this.GetPosition(), otherObject.GetPosition(), range)){

                
                WithinZone(otherObject);
            }
        }

        public virtual float GetMaxInteractionRange()
        {
            return range;
        }

        internal bool IsActive()
        {
            return active;
        }
        public void SetActive(Boolean active)
        {
            this.active = active;
            DebugPrinter.Write(this + " was set to " + active);
        }

        #region Collision related

        public virtual float GetSolidCollisionSize()
        {
            return collisionSphere.Radius;
        }

        /// <summary>
        /// Will never occur for Structures
        /// </summary>
        public TryToMoveAgain HandleSolidCollision()
        {
            // Do nothing
            return TryToMoveAgain.No;
        }

        /// <summary>
        /// Will never occur for Structures
        /// </summary>
        public TryToMoveAgain HandleNoSolidCollision()
        {
            // Do nothing
            return TryToMoveAgain.No;
        }
        #endregion
    }
}
