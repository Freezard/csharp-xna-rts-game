using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;

namespace RTSgame.GameObjects.SpriteObjects
{
    class HealthBar:SpriteObject
    {
        Unit attachedUnit;
        public HealthBar(Unit attachedUnit):base(attachedUnit.GetPosition())
        {
            this.attachedUnit = attachedUnit;
            image = AssetBank.GetInstance().GetTexture("UIenemyIcon");

        }
        public override Vector3 GetPositionV3()
        {
            return attachedUnit.GetPositionV3();
        }
    }
}
