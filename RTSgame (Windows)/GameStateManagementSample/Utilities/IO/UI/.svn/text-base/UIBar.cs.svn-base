using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RTSgame.Utilities.IO.UI
{
    /// <summary>
    /// The UIBar is a status bar (for health, energy, etc)
    /// </summary>
    class UIBar : ImageComponent
    {
        public enum Direction { Up, Down, Left, Right };
        private Boolean scale;
        private float value;
        private Direction direction;
        //Size rescaling
        private Vector2 size;
        /// <summary>
        /// Create new UI Bar
        /// </summary>
        /// <param name="position">Where in UI to place it</param>
        /// <param name="image">What image to use</param>
        /// <param name="tint">What color to tint with</param>
        /// <param name="min">At what value bar should be length 0</param>
        /// <param name="max">At what value bar should be same length as image</param>
        /// <param name="direction">What direction bar INCREASES towards</param>
        /// <param name="scale">If true, image used will scale, otherwise it will crop</param>
        ///  <param name="size">Size in pixels</param>
        public UIBar(Vector2 position, String image, Color tint, float value, Direction direction, Boolean scale, Vector2 sizeInPixels)
            : base(position, image, tint)
        {
            SetValue(value);
            this.scale = scale;
            this.direction = direction;
            this.size.X = sizeInPixels.X / this.Image.Width;
            this.size.Y = sizeInPixels.Y / this.Image.Height;


        }
        public void SetValue(float value)
        {
            if (value >= 0 && value <= 1)
            {
                this.value = value;
            }
            else
            {
               throw new ArgumentOutOfRangeException();
            }
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            //size means what width/height the bar should have
            
            Vector2 scaleVector = Vector2.One;
            Rectangle sourceRectangle = new Rectangle(0, 0, Image.Width, Image.Height);
            //Additional offset is needed in some cases
            Vector2 additionalOffset = Vector2.Zero;
            if (direction == Direction.Up)
            {
                additionalOffset = new Vector2(0, (1 - value) * Image.Height);
                scaleVector = new Vector2(1, value);
                sourceRectangle = new Rectangle(0, (int)(Image.Height * (1 - value)), Image.Width, (int)(Image.Height * value));

            }
            else if (direction == Direction.Down)
            {
                scaleVector = new Vector2(1, value);
                sourceRectangle = new Rectangle(0, 0, Image.Width, (int)(Image.Height * value));
            }
            else if (direction == Direction.Left)
            {
                additionalOffset = new Vector2((1 - value) * Image.Width, 0);
                scaleVector = new Vector2(value, 1);
                sourceRectangle = new Rectangle((int)(Image.Width * (1 - value)), 0, (int)(value * Image.Width), Image.Height);
            }
            else if (direction == Direction.Right)
            {
                scaleVector = new Vector2(value, 1);
                sourceRectangle = new Rectangle(0, 0, (int)(value * Image.Width), Image.Height);
            }
            if (scale)
            {
                spriteBatch.Draw(Image, Position + offset + additionalOffset, null, tint, 0, Vector2.Zero, scaleVector*size, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(Image, Position + offset + additionalOffset, sourceRectangle, tint, 0, Vector2.Zero, size, SpriteEffects.None, 0);
            }




        }
    }
}
