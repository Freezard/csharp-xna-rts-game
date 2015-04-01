using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects
{
    class AnimatedTree : ModelObject
    {
        private float rotation = 0, rotationSpeed = 0.0005f;
        //Sails are the things that spin on a windmill
        private Vector3 sailsCenter = new Vector3(0, 100, 0);

        private Matrix[] modifiedTransforms;
        public AnimatedTree(Vector2 position)
            : base(position, new ModelComponent("BuildingWindmill"))
        {
           //recreate this if necessary
            /*
            baseTransforms = new Matrix[GetModel().Bones.Count];
            GetModel().CopyAbsoluteBoneTransformsTo(baseTransforms);
            modifiedTransforms = new Matrix[GetModel().Bones.Count];
            GetModel().CopyAbsoluteBoneTransformsTo(modifiedTransforms);
            
            angleAround.Y -= (float)Math.PI / 4;
            InitializeCollisionBox();*/
        }

       

       /* public override void updateAnimation(GameTime gameTime)
        {
            rotation += rotationSpeed * gameTime.ElapsedGameTime.Milliseconds;

            //rotation = 0;
            //Vector3 sailsCenter = Constants.TweakVector;
            //Debug.Write(sailsCenter);
           // modifiedTransforms[2] = baseTransforms[2] * Matrix.CreateTranslation(-sailsCenter) * Matrix.CreateRotationX(rotation) * Matrix.CreateTranslation(sailsCenter);
                
        }*/
        /*public override Matrix[] GetSkinTransforms()
        {
            return modifiedTransforms;
        }*/

        public override CollidableType GetCollidableType()
        {
            return CollidableType.Doodad;
        }

        public override float GetMaxInteractionRange()
        {
            throw new NotImplementedException();
        }
    }
}
