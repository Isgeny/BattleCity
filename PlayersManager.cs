using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class PlayersManager : TanksManager
    {
        private BonusManager _bonusManager;
        private Tank _tank1;
        private Tank _tank2;
        private Timer _watchDelayTimer;

        public PlayersManager(GUIForm guiForm, GameForm gameForm, BonusManager bonusManager) : base(guiForm, gameForm)
        {
            _tank1 = new FirstPlayerTank(guiForm);
            _tank2 = new SecondPlayerTank(guiForm);
            _tank1.InitializeTank();
            _tank2.InitializeTank();
            _bonusManager = bonusManager;

            _watchDelayTimer = new Timer();
            _watchDelayTimer.Interval = 10 * 1000;
            _watchDelayTimer.Tick += OnWatchDelayTimerTick;
        }

        public Tank P1Tank
        {
            get { return _tank1; }
        }

        public Tank P2Tank
        {
            get { return _tank2; }
        }

        public override void Subscribe()
        {
            foreach(Tank tank1 in Tanks)
            {
                tank1.Subscribe();
                tank1.Shoot += OnShoot;
                tank1.Destroyed += OnTankDestroyed;
                foreach(Tank tank2 in Tanks)
                    if(tank1 != tank2)
                        tank1.CheckPosition += tank2.GetCheckPositionListener();
            }

            GameForm.CompsManager.TanksDestroyed += OnCompTanksDestroyed;

            _bonusManager.CompTookBomb  += OnCompTookBomb;
            _bonusManager.CompTookWatch += OnCompTookWatch;
        }

        public override void Unsubscribe()
        {
            foreach(Tank tank1 in Tanks)
            {
                tank1.Unsubscribe();
                tank1.Shoot -= OnShoot;
                tank1.Destroyed -= OnTankDestroyed;
                foreach(Tank tank2 in Tanks)
                    if(tank1 != tank2)
                        tank1.CheckPosition -= tank2.GetCheckPositionListener();
            }

            GameForm.CompsManager.TanksDestroyed -= OnCompTanksDestroyed;

            _bonusManager.CompTookBomb  -= OnCompTookBomb;
            _bonusManager.CompTookWatch -= OnCompTookWatch;
        }

        private void OnShoot(object sender, ShellEventArgs e)
        {
            Shell s = e.Shell;
            s.Destroyed += OnShellDestroyed;
            foreach(Tank tank in Tanks)
                if(tank != s.Creator)
                    tank.CheckPosition += s.GetCheckPositionListener();
            foreach(Tank compTank in GameForm.CompsManager.Tanks)
                compTank.CheckPosition += s.GetCheckPositionListener();

            InvokeTankShoot(e);
        }

        private void OnShellDestroyed(object sender, EventArgs e)
        {
            Shell s = sender as Shell;
            foreach(Tank tank in Tanks)
                tank.CheckPosition -= s.GetCheckPositionListener();
            foreach(Tank compTank in GameForm.CompsManager.Tanks)
                compTank.CheckPosition -= s.GetCheckPositionListener();
        }

        private void OnTankDestroyed(object sender, EventArgs e)
        {
            Tank destroyedTank = sender as Tank;
            destroyedTank.Shoot -= OnShoot;
            destroyedTank.Destroyed -= OnTankDestroyed;
            AliveTanks--;
            foreach(Tank playerTank in Tanks)
                if(playerTank != destroyedTank)
                {
                    destroyedTank.CheckPosition -= playerTank.GetCheckPositionListener();
                    playerTank.CheckPosition -= destroyedTank.GetCheckPositionListener();
                }
            Tanks.Remove(destroyedTank);
            if(AliveTanks == 0)
                InvokeTanksDestroyed(new EventArgs());
        }

        private void OnCompTanksDestroyed(object sender, EventArgs e)
        {
            foreach(Tank playerTank in Tanks)
                playerTank.Respawn();
        }

        private void OnCompTookBomb(object sender, EventArgs e)
        {
            foreach(Tank tank in Tanks)
                if(!tank.RespawnTimer.Enabled)
                    tank.HP = 0;
        }

        private void OnCompTookWatch(object sender, EventArgs e)
        {
            foreach(Tank tank in Tanks)
                if(!tank.Rect.IsEmpty)
                    tank.MoveTimer.Stop();
            _watchDelayTimer.Start();
        }

        private void OnWatchDelayTimerTick(object sender, EventArgs e)
        {
            foreach(Tank tank in Tanks)
                if(!tank.Rect.IsEmpty)
                    tank.MoveTimer.Start();
            _watchDelayTimer.Stop();
        }

        public void SetPlayers(int players)
        {
            Tanks.Clear();
            Tanks.Add(_tank1);
            AliveTanks = 1;
            if(players == 2)
            {
                Tanks.Add(_tank2);
                AliveTanks = 2;
            }
        }
    }
}