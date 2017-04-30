using System;
using System.Windows.Forms;

namespace BattleCity
{
    public class PlayersManager : TanksManager
    {
        public Tank P1Tank { get; private set; }
        public Tank P2Tank { get; private set; }
        private Timer _watchDelayTimer;

        public PlayersManager(GUIForm guiForm, Field field) : base(guiForm, field)
        {
            P1Tank = new FirstPlayerTank(guiForm);
            P2Tank = new SecondPlayerTank(guiForm);
            P1Tank.InitializeTank();
            P2Tank.InitializeTank();

            _watchDelayTimer = new Timer();
            _watchDelayTimer.Interval = 10 * 1000;
            _watchDelayTimer.Tick += OnWatchDelayTimerTick;
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

            Field.CompsManager.TanksDestroyed += OnCompTanksDestroyed;

            Field.BonusManager.CompTookBomb  += OnCompTookBomb;
            Field.BonusManager.CompTookWatch += OnCompTookWatch;
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

            Field.CompsManager.TanksDestroyed -= OnCompTanksDestroyed;

            Field.BonusManager.CompTookBomb  -= OnCompTookBomb;
            Field.BonusManager.CompTookWatch -= OnCompTookWatch;
        }

        private void OnShoot(object sender, ShellEventArgs e)
        {
            Shell s = e.Shell;
            s.Destroyed += OnShellDestroyed;
            foreach(Tank tank in Tanks)
                if(tank != s.Creator)
                    tank.CheckPosition += s.GetCheckPositionListener();
            foreach(Tank compTank in Field.CompsManager.Tanks)
                compTank.CheckPosition += s.GetCheckPositionListener();

            InvokeTankShoot(e);
        }

        private void OnShellDestroyed(object sender, EventArgs e)
        {
            Shell s = sender as Shell;
            foreach(Tank tank in Tanks)
                tank.CheckPosition -= s.GetCheckPositionListener();
            foreach(Tank compTank in Field.CompsManager.Tanks)
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
            {
                Timer timer = new Timer();
                timer.Interval = 5000;
                timer.Tick += OnTanksDestroyedTimer;
                timer.Start();
            }
        }

        private void OnTanksDestroyedTimer(object sender, EventArgs e)
        {
            Timer timer = sender as Timer;
            timer.Stop();
            timer.Tick -= OnTanksDestroyedTimer;
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
            Tanks.Add(P1Tank);
            AliveTanks = 1;
            if(players == 2)
            {
                Tanks.Add(P2Tank);
                AliveTanks = 2;
            }
        }
    }
}