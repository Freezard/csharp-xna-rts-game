using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;
using RTSgame.GameObjects.Economy;
using RTSgame.Utilities;
using RTSgame.Utilities.Graphics.ParticleSystem;
using RTSgame.Utilities.IO.UI;
using RTSgame.GameObjects.Units;
using RTSgame.Utilities.Game;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Buildings
{
    /// <summary>
    /// Super-class for all building
    /// </summary>
    abstract class Building : PlayerOwnedObject, IUITrackable, ILogic
    {
        private int buildTimeMs;

        private float buildProgress;
        private float resourcesPresent;
        // protected float buildSpeed;
        private float baseScale;
        protected bool underConstruction = true;
        //protected ResourceDropOffZone buildZone;
        protected BuildingNode node;
        protected List<GameObject> allZones = new List<GameObject>();






        //Creating building at a designated buildPoint
        public Building(BuildingNode bn, Boolean instantConstruction, ModelComponent modelComp, Player owner, int maxHp)
            : base(bn.GetPosition(), modelComp, owner, maxHp)
        {
            //position = bn.GetPosition();
            //SetPosition(bn.GetPosition());
            node = bn;
            //buildZone = new ResourceDropOffZone(this, 5, true);
            //AddZone(buildZone);
            if (!node.IsFree())
            {
                throw new Exception("Error! building placed in occupied node");
            }
            node.SetFree(false);
            scale = baseScale / 2;
            //ModelComp.Texture = AssetBank.GetInstance().GetTexture("sfds");
            






        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxHp">Max HP</param>
        /// <param name="buildTime">Milliseconds it takes to build</param>
        /// <param name="instantConstruction">If building is already finished</param>
        /// <param name="baseScale">What scale the building is</param>
        public void InitBuilding(int buildTime, Boolean instantConstruction, float baseScale)
        {
            this.baseScale = baseScale;
            scale = baseScale;
            InitializeCollisionBox();



            this.buildTimeMs = buildTime;
            if (!instantConstruction)
            {
                hitPoints = 1;
                buildProgress = 0;
            }
            else
            {

                hitPoints = maxHitPoints;
                buildProgress = buildTime;
                //buildZone.SetActive(false);
            }
            scale = baseScale / 2 + baseScale / 2 * ((float)buildProgress / buildTimeMs);
        }
        public void AddZone(GameObject zone)
        {
            allZones.Add(zone);
            GameState.GetInstance().addGameObject(zone);
        }
        public abstract void finishedConstructing();
        public override void RemoveFromGame()
        {
            base.RemoveFromGame();
            node.Release();
            foreach (GameObject zone in allZones)
            {
                zone.RemoveFromGame();
            }
        }
        public bool IsUnderConstruction()
        {
            return underConstruction;

        }


        public virtual void GiveResource()
        {

            if (underConstruction)
            {
                resourcesPresent++;
                DebugPrinter.Write(resourcesPresent);

            }
        }
        public virtual void UpdateLogic(GameTime gameTime)
        {
            if (underConstruction)
            {

                buildProgress += gameTime.ElapsedGameTime.Milliseconds;
                scale = baseScale / 2 + baseScale / 2 * ((float)buildProgress / buildTimeMs);
                hitPoints += (maxHitPoints - 1) * (float)gameTime.ElapsedGameTime.Milliseconds / buildTimeMs;

                if (buildProgress >= buildTimeMs)
                {
                    underConstruction = false;


                    finishedConstructing();
                }



            }

            float radius = 15;
            float lightPower = 3.0f;
            if (underConstruction)
            {
                lightPower *= buildProgress / buildTimeMs;
                radius *= buildProgress / buildTimeMs;
            }

            ParticleManager.GetInstance().AddLight(
                this.GetPositionV3() + new Vector3(0, 3, 0), new Vector4(1, 0.5f, 0, 0),
                lightPower * DayCycle.GetNightLight(),
                radius,
                0.001f,
                true);
        }




        //Old construction code:

        /*
        if (buildProgress < resourcesPresent)
        {
            float progressBefore = buildProgress;
            buildProgress += buildSpeed * gameTime.ElapsedGameTime.Milliseconds;
                    
            if (buildProgress > resourcesPresent)
            {
                buildProgress = resourcesPresent;
            }
            float actualProgress = buildProgress - progressBefore;

            hp += (maxHp - 1) * actualProgress / buildCost;
                    
        }
        scale = baseScale / 2 + baseScale / 2 * ((float)buildProgress / buildCost);

        if (resourcesPresent >= buildCost && buildZone.IsActive())
        {

            buildZone.SetActive(false);
        }
        if (buildProgress >= buildCost)
        {
            underConstruction = false;
            finishedConstructing();
        }*/



        public override void HitPointsZero()
        {
            ParticleManager.GetInstance().AddExplosion(GetPositionV3(), Vector3.Zero, 1);
            SoundPlayer.Play3DSound("explosion", this.GetPositionV3());
            this.RemoveFromGame();
        }



        public override CollidableType GetCollidableType()
        {
            return CollidableType.Structure;
        }

        public override float GetMaxInteractionRange()
        {
            return Constants.DefaultMaxInteractionRange;
        }




    }
}
