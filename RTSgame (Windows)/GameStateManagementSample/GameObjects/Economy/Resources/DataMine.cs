using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.GameObjects.Economy;
using RTSgame.GameObjects.Economy.Zones;
using RTSgame.GameObjects.Economy.Buildings;
using RTSgame.GameObjects.Units;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Resources
{
    class DataMine : Structure
    {
        public int resourcesInside = 100;
        public int coolDownMs = Constants.DESIGN_MINE_COOLDOWN_MS;
        public int heat = 0;

        public DataMine(Vector2 newPosition)
            : base(newPosition, new ModelComponent("DoodadTree2"))
        {
            
            
           
           
          //  texture = AssetBank.GetInstance().GetTexture("DoodadTree2");
            //ResourcePickUpZone zone = new ResourcePickUpZone(this, 5, true);
           // GameState.GetInstance().addGameObject(zone);
            InitializeCollisionBox();
          
        }


        
        internal bool hasResource()
        {
            return resourcesInside > 0;
        }

        public override void UpdateLogic(GameTime gameTime)
        {
            heat -= gameTime.ElapsedGameTime.Milliseconds;
            if (heat <= 0)
            {
                heat += coolDownMs;
                GameState.GetInstance().DropMetal(Constants.DESIGN_MINE_METAL_DROP, this.GetPosition() + new Vector2(0, 2));
            }
        }

        
    
internal void takeResource()
{
 	throw new NotImplementedException();
}}
}
