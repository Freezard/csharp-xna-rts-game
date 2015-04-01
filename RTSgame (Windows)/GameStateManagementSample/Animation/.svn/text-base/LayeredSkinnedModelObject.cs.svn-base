using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using SkinnedModel;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.Utilities;
using RTSgame.Animation;

namespace RTSgame.GameObjects.Abstract
{
    abstract class LayeredSkinnedModelObject : SkinnedModelObject
    {
        /*
        protected BlendedAnimationPlayer secondaryAnimationPlayer;
        private Matrix[] previousPrimaryTransforms;
        private Matrix[] previousSecondaryTransforms;
        private Matrix[] skinTransforms;
        
        protected Matrix[] bindPose;
        private static Matrix zeroMatrix = new Matrix(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        private SkinningData skinningDataValue;

        //Check if update has been run, if it has not, skinTransforms are not initialized
        private bool properInit = false;

        public LayeredSkinnedModelObject(Vector2 position, String model)
            : base(position, model)
        {
            secondaryAnimationPlayer = new BlendedAnimationPlayer(skinningData);
            skinningDataValue = skinningData;
            skinTransforms = new Matrix[secondaryAnimationPlayer.GetSkinTransforms().Length];
            base.GetSkinTransforms().CopyTo(skinTransforms, 0);

        //      secondaryAnimationPlayer.StartClip(getClip(bindPoseClip));
         //     secondaryAnimationPlayer.Update(TimeSpan.Zero, true, Matrix.Identity);
          /    Matrix[] resultingTransforms = new Matrix[secondaryAnimationPlayer.GetSkinTransforms().Length];
           //   secondaryAnimationPlayer.GetSkinTransforms().CopyTo(bindPose, 0);
        }




        public override void updateAnimation(GameTime gameTime)
        {
            if (secondaryAnimationPlayer.CurrentClip != null)
            {
                secondaryAnimationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
            }
            base.updateAnimation(gameTime);
            

            if (animationPlayer != null && secondaryAnimationPlayer != null)
            {
 
                Matrix[] secondaryTransforms = secondaryAnimationPlayer.GetBoneTransforms();
                Matrix[] primaryTransforms = animationPlayer.GetBoneTransforms();
                Matrix[] boneTransforms = new Matrix[secondaryTransforms.Length];
        */
                #region Additive
                //Additive layering
                /*for (int i = 0; i < resultingTransforms.Length; i++)
                {

                    resultingTransforms[i] = primaryTransforms[i] + secondaryTransforms[i];
                }
                 // END additive
                 */
                #endregion
                #region version 2
                //Layering version 2
                //Comparison to bindpose
                /*
                for (int i = 0; i < resultingTransforms.Length; i++)
                {

                    if (MatricesApproximatelyEqual(primaryTransforms[i], bindPose[i]))
                    {

                        resultingTransforms[i] = secondaryTransforms[i];
                    }
                    else
                    {
                        resultingTransforms[i] = primaryTransforms[i];
                    }
                }
                // END Version 2
                */
#endregion

                #region version 1
               /* //Layering version 1
                primaryTransforms.CopyTo(boneTransforms, 0);
                //If there was no previous animation state, set it to the current state
                if (previousPrimaryTransforms == null)
                {
                    previousPrimaryTransforms = new Matrix[primaryTransforms.Length];
                    primaryTransforms.CopyTo(previousPrimaryTransforms, 0);
                }
                if (previousSecondaryTransforms == null)
                {
                    previousSecondaryTransforms = new Matrix[secondaryTransforms.Length];
                    secondaryTransforms.CopyTo(previousSecondaryTransforms, 0);
                }

                //For each matrix
                for (int i = 0; i < boneTransforms.Length; i++)
                {
                    //If primary has NOT moved (same as before) and secondary has, use secondary
                    if (primaryTransforms[i] == previousPrimaryTransforms[i] && secondaryTransforms[i] != previousSecondaryTransforms[i])
                    {
                        //so replace secondary with primary matrix
                        boneTransforms[i] = secondaryTransforms[i];
                    }
                }
                //Copy over this animation state so it can be used next update cycle.
                primaryTransforms.CopyTo(previousPrimaryTransforms, 0);
                secondaryTransforms.CopyTo(previousSecondaryTransforms, 0);
                //END Version 1*/
                #endregion

        /*
                
                
                //Create world transforms from bone transforms
                Matrix[] worldTransforms = new Matrix[boneTransforms.Length];

                worldTransforms[0] = boneTransforms[0];

                // Child bones.
                for (int bone = 1; bone < worldTransforms.Length; bone++)
                {
                    int parentBone = skinningDataValue.SkeletonHierarchy[bone];

                    worldTransforms[bone] = boneTransforms[bone] *
                                                 worldTransforms[parentBone];
                }

                //Create skin transforms from bone transforms
                

                for (int bone = 0; bone < skinTransforms.Length; bone++)
                {
                    skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] *
                                                worldTransforms[bone];
                }
                
                
            }


        }



        static Boolean approximateEqual(float c, float d)
        {
            return Math.Abs(c - d) < 0.001;
        }

        static Boolean MatricesApproximatelyEqual(Matrix a, Matrix b)
        {
            return approximateEqual(a.M11, b.M11) &&
                approximateEqual(a.M12, b.M12) &&
                approximateEqual(a.M13, b.M13) &&
                approximateEqual(a.M14, b.M14) &&
                approximateEqual(a.M21, b.M21) &&
                approximateEqual(a.M22, b.M22) &&
                approximateEqual(a.M23, b.M23) &&
                approximateEqual(a.M24, b.M24) &&
                approximateEqual(a.M31, b.M31) &&
                approximateEqual(a.M32, b.M32) &&
                approximateEqual(a.M33, b.M33) &&
                approximateEqual(a.M34, b.M34) &&
                approximateEqual(a.M41, b.M41) &&
                approximateEqual(a.M42, b.M42) &&
                approximateEqual(a.M43, b.M43) &&
                approximateEqual(a.M44, b.M44);
        }


        public override Matrix[] GetSkinTransforms()
        {

            

            if (animationPlayer != null && secondaryAnimationPlayer != null && secondaryAnimationPlayer.CurrentClip != null)
            {


               
         
                    return skinTransforms;
            }

            else
            {
                
                return base.GetSkinTransforms();
            }

        */
            /*
             // bug if primary player does not move limb
             if (animationPlayer != null && secondaryAnimationPlayer != null)
             {

                 Matrix[] secondaryTransforms = secondaryAnimationPlayer.GetSkinTransforms();
                 Matrix[] primaryTransforms = animationPlayer.GetSkinTransforms();
                 Matrix[] resultingTransforms = new Matrix[secondaryTransforms.Length];
                
                 //If there was no previous animation state, set it to the current state
                 if (previousPrimaryTransforms == null)
                 {
                     previousPrimaryTransforms = new Matrix[primaryTransforms.Length];
                     primaryTransforms.CopyTo(previousPrimaryTransforms, 0);
                 }
                 if (previousSecondaryTransforms == null)
                 {
                     previousSecondaryTransforms = new Matrix[secondaryTransforms.Length];
                     secondaryTransforms.CopyTo(previousSecondaryTransforms, 0);
                 }
                
                 //For each matrix

                 for (int i = 0; i < resultingTransforms.Length; i++)
                 {
                     //If primary has NOT moved (same as before) and secondary has, use secondary
                     /*Debug.Write("PRIMARY");
                     Debug.Write(primaryTransforms[i]);
                     Debug.Write("SECONDARY");
                     Debug.Write(secondaryTransforms[i]);
                     Debug.Write("BASE");
                     Debug.Write(baseTransforms[i]);
                     if (MatricesApproximatelyEqual(primaryTransforms[i], baseTransforms[i]))
                        
                         //primaryTransforms[i] == baseTransforms[i] )
                     {
                         //Debug.Write("USING SECONDARY");
                         //so replace secondary with primary matrix
                         resultingTransforms[i] = secondaryTransforms[i];
                     }
                     else
                     {
                        // Debug.Write("USING PRIMARY");
                         resultingTransforms[i] = primaryTransforms[i];
                     }
                 }
                 //Copy over this animation state so it can be used next update cycle.
                 //primaryTransforms.CopyTo(previousPrimaryTransforms, 0);
                 //secondaryTransforms.CopyTo(previousSecondaryTransforms, 0);
                 return resultingTransforms;
             }
             else
             {
                 return base.GetSkinTransforms();
             }*/

            }
}
