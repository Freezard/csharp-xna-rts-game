using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RTSgame.Utilities.IO.UI
{
    /// <summary>
    /// ImageComponent is a UIComponent which consists of an (static) image
    /// </summary>
    class ImageComponent : UIComponent
    {
        protected Color tint;
        private Texture2D image;

        

       
       
        /// <summary>
        /// Create new imagecomponent
        /// </summary>
        /// <param name="position">Where in UI to place it</param>
        /// <param name="imageName">What image to use</param>
        /// <param name="tint">Tint the image with this color</param>
        public ImageComponent(Vector2 position, String imageName, Color tint)
            : base(position)
        {
            image = AssetBank.GetInstance().GetTexture(imageName);
            
            this.tint = tint;
           
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (visible)
            {
               
                spriteBatch.Draw(image, Position + offset, tint);
            }
        }


        public override float GetWidth()
        {
            return image.Width;
        }

        public override float GetHeight()
        {
            return image.Height;
        }
        protected Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

    }
}
