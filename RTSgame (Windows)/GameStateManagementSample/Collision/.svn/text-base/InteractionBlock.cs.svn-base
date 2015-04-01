using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;
using RTSgame.GameObjects;
using RTSgame.Utilities.Memory;

namespace RTSgame.Collision
{

    class InteractionBlock : Block
    {
        public List<IInteractable> members;
        public List<CollidableType> types;

        private const int memberSize = 16;

        private static IWorld world;

        public InteractionBlock()
        {
            members = new List<IInteractable>(memberSize);
            types = new List<CollidableType>(memberSize);
        }

        public void SetWorld(IWorld World)
        {
            world = World;
        }

        public void AddMember(IInteractable collidable)
        {
            if (!members.Contains(collidable))
            {
                members.Add(collidable);

                if (collidable is ICollidable)
                    types.Add(((ICollidable)collidable).GetCollidableType());
                else
                    types.Add(CollidableType.None);
            }
            
        }

        public void RemoveMember(IInteractable collidable)
        {

            int listPosition = members.IndexOf(collidable);

            if (listPosition != -1)
            {
                members.RemoveAt(listPosition);
                types.RemoveAt(listPosition);
            }
            else
                DebugPrinter.Write("Error: Interaction Block did not contain member");

        }

        private List<IInteractable> GetAllPossibleCollisions()
        {
            return members;
        }

        /// <summary>
        /// Calls the supplied method with all block members
        /// who's actual position is inside the block.
        /// </summary>
        /// <param name="Method"></param>
        public void StrictBlockMembersIterator(DelegateModifyInteractable Method)
        {
            for (int i = 0; i < members.Count; i++)
            {
                IInteractable obj = members.ElementAt<IInteractable>(i);
                if (CoversStrictPosition(obj.GetPosition()))
                {
                    Method(obj);
                }
            }
        }

        /// <summary>
        /// Calls the supplied method with all block members
        /// who's actual position is inside the block.
        /// </summary>
        /// <param name="Method"></param>
        public void StrictBlockCollisionMembersIterator(DelegateModifyCollidable Method)
        {
            for (int i = 0; i < members.Count; i++)
            {
                IInteractable obj = members.ElementAt<IInteractable>(i);
                if (obj is ICollidable &&
                    CoversStrictPosition(obj.GetPosition()))
                {
                    Method((ICollidable)obj);
                }
            }
        }

        /// <summary>
        /// Calls the supplied method with all block members
        /// who's actual position is inside the block.
        /// </summary>
        /// <param name="Method"></param>
        public void StrictBlockAIMembersIterator(DelegateModifyIntelligent Method)
        {
            for (int i = 0; i < members.Count; i++)
            {
                IInteractable obj = members.ElementAt<IInteractable>(i);
                if (obj is IIntelligent &&
                    CoversStrictPosition(obj.GetPosition()))
                {
                    Method((IIntelligent)obj);
                }
            }
        }
    }
}
