using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RTSgame.Utilities
{
    public struct FloatRectangle
    {
        public float Top;
        public float Bottom;
        public float Left;
        public float Right;

        public FloatRectangle(float Top, float Bottom, float Left, float Right)
        {
            this.Top = Top;
            this.Bottom = Bottom;
            this.Left = Left;
            this.Right = Right;
        }

        public void SetFloatRectangle(float Top, float Left, float Width, float Height)
        {
            this.Top = Top;
            this.Bottom = Top + Height;
            this.Left = Left;
            this.Right = Left + Height;
        }

        public FloatRectangle(Vector2 Corner1, Vector2 Corner2)
        {
            if (Corner1.Y < Corner2.Y)
                Top = Corner1.Y;
            else
                Top = Corner2.Y;

            if (Corner1.Y >= Corner2.Y)
                Bottom = Corner1.Y;
            else
                Bottom = Corner2.Y;

            if (Corner1.X < Corner2.X)
                Left = Corner1.X;
            else
                Left = Corner2.X;

            if (Corner1.X >= Corner2.X)
                Right = Corner1.X;
            else
                Right = Corner2.X;
        }

        public void Clamp(float TopBorder, float BottomBorder, float LeftBorder, float RightBorder)
        {
            Top = MathHelper.Clamp(Top, TopBorder, BottomBorder);
            Bottom = MathHelper.Clamp(Bottom, TopBorder, BottomBorder);
            Left = MathHelper.Clamp(Left, LeftBorder, RightBorder);
            Right = MathHelper.Clamp(Right, LeftBorder, RightBorder);
        }

        public void Inflate(float Value)
        {
            Top -= Value;
            Bottom += Value;
            Left -= Value;
            Right += Value;
        }

        public bool RectangleIntersect(FloatRectangle other)
        {
            if (IntervalIntersect(Top, Bottom, other.Top, other.Bottom) &&
                IntervalIntersect(Left, Right, other.Left, other.Right))
                return true;
            return false;
        }

        private bool IntervalIntersect(float interval1start, float interval1end,
            float interval2start, float interval2end)
        {
            float interval1Width = interval1start - interval1end;
            float interval2Width = interval2start - interval2end;
            float x = interval1start - interval2start;

            if (x >= 0 && x < interval2Width)
                return true;
            if (x < 0 && x > -interval1Width)
                return true;
            return false;
        }

        public bool CircleIntersect(Vector2 center, float radius)
        {
            //check if center is entirely contained in rectangle
            if (center.X >= Left && center.X <= Right &&
                center.Y >= Top && center.Y <= Bottom)
                return true;
            
            float radiusSquared = radius * radius;

            if (Vector2.DistanceSquared(new Vector2(Right, Top), center) < radiusSquared)
            {
                return true;
            }

            if (Vector2.DistanceSquared(new Vector2(Right, Bottom), center) < radiusSquared)
            {
                return true;
            }

            if (Vector2.DistanceSquared(new Vector2(Left, Bottom), center) < radiusSquared)
            {
                return true;
            }

            if (Vector2.DistanceSquared(new Vector2(Left, Top), center) < radiusSquared)
            {
                return true;
            }

            return false;
        }
    }
}
