using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;

namespace RTSgame.GameObjects
{
    /// <summary>
    /// A camera is the way to view the 3D-world and provide a view matrix.
    /// </summary>
    class Camera
    {
        
        //Where the camera is looking right now, and where it is moving
        
        private Vector3 currentLook = Constants.GetCenterOfTheWorldV3();
        private Vector3 finalLook = Constants.GetCenterOfTheWorldV3();
        //Camera offset from the object it is following
        private Vector3 followOffset;
        private Vector3 lookOffset = new Vector3(0, 1, 0);
        //Where camera is now, and where it is moving
        private Vector3 currentPosition;
        private Vector3 finalDestination;

        private Boolean instantCameraMovement = false;
        private Boolean chaseMode = false;
        private float cameraSpeed = 4.80f;
       
        private ModelObject followObject;
        private GameObject lookAtObject;
        private Matrix viewMatrix;

        

        public Camera(Vector3 startPosition)
        {
            followOffset = startPosition;
            currentPosition = startPosition;
            finalDestination = startPosition;
        }
        public Matrix GetViewMatrix(){

            return viewMatrix;


        }

        public void ToggleChaseMode()
        {
            chaseMode =! chaseMode;
            instantCameraMovement =! instantCameraMovement;
        }

        public Boolean ChaseMode()
        {
            return chaseMode;
        }

        /// <summary>
        /// What object we are following (matching position with)
        /// </summary>
        public ModelObject Following
        {
            get
            {
                return followObject;
            }
            set
            {
                followObject = value;
                currentPosition = followObject.GetPositionV3() + followOffset;
                finalDestination = followObject.GetPositionV3();
            }
        }
        /// <summary>
        /// What object we are looking at (turning to face)
        /// </summary>
        public GameObject Looking
        {
            get
            {
                return lookAtObject;
            }
            set
            {
                lookAtObject = value;
                finalLook = lookAtObject.GetPositionV3();
                currentLook = lookAtObject.GetPositionV3();
            }
        }


        internal void Update(GameTime time)
        {
            //If we have any objects to follow, move camera towards them
            
            if (followObject != null)
            {
                if (chaseMode)
                {
                    finalDestination = Vector3.Transform(new Vector3(0, 1, -4),
                        Matrix.CreateRotationY(followObject.GetFacingAngleOnXZPlane()));
                    finalDestination += followObject.GetPositionV3();
                }
                else finalDestination = followObject.GetPositionV3() + followOffset;
            }
            if (lookAtObject != null)
            {
                if (chaseMode)
                    finalLook = lookAtObject.GetPositionV3() + lookOffset;
                else finalLook = lookAtObject.GetPositionV3();                
            }
            if (instantCameraMovement)
            {
                currentLook = finalLook;
                currentPosition = finalDestination;
            }
            else
            {
                currentPosition = Vector3.Lerp(currentPosition, finalDestination, cameraSpeed * Calculations.GetFractionalSecond(time));
                currentLook = Vector3.Lerp(currentLook, finalLook, cameraSpeed * Calculations.GetFractionalSecond(time));
            }

            viewMatrix = Matrix.CreateLookAt(currentPosition,
                        currentLook, Vector3.Up);
            
        }

        internal Vector3 GetPosition()
        {
            return currentPosition;
        }
    }
}
