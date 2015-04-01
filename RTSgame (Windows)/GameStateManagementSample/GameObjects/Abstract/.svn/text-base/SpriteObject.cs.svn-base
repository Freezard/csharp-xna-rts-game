using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RTSgame.GameObjects.Abstract
{
    //A gameObject that is drawn with a sprite, i. e. Health Bar
    abstract class SpriteObject : GameObject, IDrawableWorld
    {
        protected Texture2D image;
        public SpriteObject(Vector2 newPosition):base(newPosition)
        {
            
        }
        public virtual Texture2D GetImage()
        {
            return image;
        }
    }
}
