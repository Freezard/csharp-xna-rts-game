using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RTSgame.GameObjects.Components
{
    class SkinnedModelComponent:ModelComponent
    {
        //The animationplayer is responsible for playing the animations (duh)
        protected AnimationPlayer animationPlayer;

        
        //TODO Better layering
        
        //skinningdata holds the information gained from the model file
        private SkinningData skinningData;

       

        public SkinnedModelComponent(String modelName):base(modelName)
        {
            InitializeAnimation();
        }
        public SkinnedModelComponent(String modelName, String textureName)
            : base(modelName, textureName)
        {
            InitializeAnimation();
        }
        public SkinnedModelComponent(String modelName, Texture2D texture)
            : base(modelName, texture)
        {
            InitializeAnimation();
        }
        private void InitializeAnimation()
        {

            skinningData = Model.Tag as SkinningData;

            if (skinningData == null)
            {
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag. Don't forget to use correct content processor!");

            }

            if (skinningData != null)
            {
                skinningData.BindPose.CopyTo(baseTransforms, 0);

                //model.Bones.bo

                // Create an animation player
                animationPlayer = new AnimationPlayer(skinningData);

            }
            
        }
        public override Matrix[] GetSkinTransforms()
        {

            return animationPlayer.GetSkinTransforms();
        }
        /*public void UpdateAnimation(GameTime gameTime)
        {
            animationPlayer.Update(gameTime.ElapsedGameTime);
        }*/
        public SkinningData SkinningData
        {
            get { return skinningData; }
        }
        public AnimationPlayer AnimationPlayer
        {
            get { return animationPlayer; }
        }
    }
}
