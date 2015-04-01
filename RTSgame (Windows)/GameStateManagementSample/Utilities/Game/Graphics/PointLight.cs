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
        float power;
        float range;
        TimeSpan duration;
        bool active;

        public PointLight(int id)
        {
            this.id = id;
            duration = TimeSpan.Zero;
            active = false;
        }

        public void SetAttributes(Vector3 position, Vector4 color, float power, float range, float duration)
        {
            this.position = position;
            this.color = color;
            this.power = power;
            this.range = range;
            this.duration += TimeSpan.FromSeconds(duration);
            active = true;
        }

        public bool Update(GameTime gameTime)
        {
            duration -= gameTime.ElapsedGameTime;

            if (duration <= TimeSpan.Zero)
            {
                active = false;
                return true;
            }

            return false;
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
            return power;
        }

        public float GetRange()
        {
            return range;
        }

        public bool IsActive()
        {
            return active;
        }
    }
}
