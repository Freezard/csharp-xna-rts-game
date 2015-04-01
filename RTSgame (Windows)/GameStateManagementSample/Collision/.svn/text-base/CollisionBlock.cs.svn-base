using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;
using RTSgame.GameObjects;

namespace RTSgame.Collision
{

    class CollisionBlock : Block
    {
        public List<IInteractable> members;

        private const int memberSize = 24;

        private static IWorld world;

        public CollisionBlock()
        {
            members = new List<IInteractable>(memberSize);
        }

        public void SetWorld(IWorld World)
        {
            world = World;
        }

        public void AddMember(IInteractable collidable)
        {
            if (!members.Contains(collidable))
                members.Add(collidable);
        }

        public void RemoveMember(IInteractable collidable)
        {
            members.Remove(collidable);
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
