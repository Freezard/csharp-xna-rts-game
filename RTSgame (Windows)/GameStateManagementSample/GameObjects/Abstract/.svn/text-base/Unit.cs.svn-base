using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.AI;
using RTSgame.Collision.CollisionHandling;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Abstract
{

    /// <summary>
    /// Class that contains everything that all Units
    /// (Hero, Enemy, Minion) have in common.
    /// And implements all the behaviour that all Units must have.
    /// </summary>
    abstract class Unit : PlayerOwnedObject, IHandleSolidCollision
    {

        
        public Unit(Vector2 newPosition, ModelComponent modelComp, Player owner, int maxHitPoints)
            : base(newPosition, modelComp, owner, maxHitPoints)
        {
            
        }
       
        public override CollidableType GetCollidableType()
        {
            return CollidableType.None;
        }

        public override TryToMoveAgain HandleSolidCollision()
        {
            return ObstacleStrategy.AdjustMovement(this);
        }

        public override TryToMoveAgain HandleNoSolidCollision()
        {
            return ObstacleStrategy.HandleNoSolidCollision(this);
        }

        public virtual MovementStrategy GetObstacleHandlingStrategy()
        {
            return MovementStrategy.SearchAndSlide;
        }
        

        #region ObstacleHandling related

        private MovementStrategyPhase movementStrategyPhase = MovementStrategyPhase.FullStop;
        public void SetObstacleHandlingStrategyPhase(MovementStrategyPhase s)
        {
            movementStrategyPhase = s;
        }
        public MovementStrategyPhase GetObstacleHandlingStrategyPhase()
        {
            return movementStrategyPhase;
        }

        private int movementIteration = 0;
        /// <summary>
        /// Obstacle Handling Iteration
        /// </summary>
        public void SetOHMovementIteration(int i)
        {
            movementIteration = i;
        }
        /// <summary>
        /// Obstacle Handling Iteration
        /// </summary>
        public int GetOHMovementIteration()
        {
            return movementIteration;
        }

        private bool preferClockWise;

        public bool GetPreferClockWise()
        {
            return preferClockWise;
        }

        public void ChangePreferClockWise()
        {
            preferClockWise = !preferClockWise;
        }

        public void SetPreferClockWise(bool v)
        {
            preferClockWise = v;
        }
        #endregion


        
        internal float getHealth()
        {
            return hitPoints;
        }
    }
}
