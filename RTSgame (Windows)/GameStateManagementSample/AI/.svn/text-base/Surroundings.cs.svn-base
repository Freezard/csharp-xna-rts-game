using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects;
using RTSgame.Utilities;

namespace RTSgame.AI
{
    //A collection of objects which are important for AI characters
    class Surroundings
    {
        List<GameObject> objects = new List<GameObject>();
        public void addObject(GameObject go)
        {
            objects.Add(go);
        }
        /*public LinkedList<GameObject> GetSurroundingObjects(Vector2 position, float radius)
        {
            LinkedList<GameObject> close = new LinkedList<GameObject>();
            double radiusSquared = Math.Pow(radius, 2);
            foreach (GameObject go in objects)
            {
                
                if (Calculations.IsWithin2DRange(go.GetPosition(), position, radiusSquared))
                {
                    close.AddLast(go);
                }
            }
            return close;
        }*/
        public List<GameObject> GetSurroundingObjects(GameObject gameObject, float radius)
        {
            List<GameObject> close = new List<GameObject>();
            
            foreach (GameObject go in objects)
            {
                if (go != gameObject && Calculations.IsWithin2DRange(go.GetPosition(), gameObject.GetPosition(), radius) )
                {
                    close.Add(go);
                }
            }
            return close;
        }

        internal void Remove(GameObject go)
        {
            if (objects.Contains(go))
            {
                objects.Remove(go);
            }
        }
    }
}
