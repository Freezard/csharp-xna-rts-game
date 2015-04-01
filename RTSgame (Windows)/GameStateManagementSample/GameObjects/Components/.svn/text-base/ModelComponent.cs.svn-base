using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;

namespace RTSgame.GameObjects.Components
{
    class ModelComponent
    {
        private Model model;
        private Texture2D texture;
        protected Matrix[] baseTransforms;

       
        

        public ModelComponent(String modelName)
        {
            this.model = AssetBank.GetInstance().GetModel(modelName);
            Texture2D possibleTexture = AssetBank.GetInstance().GetTextureReturnNull(modelName);
            if (possibleTexture != null)
            {
                texture = possibleTexture;
            }
            if (baseTransforms == null)
            {
                baseTransforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(baseTransforms);
            }
        }

        public ModelComponent(String modelName, String textureName)
            : this(modelName, AssetBank.GetInstance().GetTexture(textureName))
        {

        }
        public ModelComponent(String modelName, Texture2D texture):this(modelName)
        {
            this.texture = texture;
        }


        /*
        public void SetTexture(String textureName)
        {
            texture = AssetBank.GetInstance().GetTexture(textureName);
        }*/

        public Model Model
        {
            get { return model; }
            
        }
        public Texture2D Texture
        {
            get { return texture; }
            
        }
        public Matrix[] BaseTransforms
        {
            get { return baseTransforms; }
        }
        public virtual Matrix[] GetSkinTransforms()
        {

            return BaseTransforms;
        }
    }
}
