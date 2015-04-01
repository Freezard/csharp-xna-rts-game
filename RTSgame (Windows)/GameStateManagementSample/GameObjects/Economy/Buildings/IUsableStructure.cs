using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;
using RTSgame.Utilities.IO.UI;
using RTSgame.GameObjects.Units;

namespace RTSgame.GameObjects.Economy.Buildings
{
    /// <summary>
    /// A building that is usable
    /// </summary>
    interface IUsableStructure:IUITrackable
    {
        void AttemptToUse(PlayerCharacter playerCharacter);
        
    }
}
