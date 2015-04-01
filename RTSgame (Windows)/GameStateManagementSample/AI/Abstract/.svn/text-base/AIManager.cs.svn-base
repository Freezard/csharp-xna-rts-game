using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using RTSgame.AI;

namespace RTSgame.AI.Abstract
{
    //Manages the AI of a game object
    abstract class AIManager<T>:IUpdatableAI where T : IIntelligent
    {
        //The root of the AI tree
        protected AINode<T> entryNode;
        //Which behaviour should be run
        protected Behaviour<T> chosen;
        //Associated gameObject
        protected T gameObject;
        //A collection of objects of interest to this AI manager
        protected Surroundings surroundings;
        //How far this AI "sees"
        protected int sightRange = 500;

        public AIManager(T associatedGameObject, Surroundings surroundings)
        {
            this.surroundings = surroundings;
            this.gameObject = associatedGameObject;    
        }
        //Starts the AI-tree choosing process. Must be run, but only once, to activate AI
        public void startChoose()
        {
            entryNode.choose(this);
        }
        //Sets which behaviour is active
        public void setChosen(Behaviour<T> choice)
        {
            chosen = choice;
        }
        //Gets the surroundings of the associated gameObject
        public List<GameObject> GetSurroundings()
        {
            throw new NotImplementedException();
            
        }
        //Updates the AI every frame
        public void Update(GameTime gameTime)
        {
            chosen.execute(gameTime, gameObject);
        }
        //Sets the starting position of the AI-tree
        protected void SetEntryNode(AINode<T> aiNode)
        {
            entryNode = aiNode;
            if (entryNode is Selector<T>)
            {
                ((Selector<T>)entryNode).SetAIManager(this);
            }
        }
    }
}
