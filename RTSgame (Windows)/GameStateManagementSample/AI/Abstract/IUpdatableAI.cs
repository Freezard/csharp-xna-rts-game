using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RTSgame.AI.Abstract
{
    interface IUpdatableAI
    {
        void Update(GameTime gameTime);
    }
}
