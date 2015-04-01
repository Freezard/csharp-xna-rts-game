#region File Description
//-----------------------------------------------------------------------------
// InputState.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace RTSgame.Utilities
{
    /// <summary>
    /// Helper for reading input from keyboard and gamepad.
    /// </summary>
    public class InputManager
    {
        #region Fields

        public KeyboardState CurrentKeyState;
        public GamePadState CurrentPadState;
       
        public KeyboardState LastKeyState;
        public GamePadState LastPadState;

        public bool gamePadIsConnected;
        public bool gamePadWasConnected;

        public PlayerIndex player;

        #endregion
        public InputManager(PlayerIndex player){
            this.player = player;
            
        }
        #region Properties

        public Vector2 RightTSVector
        {
            get
            {
                return new Vector2(RightTSX, RightTSY);

            }

        }
        private float RightTSY
        {
            get
            {
                if (CurrentKeyState.IsKeyDown(Keys.Up))
                {
                    return -1.0f;
                }
                else if (CurrentKeyState.IsKeyDown(Keys.Down))
                {
                    return 1.0f;
                }
                else
                {
                    return -(CurrentPadState.ThumbSticks.Right.Y);
                }
            }
        }

        private float RightTSX
        {
            get
            {
                if (CurrentKeyState.IsKeyDown(Keys.Left))
                {
                    return -1.0f;
                }
                else if (CurrentKeyState.IsKeyDown(Keys.Right))
                {
                    return 1.0f;
                }
                else
                {
                    return CurrentPadState.ThumbSticks.Right.X;
                }
            }
        }

        public Vector2 LeftTSVector
        {
            get
            {
                return new Vector2(LeftTSX, LeftTSY);

            }

        }
        private float LeftTSY
        {
            get
            {
                if (CurrentKeyState.IsKeyDown(Keys.Up))
                {
                    return -1.0f;
                }
                else if (CurrentKeyState.IsKeyDown(Keys.Down))
                {
                    return 1.0f;
                }
                else
                {
                    return -(CurrentPadState.ThumbSticks.Left.Y);
                }
            }
        }

        private float LeftTSX
        {
            get
            {
                if (CurrentKeyState.IsKeyDown(Keys.Left))
                {
                    return -1.0f;
                }
                else if (CurrentKeyState.IsKeyDown(Keys.Right))
                {
                    return 1.0f;
                }
                else
                {
                    return CurrentPadState.ThumbSticks.Left.X;
                }
            }
        }

        public Vector2 DpadVector
        {
            get
            {
                Vector2 vec = new Vector2(DpadX, DpadY);
                return vec;
            }
            
        }
        
        private int DpadX
        {
            get
            {
                if (CurrentKeyState.IsKeyDown(Keys.Left) ||
                    CurrentPadState.IsButtonDown(Buttons.DPadLeft))
                {
                    return -1;
                }
                else if (CurrentKeyState.IsKeyDown(Keys.Right) ||
                    CurrentPadState.IsButtonDown(Buttons.DPadRight))
                {
                    return 1;
                }
                else
                    return 0;
                
            }
        }
        private int DpadY
        {
            get
            {
                if (IsPressed(Buttons.DPadUp, Keys.Up))
                {
                    return -1;
                }
                else if (IsPressed(Buttons.DPadDown, Keys.Down))
                {
                    return 1;
                }
                else
                    return 0;

            }
        }


        public Boolean ButtonA
        {
            get
            {
                return IsNewPress(Buttons.A, Keys.A);

            }

        }
        public Boolean ButtonB
        {
            get
            {
                return IsNewPress(Buttons.B, Keys.S);

            }

        }
        public Boolean ButtonX
        {
            get
            {
                return IsNewPress(Buttons.X, Keys.X);

            }

        }
        public Boolean ButtonY
        {
            get
            {
                return IsNewPress(Buttons.Y, Keys.Z);

            }

        }

        public Boolean ButtonStart
        {
            get
            {
                return IsNewPress(Buttons.Start, Keys.Enter);

            }

        }
        public Boolean ShoulderRight
        {
            get
            {
                return IsNewPress(Buttons.RightShoulder, Keys.W);

            }

        }
        public Boolean ShoulderRightDown
        {
            get
            {
                return this.IsPressed(Buttons.RightShoulder, Keys.W);

            }

        }

        public Boolean ShoulderLeft
        {
            get
            {
                return IsNewPress(Buttons.LeftShoulder, Keys.Q);

            }

        }

        public float RightTriggerAmount
        {
            get
            {
                if (IsPressed(Keys.E))
                    return 1.0f;
                return CurrentPadState.Triggers.Right;

            }
        }
        public float LeftTriggerAmount
        {
            get
            {
                if (IsPressed(Keys.R))
                    return 1.0f;
                return CurrentPadState.Triggers.Left;
            }
        }
        public Boolean RightTriggerPressed
        {
            get
            {
                if (IsNewPress(Keys.E))
                    return true;
                return CurrentPadState.Triggers.Right > 0.01f && LastPadState.Triggers.Right <= 0.01f;
            }
        }
        public Boolean LeftTriggerPressed
        {
            get
            {
                if (IsNewPress(Keys.D))
                    return true;
                return CurrentPadState.Triggers.Left > 0.01f && LastPadState.Triggers.Left <= 0.01f;
            }
        }
        public Boolean RightTriggerDown
        {
            get
            {
                if (IsPressed(Keys.E))
                    return true;
                return CurrentPadState.Triggers.Right > 0.01f;
            }
        }
        public Boolean LeftTriggerDown
        {
            get
            {
                if (IsPressed(Keys.D))
                    return true;
                return CurrentPadState.Triggers.Left > 0.01f;
            }
        }

        public Boolean F1
        {
            get { return IsNewPress(Keys.F1); }
        }
        public Boolean F2
        {
            get { return IsNewPress(Keys.F2); }
        }
        public Boolean F3
        {
            get { return IsNewPress(Keys.F3); }
        }
        public Boolean F4
        {
            get { return IsNewPress(Keys.F4); }
        }
        public Boolean F5
        {
            get { return IsNewPress(Keys.F5); }
        }
        public Boolean F6
        {
            get { return IsNewPress(Keys.F6); }
        }
        public Boolean F7
        {
            get { return IsNewPress(Keys.F7); }
        }
        public Boolean F8
        {
            get { return IsNewPress(Keys.F8); }
        }


        #endregion

        #region Methods


        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public void Update()
        {
            LastKeyState = CurrentKeyState;
            LastPadState = CurrentPadState;
            gamePadWasConnected = gamePadIsConnected;

            CurrentKeyState = Keyboard.GetState();
            CurrentPadState = GamePad.GetState(player);
            gamePadIsConnected = CurrentPadState.IsConnected;

           

        }


        /// <summary>
        /// Helper for checking if a key was newly pressed during this update.
        /// </summary>
        /// 
        private bool IsPressed(Buttons button, Keys key)
        {
            return (CurrentKeyState.IsKeyDown(key) ||
                    CurrentPadState.IsButtonDown(button));
        }

        private bool IsPressed(Keys key)
        {
            return (CurrentKeyState.IsKeyDown(key));
        }

        private bool IsNewPress(Buttons button, Keys key)
        {
            return IsNewPress(button) || IsNewPress(key);
        }
        private bool IsNewPress(Keys key)
        {
            return (CurrentKeyState.IsKeyDown(key) &&
                    LastKeyState.IsKeyUp(key));
        }

        private bool IsNewPress(Buttons button)
        {
            return (CurrentPadState.IsButtonDown(button) &&
                    LastPadState.IsButtonUp(button));
        }


        #endregion
    }
}
