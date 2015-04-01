using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RTSgame.Utilities.IO.UI
{
    /// <summary>
    /// Infotext is an textcomponent that is shown a certain number of frames
    /// </summary>
    class InfoText:TextComponent
    {

        private int millisecondsLeft = 0;
        public InfoText(Vector2 position, SpriteFont font, Color color):base(position, "N/A", font, color)
        {
            visible = false;
        }
        public override void Update(GameTime gameTime)
        {
            if (millisecondsLeft > 0)
            {
                millisecondsLeft -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                visible = false;
            }

        }
        public void ShowMessage(String message, int milliseconds)
        {
            visible = true;
            text = message;
            millisecondsLeft = milliseconds;
        }
    }
}
