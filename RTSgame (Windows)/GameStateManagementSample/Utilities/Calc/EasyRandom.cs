using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSgame.Utilities
{
    //EasyRandom grants direct access to random numbers, without having
    //to worry about initializing Random objects.
    class EasyRandom
    {
        private static Random randomizer;

        private EasyRandom()
        {
           
        }

        private static void InitIfNeeded()
        {
            if (randomizer == null)
            {
                randomizer = new Random();
            }
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int Next(int maxValue)
        {
            InitIfNeeded();
            return randomizer.Next(maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static float NextFloat(int maxValue)
        {
            InitIfNeeded();
            return (float)randomizer.Next(maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static float NextFloat(float maxValue)
        {
            InitIfNeeded();
            return (float)randomizer.NextDouble() * maxValue;
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns></returns>
        public static float Next0to1()
        {
            InitIfNeeded();
            return (float)randomizer.NextDouble();
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns></returns>
        public static double NextDouble()
        {
            InitIfNeeded();
            return randomizer.NextDouble();
        }

        /// <summary>
        /// Returns either true or false with
        /// 0.5 probability for both.
        /// </summary>
        /// <returns></returns>
        public static bool Bool5050()
        {
            InitIfNeeded();

            if (randomizer.Next(2) >= 1)
                return true;
            else
                return false;
        }
    }
}
