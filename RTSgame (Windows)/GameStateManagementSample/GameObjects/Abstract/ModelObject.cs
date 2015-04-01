using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.Collision;
using RTSgame.Utilities;
using RTSgame.GameObjects.Components;


namespace RTSgame.GameObjects.Abstract
{
    //A GameObject that has a model
    abstract class ModelObject : GameObject, ICollidable, IDrawableWorld
    {
     //   protected Model model;
        
        protected float scale = 0.01f;
      //  protected Texture2D texture;
        BoundingBox collisionBox;
        BoundingSphere collisionSphere;
        protected Vector3 angleAround = Vector3.Zero;
        private ModelComponent modelComp;

        

        public ModelObject(Vector2 pos, ModelComponent modelComp):base(pos)
        {
            this.modelComp = modelComp;
            
            SetDestination(GetPosition());
            SetDestinationHeight(GetHeight());

            
            
        }

        public ModelComponent ModelComp
        {
            get { return modelComp; }

        }

        

        public void InitializeCollisionBox()
        {
            collisionBox = CollisionShape.NewBoundingBox(this);
            collisionSphere = CollisionShape.NewBoundingSphere(this);
        }

        public virtual float GetSolidCollisionSize()
        {
            return GetCollisionSphere().Radius;
        }

        public abstract float GetMaxInteractionRange();

        // Updates the position of the collision box
        public void UpdateCollisionBox()
        {
            /*
            float lengthX = (collisionBox.Max.X - collisionBox.Min.X) / 2;
            float lengthZ = (collisionBox.Max.Z - collisionBox.Min.Z) / 2;
            float lengthY = (collisionBox.Max.Y - collisionBox.Min.Y) / 2;

            collisionBox.Max.X = GetPosition().X + lengthX;
            collisionBox.Min.X = GetPosition().X - lengthX;
            collisionBox.Max.Z = GetPosition().Y - lengthZ;
            collisionBox.Min.Z = GetPosition().Y + lengthZ;
            // Joakim ninja edit:
            //collisionBox.Max.Y += GetHeight() - collisionBox.Min.Y;
            //collisionBox.Min.Y = GetHeight();
            collisionBox.Max.Y = GetHeight() + lengthY;
            collisionBox.Min.Y = GetHeight();
            */
            collisionSphere.Center = GetPositionV3();
            // Ofta radien / 2, men men...
            collisionSphere.Center.Y = GetHeight() + collisionSphere.Radius / 2;
        }


        public void UpdateCollisionBoxToDestination()
        {
            /*
            float lengthX = (collisionBox.Max.X - collisionBox.Min.X) / 2;
            float lengthZ = (collisionBox.Max.Z - collisionBox.Min.Z) / 2;
            float lengthY = (collisionBox.Max.Y - collisionBox.Min.Y) / 2;

            collisionBox.Max.X = GetDestination().X + lengthX;
            collisionBox.Min.X = GetDestination().X - lengthX;
            collisionBox.Max.Z = GetDestination().Y - lengthZ;
            collisionBox.Min.Z = GetDestination().Y + lengthZ;
            // Joakim ninja edit:
            //collisionBox.Max.Y += GetHeight() - collisionBox.Min.Y;
            //collisionBox.Min.Y = GetHeight();
            collisionBox.Max.Y = GetDestinationHeight() + lengthY;
            collisionBox.Min.Y = GetDestinationHeight();
            */
            collisionSphere.Center = GetDestinationV3();
            // Ofta radien / 2, men men...
            collisionSphere.Center.Y = GetDestinationHeight() + collisionSphere.Radius / 2;
        }

        

        public float GetScale()
        {
            return scale;
        }

        public void SetScale(float Size)
        {
            scale = Size;
        }

       

        public float GetFacingAngleOnXZPlane()
        {
            return angleAround.Y;
        }
        public Vector3 GetAngles()
        {
            return angleAround;
        }

        public void SetFacingAngleOnXZPlane(float radianAngle)
        {
            angleAround.Y = radianAngle;
        }

        

        

        /// <summary>
        /// Destroys the GameObject if it is outside the map boundaries
        /// </summary>
        public override void KillIfOutsideMap()
        {
            if (VectorIsOutsideMap(GetPosition()) ||
                VectorIsOutsideMap(GetDestination())
                )
            {
                RestrictToMap();
                RemoveFromGame();
            }
        }

        /// <summary>
        /// Fixes the GameObject so that it returns to map boundaries
        /// </summary>
        public override void RestrictToMap()
        {
            base.RestrictToMap();
            //Vector2 testPosition = position;
            destination.X = MathHelper.Clamp(destination.X, (float)1, (float)world.GetDimension() - 1);
            destination.Y = MathHelper.Clamp(destination.Y, (float)1, (float)world.GetDimension() - 1);

            //if (testPosition != position)
            //    throw new Exception("GameObject was outside map boundaries, x:" + testPosition.X + " y:" + testPosition.Y);
        }


        #region Collision related

        public BoundingSphere GetCollisionSphere()
        {
            return collisionSphere;
        }

        public BoundingBox GetCollisionBox()
        {
            return collisionBox;
        }

        public abstract CollidableType GetCollidableType();

        public override void AlignHeightToWorld()
        {
            base.AlignHeightToWorld();
        }

        
        public virtual TryToMoveAgain HandleSolidCollision()
        {
            return TryToMoveAgain.No;
        }

        public virtual TryToMoveAgain HandleNoSolidCollision()
        {

            //The basic version of this method does nothing, override in subclasses!

            return TryToMoveAgain.No;
        }

        public virtual void HandlePossibleInteraction(IInteractable other)
        {
            //The basic version of this method does nothing, override in subclasses!
        }

        #region Destination
        Vector2 destination;
        float destinationHeight;

        /// <summary>
        /// Gets the position which this object will move to eventually.
        /// Typically, for logic, it is much more stable to use GetPosition()
        /// instead of this, as destination will change a lot during the logic phase.
        /// </summary>
        /// <returns></returns>
        public virtual Vector2 GetDestination()
        {
            return destination;
        }

        /// <summary>
        /// Sets the position which this object will move to.
        /// </summary>
        /// <param name="Destination"></param>
        public virtual void SetDestination(Vector2 Destination)
        {
            destination = Destination;
        }

        /// <summary>
        /// Moves the destination position according to the given vector.
        /// </summary>
        /// <param name="Change"></param>
        public virtual void MoveDestination(Vector2 Change)
        {
            destination += Change;
        }

        /// <summary>
        /// Moves the current position towards a target.
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="Distance"></param>
        public virtual void MoveTowards(Vector2 Target, float Distance)
        {
            destination = Calculations.OffsetTowardsTarget(destination, Target, Distance);
        }

        private bool teleporting = false;

        /// <summary>
        /// Teleport to target argument.
        /// </summary>
        /// <param name="Target"></param>
        public void Teleport(Vector2 Target)
        {
            SetDestination(Target);
            teleporting = true;
        }

        /// <summary>
        /// True if this object is teleporting.
        /// </summary>
        /// <returns></returns>
        public bool GetTeleportStatus()
        {
            return teleporting;
        }

        /// <summary>
        /// Resets teleport status
        /// </summary>
        public void DoneTeleporting()
        {
            teleporting = false;
        }
        #endregion

        #region DestinationHeight
        public virtual float GetDestinationHeight()
        {
            return destinationHeight;
        }

        /// <summary>
        /// Only call this for objects not bound to the surface.
        /// </summary>
        /// <param name="Destination"></param>
        public virtual void SetDestinationHeight(float Height)
        {
            destinationHeight = Height;
        }

        /// <summary>
        /// Do not use if not for very technical reasons.
        /// Height is automatically updated for all ground-bound objects.
        /// </summary>
        public virtual void AlignDestinationHeightToGround()
        {
            SetDestinationHeight(world.GetScaledHeight(GetDestination()));
        }
        #endregion

        #region DestinationV3
        /// <summary>
        /// Returns the space coordinates for Destination.
        /// </summary>
        /// <returns></returns>
        public virtual Vector3 GetDestinationV3()
        {
            return Calculations.V2ToV3(GetDestination(), GetDestinationHeight());
        }

        /// <summary>
        /// Only call this for objects not bound to the surface.
        /// </summary>
        /// <param name="Destination"></param>
        public virtual void SetDestinationV3(Vector3 Destination)
        {
            SetDestination(Calculations.V3ToV2(Destination));
            SetDestinationHeight(Destination.Y);
        }
        #endregion

        #region HasMoved
        /// <summary>
        /// Strictly Collision related, do not use outside Collision package.
        /// </summary>
        private bool HasMoved;
        /// <summary>
        /// Strictly Collision related, do not use outside Collision package.
        /// </summary>
        public void SetHasMoved(bool value)
        {
            HasMoved = value;
        }
        /// <summary>
        /// Strictly Collision related, do not use outside Collision package.
        /// </summary>
        public bool GetHasMoved()
        {
            return HasMoved;
        }
        #endregion

        #endregion
    }
}
