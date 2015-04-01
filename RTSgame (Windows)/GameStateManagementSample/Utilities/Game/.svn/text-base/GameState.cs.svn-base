using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RTSgame.AI;
using RTSgame.AI.Abstract;
using RTSgame.Collision;
using RTSgame.GameObjects;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities.Graphics.ParticleSystem;
using RTSgame.Utilities.IO;
using RTSgame.GameObjects.Buildings;

namespace RTSgame.Utilities
{
    //The GameState is an object which keeps track of all game objects to update (to avoid code in GameplayScreen)
    class GameState
    {
        
        HashSet<IMovable> movables = new HashSet<IMovable>();

        HashSet<IAnimated> animated = new HashSet<IAnimated>();
        HashSet<Camera> cameras = new HashSet<Camera>();
        HashSet<IIntelligent> intelligents = new HashSet<IIntelligent>();
        HashSet<GameObject> allGameObjects = new HashSet<GameObject>();
        HashSet<IUpdatableAI> aiManagers = new HashSet<IUpdatableAI>();
        HashSet<ILogic> gameLogics = new HashSet<ILogic>();
        HashSet<GameObject> gameObjectsToAdd = new HashSet<GameObject>();
        HashSet<Building> aiAttackTargets = new HashSet<Building>();
        HashSet<GameUserInterface> userInterfaces = new HashSet<GameUserInterface>();
        /// <summary>
        /// Not used.
        /// </summary>
        Surroundings surroundings;
        InteractionManager interactionManager = new InteractionManager();
        ParticleManager particleSystems = ParticleManager.GetInstance();
        public int enemies = 0;

        private static GameState instance;
        private GameState()
        {
            
            
        }

        public static GameState GetInstance()
        {
            if (instance == null)
            {
                instance = new GameState();
            }
            return instance;
        }

        public void SetUpGameState(Surroundings surroundings)
        {
            this.surroundings = surroundings;
        }

        public void updateAll(GameTime gameTime)
        {
            
            //Update code here

            particleSystems.Update(gameTime);
            SoundPlayer.Update();
            RumbleManager.Update(gameTime);
            //Add new game objects
            foreach (GameObject go in gameObjectsToAdd)
            {
                addGameObjectSafe(go);
            }
            gameObjectsToAdd.Clear();

            //go through all gameObjects and remove those that are dead
            
            HashSet<GameObject> toRemove = new HashSet<GameObject>();
            foreach(GameObject go in allGameObjects)
            {
                
                if (go.ToBeRemoved())
                {
                    toRemove.Add(go);
                    surroundings.Remove(go);
                    
                    if (go is IMovable)
                    {
                        movables.Remove((IMovable)go);
                    }
                    if (go is IAnimated)
                    {
                        animated.Remove((IAnimated)go);
                    }
                    if (go is IIntelligent)
                    {
                        intelligents.Remove((IIntelligent)go);
                    }
                    if (go is IInteractable)
                    {
                        interactionManager.RemoveCollidable((IInteractable)go);
                    }
                    if (go is ILogic)
                    {
                        gameLogics.Remove((ILogic)go);
                    }
                    if (go is Building)
                    {
                        aiAttackTargets.Remove((Building)go);
                    }
                    if (go is Enemy)
                        enemies--;
                }
            }
            foreach (GameObject gameObject in toRemove)
            {
                allGameObjects.Remove(gameObject);
            }


            //Update game logic for all objects
            foreach (ILogic il in gameLogics)
            {
                il.UpdateLogic(gameTime);
            }

            // Update all interactions with objects
            interactionManager.UpdateAIInteractions(gameTime);

            //Update AI-managers
            foreach (IUpdatableAI aim in aiManagers)
            {
                aim.Update(gameTime);
            }

            // Update all interactions
            interactionManager.UpdateInteractions();

            DebugPrinter.currentDebugPhase = DebugPhase.SolidCollisionLogic;

            //Move all objects
            foreach (IMovable mov in movables)
            {
                mov.UpdateDestination(gameTime);
                mov.RestrictToMap();
            }

            //Manage collisions
            interactionManager.UpdateSolidCollisions();

            DebugPrinter.currentDebugPhase = DebugPhase.Logic;

            //Update height for all objects
            foreach (GameObject go in allGameObjects)
            {
                go.AlignHeightToWorld();
            }

            //Update animations
            foreach (IAnimated anim in animated)
            {
                anim.updateAnimation(gameTime);
            }
            
            
            //Cameras are updated after all objects have moved
            foreach (Camera cam in cameras)
            {
                cam.Update(gameTime);
            }

            foreach (GameUserInterface gui in userInterfaces)
            {
                gui.Update(gameTime);
            }
        }

        [Obsolete]
        public Surroundings getAllSurroundings()
        {
            return surroundings;
        }

        public void DropMetal(int amount, Vector2 position)
        {
            if (amount <= 0)
            {
                return;
            }

            int maxAmount = Constants.DESIGN_RESOURCE_PILE_MAX_AMOUNT;
            int numberOfPiles = 1 + (amount / maxAmount);
            if (amount % maxAmount == 0)
            {
                numberOfPiles = amount / maxAmount;
            }


            int amountPerPile = amount / numberOfPiles;
            int rest = amount % numberOfPiles;


            //Debug:
            int totalDropped = 0;

            for (int i = 0; i < numberOfPiles; i++)
            {
                int currentAmount = amountPerPile;
                if (i < rest)
                {
                    currentAmount += 1;
                }


                totalDropped += currentAmount;
                MetalResource pile = new MetalResource(position, currentAmount);
                this.addGameObject(pile);
                DrawManager.GetInstance().addDrawable(pile);
            }
            if (totalDropped != amount)
            {
                throw new ApplicationException("Did not drop correctly. Ask Jonas!");
            }
        }

        private void addGameObjectSafe(GameObject gameObject)
        {
            //Debug.Write("ADDED SAFE " + gameObject);
            gameObject.RestrictToMap();

            allGameObjects.Add(gameObject);

            surroundings.addObject(gameObject);

            if (gameObject is IMovable)
            {
                movables.Add((IMovable)gameObject);
            }
            if (gameObject is IAnimated)
            {
                animated.Add((IAnimated)gameObject);
            }
            if (gameObject is IIntelligent)
            {
                if (!(gameObject is IInteractable))
                    throw new Exception("GameState.add, IIntelligent is not IInteractable: " + gameObject);
                intelligents.Add((IIntelligent)gameObject);
            }
            if (gameObject is IInteractable)
            {
                interactionManager.AddInteractable((IInteractable)gameObject);
            }
            if (gameObject is ILogic)
            {
                gameLogics.Add((ILogic)gameObject);
            }
            if (gameObject is Building)
            {
                aiAttackTargets.Add((Building)gameObject);

            }
            if (gameObject is Enemy)
                enemies++;
        }
        public void addGameObject(GameObject gameObject)
        {
            gameObjectsToAdd.Add(gameObject);
        }

        public Vector2 GetClosestAIattackTargetPosition(Vector2 position)
        {
            /*if (!AnyAIattackTargets())
            {
                throw new ApplicationException("No targets(buildings) to attack");
            }
            float closestDist = float.PositiveInfinity;
            Vector2 closestPos = new Vector2(-10,-10);
            foreach(Building b in aiAttackTargets){
                float dist = Vector2.DistanceSquared(position, b.GetPosition());
                if(dist < closestDist){
                    closestDist = dist;
                    closestPos = b.GetPosition();
                }
            }*/
            return Calculations.PositionClosestToPositionInCollection<Building>(aiAttackTargets, position);
        }
        public Building GetClosestAIattackTargetReference(Vector2 position)
        {
            return Calculations.ObjectClosestToPositionInCollection<Building>(aiAttackTargets, position);
        }
        public bool AnyAIattackTargets()
        {
            return aiAttackTargets.Count > 0;
        }


        public void addCamera(Camera cam)
        {
            cameras.Add(cam);
        }
        public void addUserInterface(GameUserInterface gui)
        {
            userInterfaces.Add(gui);
        }

        public void addWorld(IWorld World)
        {
            interactionManager.Init(World);
        }

        public void addAIManager(IUpdatableAI aim)
        {
            aiManagers.Add(aim);
        }



        public void clearState()
        {

            cameras.Clear();
            allGameObjects.Clear();
            aiManagers.Clear();
            gameLogics.Clear();
            gameObjectsToAdd.Clear();
            movables.Clear();
            animated.Clear();
            intelligents.Clear();
            enemies = 0;
        }

    }
}
