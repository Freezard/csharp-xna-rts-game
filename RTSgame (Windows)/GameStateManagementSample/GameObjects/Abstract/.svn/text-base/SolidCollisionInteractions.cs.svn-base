using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSgame.GameObjects.Abstract
{
    public enum CollidableType { None = 0, Doodad = 1, Structure = 2, Hero = 3, Minion = 4, Enemy = 5, Projectile = 6, AiRange = 7, };

    static public class SolidCollisionInteractions
    {
        private static bool[,] solidInteractions = new bool[8,8];

        public static void Init()
        {
            //solidInteractions = new bool[8,8];

            //Enemies:
            solidInteractions[(int)CollidableType.Enemy, (int)CollidableType.Doodad] = true;
            solidInteractions[(int)CollidableType.Enemy, (int)CollidableType.Enemy] = true;
            solidInteractions[(int)CollidableType.Enemy, (int)CollidableType.Hero] = true;
            solidInteractions[(int)CollidableType.Enemy, (int)CollidableType.Minion] = true;
            solidInteractions[(int)CollidableType.Enemy, (int)CollidableType.Structure] = true;

            //Enemies:
            solidInteractions[(int)CollidableType.Hero, (int)CollidableType.Doodad] = true;
            solidInteractions[(int)CollidableType.Hero, (int)CollidableType.Enemy] = true;
            solidInteractions[(int)CollidableType.Hero, (int)CollidableType.Hero] = true;
            solidInteractions[(int)CollidableType.Hero, (int)CollidableType.Minion] = false;
            solidInteractions[(int)CollidableType.Hero, (int)CollidableType.Structure] = true;
            solidInteractions[(int)CollidableType.Structure, (int)CollidableType.Hero] = false;

            //Minion:
            solidInteractions[(int)CollidableType.Minion, (int)CollidableType.Doodad] = false;
            solidInteractions[(int)CollidableType.Minion, (int)CollidableType.Enemy] = false;
            solidInteractions[(int)CollidableType.Minion, (int)CollidableType.Hero] = false;
            solidInteractions[(int)CollidableType.Minion, (int)CollidableType.Minion] = false;
            solidInteractions[(int)CollidableType.Minion, (int)CollidableType.Structure] = false;
            
            #region testing
            /*
            Console.WriteLine("SolidCollisionInteractions table checking:");
            for (int i = 0; i < 8; i++)
            {
                for (int i2 = 0; i2 < 8; i2++)
                {
                    Console.WriteLine(i + " " + i2 + " " + solidInteractions[i, i2]);
                }
            }
            Console.WriteLine("SolidCollisionInteractions table checking done");
            */

            #endregion
        }

        public static bool doesInteract(CollidableType source, CollidableType target)
        {
            return solidInteractions[(int) source, (int) target];
        }

    }
}
