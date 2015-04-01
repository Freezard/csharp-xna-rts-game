using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkinnedModel;
using RTSgame.Animation;
using RTSgame.Utilities;

namespace RTSgame.GameObjects.Components
{
    class LayeredSkinnedModelComponent:SkinnedModelComponent
    {
        private AnimationPlayer secondPlayer;

        
        public LayeredSkinnedModelComponent(String modelName):base(modelName)
        {
            secondPlayer = new AnimationPlayer(this.SkinningData);
        }
        public LayeredSkinnedModelComponent(String modelName, String textureName)
            : base(modelName, textureName)
        {
            secondPlayer = new AnimationPlayer(this.SkinningData);
        }
        public LayeredSkinnedModelComponent(String modelName, Texture2D texture)
            : base(modelName, texture)
        {
            secondPlayer = new AnimationPlayer(this.SkinningData);
        }
        public AnimationPlayer SecondPlayer
        {
            get { return secondPlayer; }
        }
        public override Matrix[] GetSkinTransforms()
        {
            
            Matrix[] skinTransforms = new Matrix[animationPlayer.GetSkinTransforms().Length];
           //  Additive
            for(int i = 0; i < skinTransforms.Length; i++)
            {
                skinTransforms[i] = animationPlayer.GetSkinTransforms()[i] + secondPlayer.GetSkinTransforms()[i];
            }
            
            /* Work in progress
            for (int i = 0; i < skinTransforms.Length; i++)
            {

                if (Calculations.MatricesApproximatelyEqual(animationPlayer.GetSkinTransforms()[i], baseTransforms[i]))
                {

                    skinTransforms[i] = secondPlayer.GetSkinTransforms()[i];
                    
                }
                else
                {
                    skinTransforms[i] = animationPlayer.GetSkinTransforms()[i];
                   
                }
            }*/
            return skinTransforms;
        }
    }
}
