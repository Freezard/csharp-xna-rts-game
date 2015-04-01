using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSgame.GameObjects.Abstract;

namespace RTSgame.GameObjects.Buildings
{
    interface ICreatesUnits
    {
         void createUnit(Unit type);
    }
}
