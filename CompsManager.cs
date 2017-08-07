using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class CompsManager : TanksManager
    {
        public Field Field { get; set; }
        public List<Tank> Tanks { get; set; }
        public List<Tank> TanksOnField { get; set; }

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
        public event EventHandler BonusTankShoot;

        private Timer _respawnTimer;
        private int _currentRespawnTank;
        private const int MAX_STAGE_TANKS = 20;

        public CompsManager(GUIForm guiForm, Field field) : base(guiForm, field)
        {
            Field = field;

            Tanks = new List<Tank>();
            TanksOnField = new List<Tank>();

            _respawnTimer = new Timer();
            _respawnTimer.Tick += OnRespawnTimerTick;

            for(int i = 0; i < MAX_STAGE_TANKS; i++)
                Tanks.Add(new CompTank(GUIForm, false));
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
            foreach(var tank1 in Tanks)
            {
                tank1.Unsubscribe();
                tank1.CheckPosition -= Field.GetCheckPositionListener();
                tank1.Shot -= OnShot;
                tank1.Destroyed -= OnTankDestroyed;
                var compTank = tank1 as CompTank;
                if(compTank.IsBonus)
                    compTank.BonusShoot -= OnBonusShoot;

                foreach(var tank2 in Tanks)
                    if(tank1 != tank2)
                    {
                        tank1.CheckPosition -= tank2.GetCheckPositionListener();
                        tank2.CheckPosition -= tank1.GetCheckPositionListener();
                    }
                /**/
                foreach(var playerTank in Field.PlayersManager.Tanks)
                {
                    playerTank.CheckPosition -= compTank.GetCheckPositionListener();
                    compTank.CheckPosition -= playerTank.GetCheckPositionListener();
                }
            }
            TanksOnField.Clear();
            _respawnTimer.Stop();
        }

        private void InvokeTankShot(ShellEventArgs e)
        {
            TankShot?.Invoke(this, e);
        }

        private void InvokeTanksDestroyed(EventArgs e)
        {
            TanksDestroyed?.Invoke(this, e);
        }

        private void InvokeBonusTankShoot(EventArgs e)
        {
            BonusTankShoot?.Invoke(this, e);
        }

        private void OnShot(object sender, ShellEventArgs e)
        {
            InvokeTankShot(e);
        }

        private void OnBonusShoot(object sender, EventArgs e)
        {
            InvokeBonusTankShoot(e);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            if(new Rectangle(928, 64, 64, 320).IntersectsWith(e.ClipRectangle))
            {
                var g = e.Graphics;
                for(int i = _currentRespawnTank, j = 0; i <= 19; i += 2, j++)
                    g.DrawImageUnscaled(Properties.Resources.Comp_Icon, 928, 64 + j * 32);
                for(int i = _currentRespawnTank, j = 0; i <= 18; i += 2, j++)
                    g.DrawImageUnscaled(Properties.Resources.Comp_Icon, 960, 64 + j * 32);
            }
        }

        private void OnTankDestroyed(object sender, EventArgs e)
        {
            var destroyedTank = sender as Tank;
            destroyedTank.RemoveCheckPositionListeners();
            destroyedTank.RemoveShotListeners();
            destroyedTank.RemoveDestroyedListeners();

            foreach(var compTankOnField in TanksOnField)
                if(compTankOnField != destroyedTank)
                    compTankOnField.CheckPosition -= destroyedTank.GetCheckPositionListener();

            foreach(var playerTank in Field.PlayersManager.Tanks)
                playerTank.CheckPosition -= destroyedTank.GetCheckPositionListener();

            foreach(var shell in Field.Shells)
                shell.CheckPosition -= destroyedTank.GetCheckPositionListener();

            TanksOnField.Remove(destroyedTank);

            AliveTanks--;
        }

        private void OnTanksDestroyedDelayTimerTick(object sender, EventArgs e)
        {
            var tanksDestroyedDelayTimer = sender as Timer;
            tanksDestroyedDelayTimer.Stop();
            tanksDestroyedDelayTimer.Tick -= OnTanksDestroyedDelayTimerTick;
            _respawnTimer.Stop();

            InvokeTanksDestroyed(new EventArgs());
        }

        public void SetNewStageTanksParameters()
        {
            TanksOnField.Clear();
            AliveTanks = MAX_STAGE_TANKS;
            _respawnTimer.Interval = TankRespawnTime();
            _respawnTimer.Start();
            _currentRespawnTank = 0;            

            ((CompTank)Tanks[3]).IsBonus = true;
            ((CompTank)Tanks[10]).IsBonus = true;
            ((CompTank)Tanks[18]).IsBonus = true;

            //DEBUG
            //foreach(var tank in Tanks)
            //    ((CompTank)tank).IsBonus = true;
        }

        public void SetNewGameTanksParameters(int players)
        {
            TanksOnField.Clear();
            foreach(var tank in Tanks)
                tank.SetNewGameParameters();
        }

        public int CountTanks()
        {
            return Tanks.Count;
        }

        private int TankRespawnTime()
        {
            return ((190 - Field.Stage * 4 - (Field.PlayersManager.CountTanks() - 1) * 20) / 60) * 1000;
        }

        private int MaxTanksOnField()
        {
            return (Field.PlayersManager.CountTanks() == 1) ? 4 : 6;
        }

        private void OnRespawnTimerTick(object sender, EventArgs e)
        {
            if(TanksOnField.Count < MaxTanksOnField())
            {
                var currentRespawnTank = Tanks[_currentRespawnTank];
                currentRespawnTank.SetNewStageParameters();
                currentRespawnTank.Subscribe();
                currentRespawnTank.CheckPosition += Field.GetCheckPositionListener();
                currentRespawnTank.Shot += OnShot;
                currentRespawnTank.Destroyed += OnTankDestroyed;
                var compTank = currentRespawnTank as CompTank;
                if(compTank.IsBonus)
                    compTank.BonusShoot += OnBonusShoot;

                foreach(var compTankOnField in TanksOnField)
                {
                    compTankOnField.CheckPosition += currentRespawnTank.GetCheckPositionListener();
                    currentRespawnTank.CheckPosition += compTankOnField.GetCheckPositionListener();
                }
                foreach(var playerTank in Field.PlayersManager.Tanks)
                {
                    playerTank.CheckPosition += compTank.GetCheckPositionListener();
                    compTank.CheckPosition += playerTank.GetCheckPositionListener();
                }
                TanksOnField.Add(compTank);

                _currentRespawnTank++;

                if(_currentRespawnTank == MAX_STAGE_TANKS)
                    _respawnTimer.Stop();
                GUIForm.Invalidate(new Rectangle(928, 64, 64, 320));
            }
        }
    }
}