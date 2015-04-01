using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects;
using RTSgame.AI.Abstract;
using RTSgame.Utilities;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.AI
{
    class BehaviourB : Behaviour<Minion>
    {
        GameObject go;
        public void SetParametres(GameObject go)
        {
            this.go = go;
        }
        public override void execute(GameTime gameTime, Minion gameObject)
        {
            
            parent.finished();
            /*
            if (Calculations.Length2DSquared(go.GetPosition(), gameObject.GetPosition()) < 100 * 100)
            {
                parent.finished();

            }
            else
            {
                gameObject.MoveTo(go.GetPosition(), 100);
            } */

        }
    }
}
