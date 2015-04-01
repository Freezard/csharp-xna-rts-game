using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSgame.Utilities.Calc
{
    class Clock
    {
        public float CurrentTime;
        public float ClockSpeed = 1;
        public readonly float MaxTime;
        float targetTime;
        bool halt = false;
        bool goAroundToReachTarget = false;
        bool hasATarget = false;

        public Clock(float MaxTime)
        {
            this.MaxTime = MaxTime;
        }

        public Clock(float MaxTime, float InitialTime)
        {
            this.MaxTime = MaxTime;
            this.CurrentTime = InitialTime;
        }

        public void LeapForward(float Time)
        {
            if (!halt)
            {
                if (hasATarget)
                {
                    if (goAroundToReachTarget)
                    {
                        CurrentTime += Time * ClockSpeed;
                        if (CurrentTime > MaxTime)
                        {
                            CurrentTime = CurrentTime - MaxTime;
                            goAroundToReachTarget = false;
                        }
                    }
                    else
                    {
                        if (CurrentTime < targetTime)
                        {
                            CurrentTime += Time * ClockSpeed;
                        }
                    }

                }
                else // has no target, should always go around
                {
                    CurrentTime += Time * ClockSpeed;
                    if (CurrentTime > MaxTime)
                        CurrentTime = CurrentTime - MaxTime;
                }
            }
        }

        public void Halt()
        {
            halt = true;
        }

        public void UnHalt()
        {
            halt = false;
        }

        public void SetTargetTime(float TargetTime)
        {
            if (TargetTime > MaxTime)
                TargetTime = MaxTime;

            this.targetTime = TargetTime;
            hasATarget = true;

            if (CurrentTime > targetTime)
                goAroundToReachTarget = true;
        }

        public void DeTarget()
        {
            hasATarget = false;
        }

        public void SetTime(float Time)
        {
            CurrentTime = Time;
        }
    }
}
