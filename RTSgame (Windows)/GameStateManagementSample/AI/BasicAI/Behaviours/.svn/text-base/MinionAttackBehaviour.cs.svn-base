using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects;
using RTSgame.AI.BasicAI.Priorities;
using RTSgame.GameObjects.Units;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.GameObjects.Abstract;
using RTSgame.GameObjects.Projectiles;

namespace RTSgame.AI.BasicAI.Behaviours
{
    class MinionAttackBehaviour:BasicBehaviour<Minion>
    {
        private int coolDown = 1000;
        private int heat = 0;
        //private ConstantPriority<Minion> constantPriority;
        public MinionAttackBehaviour(Priority<Minion> priority)
            : base(priority)
        {
 
        }

        public override bool FulfilCriteria(Minion me, GameObjects.Abstract.IInteractable otherGameObject)
        {
            if(otherGameObject is PlayerOwnedObject){
                return !(((PlayerOwnedObject)otherGameObject).Owner == me.Owner) && heat <= 0;
            }else{
                return false;
            }
            
        }

        public override void Update(GameTime gameTime)
        {
            heat -= gameTime.ElapsedGameTime.Milliseconds;


        }
        public override void ApplyOn(Minion me, GameObjects.Abstract.IInteractable otherGameObject)
        {
            
            //Consider: shot should be fired straight ahead, and if wrong angle, turn around instead

            heat = coolDown;
            SoundPlayer.Play3DSound("shoot", me.GetPositionV3());

            Projectile newShot = new PlayerCodedProjectile(
                            me,
                            me.Owner,
                            (GameObject)otherGameObject,
                            true,
                            Vector3.Zero,
                            6);
           
            GameState.GetInstance().addGameObject(newShot);
           //DrawManager.GetInstance().addDrawable(newShot);
            
        }
    }
}
