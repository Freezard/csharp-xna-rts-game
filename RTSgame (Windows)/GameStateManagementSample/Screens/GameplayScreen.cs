#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RTSgame.ScreenManagement;
using RTSgame.GameObjects;
using RTSgame.Utilities;
using RTSgame.GameObjects.Abstract;
using RTSgame.Levels;
using RTSgame.AI;

#endregion

namespace RTSgame.Screens
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    /// 

    class GameplayScreen : GameScreen
    {
        #region Fields
        GameState gameState = GameState.GetInstance();
        //SpriteBatch spriteBatch;
        DrawManager drawManager = DrawManager.GetInstance();


        //float aspectRatio;

        // Set the position of the model in world space, and set the rotation.
        // Set the position of the camera in world space, for our view matrix.
        //Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 1000.0f);



        //Viewports tell XNA where to draw (Splitscreen!!!)

      
        //The alpha-value (transparency) when game is paused
        float pauseAlpha;


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            CollisionInteractions.Init();

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }
        public void AddGameObjects(Camera camera)
        {
            

        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            DebugPrinter.Write("LOAD");
            
            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            DebugPrinter.currentDebugPhase = DebugPhase.Logic;

            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                gameState.updateAll(gameTime);
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(ScreenInputState input)
        {
            DebugPrinter.currentDebugPhase = DebugPhase.Input;

            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            
            
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        /// 
        public override void Draw(GameTime gameTime)
        {
            DebugPrinter.currentDebugPhase = DebugPhase.Graphics;

            // This game has a blue background. Why? Because!
            //ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
              //                                 Color.Pink, 0, 0);
            // Hey, why am I Mr. Pink?
            
                drawManager.DrawAll(gameTime);

                ScreenManager.GraphicsDevice.Viewport = Constants.StandardViewPort;
            
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
           // Graphics
            //ScreenManager.GraphicsDevice.
            base.Draw(gameTime);
        }


        #endregion
    }
}
