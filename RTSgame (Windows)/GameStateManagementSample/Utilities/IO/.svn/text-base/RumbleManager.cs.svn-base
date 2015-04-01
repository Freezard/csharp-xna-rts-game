using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace RTSgame.Utilities.IO
{
    /// <summary>
    /// The rumblemanager manages controller vibration
    /// </summary>
    static class RumbleManager
    {

        //List<Vibration> vibrations = new List<Vibration>();
        static int[] vibrations = new int[4];
       
        
        /// <summary>
        /// Start a vibration
        /// 
        /// </summary>
        /// <param name="playerIndex">The controller to shake</param>
        /// <param name="leftMotor">amount rumble in left motor [0,1]</param>
        /// <param name="rightMotor">amount rumble in right motor [0,1]</param>
        /// <param name="rumbleTimeMs">number of milliseconds to rumble</param>
        public static void StartVibration(PlayerIndex playerIndex, float leftMotor, float rightMotor, int rumbleTimeMs)
        {
            vibrations[(int)playerIndex] = rumbleTimeMs; //new Vibration(leftMotor, rightMotor, rumbleTimeMs);
            GamePad.SetVibration(playerIndex, leftMotor, rightMotor);
        }
        public static void Update(GameTime gameTime)
        {
            int elapsedTime = gameTime.ElapsedGameTime.Milliseconds;
            for (int i = 0; i < 4; i++)
            {
                if (vibrations[i] > -1)
                {
                    vibrations[i] -= elapsedTime;
                    if (vibrations[i] <= 0)
                    {
                        vibrations[i] = -1;
                        GamePad.SetVibration((PlayerIndex)i, 0, 0);
                    }
                }
            }

        }
        //This class is useful later...
        /*class Vibration
        {
            float leftMotor, rightMotor;
            int rumbleTimeLeft;
            
            public Vibration(float leftMotor, float rightMotor, int rumbleTimeLeft)
            {
                this.leftMotor = leftMotor;
                this.rightMotor = rightMotor;
                this.rumbleTimeLeft = rumbleTimeLeft;
                
            }
            public void DecreaseTime(int ms)
            {
                rumbleTimeLeft -= ms;
            }
            public Boolean TimeLeft()
            {
                return rumbleTimeLeft > 0;
            }
        }*/
    }
}
