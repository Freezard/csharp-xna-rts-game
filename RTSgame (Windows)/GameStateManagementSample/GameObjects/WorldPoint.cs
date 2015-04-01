using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;

namespace RTSgame.GameObjects
{
    //WorldPoint represents the values that we wish to store
    //on every intersection of the world.
    //Feel free to add things if you like,
    //but be cautious since there are a lot of these.

    //If this is a struct, then it is not treated by
    //the Garbage Collector, which is good.
    struct WorldPoint
    {
        public float Height;
    }
}
