using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using System;
using RTSgame.Utilities.IO.UI;
using RTSgame.Utilities.Memory;
using RTSgame.Utilities.World;

namespace RTSgame.GameObjects.Abstract
{
    //The abstract superclass of all objects in the game world
    abstract class GameObject : AlwaysAllocatedObject, IUITrackable // can implement IInteractable
    {
        //The GameObjects position in the world
        private Vector2 position = Constants.GetCenterOfTheWorldV2();

        protected float height = 0.0f;

        protected static World world;
        //The dead value indicates that it should be removed from gameState and drawManager
        protected bool toBeRemoved = false;

        // Current Game logic phase this object is in
        protected Phase currentPhase = Phase.Initial;
        
        //The drawManager and gameState to add new GameObjects to when they enter play
       
        public GameObject(Vector2 Position)
        {
            SetPosition(Position);



            if (world != null)
            {
                RestrictToMap();
                AlignHeightToWorld();
            }
        }

        public GameObject(Vector3 Position)
        {
            SetPosition(Calculations.V3ToV2(Position));

            if (world != null)
            {
                RestrictToMap();
            }

            SetHeight(Position.Y);
        }
 
        public static void SetWorldForAllGameObjects(World world)
        {
            GameObject.world = world;
        }

        public virtual void RemoveFromGame()
        {
            //Debug.Write("GameObject.Kill: I'm dying: " + this);

            toBeRemoved = true;
           
        }
        public bool ToBeRemoved()
        {
            return toBeRemoved;
        }


        #region Position
        /// <summary>
        /// Gets the current position.
        /// </summary>
        /// <returns></returns>
        public virtual Vector2 GetPosition()
        {
            return position;
        }

        /// <summary>
        /// Sets the current position. Typically, this should never be used
        /// except for initializations. Use SetDestination instead.
        /// </summary>
        /// <param name="Position"></param>
        public virtual void SetPosition(Vector2 Position)
        {
            position = Position;
        } 
        #endregion

        #region Height
        /// <summary>
        /// Returns the height of this object.
        /// </summary>
        /// <returns></returns>
        public virtual float GetHeight()
        {
            return height;
        }

        /// <summary>
        /// Do not call for ground-bound objects, their height is
        /// automatically updated.
        /// </summary>
        /// <param name="newHeight"></param>
        public virtual void SetHeight(float newHeight)
        {
            height = newHeight;
        }

        /// <summary>
        /// Do not call for ground-bound objects, their height is
        /// automatically updated.
        /// </summary>
        public virtual void AlignHeightToWorld()
        {
            SetHeight(world.GetScaledHeight(position));
        }
        #endregion

        #region PositionV3
        /// <summary>
        /// Returns the space coordinates of this object.
        /// </summary>
        /// <returns></returns>
        public virtual Vector3 GetPositionV3()
        {
            return Calculations.V2ToV3(GetPosition(), GetHeight());
        }

        /// <summary>
        /// Do not call for ground-bound objects, their height is
        /// automatically updated.
        /// </summary>
        /// <param name="Position"></param>
        public virtual void SetPositionV3(Vector3 Position)
        {
            SetPosition(Calculations.V3ToV2(Position));
            SetHeight(Position.Y);
        }
        #endregion



        public static bool VectorIsOutsideMap(Vector2 Position)
        {
            return (Position.X < 0 || Position.X > world.GetDimension() ||
                    Position.Y < 0 || Position.Y > world.GetDimension());
        }

        /// <summary>
        /// Fixes the GameObject so that it returns to map boundaries
        /// </summary>
        public virtual void RestrictToMap()
        {
            //Vector2 testPosition = position;
            position.X = MathHelper.Clamp(position.X, (float) 1, (float) world.GetDimension() - 1);
            position.Y = MathHelper.Clamp(position.Y, (float) 1, (float) world.GetDimension() - 1);

            //if (testPosition != position)
            //    throw new Exception("GameObject was outside map boundaries, x:" + testPosition.X + " y:" + testPosition.Y);
        }

        /// <summary>
        /// Destroys the GameObject if it is outside the map boundaries
        /// </summary>
        public virtual void KillIfOutsideMap()
        {
            if (VectorIsOutsideMap(position))
            {
                RestrictToMap();
                RemoveFromGame();
            }
            
        }

        public void SetToCurrentPhase()
        {
            currentPhase = UpdatePhase.CurrentPhase;
        }

        public Phase GetPhase()
        {
            return currentPhase;
        }

        public override string ToString()
        {
            return base.ToString() + " @ " + this.GetPosition();
        }

    }
}
