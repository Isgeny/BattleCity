using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class PlayersManager : TanksManager
    {
        public Field Field { get; set; }
        public List<Tank> Tanks { get; set; }
        public Tank P1Tank { get; private set; }
        public Tank P2Tank { get; private set; }

        private int _aliveTanks;
        public int AliveTanks
        {
            get { return _aliveTanks; }
            set
            {
                _aliveTanks = value;
                if(_aliveTanks == 0)
                {
                    var tanksDestroyedDelayTimer = new Timer();
                    tanksDestroyedDelayTimer.Interval = 5000;
                    tanksDestroyedDelayTimer.Tick += OnTanksDestroyedDelayTimerTick;
                    tanksDestroyedDelayTimer.Start();
                }
            }
        }

        public event ShellEventHandler TankShot;
        public event EventHandler TanksDestroyed;

        public PlayersManager(GUIForm guiForm, Field field) : base(guiForm, field)
        {
            Field = field;
            Tanks = new List<Tank>();

            P1Tank = new FirstPlayerTank(guiForm);
            P2Tank = new SecondPlayerTank(guiForm);
        }

        public override void Subscribe()
        {
            foreach(var tank1 in Tanks)
            {
                tank1.Subscribe();
                tank1.CheckPosition += Field.GetCheckPositionListener();
                tank1.Shot += OnShot;
                tank1.Destroyed += OnTankDestroyed;

                foreach(var tank2 in Tanks)
                    if(tank1 != tank2)
                        tank1.CheckPosition += tank2.GetCheckPositionListener();
            }
        }

        public override void Unsubscribe()
        {
            foreach(var tank1 in Tanks)
            {
                tank1.Unsubscribe();
                tank1.CheckPosition -= Field.GetCheckPositionListener();
                tank1.Shot -= OnShot;
                tank1.Destroyed -= OnTankDestroyed;

                foreach(var tank2 in Tanks)
                    if(tank1 != tank2)
                        tank1.CheckPosition -= tank2.GetCheckPositionListener();
            }
        }

        private void InvokeTankShot(ShellEventArgs e)
        {
            TankShot?.Invoke(this, e);
        }

        private void InvokeTanksDestroyed(EventArgs e)
        {
            TanksDestroyed?.Invoke(this, e);
        }

        private void OnShot(object sender, ShellEventArgs e)
        {
            InvokeTankShot(e);
        }

        private void OnTankDestroyed(object sender, EventArgs e)
        {
            var destroyedTank = sender as Tank;
            destroyedTank.Unsubscribe();
            destroyedTank.CheckPosition -= Field.GetCheckPositionListener();
            destroyedTank.Shot -= OnShot;
            destroyedTank.Destroyed -= OnTankDestroyed;

            foreach(var playerTank in Tanks)
                if(playerTank != destroyedTank)
                {
                    playerTank.CheckPosition -= destroyedTank.GetCheckPositionListener();
                    destroyedTank.CheckPosition -= playerTank.GetCheckPositionListener();
                }
            Tanks.Remove(destroyedTank);

            foreach(var compTank in Field.CompsManager.Tanks)
                compTank.CheckPosition -= destroyedTank.GetCheckPositionListener();

            AliveTanks--;
        }

        private void OnTanksDestroyedDelayTimerTick(object sender, EventArgs e)
        {
            var tanksDestroyedDelayTimer = sender as Timer;
            tanksDestroyedDelayTimer.Stop();
            tanksDestroyedDelayTimer.Tick -= OnTanksDestroyedDelayTimerTick;
            InvokeTanksDestroyed(new EventArgs());
        }

        public void SetNewStageTanksParameters()
        {
            foreach(var tank in Tanks)
                tank.SetNewStageParameters();
        }

        public void SetNewGameTanksParameters(int players)
        {
            Tanks.Clear();
            Tanks.Add(P1Tank);
            AliveTanks = 1;
            if(players == 2)
            {
                Tanks.Add(P2Tank);
                AliveTanks = 2;
            }
            foreach(var tank in Tanks)
                tank.SetNewGameParameters();
        }

        public int CountTanks()
        {
            return Tanks.Count;
        }
    }
}