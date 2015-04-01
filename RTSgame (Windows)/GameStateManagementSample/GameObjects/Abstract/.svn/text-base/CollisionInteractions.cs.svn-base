using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSgame.GameObjects.Abstract
{
    public enum CollidableType : byte { None, Doodad, Structure, Hero, Minion, Enemy, Projectile, StructureSite, Resource };

    public enum CollisionType : byte { NoCollision, Collision2D, Collision3D }

    static public class CollisionInteractions
    { 
        private static int numOfCollisionTypes = Enum.GetValues(typeof(CollidableType)).Length;

        private static CollisionType[,] solidInteractions = new CollisionType[numOfCollisionTypes, numOfCollisionTypes];

        /// <summary>
        /// Does not include Doodads!
        /// </summary>
        private static bool[] hasRelevantCollisions = new bool[numOfCollisionTypes];

        public static void Init()
        {
            
            #region SolidInteractions
		    // Enemies:
            solidInteractions[(int)CollidableType.Enemy, (int)CollidableType.Doodad] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Enemy, (int)CollidableType.Enemy] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Enemy, (int)CollidableType.Hero] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Enemy, (int)CollidableType.Minion] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Enemy, (int)CollidableType.Structure] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Enemy, (int)CollidableType.StructureSite] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Enemy, (int)CollidableType.Resource] = CollisionType.NoCollision;

            // Hero:
            solidInteractions[(int)CollidableType.Hero, (int)CollidableType.Doodad] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Hero, (int)CollidableType.Enemy] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Hero, (int)CollidableType.Hero] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Hero, (int)CollidableType.Minion] = CollisionType.NoCollision;
            solidInteractions[(int)CollidableType.Hero, (int)CollidableType.Structure] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Hero, (int)CollidableType.StructureSite] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Hero, (int)CollidableType.Resource] = CollisionType.NoCollision;

            // Minion:
            solidInteractions[(int)CollidableType.Minion, (int)CollidableType.Doodad] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Minion, (int)CollidableType.Enemy] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Minion, (int)CollidableType.Hero] = CollisionType.NoCollision;
            solidInteractions[(int)CollidableType.Minion, (int)CollidableType.Minion] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Minion, (int)CollidableType.Structure] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Minion, (int)CollidableType.StructureSite] = CollisionType.Collision2D;
            solidInteractions[(int)CollidableType.Minion, (int)CollidableType.Resource] = CollisionType.NoCollision;

            // Projectiles:
            solidInteractions[(int)CollidableType.Projectile, (int)CollidableType.Doodad] = CollisionType.Collision3D;
            solidInteractions[(int)CollidableType.Projectile, (int)CollidableType.Enemy] = CollisionType.NoCollision;
            solidInteractions[(int)CollidableType.Projectile, (int)CollidableType.Hero] = CollisionType.NoCollision;
            solidInteractions[(int)CollidableType.Projectile, (int)CollidableType.Minion] = CollisionType.NoCollision;
            solidInteractions[(int)CollidableType.Projectile, (int)CollidableType.Structure] = CollisionType.NoCollision;
            solidInteractions[(int)CollidableType.Projectile, (int)CollidableType.StructureSite] = CollisionType.NoCollision;
            solidInteractions[(int)CollidableType.Projectile, (int)CollidableType.Resource] = CollisionType.NoCollision;

            solidInteractions[(int)CollidableType.Structure, (int)CollidableType.StructureSite] = CollisionType.NoCollision;

	        #endregion

            // Set up hasRelevantCollisions
            for (int i = 0; i < numOfCollisionTypes; i++)
            {
                bool foundRelevantCollision = false;
                for (int j = 0; j < numOfCollisionTypes; j++)
                {
                    if (solidInteractions[i,j] != CollisionType.NoCollision &&
                        (j != (int)CollidableType.Doodad)) //Doodads doesn't count, because they have their own check
                    {
                        foundRelevantCollision = true;
                    }
                }

                hasRelevantCollisions[i] = foundRelevantCollision;
            }

            #region testing
            /*
            Debug.Write("SolidCollisionInteractions table checking:");
            for (int i = 0; i < 8; i++)
            {
                for (int i2 = 0; i2 < 8; i2++)
                {
                    Debug.Write(i + " " + i2 + " " + solidInteractions[i, i2]);
                }
            }
            Debug.Write("SolidCollisionInteractions table checking done");
            */

            #endregion
        }

        public static CollisionType GetCollisionType(CollidableType source, CollidableType target)
        {
            return solidInteractions[(int) source, (int) target];
        }

        public static bool HasRelevantCollisions(CollidableType CollidableType)
        {
            return hasRelevantCollisions[(int)CollidableType];
        }
    }
}
