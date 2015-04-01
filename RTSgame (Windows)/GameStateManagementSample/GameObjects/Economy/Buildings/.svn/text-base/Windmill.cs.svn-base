using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Buildings;
using RTSgame.GameObjects.Economy.Zones;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;
using RTSgame.GameObjects.Components;
using Microsoft.Xna.Framework.Graphics;

namespace RTSgame.GameObjects.Economy.Buildings
{
    /// <summary>
    /// Test building for contributebuilding
    /// </summary>
    class Windmill:ContributeBuilding,IAnimated
    {
        private float rotation = 0, rotationSpeed = 0.0f;
        private int maxEnergyModified = 0;
        //Sails are the things that spin on a windmill
        private Vector3 sailsCenter = new Vector3(0, 100, 0);
        private Matrix[] modifiedTransforms;
        public Windmill(BuildingNode bn, Boolean instantConstruction, Player owner)
            : base(bn, instantConstruction, new ModelComponent("BuildingWindmill", "BuildingWindmill"+owner.GetPlayerColorString()), owner, 100)
        {
            
          //  baseTransforms = new Matrix[GetModel().Bones.Count];
         //   GetModel().CopyAbsoluteBoneTransformsTo(baseTransforms);
          //      modifiedTransforms = new Matrix[GetModel().Bones.Count];
            //    GetModel().CopyAbsoluteBoneTransformsTo(modifiedTransforms);
                
                angleAround.Y -= (float)Math.PI / 4;
            
                
                

                InitBuilding(1000, instantConstruction, 0.03f);
                
              //  InitializeCollisionBox();
        }
        public override float GetSolidCollisionSize()
        {
            return base.GetSolidCollisionSize()/2;
        }
        public void updateAnimation(GameTime gameTime)
        {
            
            rotation += rotationSpeed * gameTime.ElapsedGameTime.Milliseconds;

            //rotation = 0;
            //Vector3 sailsCenter = Constants.TweakVector;
            //Debug.Write(sailsCenter);

            // reenable animation

      //      modifiedTransforms[2] = baseTransforms[2] * Matrix.CreateTranslation(-sailsCenter) * Matrix.CreateRotationX(rotation) * Matrix.CreateTranslation(sailsCenter);

        }
       /* public override Matrix[] GetSkinTransforms()
        {
            return modifiedTransforms;
        }*/
        public override void UpdateLogic(GameTime gameTime)
        {
            base.UpdateLogic(gameTime);
            if (!underConstruction)
            {
                rotationSpeed = GetEfficiency()*0.0005f;

                //Increase player energy
                Owner.AddEnergy((int)(Constants.DESIGN_WINDMILL_ENERGY_RECHARGE * GetEfficiency()));
                //Increase player max energy
                int modifyMaxEnergyNow = (int)(Constants.DESIGN_WINDMILL_ENERGY_MAX * GetEfficiency());

                Owner.ModifyMaxEnergy(modifyMaxEnergyNow - maxEnergyModified);
                maxEnergyModified = modifyMaxEnergyNow;
                
            }
        }
    }
}
