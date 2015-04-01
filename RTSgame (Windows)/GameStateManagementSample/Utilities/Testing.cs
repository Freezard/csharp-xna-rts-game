using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSgame.Utilities
{
    // Bara för o testa saker.
    // Testade hur mycket en Lista ökar varje gång man överskrider
    // dess kapacitet. (visade sej vara det dubbla jämfört med hur stor den var).
    // Committar den då det blev fel på projektfilen (som jag commita) utan den.

    static class Testing
    {
        static List<int> list1;
        static List<int> list2;

        public static void PrintLists()
        {
            Console.WriteLine("-------");
            Console.WriteLine(list1.Capacity);
            Console.WriteLine(list2.Capacity);
        }

        public static void TestStuff()
        {
            list1 = new List<int>();
            list2 = new List<int>(12);

            for (int i = 0; i < 100; i++)
            {
                PrintLists();
                list1.Add(1);
                list2.Add(1);
            }
        }
    }
}
