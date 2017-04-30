using System;
using System.Collections.Generic;

namespace BattleCity
{
    public abstract class TanksManager : Object
    {
        protected Field Field { get; private set; }
        private List<Tank> _tanks;
        private int _aliveTanks;

        public event ShellEventHandler TankShoot;
        public event EventHandler TanksDestroyed;

        public TanksManager(GUIForm guiForm, Field field) : base(guiForm)
        {
            Field = field;
            Tanks = new List<Tank>();
        }

        public List<Tank> Tanks
        {
            get { return _tanks; }
            set { _tanks = value; }
        }

        public int AliveTanks
        {
            get { return _aliveTanks; }
            set { _aliveTanks = value; }
        }

        public int CountTanks()
        {
            return _tanks.Count;
        }

        protected void InvokeTankShoot(ShellEventArgs e)
        {
            TankShoot?.Invoke(this, e);
        }

        protected void InvokeTanksDestroyed(EventArgs e)
        {
            TanksDestroyed?.Invoke(this, e);
        }

        public virtual void InitializeTanks()
        {
            foreach(Tank tank in Tanks)
                tank.InitializeTank();
        }
    }
}