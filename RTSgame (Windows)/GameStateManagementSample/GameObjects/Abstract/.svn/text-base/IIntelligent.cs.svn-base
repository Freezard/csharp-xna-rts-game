using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RTSgame.GameObjects.Abstract
{
    interface IIntelligent
    {
        void AIUpdatePreInteractions(GameTime gameTime);

        void AIUpdatePostInteractions(GameTime gameTime);

        void AIInteract(IInteractable gameObject);

        float GetMaxAIInteractionRange();
    }
}
