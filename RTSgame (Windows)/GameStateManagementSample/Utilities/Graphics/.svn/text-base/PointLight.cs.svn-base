using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RTSgame.Utilities.Graphics
{
    class PointLight
    {
        int id;
        Vector3 position;
        Vector4 color;
        float originalPower;
        float currentPower;
        float range;
        TimeSpan duration;
        float originalLifeTime;
        bool active;
        bool prioritized;

        public PointLight(int id)
        {
            this.id = id;
            duration = TimeSpan.Zero;
            active = false;
        }

        public void SetAttributes(Vector3 position, Vector4 color, float power, float range, float duration, bool isPrioritized = false)
        {
            this.position = position;
            this.color = color;
            this.originalPower = power;
            this.currentPower = power;
            this.range = range;
            this.duration = TimeSpan.FromSeconds(duration);
            this.originalLifeTime = (float) TimeSpan.FromSeconds(duration).TotalSeconds;
            this.prioritized = isPrioritized;
            active = true;
        }

        public void Update(GameTime gameTime)
        {
            if (active)
            {
                duration -= gameTime.ElapsedGameTime;

                if (duration <= TimeSpan.Zero)
                {
                    active = false;
                }

                if (active && duration != TimeSpan.Zero)
                {
                    currentPower = originalPower * (((float) duration.TotalSeconds) / originalLifeTime);
                }
            }

            
        }

        public int GetId()
        {
            return id;
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public Vector4 GetColor()
        {
            return color;
        }

        public float GetPower()
        {
            return currentPower;
        }

        public float GetRange()
        {
            return range;
        }

        public bool IsActive()
        {
            return active;
        }

        public bool IsPrioritized()
        {
            return prioritized;
        }
    }
}
