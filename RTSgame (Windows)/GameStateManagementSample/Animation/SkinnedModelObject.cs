using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using RTSgame.Animation;
using SkinnedModel;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.Utilities;
using RTSgame.GameObjects.Units;

namespace RTSgame.GameObjects.Abstract
{
    //The base class for models who are animated
    abstract class SkinnedModelObject
    {
        /*
        //The animationplayer is responsible for playing the animations (duh)
        protected BlendedAnimationPlayer animationPlayer;
        //skinningdata holds the information gained from the model file
        protected SkinningData skinningData;

        public SkinnedModelObject(Vector2 position, String model):base(position, model)
        {
            InitializeAnimation();
        }*/
        //Initializes Animation, must be run
      /*  private void InitializeAnimation()
        {
            
            skinningData = model.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag. Don't forget to use correct content processor!");
            
            if (baseTransforms == null)
            {
                baseTransforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(baseTransforms);
            }

            if (skinningData != null)
            {
                skinningData.BindPose.CopyTo(baseTransforms, 0);

                //model.Bones.bo

                // Create an animation player
                animationPlayer = new BlendedAnimationPlayer(skinningData);

            }
            DebugPrinter.Write("INIT ANIM FOR " + this);
             
        }
        //Gets a clip with a specific name
        protected AnimationClip getClip(String name)
        {
            return skinningData.AnimationClips[name];
        }


        //Gets the skin transforms associated with this gameObject
        public override Matrix[] GetSkinTransforms()
        {

            if (animationPlayer != null)
            {
                
                return animationPlayer.GetSkinTransforms();
            }
            else
            {
               

                return base.GetSkinTransforms();
            }

        }
        
        
        //Updates the animation
        public virtual void updateAnimation(GameTime gameTime)
        {
            
            animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
          
            //animationPlayer.
        }*/
    }
}
