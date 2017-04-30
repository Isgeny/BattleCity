using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public class CompsManager : TanksManager
    {
        private int _aliveTanks;
        private int _tanksOnField;
        private Timer _respawnTimer;
        private int _currentRespawnTank;
        private int _maxTankOnField;
        private Timer _watchDelayTimer;

        public CompsManager(GUIForm guiForm, Field field) : base(guiForm, field)
        {
            for(int i = 0; i < 20; i++)
            {
                bool isBonus = false;
                if(i == 3 || i == 10 || i == 18)
                    isBonus = true;
                CompTank compTank = new CompTank(GUIForm, isBonus);
                if(isBonus)
                    compTank.BonusShoot += Field.BonusManager.GetBonusTankShootedListener();
                Tanks.Add(compTank);
            }
            _respawnTimer = new Timer();
            _watchDelayTimer = new Timer();
            _watchDelayTimer.Interval = 10 * 1000;
            _watchDelayTimer.Tick += OnWatchDelayTimerTick;
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;

            _respawnTimer.Interval = TankRespawnTime();
            _respawnTimer.Tick += OnRespawnTimer;
            _respawnTimer.Start();

            Field.BonusManager.PlayerTookBomb  += OnPlayerTookBomb;
            Field.BonusManager.PlayerTookWatch += OnPlayerTookWatch;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;

            _respawnTimer.Tick -= OnRespawnTimer;
            _respawnTimer.Stop();

            Field.BonusManager.PlayerTookBomb  -= OnPlayerTookBomb;
            Field.BonusManager.PlayerTookWatch -= OnPlayerTookWatch;

            foreach(Tank tank1 in Tanks)
            {
                tank1.Unsubscribe();
                tank1.Shoot -= OnShoot;
                tank1.Destroyed -= OnTankDestroyed;
                foreach(Tank tank2 in Tanks)
                    if(tank1 != tank2)
                        tank1.CheckPosition -= tank2.GetCheckPositionListener();
                foreach(Tank playerTank in Field.PlayersManager.Tanks)
                {
                    playerTank.CheckPosition -= tank1.GetCheckPositionListener();
                    tank1.CheckPosition -= playerTank.GetCheckPositionListener();
                }
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            if(new Rectangle(928, 64, 64, 320).IntersectsWith(e.ClipRectangle))
            {
                Graphics g = e.Graphics;
                for(int i = _currentRespawnTank, j = 0; i <= 19; i += 2, j++)
                    g.DrawImageUnscaled(Properties.Resources.Comp_Icon, 928, 64 + j * 32);
                for(int i = _currentRespawnTank, j = 0; i <= 18; i += 2, j++)
                    g.DrawImageUnscaled(Properties.Resources.Comp_Icon, 960, 64 + j * 32);
            }
        }

        private void OnRespawnTimer(object sender, EventArgs e)
        {
            if(_tanksOnField < _maxTankOnField && _currentRespawnTank != 20)
            {
                if(Tanks[_currentRespawnTank].Rect.IsEmpty || RespawnCollision(Tanks[_currentRespawnTank]))
                {
                    Tanks[_currentRespawnTank].Respawn();
                    return;
                }

                Tanks[_currentRespawnTank].Subscribe();
                Tanks[_currentRespawnTank].Shoot += OnShoot;
                Tanks[_currentRespawnTank].Destroyed += OnTankDestroyed;
                Tanks[_currentRespawnTank].ShowRespawnAnimation();

                for(int i = 0; i < _currentRespawnTank; i++)
                    if(Tanks[_currentRespawnTank] != Tanks[i])
                        Tanks[i].CheckPosition += Tanks[_currentRespawnTank].GetCheckPositionListener();

                foreach(Tank playerTank in Field.PlayersManager.Tanks)
                {
                    playerTank.CheckPosition += Tanks[_currentRespawnTank].GetCheckPositionListener();
                    Tanks[_currentRespawnTank].CheckPosition += playerTank.GetCheckPositionListener();
                }

                _tanksOnField++;
                _currentRespawnTank++;

                GUIForm.Invalidate(new Rectangle(928, 64, 64, 320));
            }
        }

        private bool RespawnCollision(Tank tank)
        {
            for(int i = 0; i < _currentRespawnTank; i++)
                if(Tanks[i].Rect.IntersectsWith(Rectangle.Inflate(tank.Rect, 32, 32)) && Tanks[i].IsAlive())
                    return true;
            foreach(Tank playerTank in Field.PlayersManager.Tanks)
                if(playerTank.Rect.IntersectsWith(Rectangle.Inflate(tank.Rect, 32, 32)) && playerTank.IsAlive())
                    return true;
            return false;
        }

        private void OnShoot(object sender, ShellEventArgs e)
        {
            Shell s = e.Shell;
            s.Destroyed += OnShellDestroyed;
            foreach(Tank tank in Tanks)
                if(tank != s.Creator)
                    tank.CheckPosition += s.GetCheckPositionListener();
            foreach(Tank playerTank in Field.PlayersManager.Tanks)
                playerTank.CheckPosition += s.GetCheckPositionListener();
            InvokeTankShoot(e);
        }

        private void OnShellDestroyed(object sender, EventArgs e)
        {
            Shell s = sender as Shell;
            foreach(Tank tank in Tanks)
                tank.CheckPosition -= ((Shell)sender).GetCheckPositionListener();
            foreach(Tank playerTank in Field.PlayersManager.Tanks)
                playerTank.CheckPosition -= s.GetCheckPositionListener();
        }

        private void OnTankDestroyed(object sender, EventArgs e)
        {
            CompTank tank = sender as CompTank;
            tank.Shoot -= OnShoot;
            GUIForm.Invalidate(tank.Rect);
            _aliveTanks--;
            _tanksOnField--;

            foreach(Tank compTank in Tanks)
                if(tank != compTank)
                {
                    compTank.CheckPosition -= tank.GetCheckPositionListener();
                    tank.CheckPosition -= compTank.GetCheckPositionListener();
                }

            foreach(Tank playerTank in Field.PlayersManager.Tanks)
            {
                playerTank.CheckPosition -= tank.GetCheckPositionListener();
                tank.CheckPosition -= playerTank.GetCheckPositionListener();
            }

            if(_aliveTanks == 0)
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
            InitializeTanks();

            foreach(Tank tank1 in Tanks)
            {
                tank1.Shoot -= OnShoot;
                tank1.Destroyed -= OnTankDestroyed;
                foreach(Tank tank2 in Tanks)
                    if(tank1 != tank2)
                        tank1.CheckPosition -= tank2.GetCheckPositionListener();
                foreach(Tank playerTank in Field.PlayersManager.Tanks)
                {
                    playerTank.CheckPosition -= tank1.GetCheckPositionListener();
                    tank1.CheckPosition -= playerTank.GetCheckPositionListener();
                }
            }
        }

        private void OnPlayerTookBomb(object sender, EventArgs e)
        {
            foreach(Tank tank in Tanks)
                if(!tank.Rect.IsEmpty && !tank.RespawnTimer.Enabled)
                    tank.HP = 0;
        }

        private void OnPlayerTookWatch(object sender, EventArgs e)
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

        public override void InitializeTanks()
        {
            base.InitializeTanks();
            _aliveTanks = 20;
            _tanksOnField = 0;
            _currentRespawnTank = 0;
            _maxTankOnField = (Field.PlayersManager.CountTanks() == 1) ? 4 : 6;
        }

        private int TankRespawnTime()
        {
            return ((190 - Field.Stage * 4 - (Field.PlayersManager.CountTanks() - 1) * 20) / 60) * 1000;
        }
    }
}