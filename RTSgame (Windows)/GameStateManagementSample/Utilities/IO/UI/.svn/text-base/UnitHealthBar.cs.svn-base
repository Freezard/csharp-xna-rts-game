using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.Utilities.IO.UI
{
    class UnitHealthBar:UIBar
    {
        private PlayerOwnedObject poo;
        public UnitHealthBar(Vector2 position, String image, Color tint, Direction direction, Boolean scale, PlayerOwnedObject poo, Vector2 sizeInPixels)
            : base(position, image, tint, poo.GetHealthScaled() / 2, direction, scale, sizeInPixels)
        {
            this.poo = poo;
            this.visible = true;
            
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
           SetValue(poo.GetHealthScaled());
          
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Vector2 offset)
        {
            base.Draw(spriteBatch, offset);
            
        }
    }
}
