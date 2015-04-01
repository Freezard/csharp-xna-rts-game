using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.GameObjects.Components;

namespace RTSgame.GameObjects.Units
{
    class TestUnit:Unit
    {
        public TestUnit(Vector2 Position, String texturen )
            : base(Position, new ModelComponent("MechRobotGå"), null ,100)
        {

          //  texture = AssetBank.GetInstance().GetTexture(texturen);
        

            scale = 0.18f;
            
            
          //  animationPlayer.StartClip(getClip("Take 001"));
            InitializeCollisionBox();
            
        }
        public override void HitPointsZero()
        {
            //DO NOTHING
        }

        public override float GetMaxInteractionRange()
        {
            return 15;
        }
    }
}
