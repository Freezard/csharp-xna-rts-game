using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RTSgame.Utilities.IO.UI
{
    /// <summary>
    /// A dynamic text component which displays the amount of metal the player has
    /// </summary>
    class MetalCounter:TextComponent
    {
        Player associatedPlayer;
        public MetalCounter(Vector2 position, Player player, SpriteFont font, Color color)
            : base(position, "N/A", font, color)
        {
            associatedPlayer = player;
        }
        public override void Update(GameTime gameTime)
        {
            text = associatedPlayer.GetMetal().ToString();
        }


    }
}
