using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using RTSgame.AI.BasicAI.Priorities;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Projectiles;

namespace RTSgame.AI.BasicAI
{
    //This behaviour shoots projectiles at other objects of type TypeShootTarget
    class ShootBehaviour<TypeAgent, TypeShootTarget> : BasicBehaviour<TypeAgent> 
        where TypeAgent : AIControlledUnit
        where TypeShootTarget : IInteractable
    {
        private int coolDown;
        private int heat = 0;
        public ShootBehaviour(Priority<TypeAgent> priority, int cooldownMs)
            : base(priority)
        {
            this.coolDown = cooldownMs;
        }
        public override bool FulfilCriteria(TypeAgent me, GameObjects.Abstract.IInteractable otherGameObject)
        {
            //Consider checking angle to target if firing straight
            return (otherGameObject is TypeShootTarget) && heat <= 0;
        }

        public override void Update(GameTime gameTime)
        {
            heat -= gameTime.ElapsedGameTime.Milliseconds;


        }
        public override void ApplyOn(TypeAgent me, GameObjects.Abstract.IInteractable otherGameObject)
        {
            
            //Consider: shot should be fired straight ahead, and if wrong angle, turn around instead

            heat = coolDown;
            SoundPlayer.Play3DSound("shoot", me.GetPositionV3());

            //Projectile newShot = new SimpleShot<TypeShootTarget>(me, (GameObject)otherGameObject, true, new Vector3(0,0,0));
            throw new NotImplementedException();
           // GameState.GetInstance().addGameObject(newShot);
           // DrawManager.GetInstance().addDrawable(newShot);
            
        }
    }
}
