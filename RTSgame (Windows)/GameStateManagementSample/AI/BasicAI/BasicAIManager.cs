using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;

namespace RTSgame.AI.BasicAI
{
    //This AI manager is responsible for controlling one ai-controlled, which calls choosebehaviour.
    class BasicAIManager<TypeAgent> where TypeAgent:AIControlledUnit
    {
        private List<BasicBehaviour<TypeAgent>> behaviours = new List<BasicBehaviour<TypeAgent>>();
        private BasicBehaviour<TypeAgent> idleBehaviour;
        private TypeAgent controlledUnit;
        private Surroundings surroundings;

        private float highestPriority;
        private BasicBehaviour<TypeAgent> chosenBehaviour;
        private IInteractable affectedObject;

        public BasicAIManager(TypeAgent controlledUnit, Surroundings surroundings){
            this.controlledUnit = controlledUnit;
            this.surroundings = surroundings;
        }


        public void AddBehaviour(BasicBehaviour<TypeAgent> bb)
        {
            behaviours.Add(bb);
        }
        public void SetIdleBehaviour(BasicBehaviour<TypeAgent> idleb)
        {
            this.idleBehaviour = idleb;
        }
        public void FindProperBehaviour(float sightRange, GameTime gameTime)
        {
            //Get surroundings
            //List<GameObject> surround = surroundings.GetSurroundingObjects(controlledUnit, sightRange);

            // Init values
            highestPriority = float.NegativeInfinity;
            chosenBehaviour = null;
            affectedObject = null;
            
            // Update Behaviours
            foreach (BasicBehaviour<TypeAgent> behaviour in behaviours)
            {
                behaviour.Update(gameTime);
            }
            
            // ^ PRE interactions ^


            // V POST interactions V
        }

        public void ChooseBehaviour()
        {
            //If there was no behaviour applicable, use the idle behaviour
            if (chosenBehaviour == null)
            {
                chosenBehaviour = idleBehaviour;
            }
            //Now when there is a behaviour chosen, apply it!
            if (chosenBehaviour != null) //chosenBehaviour could be null if no idle behaviour is set
            {
                /*if (affectedObject == null)
                {
                    throw new Exception("It seems you used an idle behaviour requiring an affectedObject, which does not make sense!");
                }*/
                chosenBehaviour.ApplyOn(controlledUnit, affectedObject);
            }
        }

        public void AIInteraction(IInteractable gameObject, float sightRange)
        {
            if (gameObject is IInteractable)
            {
                //For each gameobject in surroundings, evaluate behaviours
                foreach (BasicBehaviour<TypeAgent> behaviour in behaviours)
                {
                    //First check that the behaviour fulfils the basic criteria
                    if (behaviour.FulfilCriteria(controlledUnit, gameObject))
                    {
                        //Now, calculate the priority this behaviour has
                        float priority = behaviour.GetPriority(controlledUnit, gameObject, sightRange);
                        //If it is the highest yet, use it!
                        if (highestPriority < priority)
                        {
                            highestPriority = priority;
                            chosenBehaviour = behaviour;
                            affectedObject = gameObject;
                        }
                    }
                }
            }
            else
                throw new Exception("All gameObjects should implement interactable");
        }
    }
}
