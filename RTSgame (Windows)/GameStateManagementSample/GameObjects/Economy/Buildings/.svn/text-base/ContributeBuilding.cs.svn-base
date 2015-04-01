using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Buildings;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.GameObjects.Economy.Zones;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Economy.Buildings
{
    /// <summary>
    /// A building that has contributors around it
    /// </summary>
    abstract class ContributeBuilding: Building
    {
        
        private int contributors = 0;
        private float efficiency = 0;
        protected const int NUMBER_OF_CONTRIBUTORS_FOR_HALF_EFFICIENCY = 5;



        public ContributeBuilding(BuildingNode bn, Boolean instantConstruction, ModelComponent modelComp, Player owner, int maxHp)
            : base(bn, instantConstruction, modelComp, owner, maxHp)
        {

        }
        internal void Contribute()
        {
            if (!underConstruction)
            {
                contributors++;
                RecalculateEfficiency();
            }

        }
        public override void finishedConstructing()
        {
            
            AddZone(new ContributeZone(this, 5, true));
        }
        public float GetEfficiency()
        {
            return efficiency;
        }
        public void RecalculateEfficiency()
        {
            //k = ln(0.5)/x' where x' is contributors for 50% efficiency
            double k = Math.Log(0.5)/NUMBER_OF_CONTRIBUTORS_FOR_HALF_EFFICIENCY;
            //formula for efficiency = 1-e^(kx) where x are current contributors
            efficiency = (float)(1 - Math.Exp(k * contributors));

        }
        public override void UpdateLogic(GameTime gameTime)
        {
            base.UpdateLogic(gameTime);
          
            contributors = 0;
        }
    }
    
}
