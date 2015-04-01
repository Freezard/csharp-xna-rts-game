using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.Collision.CollisionHandling
{

    static class ObstacleStrategy
    {
        /* Things that must be set manually be each phase:
         * MovementIteration
         * tryToMoveAgain
         * (Destination)
         * Call to stucktest
         */
         
        // Future improvements:
        // *make movementIteration local or
        //  replace it completely with attempt
        //  (only if it feels unecessary to divide
        //   up movement over several frames)
        // *make "collidable" argument a
        //  static refenence instead (clears up code)
        // *make sure MovementStrategyPhase is never set
        //  manually in the Strategy Methods
        // *Have more different constants for the different
        //  behaviours
        // *replace MovementStrategy with subclassing
        // *make MovementStrategyPhase local only
         
        /*
         * SAS = Search And Slide
         */

        // Sets the maximum angle to 2 steps above 90'
        // Big angle for minions and alike that which
        // to get around stuff
        //public const float SASMovementIterationSubAngle = (float)Math.PI * 2.0f / ((float)(MovementIterationMax - 2) * 3.0f);
        public const float SASMovementIterationSubAngle = (float)Math.PI * 3.0f / ((float)(MovementIterationMax) * 4.0f);

        // Sets the maximum angle to 90'
        // For things like player character
        public const float PlayerIterationSubAngle = (float)Math.PI * 1.0f / ((float)(MovementIterationMax) * 2.0f);

        public const int MovementIterationMax = 8;
        public const int StuckTestIterationMax = 4;

        // Sets the maximum angle to 1 step below 360'
        private const float StuckIterationSubAngle = (float)Math.PI * 2.0f / ((float)MovementIterationMax - 1);


        private static int movementAttempt;
        private static Vector2 PossibleMovement;
        private static TryToMoveAgain tryToMoveAgain;

        //private static bool preferClockWise = false;

        public static TryToMoveAgain AdjustMovement(IHandleSolidCollision collidable)
        {
            // both GetMovementAttempt and PossibleMovement
            // could technically be static ObstacleStrategy
            // variables instead
            if (GetMovementAttempt() == 0)
                PossibleMovement = collidable.GetDestination() - collidable.GetPosition();

            tryToMoveAgain = TryToMoveAgain.No;

            // We assume that we find a critical phase
            bool abortRegularBehaviour = true;

            // is there a critical phase?
            switch (collidable.GetObstacleHandlingStrategyPhase())
            {
                case MovementStrategyPhase.InternalStuckTest:
                    collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.StuckHandling);
                    collidable.SetOHMovementIteration(0);
                    StuckHandling(collidable);
                    break;
                case MovementStrategyPhase.IsStuck:
                    StuckTest(collidable);
                    break;
                case MovementStrategyPhase.StuckHandling:
                    StuckHandling(collidable);
                    break;
                case MovementStrategyPhase.PassThrough:
                    collidable.SetDestination(collidable.GetPosition() + PossibleMovement);
                    collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.Passive);
                    tryToMoveAgain = TryToMoveAgain.No;
                    break;
                default:
                    abortRegularBehaviour = false;
                    break;
            }

            if (!abortRegularBehaviour)
            {
                switch (collidable.GetObstacleHandlingStrategy())
                {
                    case MovementStrategy.SearchPathForPlayer:
                        // Initialize:
                        // SearchPathForPlayer.GetOHMovementIteration has 1 as default value
                        if (collidable.GetOHMovementIteration() == 0 &&
                            collidable.GetObstacleHandlingStrategyPhase() == MovementStrategyPhase.Passive)
                        {
                            collidable.SetPreferClockWise(EasyRandom.Bool5050());
                            collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.PBumb);
                        }
                        if (collidable.GetObstacleHandlingStrategyPhase() == MovementStrategyPhase.PSeemsOk)
                        {
                            collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.PSliding);
                        }

                        SearchPathForPlayer(collidable);
                        break;
                    case MovementStrategy.PassThrough:
                        PassThrough(collidable);
                        break;
                    case MovementStrategy.SimpleSlide:
                        SimpleSlide(collidable);
                        break;
                    case MovementStrategy.ExtendedSlide:
                        ExtendedSlide(collidable);
                        break;
                    case MovementStrategy.SearchAndSlide:
                        SearchAndSlide(collidable);
                        break;
                    default:
                        FullStop(collidable);
                        break;
                }
            }

            SetMovementAttempt(GetMovementAttempt() + 1);

            if (tryToMoveAgain == TryToMoveAgain.No)
                SetMovementAttempt(0);

            return tryToMoveAgain;
        }

        private static void FullStop(IHandleSolidCollision collidable)
        {
            collidable.SetDestination(collidable.GetPosition());
            tryToMoveAgain = TryToMoveAgain.No;
        }

        private static void PassThrough(IHandleSolidCollision collidable)
        {
            collidable.SetDestination(collidable.GetPosition() + PossibleMovement);
            collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.Passive);

            // TODO: for player, this should be set to 1.
            // or better, make players start value 0 like the others.
            collidable.SetOHMovementIteration(0);

            tryToMoveAgain = TryToMoveAgain.No;
        }

        private static void GiveUp(IHandleSolidCollision collidable)
        {
            StuckTest(collidable);
            //return FullStop(collidable);
        }

        private static void SimpleSlide(IHandleSolidCollision collidable)
        {
            // not needed I think:
            tryToMoveAgain = TryToMoveAgain.No;

            if (GetMovementAttempt() >= 2)
            {
                GiveUp(collidable);
            }
            else
                ExtendedSlide(collidable);

        }

        private static void ExtendedSlide(IHandleSolidCollision collidable)
        {
            
            // let's determine in which direction we would
            // like to go first
            Vector2 firstDirection;
            Vector2 secondDirection;

            if (PossibleMovement.X >= PossibleMovement.Y)
            {
                firstDirection = new Vector2(PossibleMovement.X, 0);
                secondDirection = new Vector2(0, PossibleMovement.Y);
            }
            else
            {
                firstDirection = new Vector2(0, PossibleMovement.Y);
                secondDirection = new Vector2(PossibleMovement.X, 0);
            }

            // OK, now that is done.
            // Let's try moving!

            //TODO: if our motion stops, then checkforcollision will
            // give ok(!). this is wrong.

            tryToMoveAgain = TryToMoveAgain.Yes;

            switch (GetMovementAttempt())
            {
                case 0:
                    // lets try a restricted movement
                    collidable.SetDestination(collidable.GetPosition() + firstDirection);
                    break;
                case 1:
                    // first direction was not possible, lets try the second:
                    collidable.SetDestination(collidable.GetPosition() + secondDirection);
                    break;
                case 2:
                    // test 90 degrees movement.
                    collidable.SetDestination(collidable.GetPosition() +
                        Calculations.RotateClockwise(
                            PossibleMovement,
                            Calculations.Pi2th));
                    break;
                case 3:
                    // test 90 degrees movement in other direction
                    collidable.SetDestination(collidable.GetPosition() +
                        Calculations.RotateCounterClockwise(
                            PossibleMovement,
                            Calculations.Pi2th));
                    break;
                default:
                    // no attempt was successfull, lets give up
                    GiveUp(collidable);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collidable"></param>
        private static void SearchAndSlide(IHandleSolidCollision collidable)
        {
            tryToMoveAgain = TryToMoveAgain.No;

            switch (GetMovementAttempt())
            {
                case 0:
                    //Vector2 PossibleMovement = collidable.GetDestination() - collidable.GetPosition();

                    // change strategy if needed

                    if (collidable.GetObstacleHandlingStrategyPhase() == MovementStrategyPhase.NarrowingSearch &&
                        collidable.GetOHMovementIteration() <= 0)
                    {
                        StuckTest(collidable);
                        break;
                    }

                    if (collidable.GetOHMovementIteration() <= 0)
                    {
                        collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.WideningSearch);
                        collidable.SetOHMovementIteration(0);
                        //if (EasyRandom.Bool5050())
                        //    collidable.ChangePreferClockwiseMovement();
                    }

                    if (collidable.GetOHMovementIteration() >= MovementIterationMax)
                    {
                        collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.NarrowingSearch);
                        //collidable.ChangePreferClockwiseMovement();
                        collidable.SetOHMovementIteration(MovementIterationMax - 1);
                    }

                    // try the new position
                    collidable.SetDestination(GetSearchAlternatePathForUnit(collidable, SASMovementIterationSubAngle));
                    tryToMoveAgain = TryToMoveAgain.Yes;
                    break;

                default:
                    // act according to strategy
                    switch (collidable.GetObstacleHandlingStrategyPhase())
                    {
                        case MovementStrategyPhase.NarrowingSearch:
                            DecMovementIteration(collidable);
                            //collidable.ChangePreferClockwiseMovement();
                            break;
                        case MovementStrategyPhase.WideningSearch:
                            IncMovementIteration(collidable);
                            IncMovementIteration(collidable);
                            //collidable.ChangePreferClockwiseMovement();
                            break;
                        case MovementStrategyPhase.Sliding:
                            IncMovementIteration(collidable);
                            IncMovementIteration(collidable);
                            break;
                    }

                    // reset movement
                    collidable.SetDestination(collidable.GetPosition());

                    tryToMoveAgain = TryToMoveAgain.No;
                    break;
            }
        }

        private static void SearchPathForPlayer(IHandleSolidCollision collidable)
        {
            if (GetMovementAttempt() > MovementIterationMax)
            {
                StuckTest(collidable);
            }
            else
            {
                collidable.SetDestination(GetSearchAlternatePathForPlayer(collidable, PlayerIterationSubAngle));
                tryToMoveAgain = TryToMoveAgain.Yes;
                IncMovementIteration(collidable);
            }
        }

        private static void StuckTest(IHandleSolidCollision collidable)
        {
            collidable.SetDestination(collidable.GetPosition());
            collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.InternalStuckTest);
            tryToMoveAgain = TryToMoveAgain.Yes;
        }

        private static void StuckHandling(IHandleSolidCollision collidable)
        {
            // Gör en search runtomkring. låt avståndet va lite kortare.
            // om man hittar någonstans man kan gå,
            // behöver inte nödvändigtvis flytta dit.
            // om man inte hittar någonstans man kan gå
            // byt till PassThrough

            // Discussion: if PossibleMovement is zero, use something
            // else instead, like getSpeed or Constants.MinSpeed
            // -? PossibleMovement should never be zero in the first place,
            //    that situation is catched before reaching here.

            // If we have tested all directions, and all failed, let
            // the object go where it likes.
            if (collidable.GetOHMovementIteration() >= StuckTestIterationMax)
            {
                // DEBUG
                //if (collidable.GetCollidableType() == CollidableType.Hero)
                //    Debug.Write("ObstacleStrategy.StuckHandling: " + collidable + " got stuck!");
                
                // Let's just go straight forward 
                const float stuckSpeed = 0.3f;
                collidable.SetDestination(collidable.GetPosition() + PossibleMovement * stuckSpeed);

                // No more stuck testing
                collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.IsStuck);

                // TODO: for player, this should be set to 1.
                // or better, make players start value 0 like the others.
                collidable.SetOHMovementIteration(0);

                // No further checking
                tryToMoveAgain = TryToMoveAgain.No;
            }
            else
            {
                // DEBUG
                //if (collidable.GetCollidableType() == CollidableType.Hero)
                //    Debug.Write("ObstacleStrategy.StuckHandling: " + collidable + " didn't work, iterations: " + collidable.GetOHMovementIteration());
                
                collidable.SetDestination(GetStuckAlternatePath(collidable, StuckIterationSubAngle, 0.8f));
                IncMovementIteration(collidable);
                tryToMoveAgain = TryToMoveAgain.Yes;
            }
        }

        public static TryToMoveAgain HandleNoSolidCollision(IHandleSolidCollision collidable)
        {
            if (collidable.GetObstacleHandlingStrategyPhase() == MovementStrategyPhase.PassThrough ||
                collidable.GetObstacleHandlingStrategyPhase() == MovementStrategyPhase.InternalStuckTest)
            {
                collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.Passive);
                if (collidable.GetObstacleHandlingStrategy() == MovementStrategy.SearchPathForPlayer)
                    collidable.SetOHMovementIteration(0);
            }
            else
            if (collidable.GetObstacleHandlingStrategyPhase() == MovementStrategyPhase.StuckHandling)
            {
                collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.Passive);
                collidable.SetOHMovementIteration(0);
            }
            else
            if (collidable.GetObstacleHandlingStrategy() == MovementStrategy.SearchPathForPlayer)
            {
                if (collidable.GetObstacleHandlingStrategyPhase() == MovementStrategyPhase.PBumb)
                    collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.PSliding);
                else
                if (collidable.GetObstacleHandlingStrategyPhase() == MovementStrategyPhase.PSliding)
                    collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.PSeemsOk);
                else
                    if (collidable.GetObstacleHandlingStrategyPhase() == MovementStrategyPhase.PSeemsOk)
                    {
                        //Debug.Write("NoSolidCollision: seems ok");
                        collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.Passive);
                    }

                collidable.SetOHMovementIteration(0);
            }
            else
            if (collidable.GetObstacleHandlingStrategy() == MovementStrategy.SearchAndSlide)
            {

                DecMovementIteration(collidable);
                collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.Sliding);
                if (collidable.GetOHMovementIteration() <= 0)
                    collidable.SetObstacleHandlingStrategyPhase(MovementStrategyPhase.Passive);


            }
            
            SetMovementAttempt(0);

            return TryToMoveAgain.No;
        }

        private static Vector2 GetSearchAlternatePathForPlayer(IHandleSolidCollision collidable, float MovementIterationSubAngle)
        {
            int movementIteration = collidable.GetOHMovementIteration();
            if (collidable.GetOHMovementIteration() % 2 == 1 &&
                collidable.GetObstacleHandlingStrategyPhase() == MovementStrategyPhase.PBumb)
            {
                movementIteration++;
                collidable.ChangePreferClockWise();
            }

            double angle = (double) movementIteration * MovementIterationSubAngle;

            const float angleImportance = (63.0f / 64.0f);
            const float effectRegardlessOfAngle = 1 - angleImportance;

            float lengthModifier = (float)Math.Abs(Math.Cos(angle)) * angleImportance + effectRegardlessOfAngle;

            if (collidable.GetPreferClockWise())
            {
                return collidable.GetPosition() + Calculations.RotateClockwise(
                    PossibleMovement * lengthModifier, angle);
            }
            else
            {
                return collidable.GetPosition() + Calculations.RotateCounterClockwise(
                    PossibleMovement * lengthModifier, angle);
            }
        }


        private static Vector2 GetSearchAlternatePathForUnit(IHandleSolidCollision collidable, float MovementIterationSubAngle)
        {
            int movementIteration = collidable.GetOHMovementIteration();
            if (collidable.GetOHMovementIteration() % 2 == 1)
            {
                movementIteration++;
                if (collidable.GetObstacleHandlingStrategyPhase() != MovementStrategyPhase.Sliding)
                    collidable.ChangePreferClockWise();
            }

            double angle = (double)movementIteration * MovementIterationSubAngle;

            //const float angleImportance = (1.0f / 3.0f);
            //const float effectRegardlessOfAngle = 1 - angleImportance;

            //float lengthModifier = (float)Math.Abs(Math.Cos(angle)) * angleImportance + effectRegardlessOfAngle;

            float lengthModifier = 1.0f;

            //if (collidable.GetOHPreferClockwiseMovement())
            if (collidable.GetPreferClockWise())
            {
                return collidable.GetPosition() + Calculations.RotateClockwise(
                    PossibleMovement * lengthModifier, angle);
            }
            else
            {
                return collidable.GetPosition() + Calculations.RotateCounterClockwise(
                    PossibleMovement * lengthModifier, angle);
            }
        }


        private static Vector2 GetStuckAlternatePath(IHandleSolidCollision collidable, float MovementIterationSubAngle, float lengthModifier)
        {
            double angle = (double)(collidable.GetOHMovementIteration() + 1) * StuckIterationSubAngle;

            return collidable.GetPosition() + Calculations.RotateClockwise(
                PossibleMovement * lengthModifier, angle);
        }


        private static void DecMovementIteration(IHandleSolidCollision collidable)
        {
            collidable.SetOHMovementIteration(collidable.GetOHMovementIteration() - 1);
            collidable.SetOHMovementIteration(Math.Max(0, collidable.GetOHMovementIteration()));
        }
        
        private static void IncMovementIteration(IHandleSolidCollision collidable)
        {
            collidable.SetOHMovementIteration(collidable.GetOHMovementIteration() + 1);
            collidable.SetOHMovementIteration(Math.Min(MovementIterationMax, collidable.GetOHMovementIteration()));
        }

        private static int GetMovementAttempt()
        {
            return movementAttempt;
        }
        private static void SetMovementAttempt(int v)
        {
            movementAttempt = v;
        }
    }
}
