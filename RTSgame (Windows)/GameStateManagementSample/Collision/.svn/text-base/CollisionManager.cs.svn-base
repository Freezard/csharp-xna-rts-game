using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;
using System.Collections;

namespace RTSgame.Collision
{

    /// <summary>
    /// Manages Collisions.
    /// </summary>
    
    //Vector = float coordinates in the real world
    //WorldPoint = int coordinates in the real world
    //BlockPoint = int coordinates over the blockmap

    public delegate bool DelegateTest(IInteractable c);

    delegate void DelegateModifyInteractable(IInteractable c);

    delegate void DelegateModifyCollidable(ICollidable c);

    delegate void DelegateModifyCollidables(IInteractable c1, IInteractable c2);

    delegate void DelegateModifyIntelligent(IIntelligent c);

    class CollisionManager
    {
        private HashSet<IInteractable> allMembers;
        private HashSet<ICollidable> allCollidableMembers;
        private int blockDimension = 8;
        private BlockMap<CollisionBlock> blocks;
        private IWorld world;

        private delegate void DelegateChangeCollidableGetBlock(IInteractable c, CollisionBlock b);

        private delegate bool DelegateCheckForSolidCollision(ICollidable c, CollisionBlock b);

        #region public methods: init, add, remove, updateAll
        /// <summary>
        /// 
        /// </summary>
        /// <param name="World"></param>
        public void Init(IWorld World)
        {
            this.world = World;

            CollisionMath.SetWorld(World);

            allMembers = new HashSet<IInteractable>();
            allCollidableMembers = new HashSet<ICollidable>();

            blocks = new BlockMap<CollisionBlock>(
                blockDimension,
                world.GetWorldBoundaries(),
                0);

            blocks.GetValue(0, 0).SetWorld(world);

            blocks.ChangeEveryPoint(InitCollisionBlocks);

        }

        private void InitCollisionBlocks(ref CollisionBlock c)
        {
            //If you need to do any specific CollisionBlock things,
            //do them here
        }

        public void AddInteractable(IInteractable collidable)
        {
            //collidable.RestrictPositionToMap();
            allMembers.Add(collidable);
            AddCollidableToNewBlock(collidable);

            if (collidable is ICollidable)
            {
                allCollidableMembers.Add((ICollidable)collidable);
            }
        }

        public void RemoveCollidable(IInteractable collidable)
        {
            RemoveCollidableFromOldBlock(collidable);
            allMembers.Remove(collidable);

            if (collidable is ICollidable)
                allCollidableMembers.Remove((ICollidable)collidable);
        }

        /// <summary>
        /// Update interactions between AI Objects and all other Interactables
        /// </summary>
        public void UpdateAIInteractions(GameTime gameTime)
        {
            UpdatePhase.CurrentPhase = Phase.PreparedForAIInteraction;

            // Pre interactions
            foreach (IInteractable collidable in allMembers)
            {
                if (collidable is IIntelligent)
                {
                    collidable.SetToCurrentPhase();
                    ((IIntelligent)collidable).AIUpdatePreInteractions(gameTime);
                }
            }

            // All AI objects interact with objects
            UpdatePhase.CurrentPhase = Phase.DoneAIInteraction;
            ForAllIterator(ResolveAIInteraction);

            // Post interactions
            foreach (IInteractable collidable in allMembers)
            {
                if (collidable is IIntelligent)
                {
                    collidable.SetToCurrentPhase();
                    ((IIntelligent)collidable).AIUpdatePostInteractions(gameTime);
                }
            }
        }

        /// <summary>
        /// Update interactions between all Interactables
        /// </summary>
        public void UpdateInteractions()
        {
            UpdatePhase.CurrentPhase = Phase.PreparedForInteraction;
            foreach (IInteractable collidable in allMembers)
            {
                collidable.SetToCurrentPhase();
            }

            // Resolve logical collision for all
            UpdatePhase.CurrentPhase = Phase.DoneInteraction;
            ForAllIterator(ResolveLogicalCollision);
        }

        /// <summary>
        /// Update Solid Collisions
        /// </summary>
        public void UpdateSolidCollisions()
        {
            UpdatePhase.CurrentPhase = Phase.PreparedForCollision;
            foreach (ICollidable collidable in allCollidableMembers)
            {
                InitiateForCollisionUpdate((ICollidable) collidable);
            }

            // for all, teleport (bool, position)

            // for all, restrict to map, needed?

            // Adjust movement for all
            UpdatePhase.CurrentPhase = Phase.DoneSolidCollision;
            ForAllCollidableIterator(ResolveSolidCollision);

            foreach (ICollidable collidable in allCollidableMembers)
            {
                if (NeedToUpdateCollidablesBlockPresence(collidable))
                    UpdateCollidablesBlockPresence(collidable);

                //TODO: actually, try to do this even later
                collidable.SetPosition(collidable.GetDestination());

                // If I3DCollidable ain't bound to the ground,
                // then we need to manually update it's height
                if (collidable is I3DCollidable)
                {
                    I3DCollidable collidable3D = (I3DCollidable) collidable;
                    collidable3D.SetHeight(collidable3D.GetDestinationHeight());
                }
            }

        } 
        #endregion

        #region private methods

        private bool NeedToUpdateCollidablesBlockPresence(ICollidable collidable)
        {
            if (collidable.GetDestination().X > 300)
            {
                Utilities.DebugPrinter.Write("CollisionManager.NeedToUpdate...: out of X bounds" + collidable + " coord: " + collidable.GetPosition());
                
            }

            CollisionBlock b = blocks.GetBlockViaWorldPoint(Calculations.V2ToPoint(collidable.GetDestination()));
            CollisionBlock b2 = blocks.GetBlockViaWorldPoint(Calculations.V2ToPoint(collidable.GetPosition()));

            return (b != b2);
        }

        private void UpdateCollidablesBlockPresence(ICollidable collidable)
        {
            RemoveCollidableFromOldBlock(collidable);
            AddCollidableToNewBlock(collidable);
        }

        private void AddCollidableToNewBlock(IInteractable collidable)
        {
            if (collidable is ICollidable)
            {
                CollisionBlock b = blocks.GetBlockViaWorldPoint(Calculations.V2ToPoint(((ICollidable)collidable).GetDestination()));
                b.AddMember(collidable);
            }
            else
            {
                CollisionBlock b = blocks.GetBlockViaWorldPoint(Calculations.V2ToPoint(collidable.GetPosition()));
                b.AddMember(collidable);
            }
        }

        private void RemoveCollidableFromOldBlock(IInteractable collidable)
        {
            CollisionBlock b = blocks.GetBlockViaWorldPoint(Calculations.V2ToPoint(collidable.GetPosition()));
            b.RemoveMember(collidable);
        }

        /// <summary>
        /// Makes an object ready for the entire interaction phase
        /// </summary>
        /// <param name="C"></param>
        public void InitiateForInteractionUpdate(IInteractable C)
        {
            C.SetToCurrentPhase();
        }

        /// <summary>
        /// Makes an object ready for the entire collision phase
        /// </summary>
        /// <param name="C"></param>
        public void InitiateForCollisionUpdate(ICollidable C)
        {
            C.SetToCurrentPhase();

            if (C.GetPosition() != C.GetDestination())
                C.SetHasMoved(true);
            else
                C.SetHasMoved(false);
        }
        
        /// <summary>
        /// Calls the supplied method exactly once for every collidable
        /// </summary>
        /// <param name="Method"></param>
        private void ForAllIterator(DelegateModifyInteractable Method)
        {
            // for all members
            foreach (IInteractable collidable in allMembers)
            {
                if (!(collidable.GetPhase() == UpdatePhase.CurrentPhase))
                {
                    // retrieve this block
                    CollisionBlock b = blocks.GetBlockViaWorldPoint(Calculations.V2ToPoint(collidable.GetPosition()));

                    // and update for everyone strictly in it
                    b.StrictBlockMembersIterator(Method);

                    b.StrictBlockMembersIterator(SetToLatestPhase);
                }
            }
        }
        /// <summary>
        /// Calls the supplied method exactly once for every collidable
        /// </summary>
        /// <param name="Method"></param>
        private void ForAllCollidableIterator(DelegateModifyCollidable Method)
        {
            // for all members
            foreach (ICollidable collidable in allCollidableMembers)
            {
                if (!(collidable.GetPhase() == UpdatePhase.CurrentPhase))
                {
                    // retrieve this block
                    CollisionBlock b = blocks.GetBlockViaWorldPoint(Calculations.V2ToPoint(collidable.GetPosition()));

                    // and update for everyone strictly in it
                    b.StrictBlockCollisionMembersIterator(Method);

                    b.StrictBlockMembersIterator(SetToLatestPhase);
                }
            }
        }

        private void ResolveAIInteraction(IInteractable collidable)
        {
            collidable.SetToCurrentPhase();

            ChangeBlocksViaWorldCircle(
                ResolveInteractionBetweenIntelligentAndBlock,
                collidable,
                collidable.GetPosition(),
                collidable.GetMaxInteractionRange());
        }

        private void ResolveInteractionBetweenIntelligentAndBlock(IInteractable collidable, CollisionBlock block)
        {
            collidable.SetToCurrentPhase();

            if (collidable is IIntelligent)
            {
                for (int i = 0; i < block.members.Count; i++)
                {
                    IInteractable other = block.members.ElementAt<IInteractable>(i);
                    if (collidable != other)
                        ((IIntelligent)collidable).AIInteract(other);
                }
            }
        }

        private void SetToLatestPhase(IInteractable collidable)
        {
            collidable.SetToCurrentPhase();
        }

        private void ResolveLogicalCollision(IInteractable collidable)
        {
            collidable.SetToCurrentPhase();

            ChangeBlocksViaWorldCircle(
                ResolveInteractionBetweenInteractableAndBlock,
                collidable,
                collidable.GetPosition(),
                collidable.GetMaxInteractionRange());
        }

        private void ResolveInteractionBetweenInteractableAndBlock(IInteractable collidable, CollisionBlock block)
        {
            for (int i = 0; i < block.members.Count; i++)
            {
                IInteractable other = block.members.ElementAt<IInteractable>(i);
                if (collidable != other)
                    collidable.HandlePossibleInteraction(other);
            }
        }

        private void ResolveSolidCollision(ICollidable collidable)
        {

            if (collidable.GetHasMoved())
            {

                int iterations = 0;
                const int maxIterations = 50;
                TryToMoveAgain tryAgain = TryToMoveAgain.Yes;

                // This part calculates and makes the final decision
                // on collidable's destination
                while (tryAgain == TryToMoveAgain.Yes && iterations <= maxIterations)
                {
                    collidable.UpdateCollisionBoxToDestination();

                    if (SolidCollisionCheck(collidable))
                    {
                        tryAgain = collidable.HandleSolidCollision();
                    }
                    else
                    {
                        tryAgain = collidable.HandleNoSolidCollision();
                    }

                    iterations++;
                    if (iterations == maxIterations - 1)
                    {
                        if (collidable is IHandleSolidCollision)
                            throw new Exception("CM.ResolveSolidC, too many iterations: " + collidable +
                                " strategy: " + ((IHandleSolidCollision)collidable).GetObstacleHandlingStrategy() +
                                " phase: " + ((IHandleSolidCollision)collidable).GetObstacleHandlingStrategyPhase());
                        else
                            throw new Exception("CM.ResolveSolidC, too many iterations: " + collidable);
                    }
                }
            }

            collidable.UpdateCollisionBoxToDestination();

            collidable.SetToCurrentPhase();
        }

        private bool SolidCollisionCheck(ICollidable collidable)
        {
            if (CollisionMath.GetDoodadCollision(collidable))
                return true;

            return CheckForSolidCollision(
                RestrictMovementDelegate,
                collidable,
                collidable.GetDestination(),
                collidable.GetPosition(),
                // change to collidable.GetSize() + Constants.MaxSize + Constants.MaxSpeed
                // must now include MaxSpeed, in case units have moved into a block,
                // but ain't present in it yet!
                // size * sqrt(2)?
                collidable.GetMaxInteractionRange());

        }

        private bool RestrictMovementDelegate(ICollidable collidable, CollisionBlock block)
        {
            for (int i = 0; i < block.members.Count; i++)
            {
                IInteractable otherObj = block.members.ElementAt<IInteractable>(i);

                if (!(otherObj is ICollidable))
                    continue;

                ICollidable other = (ICollidable) otherObj;

                CollisionType collisionType = CollisionInteractions.doesInteract(
                    collidable.GetCollidableType(), other.GetCollidableType());
                
                if (collisionType == CollisionType.NoCollision)
                    continue;

                if (collidable == other)
                    continue;

                if (other is Unit && ((Unit)other).GetObstacleHandlingStrategyPhase() == MovementStrategyPhase.IsStuck)
                    continue;


                // If we find a collision, abort the search (return)
                // and return true

                if (collisionType == CollisionType.Collision2D &&
                    CollisionMath.AreSolidColliding(collidable, other))
                {
                    return true;
                }

                // If we find a collision, abort the search (return)
                // and return true
                else if (collisionType == CollisionType.Collision3D &&
                    CollisionMath.AreSolidColliding3D(collidable, other))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Applies the function to all the values, which are either
        /// inside the first circle, or inside the second circle.
        /// Arguments are world format.
        /// </summary>
        /// <param name="Del">Method to be applied to all values</param>
        /// <param name="center1">First circle center, world format</param>
        /// <param name="center1">Second circle center, world format</param>
        /// <param name="reach">Radius of the circle, world format.</param>
        private void ChangeBlocksViaDoubleWorldCircle(DelegateChangeCollidableGetBlock Del, IInteractable target, Vector2 center1, Vector2 center2, float radius)
        {

            // Get the world coordinate borders
            Rectangle borders = Calculations.V2V2toRectangle(center1, center2);
            borders.Inflate(((int)radius) + 1, ((int)radius) + 1);
            Calculations.ClampRectangle(ref borders, blocks.GetMapBoundaries());

            // Get the blockpoints.
            Point startBlock = blocks.GetBlockPoint(new Point(borders.Left, borders.Top));
            Point endBlock = blocks.GetBlockPoint(new Point(borders.Right, borders.Bottom));

            // here is code for clamping the start and end block,
            // but it shouldn't be needed
            #region further clamping
            /*
            int xLeft = Calculations.Clamp((int)(startBlock.X), 0, XDimension - 1);
            int xRight = Calculations.Clamp((int)(endBlock.X), 0, XDimension - 1);
            int yUp = Calculations.Clamp((int)(startBlock.Y), 0, YDimension - 1);
            int yDown = Calculations.Clamp((int)(endBlock.Y), 0, YDimension - 1);
            */
            #endregion

            // Iterate through all these points
            for (int x = startBlock.X; x <= endBlock.X; x++)
            {
                for (int y = startBlock.Y; y <= endBlock.Y; y++)
                {
                    CollisionBlock block = blocks.Data[x, y];

                    Rectangle blockArea = block.GetOverlappingBoundaryRectangle();

                    if (Calculations.RectangleCircleIntersect(blockArea, center1, radius) ||
                        Calculations.RectangleCircleIntersect(blockArea, center2, radius))
                    {
                        Del.Invoke(target, blocks.Data[x, y]);
                    }
                }
            }
        }

        /// <summary>
        /// Applies the function to all the values, which are
        /// inside the circle
        /// </summary>
        /// <param name="Del">Method to be applied to all values</param>
        /// <param name="center">First circle center, world format</param>
        /// <param name="reach">Radius of the circle, world format.</param>
        private void ChangeBlocksViaWorldCircle(DelegateChangeCollidableGetBlock Del, IInteractable target, Vector2 center, float radius)
        {
            // Get the blockpoints.
            Point startBlock = blocks.GetBlockPoint2(
                Calculations.V2ToPoint(center - new Vector2(radius, radius)));
            Point endBlock = blocks.GetBlockPoint2(
                Calculations.V2ToPoint(center + new Vector2(radius, radius)));

            // Iterate through all these points
            for (int x = startBlock.X; x <= endBlock.X; x++)
            {
                for (int y = startBlock.Y; y <= endBlock.Y; y++)
                {
                    CollisionBlock block = blocks.Data[x, y];

                    Rectangle blockArea = block.GetStrictBoundaryRectangle();

                    if (Calculations.RectangleCircleIntersect(blockArea, center, radius))
                    {
                        Del.Invoke(target, blocks.Data[x, y]);
                    }
                }
            }
        }

        /// <summary>
        /// Applies the function to all the values, which are either
        /// inside the first circle, or inside the second circle.
        /// Arguments are world format.
        /// </summary>
        /// <param name="Del">Method to be applied to all values</param>
        /// <param name="center1">First circle center, world format</param>
        /// <param name="center1">Second circle center, world format</param>
        /// <param name="reach">Radius of the circle, world format.</param>
        private bool CheckForSolidCollision(DelegateCheckForSolidCollision Del, ICollidable target, Vector2 center1, Vector2 center2, float radius)
        {

            // Get the world coordinate borders
            Rectangle borders = Calculations.V2V2toRectangle(center1, center2);
            borders.Inflate(((int)radius) + 1, ((int)radius) + 1);
            Calculations.RestrictRectangle(ref borders, blocks.GetMapBoundaries());

            // Get the blockpoints.
            Point startBlock = blocks.GetBlockPoint(new Point(borders.Left, borders.Top));
            Point endBlock = blocks.GetBlockPoint(new Point(borders.Right, borders.Bottom));

            // Further clamping possible (currently deactivated
            #region further possible clamping
            /*
            int xLeft = Calculations.Clamp((int)(startBlock.X), 0, XDimension - 1);
            int xRight = Calculations.Clamp((int)(endBlock.X), 0, XDimension - 1);
            int yUp = Calculations.Clamp((int)(startBlock.Y), 0, YDimension - 1);
            int yDown = Calculations.Clamp((int)(endBlock.Y), 0, YDimension - 1);
            */
            //better:
            //startBlock = blocks.ClampPoint(startBlock);
            //endBlock = blocks.ClampPoint(endBlock);
            #endregion

            // Iterate through all these points
            for (int x = startBlock.X; x <= endBlock.X; x++)
            {
                for (int y = startBlock.Y; y <= endBlock.Y; y++)
                {
                    CollisionBlock block = blocks.Data[x, y];

                    Rectangle blockArea = block.GetOverlappingBoundaryRectangle();

                    if (Calculations.RectangleCircleIntersect(blockArea, center1, radius) ||
                        Calculations.RectangleCircleIntersect(blockArea, center2, radius))
                    {
                        if (Del.Invoke(target, blocks.Data[x, y]))
                            return true;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}
