using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;
using Microsoft.Xna.Framework.Graphics;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Doodads;
using RTSgame.Utilities.Calc;
using RTSgame.Utilities.Memory;

namespace RTSgame.Utilities.World
{

    /// <summary>
    /// Container for all WorldObjects.
    /// </summary>
    class World : IWorld
    {

        /// <summary>
        /// Specifies how many data values there are
        /// in each row and column in this world
        /// </summary>
        private int dimension = Constants.WorldDimension;

        /// <summary>
        /// Gives a rectangle which corners mark all the valid
        /// data values in this world.
        /// </summary>
        private Rectangle worldBoundaries;

        /// <summary>
        /// How far between every point.
        /// Note that we can graphically scale in addition to this.
        /// </summary>
        private float Scaling = Constants.WorldScale;

        /// <summary>
        /// Texture for this world
        /// </summary>
        private Texture2D worldTexture;
        
        /// <summary>
        /// Container for all worldbound values
        /// </summary>
        private Array2D<WorldObject> worldMatrix;

        /// <summary>
        /// Map of all regions
        /// </summary>
        public BlockMap<Region> RegionMap;

        private SeedRandom seedRandom;

        public bool forestOnly = Constants.lotsOfTrees;

        public SmallAreaData[] SmallAreaList = new SmallAreaData[Enum.GetValues(typeof(SquareTerrainType)).Length];
        public LargeAreaData[] LargeAreaList = new LargeAreaData[Enum.GetValues(typeof(LargeAreaType)).Length];


        public World(int Seed)
        {
            worldMatrix = new Array2D<WorldObject>(dimension);

            worldBoundaries = new Rectangle(0, 0, dimension - 1, dimension - 1);

            seedRandom = new SeedRandom(Seed);
            
            worldTexture = AssetBank.GetInstance().GetTexture("TerrainMultiTexture1_3");

            // World Generation

            SetWorldBorder();

            GenerateWorld();

            CalculateHeightForDoodads();
        }
        
        //These functions will go into world generation soon

        private void getRandomInnerBox(ref Point UpperLeftCorner, ref Point LowerRightCorner, int NewMinimumSize, int Variance)
        {

            int newXPositionVarianceSpan = LowerRightCorner.X - UpperLeftCorner.X - NewMinimumSize - Variance;
            int newYPositionVarianceSpan = LowerRightCorner.Y - UpperLeftCorner.Y - NewMinimumSize - Variance;

            Point NewUpperLeftCorner = new Point(
                seedRandom.Next(newXPositionVarianceSpan) + UpperLeftCorner.X,
                seedRandom.Next(newYPositionVarianceSpan) + UpperLeftCorner.Y);

            LowerRightCorner = new Point(
                seedRandom.Next(Variance) + NewMinimumSize + NewUpperLeftCorner.X,
                seedRandom.Next(Variance) + NewMinimumSize + NewUpperLeftCorner.Y);

            UpperLeftCorner = NewUpperLeftCorner;

            Calculations.ClampPointToRectangle(UpperLeftCorner, ref worldBoundaries);
            Calculations.ClampPointToRectangle(LowerRightCorner, ref worldBoundaries);
        }

        private void getRandomInnerBox2(ref Point UpperLeftCorner, ref Point LowerRightCorner, int NewSize)
        {
            // if the smaller box fills the original box, there is still one position right?
            // well this "error" exists even if the smaller box is smaller then the original box.
            int oneMorePosition = 1;

            int numOfXPossibleCornerPositions = LowerRightCorner.X - UpperLeftCorner.X - NewSize + oneMorePosition;
            int numOfYPossibleCornerPositions = LowerRightCorner.Y - UpperLeftCorner.Y - NewSize + oneMorePosition;

            // There should be at least one possible position, no matter what.
            numOfXPossibleCornerPositions = Math.Max(numOfXPossibleCornerPositions, 1);
            numOfYPossibleCornerPositions = Math.Max(numOfYPossibleCornerPositions, 1);

            UpperLeftCorner = new Point(
                seedRandom.Next(numOfXPossibleCornerPositions) + UpperLeftCorner.X,
                seedRandom.Next(numOfYPossibleCornerPositions) + UpperLeftCorner.Y);

            // The potential new corner
            Point NewLowerRightCorner = new Point(
                UpperLeftCorner.X + NewSize,
                UpperLeftCorner.Y + NewSize);

            // LowerRightCorner should not be set further away then it already was
            LowerRightCorner = new Point(
                Math.Min(NewLowerRightCorner.X, LowerRightCorner.X),
                Math.Min(NewLowerRightCorner.Y, LowerRightCorner.Y));

            // Just in case
            Calculations.ClampPointToRectangle(UpperLeftCorner, ref worldBoundaries);
            Calculations.ClampPointToRectangle(LowerRightCorner, ref worldBoundaries);
        }

        public void GenerateWorld()
        {
            InitSmallAreaDataList();
            InitLargeAreaDataList();

            Rectangle regionMapBoundaries = new Rectangle(
                Constants.WorldBorderSize,
                Constants.WorldBorderSize,
                // Rectangle requires size of the block. We need to take
                // the world boundaries on both sides into account, thus the " * 2"
                dimension - Constants.WorldBorderSize * 2,
                dimension - Constants.WorldBorderSize * 2);

            RegionMap = new BlockMap<Region>(Constants.WorldHugeBlockSize, regionMapBoundaries);

            RegionMap.ChangeEveryPoint(CreateHeightsAroundRegion);
            worldMatrix.ChangeEveryPoint(FlattenLaneHeightAnomaly);
            worldMatrix.ChangeEveryPoint(FlattenLaneHeightDipAnomaly);

            RegionMap.ChangeEveryPoint(CreateRegionTerrain);

            worldMatrix.ChangeEveryPoint(FlattenLaneHeightAnomaly);
            worldMatrix.ChangeEveryPoint(FinalizeTerrain);

        }

        /// <summary>
        /// Creates heights around the region that stretches into and beyond nearby regions
        /// </summary>
        /// <param name="R"></param>
        private void CreateHeightsAroundRegionFarStretch(ref Region R)
        {
            const int largeAreaObstacleSize = Constants.WorldLargeAreaBleed + Constants.WorldLargeAreaObstacleBaseSize;
            bool hit = false;
            hit = CreateHeightAroundRegion(ref R, largeAreaObstacleSize * 1 / 2, hit, 0.25f, 0.25f);
            hit = CreateHeightAroundRegion(ref R, largeAreaObstacleSize * 2 / 2, hit, 0.1f, 0.9f);
            hit = CreateHeightAroundRegion(ref R, Constants.WorldHugeBlockSize / 2, hit, 0.1f, 0.9f);
            hit = CreateHeightAroundRegion(ref R, Constants.WorldHugeBlockSize - largeAreaObstacleSize, hit, 0.1f, 0.9f);
            hit = CreateHeightAroundRegion(ref R, Constants.WorldHugeBlockSize + largeAreaObstacleSize, hit, 0.05f, 0.9f);
            hit = CreateHeightAroundRegion(ref R, Constants.WorldHugeBlockSize * 2 - largeAreaObstacleSize, hit, 0.02f, 0.9f);
            hit = CreateHeightAroundRegion(ref R, Constants.WorldHugeBlockSize * 2 + largeAreaObstacleSize, hit, 0.01f, 0.9f);
        }

        private void CreateHeightsAroundRegion(ref Region R)
        {
            const int largeAreaObstacleSize = Constants.WorldLargeAreaBleed + Constants.WorldLargeAreaObstacleBaseSize;
            bool hit = false;

            //3, 2, 3, 2, 3, 2,

            hit = CreateHeightAroundRegion(ref R, Constants.WorldHugeBlockSize * 2 / 12 + 2, hit, 0.25f, 0.25f);
            hit = CreateHeightAroundRegion(ref R, Constants.WorldHugeBlockSize * 2 / 12 + 5, hit, 0.1f, 0.9f);
            hit = CreateHeightAroundRegion(ref R, Constants.WorldHugeBlockSize * 2 / 12 + 7, hit, 0.1f, 0.9f);
            hit = CreateHeightAroundRegion(ref R, Constants.WorldHugeBlockSize * 2 / 12 + 10, hit, 0.1f, 0.9f);
            hit = CreateHeightAroundRegion(ref R, Constants.WorldHugeBlockSize * 2 / 12 + 12, hit, 0.1f, 0.9f);
            hit = CreateHeightAroundRegion(ref R, Constants.WorldHugeBlockSize * 2 / 12 + 15, hit, 0.1f, 0.9f);
            hit = CreateHeightAroundRegion(ref R, Constants.WorldHugeBlockSize * 2 / 12 + 17, hit, 0.1f, 0.9f);
            //hit = CreateHeightAroundRegion(ref R, Constants.WorldHugeBlockSize * 2 / 12 + 1, hit, 0.1f, 0.9f);
            //hit = CreateHeightAroundRegion(ref R, Constants.WorldHugeBlockSize * 10 / 12, hit, 0.1f, 0.9f);
        }

        private void CreateRegionTerrain(ref Region R)
        {
            const int hugeBlockSize = Constants.WorldHugeBlockSize;
            const int largeAreaBleed = Constants.WorldLargeAreaBleed;
            const int largeAreaObstacleBaseSize = Constants.WorldLargeAreaObstacleBaseSize;
            const int largeAreaObstacleSizeWithBleed = largeAreaObstacleBaseSize + largeAreaBleed;
            // Radius around the building to be clear
            const int centralBuildingAreaRadius = 2;

            int x = R.GetUpperLeftWorldPointCorner().X;
            int y = R.GetUpperLeftWorldPointCorner().Y;

            Point UpperLeft;
            Point LowerRight;

            int xOuterLeft = x - largeAreaBleed;
            int xInnerLeft = x + largeAreaObstacleSizeWithBleed;
            int xInnerRight = x + hugeBlockSize - largeAreaObstacleSizeWithBleed;
            int xOuterRight = x + hugeBlockSize + largeAreaBleed;

            int yOuterTop = y - largeAreaBleed;
            int yInnerTop = y + largeAreaObstacleSizeWithBleed;
            int yInnerBottom = y + hugeBlockSize - largeAreaObstacleSizeWithBleed;
            int yOuterBottom = y + hugeBlockSize + largeAreaBleed;

            // Center Area is protected
            int xMiddle = (xInnerLeft + xInnerRight) / 2;
            int yMiddle = (yInnerTop + yInnerBottom) / 2;
            UpperLeft = new Point(xMiddle - centralBuildingAreaRadius, yMiddle - centralBuildingAreaRadius);
            LowerRight = new Point(xMiddle + centralBuildingAreaRadius, yMiddle + centralBuildingAreaRadius);
            CreateLargeArea(UpperLeft, LowerRight, LargeAreaType.ProtectedGrass);

            // UpperLeft LargeArea
            UpperLeft = new Point(xOuterLeft, yOuterTop);
            LowerRight = new Point(xInnerLeft, yInnerTop);
            CreateRandomBlockingLargeArea(UpperLeft, LowerRight);

            // UpperRight LargeArea
            UpperLeft = new Point(xInnerRight, yOuterTop);
            LowerRight = new Point(xOuterRight, yInnerTop);
            CreateRandomBlockingLargeArea(UpperLeft, LowerRight);

            // LowerLeft LargeArea
            UpperLeft = new Point(xOuterLeft, yInnerBottom);
            LowerRight = new Point(xInnerLeft, yOuterBottom);
            CreateRandomBlockingLargeArea(UpperLeft, LowerRight);

            // LowerRight LargeArea
            UpperLeft = new Point(xInnerRight, yInnerBottom);
            LowerRight = new Point(xOuterRight, yOuterBottom);
            CreateRandomBlockingLargeArea(UpperLeft, LowerRight);

            // Center Special area
            // Makes the special center area larger
            const int tweakValue = 3;
            UpperLeft = new Point(xInnerLeft - tweakValue, yInnerTop - tweakValue);
            LowerRight = new Point(xInnerRight + tweakValue, yInnerBottom + tweakValue);
            CreateRandomOpenLargeArea(UpperLeft, LowerRight);

        }

        /// <summary>
        /// Creates a height around a region
        /// </summary>
        /// <param name="R"></param>
        /// <param name="largeAreaObstacleSize"></param>
        /// <param name="hit"></param>
        /// <param name="ifmiss"></param>
        /// <param name="ifhit"></param>
        /// <returns></returns>
        private bool CreateHeightAroundRegion(ref Region R, int largeAreaObstacleSize, bool hit, float ifmiss, float ifhit)
        {
            if ((!hit && seedRandom.Next0to1() < ifmiss) ||
                (hit && seedRandom.Next0to1() < ifhit))
            {
                worldMatrix.ChangeValuesViaCenteredBox(
                    RaiseHeight(0.5f),
                    R.GetCenterOfBlockAsWorldPoint(),
                    largeAreaObstacleSize);
                return true;
            }
            return false;
        }

        private void CreateRandomOpenLargeArea(Point UpperLeft, Point LowerRight)
        {
            LargeAreaType randomLargeArea;

            switch (seedRandom.Next(5))
            {
                case 0:
                    randomLargeArea = LargeAreaType.MuddyOpen;
                    break;
                case 1:
                    randomLargeArea = LargeAreaType.ScatteredTrees;
                    break;
                default:
                    randomLargeArea = LargeAreaType.None;
                    break;
            }

            CreateLargeArea(UpperLeft, LowerRight, randomLargeArea);
        }

        private void CreateRandomBlockingLargeArea(Point UpperLeft, Point LowerRight)
        {
            LargeAreaType randomLargeArea;

            int random = seedRandom.Next(4);
            if (forestOnly)
                random = 3;

            switch (random)
            {
                case 0:
                    randomLargeArea = LargeAreaType.MountainComplex;
                    break;
                case 1:
                    randomLargeArea = LargeAreaType.Pit;
                    break;
                //case 2:
                //    randomLargeArea = LargeAreaType.Hills;
                //    break;
                default:
                    randomLargeArea = LargeAreaType.ThickForest;
                    break;
            }

            CreateLargeArea(UpperLeft, LowerRight, randomLargeArea);
        }

        private Array2D<WorldObject>.DelegateChangeValue SetSquareTerrainType(SquareTerrainType STT)
        {
            return delegate(ref WorldObject Wo)
            {
                Wo.SquareTerrainType = STT;
            };
        }

        private Array2D<WorldObject>.DelegateChangeValue SetLargeAreaType(LargeAreaType LAT)
        {
            return delegate(ref WorldObject Wo)
            {
                Wo.LargeAreaType = LAT;
            };
        }

        private void CreateLargeArea(Point LargeUpperLeftCorner, Point LargeLowerRightCorner, LargeAreaType LAT)
        {
            LargeAreaData LAD = LargeAreaList[(int)LAT];

            for (int i = 0; i < LAD.Iterations; i++)
            {
                CreateRandomlyPlacedArea(
                    LargeUpperLeftCorner,
                    LargeLowerRightCorner,
                    LAD.AreaSizeMin,
                    LAD.AreaSizeExtra,
                    LAD.STT,
                    LAT);
            }
        }

        private void CreateRandomlyPlacedArea(Point LargeUpperLeftCorner, Point LargeLowerRightCorner, int AreaSizeMin, int AreaSizeRandomlyExtra, SquareTerrainType STT, LargeAreaType LAT)
        {
            Point smallUpperLeftCorner = LargeUpperLeftCorner;
            Point smallLowerRightCorner = LargeLowerRightCorner;

            int newAreaSize = AreaSizeMin + seedRandom.Next(AreaSizeRandomlyExtra);

            getRandomInnerBox2(ref smallUpperLeftCorner, ref smallLowerRightCorner, newAreaSize);

            CreateSmallArea(smallUpperLeftCorner, smallLowerRightCorner, STT, LAT);
        }

        private void CreateSmallArea(Point Small_UpperLeftCorner, Point Small_LowerRightCorner, SquareTerrainType STT, LargeAreaType LAT)
        {
            // Calculate middle point of the small area
            Point middle = new Point(
                (Small_UpperLeftCorner.X + Small_LowerRightCorner.X) / 2,
                (Small_UpperLeftCorner.Y + Small_LowerRightCorner.Y) / 2);

            float plateauHeight = worldMatrix.GetValue(middle).Height;
            bool negativePlateau;


            if (GetWorldPoint(middle).LargeAreaType != LAT)
            {
                plateauHeight = plateauHeight + SmallAreaList[(int)STT].HeightIncrease;
                negativePlateau = (SmallAreaList[(int)STT].HeightIncrease < 0);
            }
            else
            {
                plateauHeight = plateauHeight + SmallAreaList[(int)STT].SelfHeightIncrease;
                negativePlateau = (SmallAreaList[(int)STT].HeightIncrease < 0);
            }

            Point small_UpperLeftCornerForHeight = new Point(Small_UpperLeftCorner.X + 1, Small_UpperLeftCorner.Y + 1);
            Calculations.ClampPointToRectangle(small_UpperLeftCornerForHeight, ref worldBoundaries);

            if (GetWorldPoint(middle).LargeAreaType == LAT || GetWorldPoint(middle).LargeAreaType == LargeAreaType.None)
            {

                worldMatrix.ChangeValuesViaSpecifiedBox(
                    DoAllIWant(STT, LAT, plateauHeight, negativePlateau),
                    Small_UpperLeftCorner,
                    Small_LowerRightCorner);

            }
        }

        private Array2D<WorldObject>.DelegateChangeValueGetLocation DoAllIWant(SquareTerrainType STT, LargeAreaType LAT, float PlateauHeight, bool NegativePlateau)
        {
            return delegate(ref WorldObject Wo, Point P)
            {
                // if CheckAreaTypeCompatibility(STT, LAT),
                if ((Wo.LargeAreaType == LAT || Wo.LargeAreaType == LargeAreaType.None) &&
                        (Wo.SquareTerrainType == STT || Wo.SquareTerrainType == SquareTerrainType.None))
                {
                    // Set STT
                    Wo.SquareTerrainType = STT;

                    // add height and set LAT
                    if (GetWorldPointNoNull(new Point(P.X - 1, P.Y - 1)).SquareTerrainType == STT &&
                        GetWorldPointNoNull(new Point(P.X - 1, P.Y)).SquareTerrainType == STT &&
                        GetWorldPointNoNull(new Point(P.X, P.Y - 1)).SquareTerrainType == STT)
                    {
                        SetHeightToPlateau(ref Wo, PlateauHeight, NegativePlateau);
                        Wo.LargeAreaType = LAT;
                    }
                }

            };
        }

        private Array2D<WorldObject>.DelegateChangeValue AddHeight(SquareTerrainType STT, float PlateauHeight)
        {
            return delegate(ref WorldObject Wo)
            {

                if (Wo.SquareTerrainType != STT)
                {
                    SetHeightToPlateau(
                        ref Wo,
                        PlateauHeight + SmallAreaList[(int)STT].HeightIncrease,
                        (SmallAreaList[(int)STT].HeightIncrease < 0));
                }
                else
                    SetHeightToPlateau(
                        ref Wo,
                        PlateauHeight + SmallAreaList[(int)STT].SelfHeightIncrease,
                        (SmallAreaList[(int)STT].SelfHeightIncrease < 0));
            };
        }

        private void InitLargeAreaDataList()
        {
            // mountainComplex
            LargeAreaData mountainComplex = new LargeAreaData();
            mountainComplex.AreaSizeMin = 4;
            mountainComplex.AreaSizeExtra = 3;
            mountainComplex.Iterations = 7;
            mountainComplex.STT = SquareTerrainType.Mountain;
            LargeAreaList[(int)LargeAreaType.MountainComplex] = mountainComplex;

            // pit
            LargeAreaData pit = new LargeAreaData();
            pit.AreaSizeMin = 5;
            pit.AreaSizeExtra = 4;
            pit.Iterations = 5;
            pit.STT = SquareTerrainType.Mud;
            LargeAreaList[(int)LargeAreaType.Pit] = pit;

            // forest
            LargeAreaData thickForest = new LargeAreaData();
            thickForest.AreaSizeMin = 4;
            thickForest.AreaSizeExtra = 3;
            thickForest.Iterations = 9;
            thickForest.STT = SquareTerrainType.Forest;
            LargeAreaList[(int)LargeAreaType.ThickForest] = thickForest;

            // Hills
            LargeAreaData hills = new LargeAreaData();
            hills.AreaSizeMin = 5;
            hills.AreaSizeExtra = 4;
            hills.Iterations = 4;
            hills.STT = SquareTerrainType.Hill;
            LargeAreaList[(int)LargeAreaType.Hills] = hills;
         
            // ScatteredTrees
            LargeAreaData scatteredTrees = new LargeAreaData();
            scatteredTrees.AreaSizeMin = 4;
            scatteredTrees.AreaSizeExtra = 4;
            scatteredTrees.Iterations = 3;
            scatteredTrees.STT = SquareTerrainType.ScatteredTree;
            LargeAreaList[(int)LargeAreaType.ScatteredTrees] = scatteredTrees;
            
            // MuddyOpen
            LargeAreaData muddyOpen = new LargeAreaData();
            muddyOpen.AreaSizeMin = 3;
            muddyOpen.AreaSizeExtra = 3;
            muddyOpen.Iterations = 5;
            muddyOpen.STT = SquareTerrainType.MuddyOpen;
            LargeAreaList[(int)LargeAreaType.MuddyOpen] = muddyOpen;

            // openGrass
            LargeAreaData openGrass = new LargeAreaData();
            openGrass.AreaSizeMin = 5;
            openGrass.AreaSizeExtra = 4;
            openGrass.Iterations = 1;
            openGrass.STT = SquareTerrainType.Grass;
            LargeAreaList[(int)LargeAreaType.ProtectedGrass] = openGrass;

        }

        private void InitSmallAreaDataList()
        {
            // none
            SmallAreaData none = new SmallAreaData();
            none.HeightIncrease = 0f;
            none.SelfHeightIncrease = 0f;
            none.textureX = 0;
            none.textureY = 0;
            none.Occupied = false;
            none.doodadTable = new float[] { 1f, };
            none.textureList = new byte[] { 0 };
            SmallAreaList[(int)SquareTerrainType.None] = none;

            // mud
            SmallAreaData mud = new SmallAreaData();
            mud.HeightIncrease = -3.0f;
            mud.SelfHeightIncrease = -0.1f;
            mud.MaxHeight = -0.5f;
            mud.textureX = 0;
            mud.textureY = 1;
            mud.Occupied = true;
            mud.doodadTable = new float[] {1f, 0.0f};
            mud.textureList = new byte[] { 2, };
            SmallAreaList[(int)SquareTerrainType.Mud] = mud;

            // grass
            // WARNING: currently the STT "none" is used almost everywhere,
            // that is probably what you want to change.
            SmallAreaData grass = new SmallAreaData();
            grass.HeightIncrease = 0.0f;
            grass.SelfHeightIncrease = 0.0f;
            grass.textureX = 0;
            grass.textureY = 0;
            grass.doodadTable = new float[] { 1f, 0.0f };
            grass.textureList = new byte[] { 0, };
            SmallAreaList[(int)SquareTerrainType.Grass] = grass;

            // forest
            SmallAreaData forest = new SmallAreaData();
            forest.HeightIncrease = 0.4f;
            forest.SelfHeightIncrease = 0.4f;
            forest.textureX = 1;
            forest.textureY = 0;
            forest.Occupied = false;
            forest.textureList = new byte[] { 1, 1, 4, 4, 4, 4, 6, 6 };
            forest.doodadTable = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
            if (forestOnly)
                forest.doodadTable = new float[] { 0.0f, 1.0f, 1.0f, 1.0f };
            SmallAreaList[(int)SquareTerrainType.Forest] = forest;

            // mountain
            SmallAreaData mountain = new SmallAreaData();
            mountain.HeightIncrease = 2.0f;
            mountain.SelfHeightIncrease = 1.0f;
            mountain.textureX = 1;
            mountain.textureY = 1;
            mountain.Occupied = false;
            mountain.doodadTable = new float[] { 2.0f, 0.0f, 0.0f, 0.15f };
            mountain.textureList = new byte[] { 3, 8 };
            SmallAreaList[(int)SquareTerrainType.Mountain] = mountain;

            // mountain
            SmallAreaData borderMountain = new SmallAreaData();
            borderMountain.HeightIncrease = 2.0f;
            borderMountain.SelfHeightIncrease = 1.0f;
            borderMountain.textureX = 1;
            borderMountain.textureY = 1;
            borderMountain.Occupied = true;
            borderMountain.doodadTable = new float[] { 10.0f, 1.0f, 1.0f, 1.0f };
            borderMountain.textureList = new byte[] { 3, 8, 8 };
            SmallAreaList[(int)SquareTerrainType.BorderMountain] = borderMountain;

            // scatteredTree
            SmallAreaData scatteredTree = new SmallAreaData();
            scatteredTree.HeightIncrease = 0.0f;
            scatteredTree.SelfHeightIncrease = 0.0f;
            scatteredTree.textureX = 0;
            scatteredTree.textureY = 0;
            scatteredTree.Occupied = false;
            scatteredTree.doodadTable = new float[] { 24.0f, 1.0f, 1.0f, 1.0f };
            scatteredTree.textureList = new byte[] { 0, 0, 0, 1 };
            SmallAreaList[(int)SquareTerrainType.ScatteredTree] = scatteredTree;

            // hill
            SmallAreaData hill = new SmallAreaData();
            hill.HeightIncrease = 1.0f;
            hill.SelfHeightIncrease = 1.0f;
            hill.textureX = 0;
            hill.textureY = 0;
            hill.Occupied = false;
            hill.doodadTable = new float[] { 20.0f, 1.0f, 1.0f, 1.0f };
            hill.textureList = new byte[] { 0 };
            SmallAreaList[(int)SquareTerrainType.Hill] = hill;

            // muddyOpen
            SmallAreaData muddyOpen = new SmallAreaData();
            muddyOpen.HeightIncrease = -0.5f;
            muddyOpen.SelfHeightIncrease = 0.0f;
            muddyOpen.textureX = 0;
            muddyOpen.textureY = 0;
            muddyOpen.Occupied = false;
            muddyOpen.doodadTable = new float[] { 1.0f };
            muddyOpen.textureList = new byte[] { 6 };
            SmallAreaList[(int)SquareTerrainType.MuddyOpen] = muddyOpen;
        }

        public void FinalizeTerrain(ref WorldObject Wo, Point P)
        {
            //FlatLaneHeight(ref Wo, P);

            const float MaxWalkableSlopeHeight = 1.4f;

            ChangeIfSlopeIsWithinBoundaries(ref Wo, P, SquareTerrainType.Mountain, SquareTerrainType.Grass, float.MinValue, MaxWalkableSlopeHeight);
            ChangeIfSlopeIsWithinBoundaries(ref Wo, P, SquareTerrainType.Hill, SquareTerrainType.Mountain, MaxWalkableSlopeHeight, float.MaxValue);
            ChangeIfSlopeIsWithinBoundaries(ref Wo, P, SquareTerrainType.Forest, SquareTerrainType.Mountain, MaxWalkableSlopeHeight + 0.5f, float.MaxValue);
            ChangeIf_STT_IsInForeignArea(ref Wo, P, SquareTerrainType.Mud, SquareTerrainType.Grass, float.NegativeInfinity, 0.5f);
            ChangeIf_STT_IsInForeignArea(ref Wo, P, SquareTerrainType.MuddyOpen, SquareTerrainType.Grass, float.NegativeInfinity, 0.2f);
            //ChangeIfSlopeIsWithinBoundaries(ref Wo, P, SquareTerrainType.Grass, SquareTerrainType.Mountain, MaxWalkableSlopeHeight, float.PositiveInfinity);
            //ChangeIfSlopeIsWithinBoundaries(ref Wo, P, SquareTerrainType.HighGrass, SquareTerrainType.Mountain, MaxWalkableSlopeHeight, float.PositiveInfinity);
            //ChangeIfSlopeIsWithinBoundaries(ref Wo, P, SquareTerrainType.Hill, SquareTerrainType.Mountain, MaxWalkableSlopeHeight, float.PositiveInfinity);
            //ChangeIfSlopeIsWithinBoundaries(ref Wo, P, SquareTerrainType.Forest, SquareTerrainType.Mountain, MaxWalkableSlopeHeight, float.PositiveInfinity);

            SmallAreaData terrainType = SmallAreaList[(int)Wo.SquareTerrainType];

            int textureChoice = seedRandom.Next(terrainType.textureList.Length);
            int textureX = terrainType.textureList[textureChoice] / 3;
            int textureY = terrainType.textureList[textureChoice] % 3;
            SetTexture(textureX, textureY).Invoke(ref Wo);


            if (terrainType.Occupied)
                SetOccupied(ref Wo);

            if ((Wo.SquareTerrainType == SquareTerrainType.Mountain ||
                Wo.SquareTerrainType == SquareTerrainType.Mud) &&
                GetMaxHeightDifferenceInSquare(P) > MaxWalkableSlopeHeight)
                SetOccupied(ref Wo);

            #region Doodad decision region
            // -- Calculate what Doodad to place --

            // Calculate the probabilities of all doodads
            float doodadWeightSum = 0;
            for (int i = 0; i < terrainType.doodadTable.Length; i++)
            {
                doodadWeightSum += terrainType.doodadTable[i];
            }

            // randomly pick a value in this interval
            float doodadChoice = seedRandom.Next0to1() * doodadWeightSum;

            // temp value that keeps track on what choice we are on
            float iterateChoices = 0;

            for (int i = 0; i < terrainType.doodadTable.Length; i++)
            {
                iterateChoices += terrainType.doodadTable[i];

                // if we have found our intended doodad value,
                // let's place this doodad
                if (doodadChoice <= iterateChoices)
                {
                    PlaceDoodadOnFlatGroundDelegate(doodadTypes[i], Constants.DoodadMaxSlopingGround).Invoke(ref Wo, P);
                    break;
                }
            } 
            #endregion

            FixDiagonalSeam(ref Wo, P);
        }


        private void FixDiagonalSeam(ref WorldObject Wo, Point P)
        {

            /* All of this merely calculates which corner height
             * which is furthest from the average corner height. */

            List<float> originalHeights = new List<float>(4);
            originalHeights.Add(GetWorldPointNoNull(P).Height);
            originalHeights.Add(GetRightObject(P).Height);
            originalHeights.Add(GetDownObject(P).Height);
            originalHeights.Add(GetWorldPointNoNull(new Point(P.X + 1, P.Y + 1)).Height);

            float lowestHeight = originalHeights.Min();

            // lets raise all height so that they are at least zero
            List<float> raisedHeights = new List<float>(4);
            for (int i = 0; i < 4; i++)
                raisedHeights.Insert(i, originalHeights.ElementAt(i) + Math.Abs(lowestHeight));

            float average = raisedHeights.Average();

            List<float> differenceToAverage = new List<float>(4);
            for (int i = 0; i < 4; i++)
                differenceToAverage.Insert(i, Math.Abs(raisedHeights.ElementAt(i) - average));

            // calculate which value that had the highest difference to the average value
            int highestIndex = 0;
            for (int i = 0; i < 4; i++)
            {
                if (differenceToAverage.ElementAt(i) > differenceToAverage.ElementAt(highestIndex))
                    highestIndex = i;
            }

            // if the height which has the largest difference towards the average lies in
            // 0 or 3, then false (seam should go UL to DR)
            // 1 or 2: then true (seam should go DL to UR)
            if (highestIndex == 1 || highestIndex == 2)
                worldMatrix.Data[P.X, P.Y].DiagonalSeamIsDLtoUR = true;
            else
                worldMatrix.Data[P.X, P.Y].DiagonalSeamIsDLtoUR = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Wo"></param>
        /// <param name="P"></param>
        /// <param name="From">Change from this TerrainType</param>
        /// <param name="To">Change to this TerrainType</param>
        /// <param name="SlopeMinimumForChange">Slope must be larger then this</param>
        /// <param name="SlopeMaximumForChange">Slope must be smaller then this</param>
        public void ChangeIfSlopeIsWithinBoundaries(ref WorldObject Wo, Point P, SquareTerrainType From, SquareTerrainType To, float SlopeMinimumForChange, float SlopeMaximumForChange)
        {
            if (Wo.SquareTerrainType == From &&
                GetMaxHeightDifferenceInSquare(P) >= SlopeMinimumForChange &&
                GetMaxHeightDifferenceInSquare(P) <= SlopeMaximumForChange)
                Wo.SquareTerrainType = To;
        }

        public void ChangeIf_STT_IsInForeignArea(ref WorldObject Wo, Point P, SquareTerrainType From, SquareTerrainType To, float SlopeMinimumForChange, float SlopeMaximumForChange)
        {
            if (Wo.SquareTerrainType == From &&
                GetMaxHeightDifferenceInSquare(P) >= SlopeMinimumForChange &&
                GetMaxHeightDifferenceInSquare(P) <= SlopeMaximumForChange &&
                (
                    GetLeftObject(P).SquareTerrainType != From ||
                    GetRightObject(P).SquareTerrainType != From ||
                    GetUpObject(P).SquareTerrainType != From ||
                    GetDownObject(P).SquareTerrainType != From))
                Wo.SquareTerrainType = To;
        }

        public void ChangeIfDoesSlopeIsLowerThenRequired(ref WorldObject Wo, Point P, SquareTerrainType From, SquareTerrainType To, float SlopeCriteria)
        {
            if (Wo.SquareTerrainType == From &&
                GetMaxHeightDifferenceInSquare(P) <= SlopeCriteria)
                Wo.SquareTerrainType = To;
                
        }

        public void FlattenLaneHeightAnomaly(ref WorldObject Wo, Point P)
        {
            float maxHorizontalNeighborHeight =
                Math.Max(GetLeftObject(P).Height, GetRightObject(P).Height);
            if (Wo.Height > maxHorizontalNeighborHeight)
                Wo.Height = maxHorizontalNeighborHeight;

            float maxVerticalNeighborHeight =
                Math.Max(GetUpObject(P).Height, GetDownObject(P).Height);
            if (Wo.Height > maxVerticalNeighborHeight)
                Wo.Height = maxVerticalNeighborHeight;
        }

        public void FlattenLaneHeightDipAnomaly(ref WorldObject Wo, Point P)
        {
            float minHorizontalNeighborHeight =
                Math.Min(GetLeftObject(P).Height, GetRightObject(P).Height);
            if (Wo.Height < minHorizontalNeighborHeight)
                Wo.Height = minHorizontalNeighborHeight;

            float minVerticalNeighborHeight =
                Math.Min(GetUpObject(P).Height, GetDownObject(P).Height);
            if (Wo.Height < minVerticalNeighborHeight)
                Wo.Height = minVerticalNeighborHeight;
        }

        public Array2D<WorldObject>.DelegateChangeValue RaiseHeight(float Height)
        {
            return delegate(ref WorldObject Wo)
            {
                Wo.Height += Height;
            };
        }

        public Array2D<WorldObject>.DelegateChangeValue SetMinimumHeight(float Height)
        {
            return delegate(ref WorldObject Wo)
            {
                Wo.Height = Math.Max(Wo.Height, Height);
            };
        }

        public Array2D<WorldObject>.DelegateChangeValue SetHeightToPlateauDelegate(float PlateauHeight, bool NegativePlateau = false)
        {
            return delegate(ref WorldObject Wo)
            {
                SetHeightToPlateau(ref Wo, PlateauHeight, NegativePlateau);
            };
        }

        public Array2D<WorldObject>.DelegateChangeValueGetLocation SetHeightToPlateauDelegateWithChecking(float PlateauHeight, bool NegativePlateau = false)
        {
            return delegate(ref WorldObject Wo, Point P)
            {
                SquareTerrainType stt = Wo.SquareTerrainType;
                LargeAreaType lat = Wo.LargeAreaType;

                if (GetWorldPointNoNull(new Point(P.X - 1, P.Y - 1)).SquareTerrainType == stt &&
                    GetWorldPointNoNull(new Point(P.X - 1, P.Y    )).SquareTerrainType == stt &&
                    GetWorldPointNoNull(new Point(P.X    , P.Y - 1)).SquareTerrainType == stt)
                {
                    SetHeightToPlateau(ref Wo, PlateauHeight, NegativePlateau);
                }
            };
        }

        public void SetHeightToPlateau(ref WorldObject Wo, float PlateauHeight, bool NegativePlateau = false)
        {
            if (NegativePlateau)
            {
                if (Wo.Height > PlateauHeight)
                    Wo.Height = PlateauHeight;
            }
            else
            if (Wo.Height < PlateauHeight)
                Wo.Height = PlateauHeight;
        }

        public Array2D<WorldObject>.DelegateChangeValue SetHeight(float Height)
        {
            return delegate(ref WorldObject Wo)
            {
                Wo.Height = Height;
            };
        }

        public Array2D<WorldObject>.DelegateChangeValue SetTexture(int TextureX, int TextureY)
        {
            return delegate(ref WorldObject Wo)
            {
                Wo.TextureIndexX = (byte)TextureX;
                Wo.TextureIndexY = (byte)TextureY;

                Wo.TextureManipulation = TextureManipulation.None;

                if (seedRandom.Bool5050())
                    Wo.TextureManipulation = Wo.TextureManipulation | TextureManipulation.VerticalSwap;

                if (seedRandom.Bool5050())
                    Wo.TextureManipulation = Wo.TextureManipulation | TextureManipulation.DiagonalSwap;

                if (seedRandom.Bool5050())
                    Wo.TextureManipulation = Wo.TextureManipulation | TextureManipulation.HorizontalSwap;

            };
        }

        private void DistortHeight(ref WorldObject p)
        {
            p.Height += seedRandom.Next0to1() * 0.1f;
        }

        public Array2D<WorldObject>.DelegateChangeValue SetDoodad(DoodadType DT, float Chance)
        {
            return delegate(ref WorldObject Wo)
            {
                if (seedRandom.Next0to1() < Chance)
                {
                    Wo.Doodad.Rotation = seedRandom.NextFloat(Calculations.Pi * 2);
                    Wo.Doodad.DoodadType = DT;
                    Wo.Doodad.DoodadSize = seedRandom.NextFloat(Constants.DoodadSizeSpan) + Constants.DoodadMinSizeFactor;
                }
                else
                {
                    Wo.Doodad.DoodadType = DoodadType.None;
                }
            };
        }

        /// <summary>
        /// Places a Doodad on the map. Only places a doodad if the ground slopes
        /// less then the supplied value.
        /// </summary>
        /// <param name="DoodadType"></param>
        /// <returns></returns>
        public Array2D<WorldObject>.DelegateChangeValueGetLocation PlaceDoodadOnFlatGroundDelegate(DoodadType DoodadType, float maxSlopeDoodadCanStandOn)
        {
            return delegate(ref WorldObject Wo, Point P)
            {
                //const float maxSlopeDoodadCanStandOn = 0.8f;
                if (GetMaxHeightDifferenceInSquare(P) < maxSlopeDoodadCanStandOn || forestOnly)
                {
                    Wo.Doodad.Rotation = seedRandom.NextFloat(Calculations.Pi * 2);
                    Wo.Doodad.DoodadType = DoodadType;
                    Wo.Doodad.DoodadSize = seedRandom.NextFloat(Constants.DoodadSizeSpan) + Constants.DoodadMinSizeFactor;
                }
            };
        }

        /// <summary>
        /// Places a Doodad on the map. 
        /// </summary>
        /// <param name="DoodadType"></param>
        /// <returns></returns>
        public Array2D<WorldObject>.DelegateChangeValue PlaceDoodadDelegate(DoodadType DoodadType)
        {
            return delegate(ref WorldObject Wo)
            {
                Wo.Doodad.Rotation = seedRandom.NextFloat(Calculations.Pi * 2);
                Wo.Doodad.DoodadType = DoodadType;
                Wo.Doodad.DoodadSize = seedRandom.NextFloat(Constants.DoodadSizeSpan) + Constants.DoodadMinSizeFactor;
            };
        }

        private bool QualifyForHeightChange(ref WorldObject Wo, Point P)
        {
            SquareTerrainType x = Wo.SquareTerrainType;

            Point left = Calculations.ClampPointToRectangle(new Point(P.X - 1, P.Y), ref worldBoundaries);
            Point above = Calculations.ClampPointToRectangle(new Point(P.X, P.Y - 1), ref worldBoundaries);

            return (x == GetWorldPoint(left).SquareTerrainType &&
                    x == GetWorldPoint(above).SquareTerrainType);
        }

        private float GetFitHeight(Point P, float maxHeight)
        {
            Point up = Calculations.ClampPointToRectangle(new Point(P.X, P.Y - 1), ref worldBoundaries);
            Point left = Calculations.ClampPointToRectangle(new Point(P.X - 1, P.Y), ref worldBoundaries);
            Point right = Calculations.ClampPointToRectangle(new Point(P.X + 1, P.Y), ref worldBoundaries);
            Point down = Calculations.ClampPointToRectangle(new Point(P.X, P.Y + 1), ref worldBoundaries);

            float highestFound = Math.Max(
                Math.Max(
                    GetWorldPoint(up).Height,
                    GetWorldPoint(down).Height)
                ,
                Math.Max(
                    GetWorldPoint(left).Height,
                    GetWorldPoint(right).Height));

            highestFound = Math.Max(
                GetWorldPoint(up).Height,
                GetWorldPoint(left).Height);

            return Math.Min(highestFound, maxHeight);
        }

        public void CalculateHeightForDoodads()
        {
            worldMatrix.ChangeEveryPoint(CalculateAndSetHeight);
        }

        public void CalculateAndSetHeight(ref WorldObject Wo, Point P)
        {
            Vector2 position = Calculations.PointToVector2(P);
            position += new Vector2(0.5f, 0.5f);

            float height = GetScaledHeight(position);

            Wo.Doodad.DoodadHeight = height;
        }










        private void SetToWorldBorder(ref WorldObject Wo)
        {
            Wo.IsCompletelyBlocked = true;
            PlaceDoodadDelegate(GetRandomDoodadType()).Invoke(ref Wo);
        }

        public void SetWorldBorder()
        {
            worldMatrix.ChangeBorderValues(SetOccupied, Constants.WorldBorderSize + 4);
            worldMatrix.ChangeBorderValuesWithSlopeCompensation(RaiseHeight(2), Constants.WorldBorderSize + 4);
            worldMatrix.ChangeBorderValuesWithSlopeCompensation(RaiseHeight(2), Constants.WorldBorderSize + 2);
            worldMatrix.ChangeBorderValuesWithSlopeCompensation(RaiseHeight(2), Constants.WorldBorderSize);
            worldMatrix.ChangeBorderValues(SetSquareTerrainType(SquareTerrainType.BorderMountain), Constants.WorldBorderSize + 4);
        }

        public void SetOccupied(ref WorldObject Wo)
        {
            Wo.IsCompletelyBlocked = true;
        }

        public Texture2D GetTexture()
        {
            return worldTexture;
        }

        /// <summary>
        /// Returns how many data values there are in each
        /// row and column in the world.
        /// </summary>
        /// <returns></returns>
        public int GetDimension()
        {
            return dimension;
        }

        /// <summary>
        /// Returns the rectangle which defines where this world begins
        /// and ends.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetWorldBoundaries()
        {
            return worldBoundaries;
        }

        public float GetScaling()
        {
            return Scaling;
        }

        public WorldObject GetWorldPoint(Point P)
        {
            return worldMatrix.GetValue(P);
        }

        public WorldObject GetWorldPointNoNull(Point P)
        {
            P = Calculations.ClampPointToRectangle(P, ref worldBoundaries);
            return worldMatrix.GetValue(P);
        }

        public int GetIndex(int x, int y)
        {
            return worldMatrix.GetIndex(x, y);
        }

        /// <summary>
        /// Get Height. Unscaled.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public float GetUnscaledHeight(int x, int y)
        {
            return worldMatrix.GetValue(x, y).Height;
        }


        /// <summary>
        /// Get height. Scaled properly.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public float GetScaledHeight(Vector2 v)
        {
            
            int x = GetUnscaledCoordinate(v.X);
            int y = GetUnscaledCoordinate(v.Y);
            
            if (x < 0 || y < 0 || x >= (dimension-1) || y >= (dimension-1))
            {
                return 0;
            }
            //Avrunda bort den stora positionen
            v = GetLocalUnscaledVector(v);
            float result;

            if (!IsInUpperTriangle(v))
            {
                result = CalculateHeight(
                    GetUnscaledHeight(x, y),
                    GetUnscaledHeight(x, y + 1),
                    GetUnscaledHeight(x + 1, y + 1),
                    new Vector2(v.X, v.Y));
            }
            else
            {
                result = CalculateHeight(
                    GetUnscaledHeight(x + 1, y + 1),
                    GetUnscaledHeight(x + 1, y),
                    GetUnscaledHeight(x, y),
                    //invert both x and y.
                    new Vector2(1.0f - v.X, 1.0f - v.Y));
            }
            return result * Scaling;
        }

        /// <summary>
        /// Calculates the height of a triangle in the world's heightmap
        /// </summary>
        /// <param name="heightUpperLeft"></param>
        /// <param name="heightLowerLeft"></param>
        /// <param name="heightLowerRight"></param>
        /// <param name="position">Position in this Triangle to calculate height of</param>
        /// <returns></returns>
        public float CalculateHeight(float heightUpperLeft, float heightLowerLeft, float heightLowerRight, Vector2 position)
        {
            #region triangle layout description
            /*
             *  # = position
             *  
             *   aa
             *   |\
             *   |  \ B
             *   |    \
             *C |      cc
             *   |    /
             *   |  # A
             *   |/
             *   bb
            */
            #endregion

            // Change the position vector so that it originates from the lower
            // left corner (like in real math)
            position = new Vector2(position.X, 1.0f - position.Y);

            //http://en.wikipedia.org/wiki/Triangle#Trigonometric_ratios_in_right_triangles

            //angles
            double aa = Calculations.double_Pi4th;
            double bb = Calculations.double_Pi2th - Math.Atan2(position.Y, position.X);
            double cc = (Calculations.Pi - bb) - aa;

            //distances
            // C = 1
            double inversedSinCC = 1 / Math.Sin(cc);
            float A = (float)((Math.Sin(aa)) * inversedSinCC); // Multiply with C
            float B = (float)((Math.Sin(bb)) * inversedSinCC); // Multiply with C
            float distance = Vector2.Distance(Vector2.Zero, position);

            // Height at cc
            // Because B can be up to sqrt(2), we multiply it with sqrt(0.5),
            // to make sure that it stays within 0-1
            float h = Calculations.Interpolate(heightUpperLeft, heightLowerRight, (B * Calculations.SqrtOf2));

            // Height at # (between cc and bb)
            float result = Calculations.Interpolate(heightLowerLeft, h, distance /  A);

            return result;

        }

        private bool IsInUpperTriangle(Vector2 v)
        {
            return (GetLocalUnscaledCoordinate(v.X)) >= (GetLocalUnscaledCoordinate(v.Y));
        }

        /// <summary>
        /// Return the discrete coordinate in this world
        /// which this value resides in.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int GetUnscaledCoordinate(float x)
        {
            //TODO: check in Test that this is correct:
            //Casting to an int always floors the value.
            return (int)(x / Scaling);
        }

        /// <summary>
        /// Returns the coordinate in the local square
        /// which this value resides in.
        /// Value is between 0 and 1.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public float GetLocalUnscaledCoordinate(float x)
        {
            return (x / Scaling) - GetUnscaledCoordinate(x);
        }

        /// <summary>
        /// Returns the vector in the local square
        /// which this vector resides in.
        /// Vector coordinates is between 0 and 1.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public Vector2 GetLocalUnscaledVector(Vector2 v)
        {
            return new Vector2((v.X % Scaling) / Scaling, (v.Y % Scaling) / Scaling);
        }

        /// <summary>
        /// Returns the largest difference between the 4
        /// corners in the given square.
        /// </summary>
        /// <param name="P"></param>
        /// <returns></returns>
        public float GetMaxHeightDifferenceInSquare(Point P)
        {
            float height1 = GetWorldPointNoNull(P).Height;
            float height2 = GetWorldPointNoNull(new Point(P.X + 1, P.Y)).Height;
            float height3 = GetWorldPointNoNull(new Point(P.X, P.Y + 1)).Height;
            float height4 = GetWorldPointNoNull(new Point(P.X + 1, P.Y + 1)).Height;

            float highest = Math.Max(
                Math.Max(height1, height2),
                Math.Max(height3, height4));

            float lowest = Math.Min(
                Math.Min(height1, height2),
                Math.Min(height3, height4));

            return highest - lowest;
        }
        
        public Array2D<WorldObject> GetData()
        {
            return worldMatrix;
        }

        public DoodadType[] doodadTypes = (DoodadType[])Enum.GetValues(typeof(DoodadType));
        public DoodadType GetRandomDoodadType()
        {
            if (doodadTypes == null)
            {
                doodadTypes = (DoodadType[])Enum.GetValues(typeof(DoodadType));
            }

            // Do not count in the "None" Doodad type, thus,
            // substract 1 from the different possible types, and
            // and 1 to skip "None"
            return doodadTypes[(seedRandom.Next(doodadTypes.Length - 1) + 1)];
        }

        private WorldObject GetUpObject(Point P)
        {
            return GetWorldPoint(Calculations.ClampPointToRectangle(new Point(P.X, P.Y - 1), ref worldBoundaries));
        }

        private WorldObject GetLeftObject(Point P)
        {
            return GetWorldPoint(Calculations.ClampPointToRectangle(new Point(P.X - 1, P.Y), ref worldBoundaries));
        }

        private WorldObject GetRightObject(Point P)
        {
            return GetWorldPoint(Calculations.ClampPointToRectangle(new Point(P.X + 1, P.Y), ref worldBoundaries));
        }

        private WorldObject GetDownObject(Point P)
        {
            return GetWorldPoint(Calculations.ClampPointToRectangle(new Point(P.X, P.Y + 1), ref worldBoundaries));
        }
    }
}
