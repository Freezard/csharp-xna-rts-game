using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.AI;
using RTSgame.GameObjects;
using RTSgame.GameObjects.Doodads;
using RTSgame.Levels;
using RTSgame.ScreenManagement;
using RTSgame.GameObjects.Units;
using RTSgame.GameObjects.SpriteObjects;
using RTSgame.Levels.PlayableLevels;

namespace RTSgame.Utilities
{
    /// <summary>
    /// State of the current game being played.
    /// </summary>
    static class SessionHandler
    {
        static Boolean created = false;
        static Boolean multiPlayer = false;
        static ScreenManager screenManager;
        static ContentManager content;
        static GameState currentState;
        static DrawManager drawManager;
        public static Player[] players;

        public static void LoadInGame()
        {
            if(content == null)
            {
                content = screenManager.Game.Content;
                AssetBank.GetInstance().LoadContent(content);
            }
            if (!created)
            {
                drawManager = DrawManager.GetInstance();
                created = true;
                Surroundings surroundings = new Surroundings();
                currentState = GameState.GetInstance();
                currentState.SetUpGameState(surroundings);
                

                players = new Player[] {new Player("Bob", PlayerIndex.One), new Player("Rob", PlayerIndex.Two)};
                Camera[] cameras = new Camera[] { new Camera(Constants.CameraDefaultLocation), new Camera(Constants.CameraDefaultLocation)};
                if (players[0].UpdateAndGetInput().gamePadIsConnected && players[1].UpdateAndGetInput().gamePadIsConnected)
                    multiPlayer = true;
                
                for (int i = 0; i == 0 || (i ==1 && multiPlayer); i++ ) //It works, bitches!
                {
                    currentState.addCamera(cameras[i]);
                    PlayerCharacter pc;
                    if (i == 0)
                    {
                        pc = new PlayerCharacter(Constants.Player1StartingPosition, players[i]);
                    }
                    else
                    {
                        pc = new PlayerCharacter(Constants.Player2StartingPosition, players[i]);
                    }

                    players[i].SetControlledUnit(pc);
                    cameras[i].Looking = pc;
                    cameras[i].Following = pc;
                    SoundPlayer.addCamera(cameras[i]);
                    currentState.addGameObject(pc);
                    
                    
                   // currentState.addGameObject(hb);
                   // drawManager.addDrawable(hb);
                }

                drawManager.SetupDrawManager(cameras, players, 45.0f, screenManager.GraphicsDevice, multiPlayer); //TODO No magic nubers? (fov = 45.0f)

                // dont add stuff to be drawn before the drawManager has been setup
                for (int i = 0; i == 0 || (i == 1 && multiPlayer); i++)
                {
                    drawManager.addDrawable(players[i].GetControlledUnit());
                }

                LevelOne.LoadLevel (surroundings, players); //TODO Make it possible to load more than one level.
            }
        }
        /*
        private static void LoadLevel(int level, Surroundings surroundings)
        {
            String levelPath = "RTSgame.Levels.PlayableLevels";
            List<String> stuff = RTSgame.Levels.Test.LevelFinder.GetAllClasses(levelPath);
            Type t = Type.GetType(levelPath+"."+stuff[level-1]);
            MethodInfo method = t.GetMethod("LoadLevel");
            method.Invoke(null, new Object[] { surroundings });
        }*/
        public static void UnloadGame()
        {
            if (created)
            {
                created = false;
                currentState.clearState();
                DrawManager.GetInstance().clearAll();
                
            }
        }
        public static void setScreenManager(ScreenManager SM)
        {
            screenManager = SM;
        }
    }
}
