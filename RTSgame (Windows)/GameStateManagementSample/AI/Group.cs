using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects;
using RTSgame.Utilities;

namespace RTSgame.AI
{
    class Group
    {
        float formationSpacing = 3;
        public Vector2 pos;
        List<Minion> members = new List<Minion>();
        List<Vector2> membPosTri = new List<Vector2>();
        Vector2 rY, rX;
        formType form = formType.tri;

        public Group()
        {
           
        }
        public enum formType { tri , circ , sqr, line };
   

        public void RemoveClosestMinionInGroup(Vector2 position)
        {
            
            float shortestDist = float.PositiveInfinity;
            int index = -1;
            for(int i = 0; i < members.Count; i++)
            {
                float dist = Vector2.DistanceSquared(position, members[i].GetPosition());
                if (dist < shortestDist)
                {
                    shortestDist = dist;
                    index = i;
                }
            }
            if (index >= 0)
            {
                members.ElementAt(index).LeaveGroup();
                members.RemoveAt(index);
                membPosTri.RemoveAt(members.Count);// Better to remove the last one imho /Rip
            }
        }
        public void UpdatePosTri()
        {
            int index = 0;
            for (int row = 0; index < members.Count; row++)
            {
                for (int inrow = 0; (inrow < row + 1) && (index < members.Count); inrow++,index++)
                {
                    membPosTri[index] = new Vector2(inrow,row);
                }
            }
        }

        //leker fortfarande
        public void MoveGroupInFormation(Vector2 pos, float angle)
        {
            if (members.Count != 0)
            {
                Vector2 corr = new Vector2(0, 0);
                int index = 0;
                this.pos = pos;
                UpdaterXrY(pos);
                foreach (Minion m in members)
                {
                    MoveMinionInFormation(m, index);
                    index++;
                }
            }
        }
        public void UpdaterXrY(Vector2 pos) 
        {
            if (members.Count > 0)
            {
                rY = members[0].GetPosition() - pos;
                rY.Normalize();
                float xAngle = (float)((float)Math.Atan2(rY.Y, rY.X) + Math.PI / 2);
                rX = Calculations.AngleToV2(xAngle, 1);
            }
        }

        internal void Add(Minion member)
        {
            members.Add(member);
            membPosTri.Add(new Vector2(0, 0) + rY);
            member.JoinGroup(this);
            UpdatePosTri();

        }
        public bool Contains(Minion member)
        {
            return members.Contains(member);
        }
        public void SwitchFormation()
        {
            if (form.Equals(formType.line))
                form = formType.tri;

            else    
                form++;
        }

        public void MoveMinionInFormation(Minion min, int index)
        {
            Vector2 corr = new Vector2(0,0);
            switch (form)
            {
                case formType.tri:
                    //For each minion, move into the determined pattern multiplied by the orthogonal vectors rX, rY (here in triformation)
                    corr = pos + membPosTri[index].Y * (rY - (rX / 2)) + rY + membPosTri[index].X * rX;
                    min.MoveToAndStop(corr, 3f);
                    break;

                case formType.circ:
                    //For each minion, surround some position (usually a PlayerCharacter)
                    corr = pos + Calculations.AngleToV2((float)((index) * 2 * Math.PI) / members.Count, formationSpacing);
                    min.MoveToAndStop(corr, 3f);
                    break;
                    
                case formType.sqr:
                    //For each minion, run around inside a square
                    corr = pos - (new Vector2(formationSpacing / 2, formationSpacing / 2)) + new Vector2((float)Utilities.EasyRandom.Next0to1(), (float)Utilities.EasyRandom.Next0to1()) * formationSpacing;
                    min.MoveToAndStop(corr, 5f);
                    break;
                    
                case formType.line:
                    if (index == 0)
                        min.MoveToAndStop(pos, 2f);
                    //Follow the leader (the person in front of the minion)
                    else
                        min.MoveToAndStop(members[index - 1].GetPosition(), 2f);
                    break;

                default:
                    break;
            }
        }

        public void MoveMinionInFormation(Minion min)
        {
            MoveMinionInFormation(min, members.IndexOf(min));
        }

        internal void UpdatePos(Vector2 newPos)
        {
            pos = newPos;
            if (form.Equals(formType.tri))
            {
                UpdatePosTri();
            }
        }
        public void Disband()
        {
            foreach (Minion member in members)
            {
                member.LeaveGroup();
            }
            members.Clear();
            membPosTri.Clear();
        }

        internal bool IsEmpty()
        {
            return members.Count == 0;
        }
    }
}
