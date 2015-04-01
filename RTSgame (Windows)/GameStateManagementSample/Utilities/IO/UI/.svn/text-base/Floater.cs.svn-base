using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RTSgame.Utilities.IO.UI
{
    /// <summary>
    /// A tracker is an image UI component that is set on top of a point in 3D space, and may follow that object around
    /// </summary>
    class Floater : UIComponent
    {
        private IUITrackable trackedObject;

        
        private GameUserInterface ui;
        private Vector3 position3d;
        private Vector3 offset3d = Vector3.Zero;

        
        private bool edging = false;
        private UIComponent containedComponent;
        public Floater(UIComponent component, GameUserInterface ui, IUITrackable tracked, bool edging)
            : this(component, ui, edging)
        {
            trackedObject = tracked;
            this.visible = true;

        }
        public Floater(UIComponent component, GameUserInterface ui, Vector3 position3d, bool edging)
            : this(component, ui, edging)
        {
            this.position3d = position3d;
            this.visible = true;

        }
        public Floater(UIComponent component, GameUserInterface ui, bool edging)
            : base(Vector2.Zero)
        {
            this.ui = ui;
            this.edging = edging;
            containedComponent = component;


        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            containedComponent.Update(gameTime);
            if (trackedObject != null)
            {
                position3d = trackedObject.GetPositionV3();
                // Console.WriteLine(position);
            }
            
            Position = ui.Project(position3d + offset3d);
           
            if (edging)
            {
                //First, we must decide which edge of the window the the floater should stick to
                //We do this by dividing the entire area where position could be into four parts,
                // by drawing two lines through opposing corners, meeting in the centre
                //Example: a point above both these lines is in the top part of the area, and should stick to the top side of the window
                
                //Centre of window
                Vector2 centre = new Vector2(ui.GetWidth(), ui.GetHeight()) / 2;
                
                //Two lines, a and b, with gradients. The origin is the centre of the window
                float gradientA = centre.Y / (centre.X - ui.GetWidth());
                float gradientB = centre.Y / centre.X;
                
                //uuuu
                //TODO finish more mathematically correct

                //Sides of window (for image drawing purposes)
                float rightSide = ui.GetWidth() - containedComponent.GetWidth();
                float bottomSide = ui.GetHeight() - containedComponent.GetHeight();
                Vector2 edgePoint;
                /*if (position.X < 0)
                {
                    //Stick Left side of window
                    edgePoint.X = 0;
                    edgePoint.Y = centre.Y - (0 - centre.X) * (centre.Y - position.Y) / (position.X - centre.X);
                    position = edgePoint;
                    
                }
                else if (position.X > rightSide)
                {
                    //Stick right side of window
                    edgePoint.X = rightSide;
                    edgePoint.Y = centre.Y - (rightSide-centre.X)*(centre.Y-position.Y)/(position.X-centre.X);
                    position = edgePoint;
                    
                }*/
               // Console.WriteLine(position);
              //  Console.WriteLine(float.MaxValue);
                if (Position.Y < 0)
                {
                   // Console.WriteLine("TOP");
                    //Stick top side of window
                    edgePoint.Y = 0;


                    //X = x-(x-m)*(Y-y)/(n-y)
                    //edgePoint.X = position.X-(position.X-centre.X)*(0-position.Y)/(centre.Y-position.Y);
                    //X = m-(m-x)(n-Y)/(n-y)
                    // edgePoint.X = centre.X - (centre.X - position.X) * (centre.Y - 0) / (centre.Y - position.Y);
                    //reverse
                    edgePoint.X = centre.X - (0 - centre.Y) * (centre.X - Position.X) / (Position.Y - centre.Y);

                    Position = edgePoint;
                }
                else if (Position.Y > bottomSide)
                {
               //     Console.WriteLine("BOT");
                    //Stick bottom side of window
                    edgePoint.Y = bottomSide;

                    edgePoint.X = centre.X - (bottomSide - centre.Y) * (centre.X - Position.X) / (Position.Y - centre.Y);
                    Position = edgePoint;
                }


            }

        }
        public IUITrackable TrackedObject
        {
            get { return trackedObject; }
            set { trackedObject = value; }
        }
        public Vector3 Offset3d
        {
            get { return offset3d; }
            set { offset3d = value; }
        }
        /*public void SetTrackedObject(IUITrackable track)
        {

            visible = true;
            trackedObject = track;
        }
        public void StopTracking()
        {
            visible = false;
            trackedObject = null;
        }*/

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if(visible)
                containedComponent.Draw(spriteBatch, offset+Position);
        }

        public override float GetWidth()
        {
            return containedComponent.GetWidth();
        }

        public override float GetHeight()
        {
            return containedComponent.GetHeight();
        }
    }
}
