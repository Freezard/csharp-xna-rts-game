using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework;
using RTSgame.Utilities;
using RTSgame.Collision;

namespace RTSgame.GameObjects.Doodads
{
    // Cube Tree
    class Tree1 : BaseTree
    {
        public Tree1(Vector2 newPosition)
            : base(newPosition, 1)
        {
            GetStandardSize();

        }
        public override float GetStandardSize()
        {
            return 0.011f;
        }
        public override float GetSolidCollisionSize()
        {
            return base.GetSolidCollisionSize() / 1.8f;
        }
    }

    // Cylinder Tree
    class Tree2 : BaseTree
    {
        public Tree2(Vector2 newPosition)
            : base(newPosition, 2)
        {
            
        }
        public override float GetStandardSize()
        {
            return 0.016f;
        }
        public override float GetSolidCollisionSize()
        {
            return base.GetSolidCollisionSize() / 1.5f;
        }
    }

    // Cone Tree
    class Tree3 : BaseTree
    {
        public Tree3(Vector2 newPosition)
            : base(newPosition, 3)
        {
         
        }
        public override float GetStandardSize()
        {
            return 0.03f;
        }
        public override float GetSolidCollisionSize()
        {
            return base.GetSolidCollisionSize() / 1.0f;
        }
    }
}
