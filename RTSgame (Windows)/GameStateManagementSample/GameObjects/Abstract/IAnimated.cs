using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RTSgame.GameObjects.Abstract
{
    //Interface for animated GameObjects
    interface IAnimated
    {
        void updateAnimation(GameTime gameTime);
    }
}
