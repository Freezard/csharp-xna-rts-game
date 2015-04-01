using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RTSgame.GameObjects.Abstract
{
    abstract class AnimatedSpriteObject: SpriteObject, IAnimated
    {
        public AnimatedSpriteObject(Vector2 newPosition)
            : base(newPosition)
        {

        }
        public override Microsoft.Xna.Framework.Graphics.Texture2D GetImage()
        {
            //Should return part of spritesheet according to current animation frame
            return base.GetImage();
        }
        public abstract void updateAnimation(GameTime gameTime);
    }
}
