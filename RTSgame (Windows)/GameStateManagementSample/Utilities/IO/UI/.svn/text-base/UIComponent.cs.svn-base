using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RTSgame.Utilities.IO.UI
{
    /// <summary>
    /// A UI-component is a part of the User Interface
    /// </summary>
    abstract class UIComponent
    {


        private Vector2 position;

        
        protected Boolean visible = true;
        public UIComponent(Vector2 position)
        {
            this.position = position;
        }
        public virtual void Update(GameTime gameTime)
        {
            //Do nothing, replace in subclass
        }
        public void SetVisible(Boolean value)
        {
            visible = value;
        }
        /// <summary>
        /// Draw the UIComponent
        /// </summary>
        /// <param name="spriteBatch">spritebatch to use</param>
        /// <param name="offset">offset to move the uicomponent</param>
        public abstract void Draw(SpriteBatch spriteBatch, Vector2 offset);
        public Boolean IsVisible()
        {
            return visible;
        }


        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public abstract float GetWidth();
        public abstract float GetHeight();

    }
}
