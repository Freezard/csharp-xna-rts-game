using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSgame.GameObjects.Abstract
{
    public enum MovementStrategyPhase : byte
    {
        // Standard
        Passive,
        // Player
        PBumb, PSliding, PSeemsOk,
        // Search and Slide
        WideningSearch, NarrowingSearch, Sliding,
        // Critical phases
        InternalStuckTest, IsStuck, StuckHandling,
        PassThrough,
        // not implemented yet (probably don't have to)
        FullStop
    };

    public enum MovementStrategy : byte
    {
        SearchPathForPlayer,
        PassThrough,
        FullStop,
        SimpleSlide,
        ExtendedSlide,
        SearchAndSlide
    }

    interface IHandleSolidCollision : ICollidable
    {
        MovementStrategy GetObstacleHandlingStrategy();


        // ---------------------------------------------
        // Internal: Do not override
        // ---------------------------------------------

        int GetOHMovementIteration();
        void SetOHMovementIteration(int i);

        MovementStrategyPhase GetObstacleHandlingStrategyPhase();
        void SetObstacleHandlingStrategyPhase(MovementStrategyPhase m);

        bool GetPreferClockWise();

        void ChangePreferClockWise();

        void SetPreferClockWise(bool v);

    }
}
