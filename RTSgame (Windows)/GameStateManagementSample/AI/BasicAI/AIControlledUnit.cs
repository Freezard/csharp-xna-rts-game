using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.AI;
using RTSgame.AI.BasicAI;
using RTSgame.GameObjects.Components;


namespace RTSgame.GameObjects.Abstract
{
    /// <summary>
    /// Class that contains everything that all Units
    /// (Enemy, Minion) have in common.
    /// And implements all the behaviour that all Units must have.
    /// Most importantly, contains methods used by AI-managers to control them.
    /// </summary>
    abstract class AIControlledUnit : Unit, ILogic, IIntelligent, IMovable
    {
        protected Vector2 velocity = Vector2.Zero;
        protected float targetAngle = 0;
        protected float targetSpeed = 0;
        //private float currentAngle = 0;
        private Vector2 shove;
        
        
        protected float maxSpeed = 10;
        protected float sightRange = 5;
        protected float shoveDistanceSquared = 0.3f*0.3f;


        //protected float turnSpeed = 5000.0f, accelerationRate = 1000.0f; 
        

        public AIControlledUnit(Vector2 pos, ModelComponent modelComp, Player owner, int hitpoints)
            : base(pos, modelComp, owner, hitpoints)
        {
            
        }

        public override void HitPointsZero()
        {
            this.RemoveFromGame();
        }
        public void SetTargetAngle(float angle)
        {
            targetAngle = angle;
        }
        public void SetTargetSpeedScale(float scale)
        {
            targetSpeed = scale * maxSpeed;
            
        }
        //Set direction to move away from a position
        public void MoveAwayFrom(Vector2 scaryPos, float speedScale)
        {
            float dx = GetPosition().X - scaryPos.X;
            float dy = GetPosition().Y - scaryPos.Y;
            targetAngle = (float)(Math.Atan2(dy, dx));
            targetSpeed = maxSpeed * speedScale;
        }
        //Shove this unit to avoid clumping
        public void ShoveAwayFrom(Vector2 obstaclePos, float shoveFactor)
        {
            float dx = GetPosition().X - obstaclePos.X;
            float dy = GetPosition().Y - obstaclePos.Y;
            Vector2 vec = new Vector2(dx, dy);
            // do not divide by zero
            shove = shoveFactor * vec / (vec.Length() + 0.01f);
        }
        //Set direction to move to a position
       /* public void MoveToAndBrake(Vector2 nicePos, float speedScale)
        {
            float dx = nicePos.X - GetPosition().X;
            float dy = nicePos.Y - GetPosition().Y;
            targetAngle = (float)(Math.Atan2(dy, dx));
            float brakeFactor = 1.0f;
            int brakeRange = 1;
            if(Calculations.IsWithin2DRange(nicePos, GetPosition(), brakeRange)){

                brakeFactor = Vector2.Distance(GetPosition(), nicePos)/brakeRange;

                }

                
                
                
            targetSpeed = maxSpeed * speedScale * brakeFactor;

                
            

        }*/
        public void MoveToAndStop(Vector2 nicePos, float speedScale){
            float dx = nicePos.X - GetPosition().X;
            float dy = nicePos.Y - GetPosition().Y;
            targetAngle = (float)(Math.Atan2(dy, dx));
            targetSpeed = maxSpeed * speedScale;
            if(this is Minion)
                Console.WriteLine(targetSpeed);
            if(Calculations.IsWithin2DRange(nicePos, this.GetPosition(), targetSpeed)){
                targetSpeed = Vector2.Distance(nicePos, this.GetPosition());
            }
        }
        public void MoveTo(Vector2 nicePos, float speedScale)
        {
            float dx = nicePos.X - GetPosition().X;
            float dy = nicePos.Y - GetPosition().Y;
            targetAngle = (float)(Math.Atan2(dy, dx));
            targetSpeed = maxSpeed * speedScale;
        }
        public abstract void AIUpdatePreInteractions(GameTime gameTime);

        public abstract void AIUpdatePostInteractions(GameTime gameTime);

        public abstract void AIInteract(IInteractable gameObject);

        public abstract void UpdateLogic(GameTime gameTime);


        //Move the unit according to its AI wishes
        public virtual void UpdateDestination(GameTime gameTime)
        {
            
            velocity = Calculations.AngleToV2(targetAngle, targetSpeed);
            if (velocity != Vector2.Zero)
            {

                MoveDestination(velocity * gameTime.ElapsedGameTime.Milliseconds / 1000);

                float targetAng = (float)(Math.Atan2(-velocity.Y, velocity.X) +  Math.PI / 2);
                while (Math.Abs(angleAround.Y - targetAng) > Math.PI + 0.1)
                {
                    if (targetAng > angleAround.Y)
                    {
                        targetAng -= 2 * (float)Math.PI;
                    }
                    else
                    {
                        targetAng += 2 * (float)Math.PI;
                    }

                }
                angleAround.Y = Calculations.Interpolate(angleAround.Y, targetAng, 0.12f);

            }


            //Old movement code
            /*
            float secondsPassed = (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
            float accelerationAmount = accelerationRate * secondsPassed;
            float turnAmount = turnSpeed * secondsPassed;
            
            float currentSpeed = velocity.Length();
            //Modify current speed to match target speed, but gradually
            if (Math.Abs(currentSpeed - targetSpeed) < accelerationAmount)
            {
                currentSpeed = targetSpeed;
            }
            else if (currentSpeed < targetSpeed)
            {
                currentSpeed += accelerationAmount;
            }
            else
            {
                currentSpeed -= accelerationAmount;
            }
            
            //Do the same for angle...

            //First we need to find what targetangle should be (since the angle is cyclic)
            //We want to find the target angle which is closest, i.e. where the difference between the two are less than 180*.

            while (Math.Abs(currentAngle - targetAngle) > Math.PI+0.1)
            {

                if (targetAngle > currentAngle)
                {
                    targetAngle -= 2 * (float)Math.PI;
                }
                else
                {
                    targetAngle += 2 * (float)Math.PI;
                }

            }

            if (Math.Abs(currentAngle - targetAngle) < turnAmount)
            {
                currentAngle = targetAngle;
            }
            else if (targetAngle > currentAngle)
            {
                currentAngle += turnAmount;
            }
            else
            {
                currentAngle -= turnAmount;
            }

            velocity = Calculations.AngleToV2(currentAngle, currentSpeed);



            if (velocity != Vector2.Zero)
            {

                MoveDestination(velocity * gameTime.ElapsedGameTime.Milliseconds / 1000);


                angleAround.Y = Calculations.V2ToAngle(velocity) + Calculations.Pi2th;


            }

            MoveDestination(shove);
            shove = Vector2.Zero;
             */
        }

        internal void Stop()
        {
          //  DateTime hammerTime = System.DateTime.Now;
            targetSpeed = 0;
        }

        public abstract float GetMaxAIInteractionRange();

    }
}
