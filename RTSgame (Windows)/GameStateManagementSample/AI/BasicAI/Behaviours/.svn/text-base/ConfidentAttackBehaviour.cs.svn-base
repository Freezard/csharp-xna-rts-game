using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using RTSgame.AI.BasicAI.Priorities;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Buildings;
using RTSgame.GameObjects.Projectiles;

namespace RTSgame.AI.BasicAI.Behaviours
{
    class ConfidentAttackBehaviour:BasicBehaviour<Enemy>
    {
        private int coolDown;
        private int heat = 0;
        public ConfidentAttackBehaviour(Priority<Enemy> priority, int cooldownMs)
            : base(priority)
        {
            this.coolDown = cooldownMs;
        }
        public override bool FulfilCriteria(Enemy me, IInteractable otherGameObject)
        {

            return heat <= 0 /*&& me.IsConfidentToAttack()*/ && GameState.GetInstance().AnyAIattackTargets() ;
        }
        public override void Update(GameTime gameTime)
        {
            heat -= gameTime.ElapsedGameTime.Milliseconds;


        }
        public override void ApplyOn(Enemy me, IInteractable otherGameObject)
        {
            Building target = GameState.GetInstance().GetClosestAIattackTargetReference(me.GetPosition());
            if (Vector2.DistanceSquared(target.GetPosition(), me.GetPosition()) > 5)
            {
                me.MoveToAndStop(target.GetPosition(), 0.5f);
            }
            else
            {
                SoundPlayer.PlaySound("shoot");
                Projectile newShot = new EnemyProjectile(me, target, true, new Vector3(0, 0, 0));

                GameState.GetInstance().addGameObject(newShot);
                DrawManager.GetInstance().addDrawable(newShot);
                heat = coolDown;
            }
            
        }
    }
}
