using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.Utilities
{
    static class Calculations
    {
        /// <summary>
        /// Pi / 8
        /// </summary>
        public const float Pi8th = (float)Math.PI / (float)8;
        /// <summary>
        /// Pi / 6
        /// </summary>
        public const float Pi6th = (float)Math.PI / (float)6;
        /// <summary>
        /// Pi / 4
        /// </summary>
        public const float Pi4th = (float)Math.PI / (float)4;
        /// <summary>
        /// Pi / 2
        /// </summary>
        public const float Pi2th = (float)Math.PI / (float)2;
        /// <summary>
        /// Pi
        /// </summary>
        public const float Pi = (float)Math.PI;
        /// <summary>
        /// Pi / 8 (double type)
        /// </summary>
        public const double double_Pi8th = Math.PI / (double)8;
        /// <summary>
        /// Pi / 6 (double type)
        /// </summary>
        public const double double_Pi6th = Math.PI / (double)6;
        /// <summary>
        /// Pi / 4 (double type)
        /// </summary>
        public const double double_Pi4th = Math.PI / (double)4;
        /// <summary>
        /// Pi / 2 (double type)
        /// </summary>
        public const double double_Pi2th = Math.PI / (double)2;
        /// <summary>
        /// Square root out of 2
        /// </summary>
        static public readonly float SqrtOf2 = (float) Math.Sqrt(0.5);

        /// <summary>
        /// Pi * 2
        /// </summary>
        public const float DoublePi = (float)Math.PI * 2;


        static public Vector2 AngleToV2(float angle, float length)
        {
            Vector2 direction = Vector2.Zero;
            direction.X = (float)Math.Cos(angle) * length;
            direction.Y = (float)Math.Sin(angle) * length;
            return direction;
        }

        /// <summary>
        /// Angle is regular radian, but the resulting Vector2
        /// has Y flipped to the usual game standard.
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static public Vector2 AngleToGameV2(float angle, float length)
        {
            Vector2 direction = Vector2.Zero;
            direction.X = (float)Math.Cos(angle) * length;
            direction.Y = -(float)Math.Sin(angle) * length;
            return direction;
        }
        //Y positive is down
        static public float V2ToAngle(Vector2 direction)
        {
            return (float)Math.Atan2(-direction.Y, direction.X);
        }

        public static Boolean approximateEqual(float c, float d)
        {
            return Math.Abs(c - d) < 0.001;
        }

        public static Boolean MatricesApproximatelyEqual(Matrix a, Matrix b)
        {
            return approximateEqual(a.M11, b.M11) &&
                approximateEqual(a.M12, b.M12) &&
                approximateEqual(a.M13, b.M13) &&
                approximateEqual(a.M14, b.M14) &&
                approximateEqual(a.M21, b.M21) &&
                approximateEqual(a.M22, b.M22) &&
                approximateEqual(a.M23, b.M23) &&
                approximateEqual(a.M24, b.M24) &&
                approximateEqual(a.M31, b.M31) &&
                approximateEqual(a.M32, b.M32) &&
                approximateEqual(a.M33, b.M33) &&
                approximateEqual(a.M34, b.M34) &&
                approximateEqual(a.M41, b.M41) &&
                approximateEqual(a.M42, b.M42) &&
                approximateEqual(a.M43, b.M43) &&
                approximateEqual(a.M44, b.M44);
        }



        /// <summary>
        /// Rotates a vector. Y positive is down.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="angle"></param>
        /// <returns></returns>

        public static Vector2 RotateCounterClockwise(Vector2 v, double angle)
        {
            //Source: http://en.wikipedia.org/wiki/Transformation_matrix
            //x' = xcosθ + ysinθ and y' = − xsinθ + ycosθ
            float CosAngle = (float)Math.Cos(angle);
            float SinAngle = (float)Math.Sin(angle);
            float x = v.X * CosAngle + v.Y * SinAngle;
            float y = (-v.X * SinAngle) + v.Y * CosAngle;
            return new Vector2(x, y);
        }

        /// <summary>
        /// Rotates a vector. Y positive is down.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector2 RotateClockwise(Vector2 v, double angle)
        {
            //x' = xcosθ - ysinθ and y' = xsinθ + ycosθ
            float CosAngle = (float)Math.Cos(angle);
            float SinAngle = (float)Math.Sin(angle);
            float x = v.X * CosAngle - v.Y * SinAngle;
            float y = (v.X * SinAngle) + v.Y * CosAngle;
            return new Vector2(x, y);
        }

        /// <summary>
        /// Restricts the argument to be within or
        /// equal to the min and max arguments.
        /// For float, there is already MathHelper.Clamp()!
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static int Clamp(int value, int min, int max)
        {
            return Math.Min(max, Math.Max(min, value));
        }

        /// <summary>
        /// Restricts the argument to be within the
        /// max and min values. The supplied error message
        /// is written to the console if clamping occured.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static int Clamp(int value, int min, int max, String errorMsg)
        {
            int v = Math.Min(max, Math.Max(min, value));
            if (v != value)
                DebugPrinter.Write(errorMsg + " , Clamped value: " + value);
            return v;
        }
        public static Vector2 PositionClosestToPositionInCollection<T>(ICollection<T> collection, Vector2 position) where T:GameObject
        {
            if (collection.Count == 0)
            {
                throw new ArgumentException("Empty collection");
            }
            float closestDist = float.PositiveInfinity;
            Vector2 closestPos = new Vector2(-10, -10);
            foreach (T t in collection)
            {
                float dist = Vector2.DistanceSquared(position, t.GetPosition());
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestPos = t.GetPosition();
                }
            }
            return closestPos;
        }
        public static T ObjectClosestToPositionInCollection<T>(ICollection<T> collection, Vector2 position) where T : GameObject
        {
            if (collection.Count == 0)
            {
                throw new ArgumentException("Empty collection");
            }
            float closestDist = float.PositiveInfinity;
            T closestT = null;
            foreach (T t in collection)
            {
                float dist = Vector2.DistanceSquared(position, t.GetPosition());
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestT = t;
                }
            }
            return closestT;
        }

        public static Point ClampPointToRectangle(Point P, ref Rectangle R)
        {
            return new Point(Clamp(P.X, R.Left, R.Right), Clamp(P.Y, R.Top, R.Bottom));
        }

        public static Point V2ToPoint(Vector2 v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        /// <summary>
        /// Converts a Point to a Vector2
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Vector2 PointToVector2(Point p)
        {
            return new Vector2((float)p.X, (float)p.Y);
        }

        /// <summary>
        /// Returns a Vector2 set to the X and Z
        /// values of the Vector3 argument.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 V3ToV2(Vector3 v)
        {
            return new Vector2(v.X, v.Z);
        }

        /// <summary>
        /// Return value is 
        /// (source.X, height, source.Y)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static Vector3 V2ToV3(Vector2 source, float height)
        {
            return new Vector3(
                source.X, height, source.Y);
        }










        /// <summary>
        /// Interpolates a value between two other values.
        /// The distance argument is how far it is from v1.
        /// Distance is intended to be between (and including) 0 and 1.
        /// </summary>
        /// <param name="h1"></param>
        /// <param name="h2"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static float Interpolate(float v1, float v2, float amount)
        {
            // Sorry skippar den här biten lite. Det är egentligen ingen
            // fara om amount är är utanför [0,1], det kanske bara
            // är ointuitivt. (Den räknar fortfarande rätt, tror jag iaf?).
            // Den räknar tillräckligt mycket fel på float för att det
            // ska uppstå situationer när amount är utanför.
            // -Joakim
            
            if (amount < 0 )
            {
                return v1;
            }
            else if (amount > 1)
            {
                return v2;
            }
            
            return v1 + amount * (v2 - v1);
        }

        /// <summary>
        /// Limits the Rectangle to be within (or equal) to the given rectangle.
        /// </summary>
        public static void ClampRectangle(ref Rectangle Target, Rectangle Limiter)
        {
            ClampRectangle(ref Target, Limiter.Top, Limiter.Bottom, Limiter.Left, Limiter.Right);
        }

        /// <summary>
        /// Limits the Rectangle to be within (or equal) the given borders.
        /// </summary>
        /// <param name="R"></param>
        /// <param name="TopBorder"></param>
        /// <param name="BottomBorder"></param>
        /// <param name="LeftBorder"></param>
        /// <param name="RightBorder"></param>
        public static void ClampRectangle(ref Rectangle R, int TopBorder, int BottomBorder, int LeftBorder, int RightBorder)
        {
            
            int rBottom = Clamp(R.Bottom, TopBorder, BottomBorder);
            int rRight = Clamp(R.Right, LeftBorder, RightBorder);

            R.Y = Clamp(R.Y, TopBorder, BottomBorder);
            R.X = Clamp(R.X, LeftBorder, RightBorder);

            R.Height = rBottom - R.Y;
            R.Width = rRight - R.X;
        }
        public static bool IsWithin3DRange(Vector3 a, Vector3 b, float range)
        {
            return Vector3.DistanceSquared(a, b) < range * range;
        }
        public static bool IsWithin2DRange(Vector2 a, Vector2 b, float range)
        {
            return Vector2.DistanceSquared(a, b) < range * range;
        }

        /// <summary>
        /// Restricts the Rectangle to be within the given rectangle,
        /// the resulting Top and Left sides may be equal,
        /// but the resulting Bottom and Right sides are garantued to
        /// be less then the Limiter argument.
        /// </summary>
        public static void RestrictRectangle(ref Rectangle Target, Rectangle Limiter)
        {
            ClampRectangle(ref Target, Limiter.Top, Limiter.Bottom - 1, Limiter.Left, Limiter.Right - 1);
        }

        /// <summary>
        /// Return the Rectangle which covers both of
        /// the supplied vectors. If both vectors are the same, 
        /// </summary>
        /// <param name="Corner1"></param>
        /// <param name="Corner2"></param>
        /// <returns></returns>
        public static Rectangle V2V2toRectangle(Vector2 Corner1, Vector2 Corner2)
        {
            Rectangle r = new Rectangle();

            if (Corner1.Y < Corner2.Y)
            {
                r.Y = (int)Corner1.Y;
                r.Height = ((int)Corner2.Y) + 1 - r.Y;
            }
            else
            {
                r.Y = (int)Corner2.Y;
                r.Height = ((int)Corner1.Y) + 1 - r.Y;
            }

            if (Corner1.X < Corner2.X)
            {
                r.X = (int)Corner1.X;
                r.Width = ((int)Corner2.X) + 1 - r.X;
            }
            else
            {
                r.X = (int)Corner2.X;
                r.Width = ((int)Corner1.X) + 1 - r.X;
            }

            return r;
        }

        public static bool RectangleCircleIntersect(Rectangle rectangle, Vector2 circleCenter, float radius)
        {
            
            // post 4:
            // http://stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection

            // Find the closest point to the circle within the rectangle
            float closestX = MathHelper.Clamp(circleCenter.X, rectangle.Left, rectangle.Right);
            float closestY = MathHelper.Clamp(circleCenter.Y, rectangle.Top, rectangle.Bottom);
            
            return Vector2.DistanceSquared(new Vector2(closestX, closestY), circleCenter) < radius*radius;
        }

        public static bool RectangleCoversV2(Rectangle rectangle, Vector2 vector)
        {
            
            if (vector.X >= rectangle.Left && vector.X <= rectangle.Right &&
                vector.Y >= rectangle.Top && vector.Y <= rectangle.Bottom)
                return true;
            return false;
        }

        /// <summary>
        /// True if Point is within or on the border of the Rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool RectangleCoversPoint(Rectangle rectangle, Point point)
        {
            if (point.X >= rectangle.Left && point.X <= rectangle.Right &&
                point.Y >= rectangle.Top && point.Y <= rectangle.Bottom)
                return true;

            return false;
        }

        /// <summary>
        /// Returns the Vector position which lies at Length
        /// length from Source towards Target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// 
        public static Vector3 OffsetTowardsTarget(Vector3 source, Vector3 target, float length)
        {
            
            if (target == source)
            {
                throw new ArgumentException("Source is same as target");
            }
            else if (length <= 0)
            {
                throw new ArgumentException("Length must be positive");
            }
            Vector3 direction = (target - source);

            return Vector3.Normalize(direction) * length;
        }

        public static Vector3 DirectionV3FixedLength(Vector3 source, Vector3 target, float length)
        {
            if (target == source)
            {
                throw new ArgumentException("Source is same as target");
            }
            else if (length <= 0)
            {
                throw new ArgumentException("Length must be positive");
            }
            Vector3 direction = (target - source);
            return Vector3.Normalize(direction) * length;
        }

        public static Vector2 OffsetTowardsTarget(Vector2 source, Vector2 target, float length)
        {
            
            if (target == source)
            {
                throw new ArgumentException("Source is same as target");
            }
            else if (length <= 0)
            {
                throw new ArgumentException("Length must be positive");
            }
            Vector2 direction = (target - source);

            return Vector2.Normalize(direction) * length;
        }

        //Spherical-Linear interpolation between two matrices using Quaternions
        public static Matrix QuaternionSlerp(Matrix a, Matrix b, float value)
        {
            Vector3 aScale, aTranslation, bScale, bTranslation;
            Quaternion aRotation, bRotation;
            a.Decompose(out aScale, out aRotation, out aTranslation);
            b.Decompose(out bScale, out bRotation, out bTranslation);
            aScale = Vector3.Lerp(aScale, bScale, value);
            aTranslation = Vector3.Lerp(aTranslation, bTranslation, value);
            aRotation = Quaternion.Slerp(aRotation, bRotation, value);
            return Matrix.CreateScale(aScale) * Matrix.CreateFromQuaternion(aRotation) * Matrix.CreateTranslation(aTranslation);
        }

        public static float GetFractionalSecond(GameTime Time)
        {
            //return Time.ElapsedGameTime.Milliseconds / 1000.0f;
            return (float) Time.ElapsedGameTime.TotalSeconds;
        }
    }
}

