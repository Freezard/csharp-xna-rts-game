using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using Microsoft.Xna.Framework.Graphics;
using SkinnedModel;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects
{
    
    

    class WalkingDude : ModelObject
    {
        int steps = 0;
        Boolean walkings = true;
        AnimationClip walking;
        AnimationClip waving;
        AnimationClip walking2;

        Matrix[] lastFrame;

        public WalkingDude(Vector2 position, int anim)
            : base(position, new ModelComponent("Pouya2"))
        {


            
            //model = AssetBank.GetInstance().GetModel("WizardSkuttande2");
            
            
            // model = AssetBank.GetInstance().GetModel("AnimationWizardSenaste");
            //texture = AssetBank.GetInstance().GetTexture("WizardSkjutarAnimation4Texture2");
            scale = 0.2f;

           
            

           //walking = getClip("Take 002");
//waving = getClip("Take 001");
            //walking2 = getClip("Take 003");
            //Model baseModel = AssetBank.GetInstance().GetModel("PouyaBase");
            
            //
          //  animationPlayer.StartClip(waving);
            //animationPlayer.Update(new TimeSpan(0), false, Matrix.Identity);
            //bindPose = new Matrix[animationPlayer.GetSkinTransforms().Length];
            //animationPlayer.GetSkinTransforms().CopyTo(bindPose, 0);
            //baseModel.CopyBoneTransformsTo(baseTransforms);
            
            //secondaryAnimationPlayer.StartClip(walking2);
            
            /*
            if (anim == 1)
            {
                animationPlayer.StartClip(waving);
                secondaryAnimationPlayer.StartClip(walking);
            }
            else if(anim == 2)
            {
                animationPlayer.StartClip(walking);
                //secondaryAnimationPlayer.StartClip(walking);
            }else{
                animationPlayer.StartClip(waving);
            }
            */
            InitializeCollisionBox();
        }
        /*public override void updateAnimation(GameTime gameTime)
        {
            
            base.updateAnimation(gameTime);

            
        }*/
     /*   public override Matrix[] GetSkinTransforms()
        {
            Matrix[] skins = base.GetSkinTransforms();

            if(lastFrame != null){
            for (int i = 0; i < skins.Length; i++)
            {
                if (skins[i] != lastFrame[i])
                {
                    Debug.Write(skins[i]);

                }

            }
            }

            lastFrame = skins;
            return skins;
        }*/

        public override CollidableType GetCollidableType()
        {
            return CollidableType.Doodad;
        }

        public override float GetMaxInteractionRange()
        {
            return 15;
        }

    }
}
