#region File Description
//-----------------------------------------------------------------------------
// AnimationPlayer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace SkinnedModel
{
    /// <summary>
    /// The animation player is in charge of decoding bone position
    /// matrices from an animation clip.
    /// </summary>
    public class AnimationPlayer
    {
        #region Fields


        // Information about the currently playing animation clip.
        protected AnimationClip currentClipValue;
        protected TimeSpan currentTimeValue;
        protected int currentKeyframe;

        protected Boolean looping = true;
        protected Boolean playing = false;
        protected float animationSpeed = 0.10f;
        protected AnimationClip idleClip;
        //proper init makes sure the animationplayer is started before any skintransforms are given
        protected Boolean properInit = false;
        protected Boolean idleIsBind = false;

        // Current animation transform matrices.
        protected Matrix[] boneTransforms;
        protected Matrix[] worldTransforms;
        protected Matrix[] skinTransforms;
        

        // Backlink to the bind pose and skeleton hierarchy data.
        protected SkinningData skinningDataValue;


        #endregion


        /// <summary>
        /// Constructs a new animation player.
        /// </summary>
        public AnimationPlayer(SkinningData skinningData)
        {
            if (skinningData == null)
                throw new ArgumentNullException("skinningData");

            skinningDataValue = skinningData;

            boneTransforms = new Matrix[skinningData.BindPose.Count];
            worldTransforms = new Matrix[skinningData.BindPose.Count];
            skinTransforms = new Matrix[skinningData.BindPose.Count];
            foreach (String s in skinningDataValue.AnimationClips.Keys)
            {
                Console.WriteLine(s);
            }
        }

        public void SetLooping(Boolean value)
        {
            looping = value;

        }
        public void SetIdleClip(AnimationClip clip, Boolean useBindPose)
        {
            idleClip = clip;
            idleIsBind = useBindPose;
        }
        public void SetAnimationSpeed(float value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException("animationSpeed");
            }
            animationSpeed = value;

        }

        public void StartClip(String clipName)
        {
            

            this.StartClip(clipName, true);
        }

        public void StartClip(String clipName, Boolean looping)
        {
            
            this.StartClip(clipName, looping, 1.0f);
        }

        public void StartClip(String clipName, Boolean looping, float animationSpeed)
        {
            this.StartClip(skinningDataValue.AnimationClips[clipName], looping, 1.0f);
        }

        public void StartClip(AnimationClip clip)
        {


            this.StartClip(clip, true);
        }

        public void StartClip(AnimationClip clip, Boolean looping)
        {

            this.StartClip(clip, looping, 1.0f);
        }


        /// <summary>
        /// Starts decoding the specified animation clip.
        /// </summary>
        /// 

        public virtual void StartClip(AnimationClip clip, Boolean looping, float animationSpeed)
        {
            
            properInit = true;
            playing = true;
            this.looping = looping;
            SetAnimationSpeed(animationSpeed);
            if (clip == null)
                throw new ArgumentNullException("The given clip is null!");

            currentClipValue = clip;

                currentTimeValue = TimeSpan.Zero;
                currentKeyframe = 0;


            // Initialize bone transforms to the bind pose.
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }
        /// <summary>
        /// Attempts to switch clip if that clip is not already playing
        /// </summary>
        /// <param name="clip"></param>
        public void SwitchClip(AnimationClip clip)
        {
            if (this.CurrentClip != clip)
            {
                StartClip(clip);
            }

        }

        public void FinishedClip(TimeSpan duration)
        {
            if (looping)
            {

                StartClip(CurrentClip);
                //Riskfritt?
              //  this.Update(duration - CurrentClip.Duration, true, Matrix.Identity);
            }
            else
            {
                playing = false;
                if (idleClip != null)
                {
                    if (!idleIsBind)
                    {
                        StartClip(idleClip);

                    }
                    else
                    {
                        UseBindPose(idleClip);
                    }

                }
            }

        }

        public void Update(TimeSpan timeSpan)
        {
            this.Update(timeSpan, true, Matrix.Identity);
        }
        /// <summary>
        /// Advances the current animation position.
        /// </summary>
        public virtual void Update(TimeSpan time, bool relativeToCurrentTime,
                           Matrix rootTransform)
        {
            if (!properInit)
            {
                throw new ApplicationException("Must first call start clip/use bind pose, otherwise skinnedmodelobject will be invisible!");
            }
            if (playing)
            {
                UpdateBoneTransforms(time, relativeToCurrentTime);
                UpdateWorldTransforms(rootTransform);
                UpdateSkinTransforms();
            }
        }


        /// <summary>
        /// Helper used by the Update method to refresh the BoneTransforms data.
        /// </summary>
        public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {
         //   time = TimeSpan.FromTicks((long)(time.Ticks * animationSpeed));

            if (currentClipValue == null)
                throw new InvalidOperationException(
                            "AnimationPlayer.Update was called before StartClip");

            // Update the animation position.
            if (relativeToCurrentTime)
            {
                time += currentTimeValue;

                // If we reached the end
                if (time >= currentClipValue.Duration || time < TimeSpan.Zero)
                {
                    FinishedClip(time);
                    return;
                }
                    
            }

            if ((time < TimeSpan.Zero) || (time >= currentClipValue.Duration))
                throw new ArgumentOutOfRangeException("time");

            // If the position moved backwards, reset the keyframe index.
            if (time < currentTimeValue)
            {
                currentKeyframe = 0;
                skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
            }

            currentTimeValue = time;

            // Read keyframe matrices.
            IList<Keyframe> keyframes = currentClipValue.Keyframes;

            while (currentKeyframe < keyframes.Count)
            {
                Keyframe keyframe = keyframes[currentKeyframe];

                // Stop when we've read up to the current time position.
                if (keyframe.Time > currentTimeValue)
                    break;

                // Use this keyframe.
                boneTransforms[keyframe.Bone] = keyframe.Transform;

                currentKeyframe++;
            }
        }


        /// <summary>
        /// Helper used by the Update method to refresh the WorldTransforms data.
        /// </summary>
        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            // Root bone.
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            // Child bones.
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = skinningDataValue.SkeletonHierarchy[bone];

                worldTransforms[bone] = boneTransforms[bone] *
                                             worldTransforms[parentBone];
            }
        }


        /// <summary>
        /// Helper used by the Update method to refresh the SkinTransforms data.
        /// </summary>
        public void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < skinTransforms.Length; bone++)
            {
                skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] *
                                            worldTransforms[bone];
            }
        }

        public List<Matrix> GetInverseBindPose()
        {
            return skinningDataValue.InverseBindPose;

        }

        /// <summary>
        /// Gets the current bone transform matrices, relative to their parent bones.
        /// </summary>
        public virtual Matrix[] GetBoneTransforms()
        {
            return boneTransforms;
        }


        /// <summary>
        /// Gets the current bone transform matrices, in absolute format.
        /// </summary>
        public virtual Matrix[] GetWorldTransforms()
        {
            return worldTransforms;
        }


        /// <summary>
        /// Gets the current bone transform matrices,
        /// relative to the skinning bind pose.
        /// </summary>
        public virtual Matrix[] GetSkinTransforms()
        {
            return skinTransforms;
        }


        /// <summary>
        /// Gets the clip currently being decoded.
        /// </summary>
        public AnimationClip CurrentClip
        {
            get { return currentClipValue; }
        }


        /// <summary>
        /// Gets the current play position.
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return currentTimeValue; }
        }

        public void UseBindPose(String clipName)
        {
            StartClip(clipName);
            Update(TimeSpan.Zero, true, Matrix.Identity);
            playing = false;
        }
        public void UseBindPose(AnimationClip clip)
        {
            StartClip(clip);
            Update(TimeSpan.Zero, true, Matrix.Identity);
            playing = false;
        }

        
    }
}
