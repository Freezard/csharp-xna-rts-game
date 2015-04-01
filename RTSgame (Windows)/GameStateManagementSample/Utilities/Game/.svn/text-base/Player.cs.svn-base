using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Units;
using RTSgame.Utilities.IO;
using RTSgame.GameObjects.Buildings;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.GameObjects.Economy.Buildings;

namespace RTSgame.Utilities
{
    /// <summary>
    /// Stores the player-state.
    /// (Everything about the player that is not covered by the Hero.)
    /// </summary>
    class Player
    {
        private PlayerIndex playerIndex;
        private InputManager input;
        private String name;
        private int energy = 0;
        private int metal = 500;
        private int maxEnergy = 100;
        private PlayerCharacter controlledUnit;
        private GameUserInterface ui;
        private List<Building> respawnLocations = new List<Building>();
        private MainBase mainBase;

        
        

        public Player(String name, PlayerIndex index)
        {
            this.playerIndex = index;
            input = new InputManager(index);
            this.name = name;
        }
        public Texture2D GetPlayerColor()
        {
            if (playerIndex == PlayerIndex.One)
            {
                return AssetBank.GetInstance().GetTexture("SolidDarkBlue");
            }
            else
            {
                return AssetBank.GetInstance().GetTexture("SolidRed");
            }
        }
        public String GetPlayerColorString()
        {
            
            if (name == "Bob")
            {
                return "Blue";
            }
            else
            {
                return "Red";
            }

        }
        public PlayerIndex GetPlayerIndex()
        {
            return playerIndex;
        }
        public void AddRespawn(Building hb)
        {
            respawnLocations.Add(hb);
        }
        public Vector2 FindClosestRespawn(Vector2 position)
        {

                return new Vector2(0, 4) + Calculations.PositionClosestToPositionInCollection<Building>(respawnLocations, position);


        }
        private Vector2 FindNextTeleportDestination(HealingBuilding hb)
        {
            int index = respawnLocations.IndexOf(hb) + 1;
            if (index == respawnLocations.Count)
            {
                index = 0;
            }
            return respawnLocations[index].GetPosition() + new Vector2(0, 2);
        }
        public Boolean BuildMenuOpen()
        {

            return ui.BuildMenuOpen();
        }
        public void SetUI(GameUserInterface gui)
        {
            ui = gui;
        }

        public Boolean HasEnergy(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("No negative amounts!");
            }
            else
            {
                return (amount <= energy);
            }
        }
        public Boolean HasMetal(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("No negative amounts!");
            }
            else
            {
                return (amount <= metal);
            }
        }
        /// <summary>
        /// Checks if player has metal; if not, signal this to UI
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Boolean HasMetalWithSignal(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("No negative amounts!");
            }
            else
            {
                if (amount <= metal)
                {
                    return true;
                }
                else
                {
                    ui.SignalNotEnoughMetal(metal, amount);
                    return false;
                }

            }
        }
        public void SubtractMetal(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("No negative amounts!");
            }
            else if (amount > metal)
            {
                throw new ArgumentException("Not enough metal!");
            }
            else
            {
                metal -= amount;
            }
        }
        public void SubtractEnergy(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("No negative amounts!");
            }
            else if (amount > energy)
            {
                throw new ArgumentException("Not enough energy!");
            }
            else
            {
                energy -= amount;
            }
        }
        public void AddMetal(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("No negative amounts!");
            }
            else
            {
                metal += amount;
            }
        }
        public void AddEnergy(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("No negative amounts!");
            }
            else
            {
                energy += amount;
                if (energy > maxEnergy)
                {
                    energy = maxEnergy;
                }

            }
        }
        internal InputManager UpdateAndGetInput()
        {
            input.Update();
            return input;
        }

        internal InputManager GetInput()
        {
            return input;
        }

        internal int GetMetal()
        {
            return metal;
        }
        internal int GetEnergy()
        {
            return energy;
        }

        internal PlayerCharacter GetControlledUnit()
        {
            return controlledUnit;
        }
        public void SetControlledUnit(PlayerCharacter playerCharacter)
        {
            this.controlledUnit = playerCharacter;
        }

        public GameUserInterface GetUI()
        {
            return ui;
        }

        internal void ModifyMaxEnergy(int p)
        {
            maxEnergy += p;
        }

        internal int GetMaxEnergy()
        {
            return maxEnergy;
        }

        internal void TeleportFrom(HealingBuilding healingBuilding)
        {
            if (respawnLocations.Count > 1)
            {
                // Since we are not interested in moving from position towards destination,
                // set both to the target destination so
                // that the player winds up there no matter what
                controlledUnit.Teleport(FindNextTeleportDestination(healingBuilding));
            }
        }

        internal void RemoveRespawn(Building building)
        {
            respawnLocations.Remove(building);
        }
        public MainBase MainBase
        {
            get { return mainBase; }
            set { mainBase = value; }
        }
    }
}
