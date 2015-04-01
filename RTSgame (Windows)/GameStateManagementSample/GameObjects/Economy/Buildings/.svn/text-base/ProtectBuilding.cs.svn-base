using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Buildings;
using RTSgame.Utilities;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Projectiles;
using RTSgame.GameObjects.Units;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Economy.Buildings
{
    /// <summary>
    /// A building that shoots at other objects
    /// </summary>
    class ProtectBuilding : ContributeBuilding, IIntelligent, IUsableStructure
    {
        private int heat = 0;
        private int coolDownTime = 1000;
        private float closest;
        private GameObject closestGO = null;

        public ProtectBuilding(BuildingNode bp, Boolean instantConstruction, Player owner)
            : base(bp, instantConstruction, new ModelComponent("BuildingControlTower", "BuildingControlTower"+owner.GetPlayerColorString()), owner, 100)
        {
            
           // texture = AssetBank.GetInstance().GetTexture("sunwindmill");



            InitBuilding(3000, instantConstruction, 0.03f);
            angleAround.Y = -(float)Math.PI / 4;
           // angleAround.Z += (float)Math.PI / 4;
       //     InitializeCollisionBox();
        }


        public void AIUpdatePreInteractions(GameTime gameTime)
        {

            closest = float.PositiveInfinity;
            closestGO = null;
        }

        public void AIInteract(IInteractable gameObject)
        {
            
            
            
            if (readyToFire() && gameObject is PlayerOwnedObject && !(((PlayerOwnedObject)gameObject).Owner == Owner) && Calculations.IsWithin2DRange(this.GetPosition(), gameObject.GetPosition(), 5))
            {
                
                float distance = Vector2.DistanceSquared(gameObject.GetPosition(), this.GetPosition());
                if (distance < closest)
                {
                    closest = distance;
                    closestGO = (GameObject)gameObject;
                }
            }
        }

        public void AIUpdatePostInteractions(GameTime gameTime)
        {
            if (readyToFire() && closestGO != null)
            {
                heat = coolDownTime;
                SoundPlayer.Play3DSound("shoot", this.GetPositionV3());
                Projectile newShot = new PlayerCodedProjectile(this, Owner, closestGO, true, Vector3.Zero, 25);//new SimpleShot<Enemy>(this, closestGO, true, new Vector3(0,1,0));
                GameState.GetInstance().addGameObject(newShot);
                //DrawManager.GetInstance().addDrawable(newShot);
            }
        }
        public override float GetSolidCollisionSize()
        {
            return base.GetSolidCollisionSize() / 1.3f;
        }

        private bool readyToFire()
        {
            return (heat <= 0) && !underConstruction;
        }

        public void AttemptToUse(PlayerCharacter playerCharacter)
        {
            heat = 0; //TODO Make up a use //Probably something along the lines of: fire a shot right now at cost of energy, or start period of increased rate of fire
        }

        public override void finishedConstructing()
        {
            base.finishedConstructing();
        }
        public override void UpdateLogic(GameTime gameTime)
        {

            heat -= (int)(gameTime.ElapsedGameTime.Milliseconds * (1 + GetEfficiency()));
           
            base.UpdateLogic(gameTime);
        }

        public float GetMaxAIInteractionRange()
        {
            return TweakConstants.PROTECTBUILDING_SIGHT_RANGE;
        }
    }
}
