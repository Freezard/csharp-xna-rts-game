using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using SkinnedModel;
using RTSgame.Utilities;

namespace RTSgame.Animation
{
    class BlendedAnimationPlayer:AnimationPlayer
    {
        Matrix[] blendBoneTransforms;
        Matrix[] blendWorldTransforms;
        Matrix[] blendSkinTransforms;
        Boolean[] finishedBlending;
        Boolean blending = false;
        private int currentBlendTime;
        private int blendTimeMs;
        private float blendValue;
        
        public BlendedAnimationPlayer(SkinningData skinningData):base(skinningData)
        {
            throw new NotImplementedException();
        }




        public void SwitchClipWithBlend(AnimationClip clip, int blendTimeMs, float blendValue, bool looping, float animationSpeed)
        {
            throw new NotImplementedException("NOT DONE YET");
            if (CurrentClip == null)
            {
                throw new NullReferenceException("Has to have previous clip to blend!");
            }
            //Get current transforms
            blendBoneTransforms = new Matrix[this.GetBoneTransforms().Length];
            this.GetBoneTransforms().CopyTo(blendBoneTransforms,0);

            blendWorldTransforms = new Matrix[this.GetWorldTransforms().Length];
            this.GetWorldTransforms().CopyTo(blendWorldTransforms, 0); 
            
            blendSkinTransforms = new Matrix[this.GetSkinTransforms().Length];
            this.GetSkinTransforms().CopyTo(blendSkinTransforms, 0);


            //Start next clip
            this.StartClip(clip, looping, animationSpeed);
            //Init blending variables
            blending = true;
            currentBlendTime = 0;
            this.blendTimeMs = blendTimeMs;
            this.blendValue = blendValue;
            finishedBlending = new Boolean[blendBoneTransforms.Length];

                

        }
        
        public override void StartClip(AnimationClip clip, bool looping, float animationSpeed)
        {
            //Cancel blending!
            base.StartClip(clip, looping, animationSpeed);
            blending = false;
        }
        public override void Update(TimeSpan time, bool relativeToCurrentTime, Matrix rootTransform)
        {

                base.Update(time, relativeToCurrentTime, rootTransform);

                if (blending)
                {
                    
                    Matrix[] goalTransforms = base.GetBoneTransforms();



                   
                    for (int i = 0; i < blendBoneTransforms.Length; i++)
                    {
                        if (finishedBlending[i])
                        {
                            blendBoneTransforms[i] = goalTransforms[i];
                        }
                        else if (blendBoneTransforms[i] != goalTransforms[i])
                        {
                            //Basic blend using lerp
                            //blendTransforms[i] = Matrix.Lerp(blendTransforms[i], goalTransforms[i], blendValue);

                            //Better blend using quaternions
                            Vector3 bScale, bTranslation, gScale, gTranslation;
                            Quaternion bRotation, gRotation;
                            blendBoneTransforms[i].Decompose(out bScale, out bRotation, out bTranslation);
                            goalTransforms[i].Decompose(out gScale, out gRotation, out gTranslation);
                            bScale = Vector3.Lerp(bScale, gScale, blendValue);
                            bTranslation = Vector3.Lerp(bTranslation, gTranslation, blendValue);
                            bRotation = Quaternion.Slerp(bRotation, gRotation, blendValue);
                            blendBoneTransforms[i] = Matrix.CreateScale(bScale) * Matrix.CreateFromQuaternion(bRotation) * Matrix.CreateTranslation(bTranslation);
                            
                        }
                        else
                        {
                            finishedBlending[i] = true;
                            DebugPrinter.Write("Finish: " + i);
                        }
                    }

                    currentBlendTime += time.Milliseconds;

                    //Update world and skin transforms

                    //Create world transforms from bone transforms
                    Matrix[] worldTransforms = new Matrix[boneTransforms.Length];
                    //parent bone
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


                    if (currentBlendTime > blendTimeMs)
                    {
                        blending = false;
                    }
                }
          
        }

        public override Matrix[] GetWorldTransforms()
        {
            if (!blending)
            {
                return base.GetWorldTransforms();
            }
            else
            {
                return blendWorldTransforms;
            }
            
        }

        public override Matrix[] GetBoneTransforms()
        {
            if (!blending)
            {
                return base.GetBoneTransforms();
            }
            else
            {
                return blendBoneTransforms;
            }
            
        }
        public override Matrix[] GetSkinTransforms()
        {
            if (!blending)
            {
                
                return base.GetSkinTransforms();
            }
            else
            {
                
                return blendSkinTransforms;
            }
        }


        

    }
}
