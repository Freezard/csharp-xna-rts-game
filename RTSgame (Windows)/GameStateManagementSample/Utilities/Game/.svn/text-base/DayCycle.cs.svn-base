using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.Utilities.IO;
using RTSgame.Utilities.Calc;

namespace RTSgame.Utilities.Game
{
    static class DayCycle
    {
        // midnight -> dawn -> sunrise -> day -> noon -> sunset -> dusk -> night -> midnight
        // 0           1       2          3      4       5         6       7        8         

        // also called, "day phase"

        public static int DayPhase = 0;

        public static Vector3 sunLightDirection;
        public static Vector4 sunColor;
        public static Vector4 shadowDimning;
        public static Vector4 ambientColor;
        public static Vector3 specularDirection;
        public static Vector4 specularIntensity;
        public static Vector4 modelSpecularIntensity;
        public static float specularShininess;


        public static float dayTime {
            get { return DayClock.CurrentTime; }}
        //= 1.5f; // global time over this day
        static float daySpeed = 6.28f / 40.0f; // how much of the day that passes per second,
                                               // a full day is 2*Pi
        //static float dayTimeTarget = 1.51f;
        public static Clock DayClock = new Clock(Calculations.DoublePi, 0f);

        public static float dayPhaseTime;
        
        static float[] daytimes;
        /*
        static float beginningOfDawn = 0;
        static float beginningOfSunrise = 0;
        static float beginningOfDay = 0;
        static float endOfDay = 0;
        static float noon = 0;
        static float beginningOfSunset = 0;
        static float endOfSunset = 0;
        static float beginningOfDusk = 0;
        static float endOfDusk = 0;
        static float beginningOfNight = 0;
        static float endOfNight = 0;
        static float midnight = 0;
        */
        static Vector4[] sunColors = {
            new Vector4(0.05f,0.05f,0.1f,0), // midnight
            new Vector4(0.05f,0.05f,0.1f,0), // dawn
            new Vector4(0.0f,0.0f,0.0f,0), // sunrise
            new Vector4(1.0f,1.0f,0.2f,0), // day
            new Vector4(1.0f,1.0f,0.4f,0), // noon
            new Vector4(1.4f,0.2f,0.0f,0), // sunset
            new Vector4(0.0f,0.0f,0.0f,0), // dusk
            new Vector4(0.05f,0.05f,0.1f,0), // night
            new Vector4(0.05f,0.05f,0.1f,0)}; // midnight again (same)

        static Vector4[] ambientColors = {
            new Vector4(0.3f,0.3f,0.6f,0), // midnight
            new Vector4(0.3f,0.3f,0.6f,0), // dawn
            new Vector4(0.4f,0.4f,0.6f,0), // sunrise
            new Vector4(0.4f,0.4f,0.6f,0), // day
            new Vector4(0.4f,0.4f,0.6f,0), // noon
            new Vector4(0.3f,0.3f,0.6f,0), // sunset
            new Vector4(0.3f,0.3f,0.6f,0), // dusk
            new Vector4(0.3f,0.3f,0.6f,0), // night
            new Vector4(0.3f,0.3f,0.6f,0)}; // midnight again (same)

        // not used anymore
        static Vector4[] shadowDimnings = {
            new Vector4(1,1,1,0), // midnight
            new Vector4(1,1,1,0), // dawn
            new Vector4(1,1,1,0), // sunrise
            new Vector4(0.6f,0.6f,0.9f,0), // day
            new Vector4(0.5f,0.5f,0.8f,0), // noon
            new Vector4(0.6f,0.6f,0.9f,0), // sunset
            new Vector4(1,1,1,0), // dusk
            new Vector4(1,1,1,0), // night
            new Vector4(1,1,1,0)}; // midnight again (same)

        static Vector4[] specularIntensityList = {
            new Vector4(0.1f,0.1f,0.1f,0), // midnight
            new Vector4(0.1f,0.1f,0.1f,0), // dawn
            new Vector4(0,0,0,0), // sunrise
            new Vector4(0,0,0,0), // day
            new Vector4(0,0,0,0), // noon
            new Vector4(0,0,0,0), // sunset
            new Vector4(0,0,0,0), // dusk
            new Vector4(0.1f,0.1f,0.1f,0), // night
            new Vector4(0.1f,0.1f,0.1f,0)}; // midnight again (same)

        static Vector4[] modelSpecularIntensityList = {
            new Vector4(0.7f,0.7f,0.7f,0), // midnight
            new Vector4(0.7f,0.7f,0.7f,0), // dawn
            new Vector4(0,0,0,0), // sunrise
            new Vector4(0,0,0,0), // day
            new Vector4(0.7f,0.7f,0.7f,0), // noon
            new Vector4(0.7f,0.7f,0.7f,0), // sunset
            new Vector4(0,0,0,0), // dusk
            new Vector4(0.7f,0.7f,0.7f,0), // night
            new Vector4(0.7f,0.7f,0.7f,0)}; // midnight again (same)

        static bool hasBeenSetUp = false;

        const float sunsize = 0.1f;
        const float horizonHeight = 0.25f;
        const float duskNdawnLine = -0.2f;
        const float sunBloom = 0.2f;
        const float sunArcSpan = 0.4f;
        const float summerTilt = 0.2f;

        

        static public void UpdateDay(GameTime gameTime)
        {
            if (!hasBeenSetUp)
                SetUp();

            InputManager input = SessionHandler.players[0].GetInput();
            if (input.F1)
            {
                DayClock.SetTargetTime(1.3f);
            }
            if (input.F2)
            {
                DayClock.SetTargetTime(3.0f);
            }

            // update daytime
            DayClock.LeapForward(daySpeed * Calculations.GetFractionalSecond(gameTime));

            for (int i = 0; i < daytimes.Length -1; i++)
            {
                if (dayTime >= daytimes[i] && dayTime < daytimes[i+1])
                {
                    DayPhase = i;
                    SetDayPhaseTime(daytimes[i], daytimes[i+1]);
                }
            }

            // sun position
            if (GetDayTimeInRadians() >= daytimes[2] && GetDayTimeInRadians() <= daytimes[6])
            {
                float sunHeight = sunsize + summerTilt + sunArcSpan * (float)-Math.Cos(GetDayTimeInRadians());

                sunLightDirection = new Vector3(
                    1.2f * (float)Math.Sin(dayTime - Calculations.double_Pi8th),
                    sunHeight,
                    -1.2f * (float)Math.Cos(dayTime - Calculations.double_Pi8th));
                //sunLightDirection = new Vector3(0.4f, 0.4f, 0.6f);
                sunLightDirection.Normalize();
            }
            else // moon position
            {
                sunLightDirection = new Vector3(
                    -0.1f,
                    0.35f,
                    0.3f);
            }

            // calculate basic specularIntensity before
            // doing the special morning shine
            specularIntensity = Vector4.Lerp(
                specularIntensityList[DayPhase],
                specularIntensityList[DayPhase + 1],
                dayPhaseTime);

            // special morning shine
            if (GetDayTimeInRadians() >= daytimes[2] && GetDayTimeInRadians() <= daytimes[3])
            {
                float glarePhaseTime = dayPhaseTime;

                if (GetDayTimeInRadians() > daytimes[3] && GetDayTimeInRadians() <= daytimes[4])
                    glarePhaseTime += 1;

                Vector4 shineColor = new Vector4(0.5f, 0.5f, 0.0f, 0);
                float maxShineMoment = 0.4f;
                float totalTime = 1.0f;
                float invMSM = totalTime - maxShineMoment;
                float sunPositionSpeedUp = 1.3f;

                specularDirection = new Vector3(
                    (float)Math.Pow((totalTime - glarePhaseTime) / totalTime, 2),
                    (float)Math.Sin(glarePhaseTime * sunPositionSpeedUp / totalTime),
                    (float)-Math.Cos(glarePhaseTime * sunPositionSpeedUp / totalTime));                

                // glare increase phase
                if (glarePhaseTime < maxShineMoment)
                {
                    glarePhaseTime = glarePhaseTime / maxShineMoment;
                    specularIntensity = Vector4.Lerp(Vector4.Zero, shineColor, glarePhaseTime);
                }
                else if (glarePhaseTime < totalTime) // glare decrease phase
                {
                    glarePhaseTime = (glarePhaseTime - maxShineMoment) / invMSM;
                    specularIntensity = Vector4.Lerp(shineColor, Vector4.Zero, glarePhaseTime);
                }
                else
                {
                    specularIntensity = Vector4.Zero;
                }
                
            }
            else
            {
                specularDirection = sunLightDirection;
            }

            specularShininess = 4;

            sunColor = Vector4.Lerp(
                sunColors[DayPhase],
                sunColors[DayPhase + 1],
                dayPhaseTime);

            // not used anymore
            shadowDimning = Vector4.Lerp(
                shadowDimnings[DayPhase],
                shadowDimnings[DayPhase + 1],
                dayPhaseTime);

            ambientColor = Vector4.Lerp(
                ambientColors[DayPhase],
                ambientColors[DayPhase + 1],
                dayPhaseTime);

            modelSpecularIntensity = Vector4.Lerp(
                modelSpecularIntensityList[DayPhase],
                modelSpecularIntensityList[DayPhase + 1],
                dayPhaseTime);

        }

        static void SetUp()
        {
            DayClock.SetTime(3.0f);
            DayClock.SetTargetTime(3.0f);
            

            // float sunHeight = sunsize + sunArcSpan * (float)-Math.Cos(GetDayTimeInRadians());
            
            // this following formula is the inverse of sunheight,
            // Math.Asin((HEIGHT - sunsize - summerTilt) / sunArcSpan)
            // it returns the first time when the sun's top is at HEIGHT

            /*
            beginningOfDawn = (float)Math.Asin((duskNdawnLine - sunsize) / sunArcSpan);
            beginningOfSunrise = (float)Math.Asin((horizonHeight - sunsize) / sunArcSpan);
            beginningOfDay = (float)Math.Asin((horizonHeight - sunsize + sunsize) / sunArcSpan);
            endOfDay = Calculations.DoublePi - beginningOfDay;
            noon = (beginningOfDay + endOfDay) / 2.0f;
            endOfSunset = Calculations.DoublePi - beginningOfSunrise;
            endOfDusk = Calculations.DoublePi - beginningOfDawn;
            beginningOfSunset = endOfDay;
            beginningOfDusk = endOfSunset;
            beginningOfNight = endOfDusk;
            midnight = 0;
            */

            // midnight -> dawn -> sunrise -> day -> noon -> sunset -> dusk -> night -> midnight
            // 0           1       2          3      4       5         6       7        8         

            daytimes = new float[9];
            /*
            daytimes[0] = 0;
            daytimes[1] = GetFirstTimeOfTheDayWhenTheSunIsThisHigh(duskNdawnLine);
            daytimes[2] = GetFirstTimeOfTheDayWhenTheSunIsThisHigh(horizonHeight);
            daytimes[3] = GetFirstTimeOfTheDayWhenTheSunIsThisHigh(horizonHeight + sunsize);
            daytimes[5] = Calculations.DoublePi - daytimes[3];
            daytimes[4] = (daytimes[3] + daytimes[5]) / 2.0f;
            daytimes[6] = Calculations.DoublePi - daytimes[2];
            daytimes[7] = Calculations.DoublePi - daytimes[1];
            daytimes[8] = Calculations.DoublePi;
            */
            
            daytimes[0] = 0;
            daytimes[1] = GetFirstTimeOfTheDayWhenTheSunIsThisHigh(horizonHeight - sunBloom);
            daytimes[2] = GetFirstTimeOfTheDayWhenTheSunIsThisHigh(horizonHeight);
            daytimes[3] = GetFirstTimeOfTheDayWhenTheSunIsThisHigh(horizonHeight + sunBloom);
            daytimes[5] = Calculations.DoublePi - daytimes[3];
            daytimes[4] = (daytimes[3] + daytimes[5]) / 2.0f;
            daytimes[6] = Calculations.DoublePi - daytimes[2];
            daytimes[7] = Calculations.DoublePi - daytimes[1];
            daytimes[8] = Calculations.DoublePi;
            
            //sunBloom

            //for (int i = 0; i < daytimes.Length; i++)
            //    daytimes[i] = i * Calculations.Pi4th;

            //for (int i = 0; i < daytimes.Length; i++)
            //    Console.WriteLine(daytimes[i]);

            hasBeenSetUp = true;
        }

        static float GetFirstTimeOfTheDayWhenTheSunIsThisHigh(float Height)
        {
            return (float)Math.Acos((Height - sunsize - summerTilt) / (sunArcSpan * -1));
        }

        static void SetDayPhaseTime(float startTime, float endTime)
        {
            float dayPhaseLength = (endTime - startTime);
            dayPhaseTime = (dayTime - startTime) / dayPhaseLength;
        }

        public static float GetDayPhaseTime_x_PiHalf()
        {
            return dayPhaseTime * Calculations.Pi2th;
        }

        public static float GetDayTimeInRadians()
        {
            return dayTime;
        }

        public static float GetNightLight()
        {
            switch (DayPhase)
            {
                case 1:
                    return 1.0f - dayPhaseTime * 0.5f;
                case 2:
                    return 0.5f - dayPhaseTime * 0.5f;
                case 3:
                case 4:
                    return 0;
                case 5:
                    return 0.0f + dayPhaseTime * 0.5f;
                case 6:
                    return 0.5f + dayPhaseTime * 0.5f;
                default:
                    return 1;
            }
        }
    }
}
