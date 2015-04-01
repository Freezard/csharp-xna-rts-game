using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.AI;
using Microsoft.Xna.Framework.Content;
using RTSgame.ScreenManagement;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.GameObjects;
using RTSgame.Levels;

namespace RTSgame.Utilities
{
    /// <summary>
    /// State of the current game being played.
    /// </summary>
    static class GameSession
    {
        static Boolean created = false;
        
        public static void CreateSession(ContentManager content, GraphicsDevice graphics)
        {

            if (!created)
            {
                created = true;
                //Assetbank breaks everytime you exit the game, so we make a new one instead
                AssetBank.GetNewInstance().LoadContent(content);
               
                Surroundings surroundings = new Surroundings();
                
                GameState.GetInstance().SetUpGameState(surroundings);


                Camera camera = new Camera(Constants.CameraDefaultLocation);
                float aspectRatio = graphics.Viewport.AspectRatio;
                Camera camera2 = new Camera(Constants.CameraDefaultLocation);
                DrawManager.GetInstance().SetUpDrawManager(camera, camera2, aspectRatio, 45.0f, graphics);
                GameState.GetInstance().addCamera(camera);
                GameState.GetInstance().addCamera(camera2);
                LevelOne.LoadLevel(camera, surroundings);
            }
        }
        public static void ClearSession()
        {
            if (created)
            {
                created = false;
                GameState.GetInstance().clearState();
                DrawManager.GetInstance().clearAll();
                
            }
        }
    }
}
