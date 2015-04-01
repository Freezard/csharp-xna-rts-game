using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RTSgame.GameObjects.Abstract
{
    interface IMovable
    {
        // Updates Destination of the IMovable
        void UpdateDestination(GameTime gameTime);

        void AlignHeightToWorld();

        // Makes sure the IMovable is within the map
        void RestrictToMap();
    }
}
