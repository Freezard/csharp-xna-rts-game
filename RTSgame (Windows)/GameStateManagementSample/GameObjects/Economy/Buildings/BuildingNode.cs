using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using RTSgame.GameObjects.Economy.Buildings;
using RTSgame.GameObjects.Components;
namespace RTSgame.GameObjects.Buildings
{
    //A location where a building may be built
    class BuildingNode : ModelObject, ILogic
    {
        private Boolean hasBuilding = false;
        //private int size;

        public BuildingNode(Vector2 newPosition)
            : base(newPosition, new ModelComponent("ByggnadGrund3", "SolidBrown"))
        {
            
           // texture = AssetBank.GetInstance().GetTexture("SolidBrown");
            scale = 0.15f;
            InitializeCollisionBox();
            
        }
        
        /*public void AddRobotFactory()
        {
            
            if (!hasBuilding)
            {
                
                RobotFactory rf = new RobotFactory(this,false);
                GameState.GetInstance().addGameObject(rf);
                DrawManager.GetInstance().addDrawable(rf);
                

            }

        }
        public void AddHealingBuilding()
        {

            if (!hasBuilding)
            {

                HealingBuilding hb = new HealingBuilding(this,false);
                GameState.GetInstance().addGameObject(hb);
                DrawManager.GetInstance().addDrawable(hb);
                

            }

        }
        public void AddWindMill()
        {
            if (!hasBuilding)
            {

                Windmill hb = new Windmill(this,false);
                GameState.GetInstance().addGameObject(hb);
                DrawManager.GetInstance().addDrawable(hb);
               

            }
        }*/


        public override CollidableType GetCollidableType()
        {
            return CollidableType.StructureSite;
        }

        public override float GetMaxInteractionRange()
        {
            return Constants.DefaultMaxInteractionRange;
        }

        internal bool IsFree()
        {
            return !hasBuilding;
        }

        public void UpdateLogic(GameTime gameTime)
        {
            //Debug.Write(this.GetPosition());
        }

        internal void Release()
        {
            hasBuilding = false;
        }

        internal void SetFree(bool p)
        {
            hasBuilding = !p;
        }
    }
}
