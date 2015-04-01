using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RTSgame.Collision;
using RTSgame.Utilities;

namespace RTSgame.GameObjects.Abstract
{

    public enum TryToMoveAgain : byte { Yes, No };

    public interface ICollidable : IInteractable
    {

        CollidableType GetCollidableType();

        // -----------------------------------------
        // These methods are usually set automatically
        // -----------------------------------------

        TryToMoveAgain HandleSolidCollision();

        TryToMoveAgain HandleNoSolidCollision();

        BoundingSphere GetCollisionSphere();

        BoundingBox GetCollisionBox();

        void UpdateCollisionBox();

        void UpdateCollisionBoxToDestination();

        void SetDestination(Vector2 v);

        Vector2 GetDestination();

        // -----------------------------------------
        // Internal methods, should not be touched
        // -----------------------------------------

        bool GetHasMoved();
        void SetHasMoved(bool b);

        void SetPosition(Vector2 v);

        bool GetTeleportStatus();

        void DoneTeleporting();

    }
}
