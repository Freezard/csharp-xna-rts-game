using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSgame.Utilities
{
    enum DebugPhase { All, None, Logic, Graphics, Input, SolidCollisionLogic };

    /// <summary>
    /// Debugprinter is used since it can be turned off, unlike Console.WriteLine
    /// </summary>
    static class DebugPrinter
    {
        public static DebugPhase currentDebugPhase = DebugPhase.Input;

        // Set to All to see all debug information
        // Set to None to disable debugging
        static DebugPhase desiredDebugPrinting = DebugPhase.None;

        public static void Write(String s)
        {
            if (ConditionForWriting())
            {
                Console.WriteLine(s);
            }
        }
        public static void Write(float f)
        {
            if (ConditionForWriting())
            {
                Console.WriteLine(f);
            }

        }
        public static void Write(Object o)
        {
            if (ConditionForWriting())
            {
                Console.WriteLine(o);
            }

        }

        private static bool ConditionForWriting()
        {
            return (desiredDebugPrinting == DebugPhase.All ||
                desiredDebugPrinting == currentDebugPhase);
        }
    }
}
