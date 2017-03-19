using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCity
{
    public class TanksManager
    {
        private System.Collections.Generic.List<BattleCity.Tank> tanks;
        public event EventHandler TanksDefeated;

        public TanksManager(GUIForm guiForm)
        {
            throw new System.NotImplementedException();
        }

        public Tank Tanks
        {
            get => default(int);
            set
            {
            }
        }

        protected void OnTanksDefeated(EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public int TankCount()
        {
            throw new System.NotImplementedException();
        }
    }
}