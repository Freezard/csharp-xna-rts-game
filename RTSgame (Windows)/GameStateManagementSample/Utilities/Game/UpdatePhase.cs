using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSgame.Utilities
{

    public enum Phase : byte { Initial,
        PreparedForAIInteraction, DoneAIInteraction,
        PreparedForInteraction, DoneInteraction,
        PreparedForCollision, DoneSolidCollision }

    static class UpdatePhase
    {
        public static Phase CurrentPhase;
    }
}
