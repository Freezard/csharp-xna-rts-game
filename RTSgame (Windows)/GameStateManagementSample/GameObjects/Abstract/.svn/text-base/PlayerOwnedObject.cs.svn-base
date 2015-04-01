using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Components;
using RTSgame.Utilities;

namespace RTSgame.GameObjects.Abstract
{
    abstract class PlayerOwnedObject:ModelObject
    {
        protected float hitPoints = 100;
        protected float maxHitPoints;
        private Player owner;

        
         public PlayerOwnedObject(Vector2 newPosition, ModelComponent modelComp, Player owner, int maxHitPoints)
            : base(newPosition, modelComp)
        {
            this.maxHitPoints = maxHitPoints;
            this.hitPoints = maxHitPoints;
            this.owner = owner;
        }
         public void Heal(float amount)
         {
             if (amount < 0)
             {
                 throw new ArgumentException("Cannot heal with negative value");
             }
             //Debug.Write(this + "is healed");
             hitPoints += amount;
             if (hitPoints > maxHitPoints)
             {
                 hitPoints = maxHitPoints;

             }
         }
         public virtual void Damage(float amount)
         {
             if (amount < 0)
             {
                 throw new ArgumentException("Cannot damage with negative value");
             }
             
             hitPoints -= amount;
             if (hitPoints <= 0 && !toBeRemoved)
             {
                 HitPointsZero();
             }
         }
         public Player Owner
         {
             get { return owner; }
         }
         public bool IsAtFullHealth()
         {
             return (hitPoints == maxHitPoints);
         }
         public abstract void HitPointsZero();

         public float GetHealthScaled()
         {
             return hitPoints / maxHitPoints;
         }
    }
}
