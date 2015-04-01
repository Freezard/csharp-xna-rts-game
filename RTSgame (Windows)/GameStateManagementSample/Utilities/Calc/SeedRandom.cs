using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSgame.Utilities.Calc
{
    class SeedRandom
    {
        public SeedRandom(int Seed)
        {
            randomizer = new Random(Seed);
        }

        private Random randomizer;

        private Random GetRandomizer()
        {
            return randomizer;
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public int Next(int maxValue)
        {
            return GetRandomizer().Next(maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public float NextFloat(int maxValue)
        {
            return (float)GetRandomizer().Next(maxValue);
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public float NextFloat(float maxValue)
        {
            return (float)GetRandomizer().NextDouble() * maxValue;
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns></returns>
        public float Next0to1()
        {
            return (float)GetRandomizer().NextDouble();
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns></returns>
        public double NextDouble()
        {
            return GetRandomizer().NextDouble();
        }

        /// <summary>
        /// Returns either true or false with
        /// 0.5 probability for both.
        /// </summary>
        /// <returns></returns>
        public bool Bool5050()
        {
            if (GetRandomizer().Next(2) >= 1)
                return true;
            else
                return false;
        }
    }
}
