using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;

namespace RTSgame.GameObjects.Abstract
{
    public enum MovementStrategyPhase
    {
        // Search and Slide
        Passive, WideningSearch, NarrowingSearch, Sliding,
        // Critical phases
        StuckTest, StuckHandling,
        PassThrough,
        // not implemented yet (probably don't have to)
        FullStop
    };

    public enum MovementStrategy
    {
        PassThrough,
        FullStop,
        SimpleSlide,
        ExtendedSlide,
        SearchAndSlide
    }

    static class ObstacleStrategy
    {
        /*
         * SAS = Search And Slide
         */

        // Sets the maximum angle to 2 steps above 90'
        public const float SASMovementIterationSubAngle = (float)Math.PI * 1.0f / ((float)(MovementIterationMax - 2) * 2.0f);
        public const int MovementIterationMax = 12;

        // Sets the maximum angle to 1 step below 360'
        public const float StuckIterationSubAngle = (float)Math.PI * 2.0f / ((float)MovementIterationMax - 1);

        private static Vector2 PossibleMovement;

        public static TryToMoveAgain AdjustMovement(Unit collidable)
        {
            // both GetMovementAttempt and PossibleMovement
            // could technically be static ObstacleStrategy
            // variables instead
            if (collidable.GetMovementAttempt() == 0)
                PossibleMovement = collidable.GetDestination() - collidable.GetPosition();

            TryToMoveAgain tryAgain = TryToMoveAgain.No;

            // We assume that we find a critical phase
            bool abortRegularBehaviour = true;

            // is there a critical phase?
            switch (collidable.GetMovementStrategyPhase())
            {
                case MovementStrategyPhase.StuckTest:
                    collidable.SetMovementStrategyPhase(MovementStrategyPhase.StuckHandling);
                    collidable.SetMovementIteration(1);
                    tryAgain = StuckHandling(collidable);
                    break;
                case MovementStrategyPhase.StuckHandling:
                    tryAgain = StuckHandling(collidable);
                    break;
                case MovementStrategyPhase.PassThrough:
                    collidable.SetDestination(collidable.GetPosition() + PossibleMovement);
                    tryAgain = TryToMoveAgain.No;
                    break;
                default:
                    abortRegularBehaviour = false;
                    break;
            }

            if (!abortRegularBehaviour)
            {
                switch (collidable.GetMovementStrategy())
                {
                    case MovementStrategy.PassThrough:
                        tryAgain = PassThrough(collidable);
                        break;
                    case MovementStrategy.SimpleSlide:
                        tryAgain = SimpleSlide(collidable);
                        break;
                    case MovementStrategy.ExtendedSlide:
                        tryAgain = ExtendedSlide(collidable);
                        break;
                    case MovementStrategy.SearchAndSlide:
                        tryAgain = SearchAndSlide(collidable);
                        break;
                    default:
                        tryAgain = FullStop(collidable);
                        break;
                }
            }

            collidable.SetMovementAttempt(collidable.GetMovementAttempt() + 1);

            if (tryAgain == TryToMoveAgain.No)
                collidable.SetMovementAttempt(0);

            return tryAgain;
        }

        public static TryToMoveAgain FullStop(Unit collidable)
        {
            collidable.SetDestination(collidable.GetPosition());
            return TryToMoveAgain.No;
        }

        private static TryToMoveAgain PassThrough(Unit collidable)
        {
            return TryToMoveAgain.No;
        }

        private static TryToMoveAgain GiveUp(Unit collidable)
        {
            return StuckTest(collidable);
            //return FullStop(collidable);
        }

        private static TryToMoveAgain SimpleSlide(Unit collidable)
        {
            TryToMoveAgain tryAgain = TryToMoveAgain.No;
            if (collidable.GetMovementAttempt() >= 2)
            {
                tryAgain = GiveUp(collidable);
            }
            else
                tryAgain = ExtendedSlide(collidable);

            return tryAgain;
        }

        private static TryToMoveAgain ExtendedSlide(Unit collidable)
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

            TryToMoveAgain tryAgain = TryToMoveAgain.Yes;

            switch (collidable.GetMovementAttempt())
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
                    tryAgain = GiveUp(collidable);
                    break;
            }

            return tryAgain;
        }

        private static TryToMoveAgain SearchAndSlide(Unit collidable)
        {
            TryToMoveAgain tryAgain = TryToMoveAgain.No;

            switch (collidable.GetMovementAttempt())
            {
                case 0:
                    //Vector2 PossibleMovement = collidable.GetDestination() - collidable.GetPosition();

                    // change strategy if needed

                    if (collidable.GetMovementIteration() <= 0)
                    {
                        collidable.SetMovementStrategyPhase(MovementStrategyPhase.WideningSearch);
                        collidable.SetMovementIteration(1);
                        if (EasyRandom.Bool5050())
                            collidable.ChangePreferClockwiseMovement();
                    }

                    if (collidable.GetMovementIteration() >= MovementIterationMax)
                    {
                        collidable.SetMovementStrategyPhase(MovementStrategyPhase.NarrowingSearch);
                        collidable.ChangePreferClockwiseMovement();
                        collidable.SetMovementIteration(MovementIterationMax - 1);
                    }

                    // try the new position
                    collidable.SetDestination(GetSearchAlternatePath(collidable, SASMovementIterationSubAngle));
                    tryAgain = TryToMoveAgain.Yes;
                    break;

                case 1:
                    // act according to strategy
                    switch (collidable.GetMovementStrategyPhase())
                    {
                        case MovementStrategyPhase.NarrowingSearch:
                            collidable.DecMovementIteration();
                            collidable.ChangePreferClockwiseMovement();
                            break;
                        case MovementStrategyPhase.WideningSearch:
                            collidable.IncMovementIteration();
                            collidable.IncMovementIteration();
                            collidable.ChangePreferClockwiseMovement();
                            break;
                        case MovementStrategyPhase.Sliding:
                            collidable.IncMovementIteration();
                            collidable.IncMovementIteration();
                            break;
                    }

                    // reset movement
                    collidable.SetDestination(collidable.GetPosition());

                    tryAgain = TryToMoveAgain.No;
                    break;
            }

            return tryAgain;
        }

        private static TryToMoveAgain StuckTest(Unit collidable)
        {
            collidable.SetDestination(collidable.GetPosition());
            collidable.SetMovementStrategyPhase(MovementStrategyPhase.StuckTest);
            return TryToMoveAgain.Yes;
        }

        private static TryToMoveAgain StuckHandling(Unit collidable)
        {
            // Gör en search runtomkring. låt avståndet va lite kortare.
            // om man hittar någonstans man kan gå,
            // behöver inte nödvändigtvis flytta dit.
            // om man inte hittar någonstans man kan gå
            // byt till PassThrough

            // TODO: if PossibleMovement is zero, use something
            // else instead, like getSpeed or Constants.MinSpeed

            if (collidable.GetMovementIteration() >= MovementIterationMax)
            {
                Console.WriteLine("ObstacleStrategy.StuckHandling: got stuck!");
                collidable.SetMovementStrategyPhase(MovementStrategyPhase.PassThrough);
                collidable.SetDestination(collidable.GetPosition() + PossibleMovement);
                return TryToMoveAgain.Yes;
            }
            else
            {
                collidable.SetDestination(GetStuckAlternatePath(collidable, StuckIterationSubAngle, 0.9f));
                collidable.IncMovementIteration();
                return TryToMoveAgain.Yes;
            }
        }

        public static TryToMoveAgain HandleNoSolidCollision(Unit collidable)
        {
            if (collidable.GetMovementStrategyPhase() == MovementStrategyPhase.PassThrough ||
                collidable.GetMovementStrategyPhase() == MovementStrategyPhase.StuckTest)
            {
                collidable.SetMovementStrategyPhase(MovementStrategyPhase.Passive);
            }
            else
            if (collidable.GetMovementStrategyPhase() == MovementStrategyPhase.StuckHandling)
            {
                collidable.SetMovementStrategyPhase(MovementStrategyPhase.Passive);
                collidable.SetMovementIteration(0);
            }
            else
            if (collidable.GetMovementStrategy() == MovementStrategy.SearchAndSlide)
            {
                if (collidable.GetMovementAttempt() == 0)
                {
                    switch (collidable.GetMovementStrategyPhase())
                    {
                        case MovementStrategyPhase.WideningSearch:
                            collidable.DecMovementIteration();
                            break;
                        case MovementStrategyPhase.Sliding:
                            collidable.DecMovementIteration();
                            break;
                    }
                    if (collidable.GetMovementIteration() <= 0)
                        collidable.SetMovementStrategyPhase(MovementStrategyPhase.Passive);
                }
                if (collidable.GetMovementAttempt() == 1)
                {
                    // let's keep doing this! change(keep) strategy to sliding:
                    collidable.SetMovementStrategyPhase(MovementStrategyPhase.Sliding);
                    // don't change desperation
                }
            }
            
            collidable.SetMovementAttempt(0);

            return TryToMoveAgain.No;
        }

        private static Vector2 GetSearchAlternatePath(Unit collidable, float MovementIterationSubAngle)
        {
            double angle = (double) collidable.GetMovementIteration() * MovementIterationSubAngle;

            const float angleImportance = (2.0f / 3.0f);
            const float effectRegardlessOfAngle = 1 - angleImportance;

            float lengthModifier = (float)Math.Abs(Math.Cos(angle)) * angleImportance + effectRegardlessOfAngle;

            if (collidable.GetPreferClockwiseMovement())
                return collidable.GetPosition() + Calculations.RotateClockwise(
                    PossibleMovement * lengthModifier, angle);
            else
                return collidable.GetPosition() + Calculations.RotateCounterClockwise(
                    PossibleMovement * lengthModifier, angle);
        }


        private static Vector2 GetStuckAlternatePath(Unit collidable, float MovementIterationSubAngle, float lengthModifier)
        {
            double angle = (double)collidable.GetMovementIteration() * StuckIterationSubAngle;

            if (collidable.GetPreferClockwiseMovement())
                return collidable.GetPosition() + Calculations.RotateClockwise(
                    PossibleMovement * lengthModifier, angle);
            else
                return collidable.GetPosition() + Calculations.RotateCounterClockwise(
                    PossibleMovement * lengthModifier, angle);
        }

    }
}
