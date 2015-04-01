using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace RTSgame.Utilities
{

    public class Array2D<T>
    {

        protected int XDimension;
        protected int YDimension;

        public T[,] Data;

        /// <summary>
        /// Allows you to change the data stored in this location.
        /// First argument = data value at a single location.
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public delegate void DelegateChangeValue(ref T data);

        /// <summary>
        /// Allows you to change the data stored in this location.
        /// First argument = data value at a single location.
        /// Second argument = This Location.
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public delegate void DelegateChangeValueGetLocation(ref T data, Point p);

        public delegate T DelSetValueGetLocation(Point p);

        //private String errorMsg = "Error: Requested Location outside Map Boundaries. Value clamped";

        /// <summary>
        /// DO NOT USE.
        /// This is reserved for subclasses, but has to be public.
        /// </summary>
        protected Array2D()
        {

        }

        public Array2D(int dimension)
        {
            InitData(dimension, dimension);
        }

        public Array2D(int xDimension, int yDimension)
        {
            InitData(xDimension, yDimension);
        }

        protected void InitData(int xDimension, int yDimension)
        {
            XDimension = xDimension;
            YDimension = yDimension;

            Data = new T[xDimension, yDimension];
        }

        public T GetValue(int x, int y)
        {
            //TODO: add clamp back in
            //const String error = "WARNING: Array2D 0001 , tried to get value out of bounds";
            //x = Calculations.Clamp(x, 0, XDimension - 1);
            //y = Calculations.Clamp(y, 0, YDimension - 1);
            return Data[x,y];
        }

        public T GetValue(Point l)
        {
            //TODO: add clamp back in
            //const String error = "WARNING: Array2D 0002 , tried to get value out of bounds";
            //l.X = Calculations.Clamp(l.X, 0, XDimension - 1);
            //l.Y = Calculations.Clamp(l.Y, 0, YDimension - 1);
            return Data[l.X,l.Y];
        }
        
        public void SetXY(int x, int y, T value)
        {
            Data[x,y] = value;
        }

        public void SetValue(Point l, T value)
        {
            Data[l.X,l.Y] = value;
        }

        public void ChangeValue(DelegateChangeValue Del, int x, int y)
        {
            Del.Invoke(ref Data[x,y]);
        }

        public void ChangeValue(DelegateChangeValue Del, Point l)
        {
            Del.Invoke(ref Data[l.X,l.Y]);
        }

        public int GetIndex(int x, int y)
        {
            //TODO: add clamp back in
            //const String error = "WARNING: Array2D 0003 , tried to get value out of bounds";
            //x = Calculations.Clamp(x, 0, XDimension, error);
            //y = Calculations.Clamp(y, 0, YDimension, error);
            return x * YDimension + y;
        }

        public Array2D<T> GetThis()
        {
            return this;
        }

        public Type GetGenericType()
        {
            return typeof(T);
        }

        public int GetSize()
        {
            return XDimension * YDimension;
        }

        public void SetEveryPoint(DelSetValueGetLocation Del)
        {
            for (int x = 0; x < XDimension; x++)
            {
                for (int y = 0; y < YDimension; y++)
                {
                    Data[x,y] = Del.Invoke(new Point(x, y));
                }
            }
        }

        /// <summary>
        /// Allows you to change every Value in the Array2D.
        /// First parameter of the argument function is the value that can be changed.
        /// </summary>
        /// <param name="myDel"></param>
        public void ChangeEveryPoint(DelegateChangeValue Del)
        {
            for (int x = 0; x < XDimension; x++)
            {
                for (int y = 0; y < YDimension; y++)
                {
                    Del.Invoke(ref Data[x,y]);
                }
            }
        }

        /// <summary>
        /// Allows you to change every Value in the Array2D.
        /// First parameter of the argument function is the value that can be changed.
        /// Second parameter of the argument function is the location in the Array2D of this value.
        /// </summary>
        /// <param name="myDel"></param>
        public void ChangeEveryPoint(DelegateChangeValueGetLocation Del)
        {
            for (int x = 0; x < XDimension; x++)
            {
                for (int y = 0; y < YDimension; y++)
                {
                    Del.Invoke(ref Data[x,y], new Point(x, y));
                }
            }
        }

        /// <summary>
        /// Allows you to change every Value in the Array2D.
        /// XSteps and YSteps sets how large steps you should take.
        /// </summary>
        /// <param name="Del"></param>
        /// <param name="XSteps"></param>
        /// <param name="YSteps"></param>
        public void ChangeEveryPointWithStepLength(DelegateChangeValue Del, int XSteps, int YSteps)
        {
            for (int x = 0; x < XDimension; x += XSteps)
            {
                for (int y = 0; y < YDimension; y += YSteps)
                {
                    Del.Invoke(ref Data[x,y]);
                }
            }
        }

        /// <summary>
        /// Allows you to change every Value in the Array2D, except those in the outer border.
        /// First parameter of the argument function is the value that can be changed.
        /// Second parameter of the argument function is the location in the Array2D of this value.
        /// </summary>
        /// <param name="myDel"></param>
        [Obsolete]
        public void ChangeEveryInnerPoint(DelegateChangeValueGetLocation Del)
        {
            for (int x = 1; x < XDimension - 1; x++)
            {
                for (int y = 1; y < YDimension - 1; y++)
                {
                    Del.Invoke(ref Data[x,y], new Point(x, y));
                }
            }
        }

        /// <summary>
        /// Allows you to change every Value in the Array2D, except those in the outer border.
        /// First parameter of the argument function is the value that can be changed.
        /// </summary>
        /// <param name="myDel"></param>
        public void ChangeEveryInnerPoint(DelegateChangeValue Del)
        {
            for (int x = 1; x < XDimension - 1; x++)
            {
                for (int y = 1; y < YDimension - 1; y++)
                {
                    Del.Invoke(ref Data[x,y]);
                }
            }
        }

        /// <summary>
        /// Applies the function to all the values, starting from the
        /// start location and reaching out to the right and down
        /// as far as the size argument.
        /// size = 1 means 1 value  is  altered,
        /// size = 3 means 9 values are altered.
        /// </summary>
        /// <param name="Del"></param>
        /// <param name="start">Start Location</param>
        /// <param name="size">Dimension of the box</param>
        public void ChangeValuesViaCornerBox(DelegateChangeValue Del, Point start, int size)
        {
            int xLeft  = Calculations.Clamp(start.X,        0, XDimension - 1);
            int xRight = Calculations.Clamp(start.X + size, 0, XDimension - 1);
            int yUp    = Calculations.Clamp(start.Y,        0, YDimension - 1);
            int yDown  = Calculations.Clamp(start.Y + size, 0, YDimension - 1);

            for (int x = start.X; x < start.X + size; x++)
            {
                for (int y = start.Y; y < start.Y + size; y++)
                {
                    Del.Invoke(ref Data[x,y]);
                }
            }
        }

        /// <summary>
        /// Applies the function to all the values, starting from the
        /// start location and reaching out in all directions
        /// as far as the size argument. Is Inclusive.
        /// </summary>
        /// <param name="Del"></param>
        /// <param name="start">Start Vector</param>
        /// <param name="reach">Dimension of the box</param>
        public void ChangeValuesViaCenteredBox(DelegateChangeValue Del, Vector2 start, float reach)
        {
            int xLeft = Calculations.Clamp((int)(start.X - reach), 0, XDimension - 1);
            int xRight = Calculations.Clamp((int)(start.X + reach), 0, XDimension - 1);
            int yTop = Calculations.Clamp((int)(start.Y - reach), 0, YDimension - 1);
            int yBottom = Calculations.Clamp((int)(start.Y + reach), 0, YDimension - 1);

            for (int x = xLeft; x <= xRight; x++ )
            {
                for (int y = yTop; y <= yBottom; y++)
                {
                    Del.Invoke(ref Data[x, y]);
                }
            }
        }

        /// <summary>
        /// Applies the function to all the values, starting from the
        /// start location and reaching out in all directions
        /// as far as the size argument. Is Inclusive.
        /// </summary>
        /// <param name="Del"></param>
        /// <param name="start">Start Vector</param>
        /// <param name="reach">Dimension of the box</param>
        public void ChangeValuesViaCenteredBox(DelegateChangeValueGetLocation Del, Vector2 start, float reach)
        {
            int xLeft = Calculations.Clamp((int) (start.X - reach), 0, XDimension - 1);
            int xRight = Calculations.Clamp((int)(start.X + reach), 0, XDimension - 1);
            int yTop = Calculations.Clamp((int)(start.Y - reach), 0, YDimension - 1);
            int yBottom = Calculations.Clamp((int)(start.Y + reach), 0, YDimension - 1);

            for (int x = xLeft; x <= xRight; x++)
            {
                for (int y = yTop; y <= yBottom; y++)
                {
                    Del.Invoke(ref Data[x, y], new Point(x,y));
                }
            }
        }

        /// <summary>
        /// Applies the function to all the values, inside a box
        /// defined by upperLeft and lowerRight as corners.
        /// Corners are included.
        /// </summary>
        /// <param name="Del"></param>
        /// <param name="upperLeft"></param>
        /// <param name="lowerRight"></param>
        public void ChangeValuesViaSpecifiedBox(DelegateChangeValue Del, Point upperLeft, Point lowerRight)
        {
            int xLeft  = Calculations.Clamp(upperLeft.X,  0, XDimension - 1);
            int xRight = Calculations.Clamp(lowerRight.X, 0, XDimension - 1);
            int yUp    = Calculations.Clamp(upperLeft.Y,  0, YDimension - 1);
            int yDown  = Calculations.Clamp(lowerRight.Y, 0, YDimension - 1);

            for (int x = upperLeft.X; x < lowerRight.X; x++)
            {
                for (int y = upperLeft.Y; y < lowerRight.Y; y++)
                {
                    Del.Invoke(ref Data[x,y]);
                }
            }
        }

        /// <summary>
        /// Applies the function to all the values, inside a box
        /// centered by center argument. Stretches out in all directions
        /// as far as reach argument. Box form.
        /// reach = 1 means 1 value  is  altered.
        /// reach = 2 means 9 values are altered.
        /// </summary>
        /// <param name="Del">Method to be applied to all values</param>
        /// <param name="center">Center Location</param>
        /// <param name="reach">Radius of the box</param>
        public void ChangeValuesViaCenteredBox(DelegateChangeValue Del, Point center, int reach)
        {
            int xLeft  = Calculations.Clamp(center.X - reach, 0, XDimension - 1);
            int xRight = Calculations.Clamp(center.X + reach, 0, XDimension - 1);
            int yUp    = Calculations.Clamp(center.Y - reach, 0, YDimension - 1);
            int yDown  = Calculations.Clamp(center.Y + reach, 0, YDimension - 1);

            for (int x = xLeft; x <= xRight; x++)
            {
                for (int y = yUp; y < yDown; y++)
                {
                    Del.Invoke(ref Data[x,y]);
                }
            }
        }

        /// <summary>
        /// Applies the function to all the values, inside a box
        /// centered by center argument. Stretches out in all directions
        /// as far as reach argument. Diamond form.
        /// reach = 1 means 1 value  is  altered.
        /// reach = 2 means 5 values are altered.
        /// reach = 3 means 13 values are altered.
        /// </summary>
        /// <param name="Del">Method to be applied to all values</param>
        /// <param name="center">Center Location</param>
        /// <param name="reach">Radius of the cross</param>
        public void ChangeValuesViaCenteredDiamond(DelegateChangeValue Del, Point center, int reach)
        {
            // reachOut = how many steps out from the center square we should take.
            int reachOut = Math.Max(0, reach - 1);

            int xLeft  = Calculations.Clamp(center.X - reachOut, 0, XDimension - 1);
            int xRight = Calculations.Clamp(center.X + reachOut, 0, XDimension - 1);
            int yUp    = Calculations.Clamp(center.Y - reachOut, 0, YDimension - 1);
            int yDown  = Calculations.Clamp(center.Y + reachOut, 0, YDimension - 1);

            int xBoxCoordinates = -1 * reachOut;
            for (int x = xLeft; x <= xRight; x++)
            {
                int yBoxCoordinates = -1 * reachOut;
                for (int y = yUp; y <= yDown; y++)
                {
                    //This creates a diagonal line.
                    if (Math.Abs(xBoxCoordinates) + Math.Abs(yBoxCoordinates) <= reachOut)
                        Del.Invoke(ref Data[x,y]);

                    yBoxCoordinates++;
                }
                xBoxCoordinates++;
            }
        }

        /// <summary>
        /// Applies the function to all the values along the border
        /// of the entire datastructure.
        /// </summary>
        /// <param name="Del">Method to be applied to all values</param>
        public void ChangeBorderValues(DelegateChangeValue Del)
        {
            int lineY = 0;
            for (int x = 0; x < XDimension; x++)
            {
                Del.Invoke(ref Data[x, lineY]);
            }

            lineY = YDimension - 1;
            for (int x = 0; x < XDimension; x++)
            {
                Del.Invoke(ref Data[x, lineY]);
            }

            int lineX = 0;
            for (int y = 1; y < YDimension - 1; y++)
            {
                Del.Invoke(ref Data[lineX, y]);
            }

            lineX = XDimension - 1;
            for (int y = 1; y < YDimension - 1; y++)
            {
                Del.Invoke(ref Data[lineX, y]);
            }
        }


        /// <summary>
        /// Applies the function to all the values along the border
        /// of the entire datastructure.
        /// </summary>
        /// <param name="Del">Method to be applied to all values</param>
        public void ChangeBorderValues(DelegateChangeValue Del, int Thickness)
        {
            //int thickness = Thickness - 1;
            int outerTop = 0;
            int outerBottom = YDimension - 1;
            int innerTop = Thickness;
            int innerBottom = outerBottom - Thickness;

            int outerLeft = 0;
            int outerRight = XDimension - 1;
            int innerLeft = Thickness;
            int innerRight = outerRight - Thickness;
            
            // Left border
            ChangeValuesViaSpecifiedBox(Del, new Point(outerLeft, outerTop), new Point(innerLeft, outerBottom));

            // Right border
            ChangeValuesViaSpecifiedBox(Del, new Point(innerRight, outerTop), new Point(outerRight, outerBottom));
            
            // Top border, but not the sides
            ChangeValuesViaSpecifiedBox(Del, new Point(innerLeft, outerTop), new Point(innerRight, innerTop));

            // Bottom border, but not the sides
            ChangeValuesViaSpecifiedBox(Del, new Point(innerLeft, innerBottom), new Point(innerRight, outerBottom));
        }

        /// <summary>
        /// Applies the function to all the values, inside a circle with
        /// the specified radius.
        /// </summary>
        /// <param name="Del">Method to be applied to all values</param>
        /// <param name="center">Center Location</param>
        /// <param name="reach">Radius of the cross</param>
        /// <param name="adjusted">If this is true, then the distance comparsion is done
        /// using the center of each square in the grid, instead of the upper left corner. Default is true.</param>
        public void ChangeValuesViaCenteredCircle(DelegateChangeValue Del, Vector2 center, float radius, bool adjusted = true)
        {
            // Get a box larger then the circle
            int xLeft = Calculations.Clamp((int)(center.X - radius), 0, XDimension - 1);
            int xRight = Calculations.Clamp((int)(center.X + radius + 1), 0, XDimension - 1);
            int yUp = Calculations.Clamp((int)(center.Y - radius), 0, YDimension - 1);
            int yDown = Calculations.Clamp((int)(center.Y + radius + 1), 0, YDimension - 1);

            Vector2 adjustment = new Vector2(0, 0);

            // if adjusted, match the center of the squares.
            if (adjusted)
                adjustment = new Vector2(0.5f, 0.5f);

            // Iterate through all these points
            for (int x = xLeft; x <= xRight; x++)
            {
                for (int y = yUp; y <= yDown; y++)
                {
                    //If the point is in range
                    if (Vector2.Distance(center, Calculations.PointToVector2(
                        new Point(x, y)) + adjustment) <= radius)
                    {
                        Del.Invoke(ref Data[x, y]);
                    }
                }
            }
        }

        /// <summary>
        /// Applies the function to all the values, inside a circle with
        /// the specified radius.
        /// </summary>
        /// <param name="Del">Method to be applied to all values</param>
        /// <param name="center">Center Location</param>
        /// <param name="reach">Radius of the cross</param>
        /// <param name="adjusted">If this is true, then the distance comparsion is done
        /// using the center of each square in the grid, instead of the upper left corner. Default is true.</param>
        public void ChangeValuesViaCenteredCircle(DelegateChangeValueGetLocation Del, Vector2 center, float radius, bool adjusted = true)
        {
            // Get a box larger then the circle
            int xLeft = Calculations.Clamp((int)(center.X - radius), 0, XDimension - 1);
            int xRight = Calculations.Clamp((int)(center.X + radius + 1), 0, XDimension - 1);
            int yUp = Calculations.Clamp((int)(center.Y - radius), 0, YDimension - 1);
            int yDown = Calculations.Clamp((int)(center.Y + radius + 1), 0, YDimension - 1);

            Vector2 adjustment = new Vector2(0, 0);

            // if adjusted, match the center of the squares.
            if (adjusted)
                adjustment = new Vector2(0.5f, 0.5f);

            // Iterate through all these points
            for (int x = xLeft; x <= xRight; x++)
            {
                for (int y = yUp; y <= yDown; y++)
                {
                    //If the point is in range
                    if (Vector2.Distance(center, Calculations.PointToVector2(
                        new Point(x, y)) + adjustment) <= radius)
                    {
                        Del.Invoke(ref Data[x, y], new Point(x, y));
                    }
                }
            }
        }

        /// <summary>
        /// Clamps the point to be within the data span.
        /// Does not raise any error for doing so.
        /// </summary>
        /// <param name="P"></param>
        /// <returns></returns>
        public void ClampPoint(ref Point P)
        {
            P.X = Calculations.Clamp(P.X, 0, XDimension - 1);
            P.Y = Calculations.Clamp(P.Y, 0, YDimension - 1);
        }

        /// <summary>
        /// Do not use this. It exists only because I implemented the heightmaps
        /// before I had done BlockMap.
        /// </summary>
        public Vector2 AdjustVectorToBlockMapVector(Vector2 v, int scale)
        {
            //float x = v.X / (float)scale;
            //float y = v.Y / (float)scale;
            return new Vector2(v.X, v.Y) / scale;
        }

        /// <summary>
        /// Do not use this. It exists only because I implemented the heightmaps
        /// before I had done BlockMap.
        /// </summary>
        public float AdjustLengthToBlockMapLength(float x, int scale)
        {
            return x / (float)scale;
        }

    } //Class
}


