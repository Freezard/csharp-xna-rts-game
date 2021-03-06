﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RTSgame.Utilities
{
    /// <summary>
    /// A class for Misc Constants.
    /// </summary>
    static class Constants
    {

        //Specifies how many data values there are
        //in each row and column
        public const int WorldLineGridDimension = 3;
        public const int WorldHugeBlockSize = 28;
        public const int WorldLargeAreaBleed = 3;
        public const int WorldLargeAreaObstacleBaseSize = 9;
        public const int WorldBorderSize = 20;

        //public const int WorldDimension = 256;
        public const int WorldDimension = WorldBorderSize * 2 + WorldHugeBlockSize * WorldLineGridDimension;

        public const int WorldScale = 1;

        public const float DoodadMinSizeFactor = 1;
        public const float DoodadMaxSizeFactor = 1.25f;
        public const float DoodadSizeSpan = DoodadMaxSizeFactor - DoodadMinSizeFactor;

        public const float DoodadMaxWorldSize = 1.0f;

        public const float DoodadMaxSlopingGround = 0.1f;

        public const float DefaultMaxInteractionRange = 3.0f;

        public const float MaxBuildingSize = 3.0f;
        public const float MaxUnitSize = 2.0f;
        public const float MaxProjectileSize = 1.0f;

        public const float MaxUnitSpeed = 1.0f;
        public const float MaxProjectileSpeed = 2.0f;
        
        public static float DefaultMaxSolidCollisionRange{
            get {
                return Math.Max(MaxBuildingSize, Math.Max(MaxUnitSize + MaxUnitSpeed, MaxProjectileSize + MaxProjectileSpeed));
                }
        }

        public const int MaxLights = 8;

        public static Vector3 TweakVector;
        public const int DESIGN_FACTORY_COST = 10, DESIGN_MINION_COST = 1, DESIGN_PROTECT_BUILDING_COST = 10, DESIGN_WINDMILL_COST = 10, DESIGN_SHRINE_COST = 10;
        public const int DESIGN_RESOURCE_PILE_MAX_AMOUNT = 10;
        public const int DESIGN_WINDMILL_ENERGY_RECHARGE = 10, DESIGN_WINDMILL_ENERGY_MAX = 20;
        public const int DESIGN_PLAYER_DISCHARGE_RATE = 5;
        public const int DESIGN_HEALING_RATE = 1, DESIGN_HEALING_COST = 1;
        public const int DESIGN_ENEMY_METAL_DROP = 20;
        public const int DESIGN_MINE_COOLDOWN_MS = 1000;
        public const int DESIGN_MINE_METAL_DROP = 10;


        public const int MaxNumOfMonsters = 10;


        public const bool FULL_SCREEN = false;

        public const bool lotsOfTrees = false;

        public static Vector3 CameraDefaultLocation{
            get{
                    return new Vector3(0, 16.0f, 16.0f);
                //return new Vector3(0, 32.0f, 1);
               }
        }

        public static Point Player1StartingRegionPoint
        {
            //get { return new Point(Constants.WorldLineGridDimension / 2, Constants.WorldLineGridDimension / 2);}
            get { return new Point(0, 2);}
        }

        public static Point Player2StartingRegionPoint
        {
            //get { return new Point(Player1StartingRegionPoint.X + 1, Player1StartingRegionPoint.Y + 1); }
            get { return new Point(2, 0); }
        }

        public static Vector2 Player1StartingPosition
        {
            get { return new Vector2(
                Player1StartingRegionPoint.X * WorldHugeBlockSize + WorldBorderSize + WorldHugeBlockSize / 2,
                Player1StartingRegionPoint.Y * WorldHugeBlockSize + WorldBorderSize + WorldHugeBlockSize / 2)
                + new Vector2(0, 4); //To not collide with main base
            }
        }

        public static Vector2 Player2StartingPosition
        {
            get { return new Vector2(
                Player2StartingRegionPoint.X * WorldHugeBlockSize + WorldBorderSize + WorldHugeBlockSize / 2,
                Player2StartingRegionPoint.Y * WorldHugeBlockSize + WorldBorderSize + WorldHugeBlockSize / 2)
                + new Vector2(0, 4); //To not collide with main base
            }
        }

        public static Vector2 GetCenterOfTheWorldV2()
        {
            return new Vector2(WorldDimension / 2, WorldDimension / 2);
        }

        public static Vector3 GetCenterOfTheWorldV3()
        {
            return new Vector3(
                GetCenterOfTheWorldV2().X,
                0,
                GetCenterOfTheWorldV2().Y);
        }
        
        public static int ScreenWidth { get {
            if (!FULL_SCREEN)
            {
                return 800;
            }
            else
            {
                return 800;
            }
        
        } 
        
        }

        public static int ScreenHeight { get{
            if (!FULL_SCREEN)
            {
                return 600;
            }
            else
            {
                return 600;
            }
        
        
        } }

        public static Viewport StandardViewPort { get { return new Viewport(0, 0, ScreenWidth, ScreenHeight); } }

        public static float Graphics_ShadowMap_DefaultDepthBias = 0.000f;
        public static float Graphics_ShadowMap_ModelDepthBias = 0.005f;
        public static float Graphics_DefaultDepthBias = 0.0075f;

        public const int FRAMES_PER_SECOND = 60;
    }
}
