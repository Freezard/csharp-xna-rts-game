using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RTSgame.GameObjects.Abstract
{
    interface ILogic
    {
        void UpdateLogic(GameTime gameTime);
        Vector2 GetPosition();
    }
}
