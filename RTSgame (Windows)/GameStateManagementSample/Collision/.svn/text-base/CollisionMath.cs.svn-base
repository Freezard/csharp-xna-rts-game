using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using RTSgame.GameObjects;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Doodads;
using RTSgame.Utilities.World;

namespace RTSgame.Collision
{
    static class CollisionMath
    {
        private static IWorld world;

        public static void SetWorld(IWorld w)
        {
            world = w;
        }

        /// <summary>
        /// Checks if the argument object collides with any doodad in the world.
        /// </summary>
        /// <param name="collidable"></param>
        /// <returns></returns>
        public static bool SolidCollisionCheckAgainstDoodads(ICollidable collidable)
        {

            //TODO: if obj has spherical shape
            //world.GetData().ChangeValuesViaCenteredCircle(CheckOccupied, obj.GetNewPosition(), obj.GetRadius());

            // If this is an object that does not collide with doodads, then quit
            if (CollisionInteractions.GetCollisionType(collidable.GetCollidableType(), CollidableType.Doodad) == CollisionType.NoCollision)
                return false;

            // Calculate which doodads to check against
            Vector2 start = collidable.GetDestination();
            float reach = collidable.GetSolidCollisionSize() + Constants.DoodadMaxWorldSize;

            int xLeft = Calculations.Clamp((int)(start.X - reach), 0, world.GetDimension() - 1);
            int xRight = Calculations.Clamp((int)(start.X + reach), 0, world.GetDimension() - 1);
            int yTop = Calculations.Clamp((int)(start.Y - reach), 0, world.GetDimension() - 1);
            int yBottom = Calculations.Clamp((int)(start.Y + reach), 0, world.GetDimension() - 1);

            for (int x = xLeft; x <= xRight; x++)
            {
                for (int y = yTop; y <= yBottom; y++)
                {
                    if (SolidCollisionCheckAgainstDoodad(collidable, ref world.GetData().Data[x,y], new Point(x,y)))
                        // If we collided with this doodad, then there is no reason to search
                        // more, return our result immediately
                        return true;
                }
            }

            // Since we never found a doodad which we collided with,
            // return this negative result
            return false;
        }

        private static bool SolidCollisionCheckAgainstDoodad(ICollidable collidable, ref WorldObject W, Point P)
        {

            if (world.GetData().Data[P.X, P.Y].IsCompletelyBlocked)
            {
                return Calculations.RectangleCircleIntersect(
                    new Rectangle(P.X, P.Y, 1, 1),
                    collidable.GetDestination(), collidable.GetSolidCollisionSize());
            }
            if (world.GetData().Data[P.X, P.Y].HasDoodad())
            {
                if (CollisionInteractions.GetCollisionType(collidable.GetCollidableType(), CollidableType.Doodad) == CollisionType.Collision2D)
                {
                    StaticDoodads.SetStaticModelObject(ref world.GetData().Data[P.X, P.Y].Doodad, P);

                    if (SolidCollisionCheckAgainstCollidable(collidable, StaticDoodads.StaticDoodadObject))
                        return true;
                }
                else
                    if (CollisionInteractions.GetCollisionType(collidable.GetCollidableType(), CollidableType.Doodad) == CollisionType.Collision3D)
                    {
                        StaticDoodads.SetStaticModelObject(ref world.GetData().Data[P.X, P.Y].Doodad, P);

                        if (SolidCollisionCheckAgainstCollidable3D(collidable, StaticDoodads.StaticDoodadObject))
                            return true;
                    }
            }

            // If we get here, there was no collision
            return false;
        }



        public static bool SolidCollisionCheckAgainstCollidable(ICollidable c1, ICollidable c2)
        {
            Vector2 c2Position;
            if (c2.GetPhase() == UpdatePhase.CurrentPhase)
                c2Position = c2.GetDestination();
            else
                c2Position = c2.GetPosition();

            return (Calculations.IsWithin2DRange(c1.GetDestination(), c2Position,
                  ((c1.GetSolidCollisionSize() + c2.GetSolidCollisionSize())
                )));
        }

        /// <summary>
        /// Checks if these two collidables bounding boxes
        /// intersects. Warning: Make sure the bounding boxes are updated
        /// to the intended value before calling!
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool SolidCollisionCheckAgainstCollidable3D(ICollidable c1, ICollidable c2)
        {
            //c2.UpdateCollisionBoxToDestination();
            return (c1.GetCollisionSphere().Intersects(c2.GetCollisionSphere()));
        }

    }
}
