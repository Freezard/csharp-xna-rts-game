using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.Utilities;
using Microsoft.Xna.Framework;

namespace RTSgame.GameObjects.Abstract
{
    public interface IInteractable
    {
        // -----------------------------------------
        // These are usually the methods that you'll need to implement
        // -----------------------------------------

        void HandlePossibleInteraction(IInteractable otherObject);

        float GetMaxInteractionRange();

        float GetSolidCollisionSize();

        //Boolean ShouldCheckForInteraction();

        // -----------------------------------------
        // These methods are usually set automatically
        // -----------------------------------------

        Phase GetPhase();
        void SetToCurrentPhase();

        Vector2 GetPosition();

    }
}
