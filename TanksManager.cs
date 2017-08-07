using System;
using System.Collections.Generic;

namespace BattleCity
{
    public abstract class TanksManager : Object
    {
        public TanksManager(GUIForm guiForm, Field field) : base(guiForm)
        {
            /*Field = field;
            Tanks = new List<Tank>();*/
        }
    }
}