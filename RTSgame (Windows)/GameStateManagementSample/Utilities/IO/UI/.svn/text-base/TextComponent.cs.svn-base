using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RTSgame.Utilities.IO.UI
{
    /// <summary>
    /// Simple text UI Component
    /// </summary>
    class TextComponent:UIComponent
    {
        protected String text;
        private SpriteFont font;
        private Color color;
 
        /// <summary>
        /// Create new textcomponent
        /// </summary>
        /// <param name="position"></param>
        /// <param name="text"></param>
        /// <param name="font">What font to use</param>
        /// <param name="color">What color text should be</param>
        public TextComponent(Vector2 position, String text, SpriteFont font, Color color):base(position)
        {
            this.text = text;
            this.font = font;
            this.color = color;
        }
        
        public TextComponent(Vector2 position, String text, String fontName, Color color)
            : base(position)
        {
            this.text = text;
            this.font = AssetBank.GetInstance().GetFont(fontName);
            this.color = color;
        }


        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (visible)
            {
                spriteBatch.DrawString(font, text, Position + offset, color);
            }

        }



        public override float GetWidth()
        {
            return font.MeasureString(text).X;
        }

        public override float GetHeight()
        {
            return font.MeasureString(text).Y;
        }
    }
}
