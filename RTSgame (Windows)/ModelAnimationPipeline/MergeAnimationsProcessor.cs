using System.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using ModelAnimationPipeline;

namespace MergeAnimationsPipeline
{
    [ContentProcessor]
    public class MergeAnimationsProcessor : SkinnedModelProcessor
    {
        public string MergeAnimations { get; set; }


        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            context.Logger.LogImportantMessage("MFSFS");
            if (!string.IsNullOrEmpty(MergeAnimations))
            {
                foreach (string mergeFile in MergeAnimations.Split(';')
                                                            .Select(s => s.Trim())
                                                            .Where(s => !string.IsNullOrEmpty(s)))
                {
                    MergeAnimation(input, context, mergeFile);
                }
            }

          
            //Original processing
            return base.Process(input, context);
        }


        void MergeAnimation(NodeContent input, ContentProcessorContext context, string mergeFile)
        {
            NodeContent mergeModel = context.BuildAndLoadAsset<NodeContent, NodeContent>(
                                                new ExternalReference<NodeContent>(mergeFile), null);

            BoneContent rootBone = MeshHelper.FindSkeleton(input);

            if (rootBone == null)
            {
                context.Logger.LogWarning(null, input.Identity, "Source model has no root bone.");
                return;
            }

            BoneContent mergeRoot = MeshHelper.FindSkeleton(mergeModel);

            if (mergeRoot == null)
            {
                context.Logger.LogWarning(null, input.Identity, "Merge model '{0}' has no root bone.", mergeFile);
                return;
            }

            foreach (string animationName in mergeRoot.Animations.Keys)
            {
                if (rootBone.Animations.ContainsKey(animationName))
                {
                    context.Logger.LogWarning(null, input.Identity,
                        "Cannot merge animation '{0}' from '{1}', because this animation already exists.",
                        animationName, mergeFile);

                    continue;
                }

                context.Logger.LogImportantMessage("Merging animation '{0}' from '{1}'.", animationName, mergeFile);

                rootBone.Animations.Add(animationName, mergeRoot.Animations[animationName]);
            }
        }
    }
}