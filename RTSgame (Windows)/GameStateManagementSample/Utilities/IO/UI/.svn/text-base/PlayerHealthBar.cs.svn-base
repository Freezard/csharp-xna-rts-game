using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.Utilities.IO.UI
{
    /// <summary>
    /// A bar displaying player health
    /// </summary>
    class PlayerHealthBar:UIBar
    {
        private Player currentPlayer;
        public PlayerHealthBar(Vector2 position, String image, Color tint, Direction direction, Boolean scale, Player player, Vector2 sizeInPixels):base(position, image, tint, 0, direction, scale, sizeInPixels)
        {
            currentPlayer = player;
        }
        public override void Update(GameTime gameTime)
        {
            Unit u = currentPlayer.GetControlledUnit();
            SetValue(u.GetHealthScaled());
        }

    }
}
